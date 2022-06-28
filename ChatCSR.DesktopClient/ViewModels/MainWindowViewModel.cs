using ChatCSR.Core;
using ChatCSR.DesktopClient.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.DesktopClient.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		#region Fields
		private string _serverIP = "";
		private string _serverPort = "";
		private string _username = "";
		private string _connectionStatus = "";

		private string _messageInput = "";

		private List<string> _messageList = new();
		private List<string> _userList = new();

		private MessageService _messageService; 
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
		public List<string> MessageList
		{
			get => _messageList;
			set => SetProperty(ref _messageList, value);
		}
		public List<string> UserList
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
		}
		#endregion

		#region Methods
		private void SendMessage()
		{
			_messageService.SendMessage(MessageInput, MessageType.Chat);
		}

		private void Connect()
		{
			ConnectionStatus = "Connecting...";
			_messageService.Connect(ServerIP, ServerPort, Username);
		}

		private void OnMessage(object? sender, List<string> messages)
		{
			messages.ForEach(x => MessageList.Add(x));
		}

		private void OnConnected(object? sender, EventArgs e)
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
