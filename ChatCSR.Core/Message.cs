namespace ChatCSR.Core
{
	public class Message
	{
		public DateTime DateTime { get; set; }
		public Guid Id { get; set; }
		public Guid Origin { get; set; }
		public string Content { get; set; }

		public Message(Guid origin, string content)
		{
			DateTime = DateTime.Now;
			Id = Guid.NewGuid();
			Origin = origin;
			Content = content;
		}
	}
}