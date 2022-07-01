using Moq;
using NUnit.Framework;
using System.Net.WebSockets;

namespace ChatCSR.Tests.ServerLogic.Managers
{
	public class ConnectionManagerTests
	{
		private Mock<WebSocket> _webSocket = null!;

		[SetUp]
		public void Setup()
		{
			_webSocket = new();
		}

		[Test]
		public void Test()
		{

		}
	}
}
