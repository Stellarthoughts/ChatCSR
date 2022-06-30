using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.ServerLogic.DB
{
	public interface IRepository
	{
        IEnumerable<MessageEntity> GetAll();
        MessageEntity GetById(int MessageID);
        void Insert(MessageEntity message);
        void Update(MessageEntity message);
        void Delete(int MessageID);
        void Save();
    }
}
