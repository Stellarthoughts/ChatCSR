using ChatCSR.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatCSR.ServerLogic.DB
{
	public class Repository : IRepository
	{
		private readonly ChatContext _chatContext = null!;

		public Repository(IServiceProvider serviceProvider)
		{
			var context = new ChatContext(
				serviceProvider.GetRequiredService<DbContextOptions<ChatContext>>());
			_chatContext = context;
			DBContextSeeder.Seed(_chatContext);
		}

		public ChatContext GetContext() => _chatContext;

		private DbSet<MessageEntity> Messages => _chatContext.MessageEntities!;

		public IEnumerable<MessageEntity> GetAll()
		{
			return Messages.ToList();
		}

		public MessageEntity GetById(int MessageID)
		{
			return Messages.Single(x => x.MessageID == MessageID);
		}

		public void Insert(MessageEntity message)
		{
			Messages.Add(message);
		}

		public void Update(MessageEntity message)
		{
			//
		}

		public void Delete(int MessageID)
		{
			MessageEntity msg = Messages.Single(x => x.MessageID == MessageID);
			Messages.Remove(msg);
		}

		public async Task Save() => await _chatContext.SaveChangesAsync();
	}
}
