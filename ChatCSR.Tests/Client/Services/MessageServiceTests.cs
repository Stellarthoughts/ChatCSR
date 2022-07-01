using ChatCSR.Core;
using ChatCSR.DesktopClient.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.WebSockets;
using System.Reflection;

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
		public void InterpretMessage_Connection()
		{
			var eventCalled = 0;
			_messageService.OnConnected += (o, e) => eventCalled += 1;
			_messageService.OnUser += (o, e) => eventCalled += 1;
			_messageService.OnMessage += (o, e) => eventCalled += 1;
			Reflection.CallPrivateMethodVoid(_messageService, "InterpretMessage",
				BindingFlags.NonPublic | BindingFlags.Instance,
				JsonConvert.SerializeObject(new ServerMessage(new(),new(),MessageType.Connection)));
			Assert.That(eventCalled == 3);
		}

		[Test]
		public void InterpretMessage_Chat()
		{
			var eventCalled = 0;
			_messageService.OnMessage += (o, e) => eventCalled += 1;
			Reflection.CallPrivateMethodVoid(_messageService, "InterpretMessage",
				BindingFlags.NonPublic | BindingFlags.Instance,
				JsonConvert.SerializeObject(new ServerMessage(new(), new(), MessageType.Chat)));
			Assert.That(eventCalled == 1);
		}

		[Test]
		public void InterpretMessage_User()
		{
			var eventCalled = 0;
			_messageService.OnNewUser += (o, e) => eventCalled += 1;
			Reflection.CallPrivateMethodVoid(_messageService, "InterpretMessage",
				BindingFlags.NonPublic | BindingFlags.Instance,
				JsonConvert.SerializeObject(new ServerMessage(new(), new(), MessageType.User)));
			Assert.That(eventCalled == 1);
		}

		[Test]
		public void InterpretMessage_UserLeft()
		{
			var eventCalled = 0;
			_messageService.OnUserLeft += (o, e) => eventCalled += 1;
			Reflection.CallPrivateMethodVoid(_messageService, "InterpretMessage",
				BindingFlags.NonPublic | BindingFlags.Instance,
				JsonConvert.SerializeObject(new ServerMessage(new(), new(), MessageType.UserLeft)));
			Assert.That(eventCalled == 1);
		}
	}
}
