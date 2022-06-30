using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.ServerLogic.DB
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
		
		public MessageEntity(string content)
		{
			Content = content;
		}
	}
}
