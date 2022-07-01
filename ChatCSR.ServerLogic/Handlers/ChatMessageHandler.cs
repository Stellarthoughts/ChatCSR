using ChatCSR.Core;
using ChatCSR.ServerLogic.DB;
using ChatCSR.ServerLogic.Managers;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace ChatCSR.ServerLogic.Handlers
{
	public class ChatMessageHandler : WebSocketHandler
	{
		private readonly IRepository _repository;
		private readonly ConcurrentDictionary<WebSocket, User> _users;

		public ChatMessageHandler(ConnectionManager webSocketConnectionManager, IRepository repository) : base(webSocketConnectionManager)
		{
			_users = new();
			_repository = repository;
		}

		public override async Task OnConnected(WebSocket socket)
		{
			await base.OnConnected(socket);

			User user = new User();
			user.Id = Guid.NewGuid().ToString();
			_users.TryAdd(socket, user);
		}

		public override async Task OnDisconnected(WebSocket socket)
		{
			_users.Remove(socket, out User? user);
			if (user == null)
				return;

			await SendMessageToAllAsync(Serialize(
				new ServerMessage(new() { user }, new() { }, MessageType.UserLeft)
				));

			await base.OnDisconnected(socket);
		}

		public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			ClientMessage msg = JsonConvert.DeserializeObject<ClientMessage>(Encoding.UTF8.GetString(buffer, 0, result.Count))!;
			await HandleDB(msg);
			await Reply(socket, msg);
		}

		private async Task HandleDB(ClientMessage message)
		{
			if (message.Type == MessageType.Chat)
			{
				_repository.Insert(new(message.Content, message.Sender));
				await _repository.Save();
			}
		}

		private async Task Reply(WebSocket socket, ClientMessage message)
		{
			_users.TryGetValue(socket, out User? user);
			if (user == null)
				return;

			switch (message.Type)
			{
				case MessageType.Connection:
					user.Name = message.Content;
					await SendMessageAsync(socket, Serialize(
						new ServerMessage(
							_users.Values.ToList(),
							_repository.GetAll().ToList()!, MessageType.Connection)
						));
					await SendMessageToGroup(
						WebSocketConnectionManager.GetAll().Select(x => x.Value).Where(x => x != socket).ToList(),
						Serialize(new ServerMessage(new() { user }, new(), MessageType.User))
						);
					break;
				case MessageType.Chat:
					await SendMessageToAllAsync(
						Serialize(new ServerMessage(new(), new() { new(message.Content, message.Sender) }, MessageType.Chat))
						);
					break;
			}
		}

		private string Serialize(ServerMessage message) => JsonConvert.SerializeObject(message);
	}
}
