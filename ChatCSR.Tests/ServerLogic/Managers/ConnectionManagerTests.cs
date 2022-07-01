using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

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
		public void 
	}
}
