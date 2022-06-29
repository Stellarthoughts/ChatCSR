using ChatCSR.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatCSR.DesktopClient.Services
{
	public class MessageService
	{
		#region Events
		public event EventHandler<List<string>> OnMessage = null!;
		public event EventHandler<List<User>> OnUser = null!;
		public event EventHandler<List<User>> OnUserLeft = null!;
		public event EventHandler OnConnected = null!;
		public event EventHandler OnBadConnection = null!;
		public event EventHandler OnBadMessageSend = null!;
		#endregion

		#region Fields
		private string _api = "message";
		private ClientWebSocket _clientWebSocket = null!;
		private User? _user;
		#endregion

		#region Methods
		public async void Connect(string ip, string port, string username)
		{
			_user = new(username);

			_clientWebSocket = new();
			try
			{
				await _clientWebSocket.ConnectAsync(new Uri($"{ip}:{port}/{_api}"), CancellationToken.None);
				OnConnected.Invoke(this, new());
			}
			catch (WebSocketException)
			{
				OnBadConnection.Invoke(this, new());
				return;
			}

			_ = ReceiveMessage();
			SendMessage(JsonConvert.SerializeObject(username), MessageType.Connection);
		}

		public void SendMessage(string messageInput, MessageType type)
		{
			if (_user == null)
				throw new Exception();

			ClientMessage messageObject = new(_user.Name, messageInput, MessageType.Chat);

			var jsonMessage = JsonConvert.SerializeObject(messageObject);
			var bytes = Encoding.UTF8.GetBytes(jsonMessage);
			try
			{
				_clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
			}
			catch (Exception)
			{
				OnBadMessageSend.Invoke(this, new());
			}
		}

		private async Task ReceiveMessage()
		{
			var buffer = new byte[1024 * 4];

			while (true)
			{
				var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					await _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
					break;
				}
				string jsonMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
				InterpretMessage(jsonMessage);
			}
		}
		private void InterpretMessage(string message)
		{
			ServerMessage? msg = JsonConvert.DeserializeObject<ServerMessage>(message);

			if (msg == null)
				throw new Exception();

			if (_user == null)
				throw new Exception();

			switch (msg.Type)
			{
				case MessageType.Connection:
					_user.Id = msg.Users[0].Id;
					OnConnected.Invoke(this, new());
					break;
				case MessageType.Chat:
					OnMessage.Invoke(this, msg.Content);
					break;
				case MessageType.User:
					OnUser.Invoke(this, msg.Users);
					break;
				case MessageType.UserLeft:
					OnUserLeft.Invoke(this, msg.Users);
					break;
				default:
					break;
			}
		}
		#endregion
	}
}