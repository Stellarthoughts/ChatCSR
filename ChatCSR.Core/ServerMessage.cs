namespace ChatCSR.Core
{
	public class ServerMessage
	{
		public MessageType Type { get; set; }
		public List<User> Users { get; set; }
		public List<MessageEntity> Content { get; set; }

		public ServerMessage(List<User> users, List<MessageEntity> content, MessageType type)
		{
			Type = type;
			Users = users;
			Content = content;
		}
	}
}
