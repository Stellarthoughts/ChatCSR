using ChatCSR.Core;
using Microsoft.EntityFrameworkCore;

namespace ChatCSR.ServerLogic.DB
{
	public class ChatContext : DbContext
	{
		public DbSet<MessageEntity>? MessageEntities { get; set; }

		public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }
	}
}
