using ChatCSR.Core;
using ChatCSR.DesktopClient.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChatCSR.DesktopClient.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		#region Fields
		private string _serverIP = string.Empty;
		private string _serverPort = string.Empty;
		private string _username = string.Empty;
		private string _connectionStatus = string.Empty;

		private string _messageInput = string.Empty;

		private ObservableCollection<string> _messageList = new();
		private ObservableCollection<string> _userList = new();

		private readonly MessageService _messageService;
		#endregion

		#region Properties
		public string ServerIP
		{
			get => _serverIP;
			set => SetProperty(ref _serverIP, value);
		}
		public string ServerPort
		{
			get => _serverPort;
			set => SetProperty(ref _serverPort, value);
		}
		public string Username
		{
			get => _username;
			set => SetProperty(ref _username, value);
		}
		public string ConnectionStatus
		{
			get => _connectionStatus;
			set => SetProperty(ref _connectionStatus, value);
		}
		public ObservableCollection<string> MessageList
		{
			get => _messageList;
			set => SetProperty(ref _messageList, value);
		}
		public ObservableCollection<string> UserList
		{
			get => _userList;
			set => SetProperty(ref _userList, value);
		}
		public string MessageInput
		{
			get => _messageInput;
			set => SetProperty(ref _messageInput, value);
		}
		#endregion

		#region Commands
		public DelegateCommand SendMessageCommand { get; private set; }
		public DelegateCommand ConnectCommand { get; private set; }
		#endregion

		#region Constructor
		public MainWindowViewModel()
		{
			ServerIP = "ws://localhost";
			ServerPort = "5000";
			Username = "username";
			ConnectionStatus = "Not connected";
			MessageInput = "message";
			SendMessageCommand = new(SendMessage);
			ConnectCommand = new(Connect);
			_messageService = new MessageService();
			_messageService.OnMessage += OnMessage;
			_messageService.OnConnected += OnConnected;
			_messageService.OnBadConnection += OnBadConnection;
			_messageService.OnBadMessageSend += OnBadMessageSend;
			_messageService.OnUser += OnUser;
			_messageService.OnUserLeft += OnUserLeft;
			_messageService.OnNewUser += OnNewUser;
		}
		#endregion

		#region Methods
		private void SendMessage()
		{
			_messageService.SendMessage(MessageInput, MessageType.Chat);
		}

		private void Connect()
		{
			MessageList.Clear();
			UserList.Clear();
			ConnectionStatus = "Connecting...";
			_messageService.Connect(ServerIP, ServerPort, Username);
		}

		private void OnMessage(object? sender, List<MessageEntity> messages)
		{
			messages.Sort((a, b) => a.Time > b.Time ? 1 : -1);
			messages.ForEach(x => MessageList.Add($"{x.Sender}: {x.Content}"));
		}

		private void OnUser(object? sender, List<User> users)
		{
			users.ForEach(x =>
			{
				UserList.Add(x.Name!);
			});
		}

		private void OnNewUser(object? sender, List<User> users)
		{
			users.ForEach(x =>
			{
				UserList.Add(x.Name!);
				MessageList.Add($"{x.Name} entered the chat.");
			});
		}

		private void OnUserLeft(object? sender, List<User> users)
		{
			users.ForEach(x => UserList.Remove(x.Name!));
		}

		private void OnConnected(object? sender, EventArgs args)
		{
			ConnectionStatus = "Connection established";
		}

		private void OnBadConnection(object? sender, EventArgs e)
		{
			ConnectionStatus = "Connection failed";
		}
		private void OnBadMessageSend(object? sender, EventArgs e)
		{
			ConnectionStatus = "Messaging failed";
		}
		#endregion
	}
}
