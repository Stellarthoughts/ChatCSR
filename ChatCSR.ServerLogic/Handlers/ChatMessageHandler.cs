using ChatCSR.Core;
using ChatCSR.ServerLogic.Managers;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace ChatCSR.ServerLogic.Handlers
{
	public class ChatMessageHandler : WebSocketHandler
	{
		public ChatMessageHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
		{
		}

		public override async Task OnConnected(WebSocket socket)
		{
			await base.OnConnected(socket);

			var socketId = WebSocketConnectionManager.GetId(socket);
			//await SendMessageToAllAsync($"{socketId} is now connected");
		}

		public override async Task OnDisconnected(WebSocket socket)
		{
			await SendMessageToAllAsync(Serialize(
				new ServerMessage(new(), new() { $"User {WebSocketConnectionManager.GetId(socket)} disconnected." }, MessageType.Chat)
				));
			await base.OnDisconnected(socket);
		}

		public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			var socketId = WebSocketConnectionManager.GetId(socket);

			ClientMessage msg = JsonConvert.DeserializeObject<ClientMessage>(Encoding.UTF8.GetString(buffer, 0, result.Count))!;

			switch(msg.Type)
			{
				case MessageType.Connection:
					User user = new User(msg.Content);
					user.Id = socketId;
					await SendMessageAsync(socket, Serialize(
						new ServerMessage(new() {user}, new() {"welcome"}, MessageType.Connection)
						));
					await SendMessageToAllAsync(Serialize(
						new ServerMessage(new(), new() {$"User {user.Name} is connected."}, MessageType.User)
						));
					break;
				case MessageType.Chat:
					await SendMessageToAllAsync(Serialize(new ServerMessage(new(), new() { $"{msg.Content}" }, MessageType.Chat)
						));
					break;
			}
		}
		private string Serialize(ServerMessage message) => JsonConvert.SerializeObject(message);
	}
}
