using ChatCSR.Core;
using ChatCSR.ServerLogic.Managers;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace ChatCSR.ServerLogic.Handlers
{
	public abstract class WebSocketHandler
	{
		protected ConnectionManager WebSocketConnectionManager { get; set; }

		public WebSocketHandler(ConnectionManager webSocketConnectionManager)
		{
			WebSocketConnectionManager = webSocketConnectionManager;
		}

		public virtual Task OnConnected(WebSocket socket)
		{
			return Task.Run(() => WebSocketConnectionManager.AddSocket(socket));
		}

		public virtual async Task OnDisconnected(WebSocket socket)
		{
			await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
		}

		public async Task SendMessageAsync(WebSocket socket, string message)
		{
			if (socket.State != WebSocketState.Open)
				return;

			await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.UTF8.GetBytes(message),
																	offset: 0,
																	count: message.Length),
									messageType: WebSocketMessageType.Text,
									endOfMessage: true,
									cancellationToken: CancellationToken.None);
		}

		public async Task SendMessageAsync(string socketId, string message)
		{
			await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), message);
		}

		public async Task SendMessageToAllAsync(string message)
		{
			foreach (var pair in WebSocketConnectionManager.GetAll())
			{
				if (pair.Value.State == WebSocketState.Open)
					await SendMessageAsync(pair.Value, message);
			}
		}

		public async Task SendMessageToGroup(List<WebSocket> sockets, string message)
		{
			foreach (var socket in sockets)
				await SendMessageAsync(socket, message);
		}

		public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
	}
}
