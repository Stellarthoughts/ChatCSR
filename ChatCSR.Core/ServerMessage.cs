using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.Core
{
	public class ServerMessage
	{
		public MessageType Type { get; set; }
		public List<User> Users { get; set; }
		public List<string> Content { get; set; }

		public ServerMessage(List<User> users, List<string> content, MessageType type)
		{
			Type = type;
			Users = users;
			Content = content;
		}
	}
}
