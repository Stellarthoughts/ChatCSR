using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatCSR.Core
{
	[Table("Message")]
	public class MessageEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int MessageID { get; set; }

		[Required]
		[MaxLength(256)]
		public string Content { get; set; }

		[Required]
		[MaxLength(256)]
		public string Sender { get; set; }

		[Required]
		public DateTime Time { get; set; }

		public MessageEntity(string content, string sender)
		{
			Content = content;
			Sender = sender;
			Time = DateTime.Now;
		}
	}
}