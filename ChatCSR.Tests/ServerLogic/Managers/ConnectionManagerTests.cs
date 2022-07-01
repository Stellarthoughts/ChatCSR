using ChatCSR.ServerLogic.Managers;
using Moq;
using NUnit.Framework;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading.Tasks;

namespace ChatCSR.Tests.ServerLogic.Managers
{
	public class ConnectionManagerTests
	{
		private Mock<WebSocket> _webSocket = null!;
		private ConnectionManager _connectionManager = null!;

		[SetUp]
		public void Setup()
		{
			_webSocket = new();
			_connectionManager = new();
		}

		[Test]
		public async Task Socket_AddedAndRemoved()
		{
			_connectionManager.AddSocket(_webSocket.Object);

			var dic = Reflection.GetObjectInFiled<ConcurrentDictionary<string, WebSocket>, ConnectionManager>(
				_connectionManager, "_sockets", BindingFlags.NonPublic | BindingFlags.Instance);

			Assert.That(dic.Count > 0);

			await _connectionManager.RemoveSocket(_connectionManager.GetId(_webSocket.Object));

			Assert.That(dic.Count == 0);
		}

		[Test]
		public void GetSocketByIdAndGetId()
		{
			_connectionManager.AddSocket(_webSocket.Object);

			var id = _connectionManager.GetId(_webSocket.Object);
			Assert.NotNull(id);
			var sock = _connectionManager.GetSocketById(id);
			Assert.That(_webSocket.Object == sock);
		}
	}
}
