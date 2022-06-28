namespace ChatCSR.Core
{
	public class ClientMessage
	{
		public MessageType Type { get; set; }
		public string Sender { get; set; }
		public string Content { get; set; }

		public ClientMessage(string sender, string content, MessageType type)
		{
			Type = type;
			Sender = sender;
			Content = content;
		}
	}
}