using ChatCSR.Core;
using ChatCSR.ServerLogic.DB;
using ChatCSR.ServerLogic.Handlers;
using ChatCSR.ServerLogic.Managers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.Tests.ServerLogic.Handlers
{
	public class ChatMessageHandlerTests
	{
		private Mock<WebSocket> _webSocket = null!;
		private Mock<ConnectionManager> _connectionManager = null!;
		private Mock<Repository> _repository = null!;
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
		public void TestReply()
		{
			Assert.Pass();
		}
	}
}
