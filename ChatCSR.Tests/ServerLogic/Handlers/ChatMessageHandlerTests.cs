using ChatCSR.Core;
using ChatCSR.ServerLogic.DB;
using ChatCSR.ServerLogic.Handlers;
using ChatCSR.ServerLogic.Managers;
using Moq;
using NUnit.Framework;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading.Tasks;

namespace ChatCSR.Tests.ServerLogic.Handlers
{
	public class ChatMessageHandlerTests
	{
		private Mock<WebSocket> _webSocket = null!;
		private Mock<ConnectionManager> _connectionManager = null!;
		private Mock<IRepository> _repository = null!;
		private ChatMessageHandler _handler = null!;

		[SetUp]
		public void Setup()
		{
			_connectionManager = new();
			_webSocket = new();
			_repository = new();
			_handler = new(_connectionManager.Object, _repository.Object);
		}

		[Test]
		public async Task OnConnected_UserGetsAdded()
		{
			_webSocket.Setup(x => x.State).Returns(WebSocketState.Open);

			await _handler.OnConnected(_webSocket.Object);

			var handlerType = typeof(ChatMessageHandler);
			var fields = handlerType.GetFields(BindingFlags.NonPublic
			| BindingFlags.Instance);
			var field = fields.First(x => x.Name == "_users");
			var dic = (ConcurrentDictionary<WebSocket, User>)field.GetValue(_handler)!;
			Assert.That(dic.Count > 0);

			await _handler.OnDisconnected(_webSocket.Object);
		}

		[Test]
		public async Task OnConnected_SocketGetsAdded()
		{
			_webSocket.Setup(x => x.State).Returns(WebSocketState.Open);

			await _handler.OnConnected(_webSocket.Object);

			var handlerType = typeof(WebSocketHandler);
			var fields = handlerType.GetFields(BindingFlags.NonPublic
			| BindingFlags.Instance);
			var field = fields.First(x => x.Name == "<WebSocketConnectionManager>k__BackingField");
			var cm = (ConnectionManager)field.GetValue(_handler)!;
			Assert.That(cm.GetAll().Count > 0);

			await _handler.OnDisconnected(_webSocket.Object);
		}

		[Test]
		public async Task Reply_()
		{
			_webSocket.Setup(x => x.State).Returns(WebSocketState.Open);

			await _handler.OnConnected(_webSocket.Object);

			var handlerType = typeof(WebSocketHandler);
			var fields = handlerType.GetFields(BindingFlags.NonPublic
			| BindingFlags.Instance);
			var field = fields.First(x => x.Name == "<WebSocketConnectionManager>k__BackingField");
			var cm = (ConnectionManager)field.GetValue(_handler)!;
			Assert.That(cm.GetAll().Count > 0);

			await _handler.OnDisconnected(_webSocket.Object);
		}
	}
}
