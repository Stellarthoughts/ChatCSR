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
		public async Task OnConnected_UserGetsAddedAndDeleted()
		{
			_webSocket.Setup(x => x.State).Returns(WebSocketState.Open);

			await _handler.OnConnected(_webSocket.Object);

			var dic = Reflection.GetObjectInFiled<ConcurrentDictionary<WebSocket, User>, ChatMessageHandler>(
				_handler, "_users", BindingFlags.NonPublic | BindingFlags.Instance);

			Assert.That(dic.Count > 0);

			await _handler.OnDisconnected(_webSocket.Object);

			Assert.That(dic.Count == 0);
		}

		[Test]
		public async Task OnConnected_SocketGetsAddedAndDeleted()
		{
			_webSocket.Setup(x => x.State).Returns(WebSocketState.Open);

			await _handler.OnConnected(_webSocket.Object);

			var cm = Reflection.GetObjectInFiled<ConnectionManager,WebSocketHandler>(
				_handler, "<WebSocketConnectionManager>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

			Assert.That(cm.GetAll().Count > 0);

			await _handler.OnDisconnected(_webSocket.Object);

			Assert.That(cm.GetAll().Count == 0);
		}

		[Test]
		public async Task Reply_DoesNotThrowReplying()
		{
			_webSocket.Setup(x => x.State).Returns(WebSocketState.Open);

			await _handler.OnConnected(_webSocket.Object);

			Assert.DoesNotThrowAsync(async () =>
			{
				await _handler.Reply(_webSocket.Object, new("username", null!, MessageType.Connection));
				await _handler.Reply(_webSocket.Object, new("username", "hello", MessageType.Chat));
			});

			await _handler.OnDisconnected(_webSocket.Object);
		}
	}
}
