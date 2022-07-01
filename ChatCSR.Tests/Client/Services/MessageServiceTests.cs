using ChatCSR.DesktopClient.Services;
using Moq;
using NUnit.Framework;
using System.Net.WebSockets;

namespace ChatCSR.Tests.Client.Services
{
	public class MessageServiceTests
	{
		private MessageService _messageService = null!;

		[SetUp]
		public void Setup()
		{
			_messageService = new();
		}

		[Test]
		public void Test()
		{

		}
	}
}
