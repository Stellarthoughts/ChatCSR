using ChatCSR.Core;

namespace ChatCSR.ServerLogic.DB
{
	public interface IRepository
	{
		IEnumerable<MessageEntity> GetAll();
		MessageEntity GetById(int MessageID);
		void Insert(MessageEntity message);
		void Update(MessageEntity message);
		void Delete(int MessageID);
		Task Save();
	}
}
