using ChatCSR.Core;
using ChatCSR.ServerLogic.Managers;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using System.Linq;
using ChatCSR.ServerLogic.DB;

namespace ChatCSR.ServerLogic.Handlers
{
	public class ChatMessageHandler : WebSocketHandler
	{
		private Repository _repository;
		public ChatMessageHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
		{
			_repository = new();
			DBContextSeeder.Seed(_repository);
		}

		public override async Task OnConnected(WebSocket socket)
		{
			await base.OnConnected(socket);
			await SendMessageToAllAsync(Serialize(
				new ServerMessage(new() {new(WebSocketConnectionManager.GetId(socket))}, new(), MessageType.User)
				));
		}

		public override async Task OnDisconnected(WebSocket socket)
		{
			await SendMessageToAllAsync(Serialize(
				new ServerMessage(new() {new(WebSocketConnectionManager.GetId(socket))}, new() {}, MessageType.UserLeft)
				));
			await base.OnDisconnected(socket);
		}

		public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			ClientMessage msg = JsonConvert.DeserializeObject<ClientMessage>(Encoding.UTF8.GetString(buffer, 0, result.Count))!;
			HandleDB(msg);
			await Reply(socket, msg);
		}

		private void HandleDB(ClientMessage message)
		{
			if(message.Type == MessageType.Chat)
			{
				_repository.Insert(new(message.Content));
				_repository.Save();
			}
		}
		
		private async Task Reply(WebSocket socket, ClientMessage message)
		{
			switch (message.Type)
			{
				case MessageType.Connection:
					await SendMessageAsync(socket, Serialize(
						new ServerMessage(new(),
						_repository.GetAll().Select(x => x.Content).ToList()!, MessageType.Connection)
						));
					break;
				case MessageType.Chat:
					await SendMessageToAllAsync(Serialize(
						new ServerMessage(new(), new() {message.Content}, MessageType.Chat)
						));
					break;
			}
		}

		private string Serialize(ServerMessage message) => JsonConvert.SerializeObject(message);
	}
}
