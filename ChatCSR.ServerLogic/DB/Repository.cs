using ChatCSR.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatCSR.ServerLogic.DB
{
	public class Repository : IRepository
	{
		private ChatContext _chatContext = null!;
		private bool _disposed;

		public Repository()
		{
			_disposed = false;
		}

		public void Initialize(IServiceProvider serviceProvider)
		{
			using var context = new ChatContext(
				serviceProvider.GetRequiredService<DbContextOptions<ChatContext>>());
			_chatContext = context;
			//context.Database.Migrate();
			DBContextSeeder.Seed(_chatContext);
		}

		public ChatContext GetContext() => _chatContext;

		private List<MessageEntity> Messages => _chatContext.MessageEntities!.ToList();

		public IEnumerable<MessageEntity> GetAll()
		{
			return Messages;
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

		public void Save() => _chatContext.SaveChanges();

		private void Dispose(bool disposing)
		{
			if (!_disposed && disposing) _chatContext.Dispose();

			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
