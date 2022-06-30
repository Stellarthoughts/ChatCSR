using ChatCSR.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.ServerLogic.DB
{
	public class Repository : DbContext, IRepository
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(@"DataSource=mydatabase.db;");
		}

		public DbSet<MessageEntity>? MessageEntities { get; set; }

		public IEnumerable<MessageEntity> GetAll()
		{
			return MessageEntities!.ToList();
		}

		public MessageEntity GetById(int MessageID)
		{
			return MessageEntities!.Single(x => x.MessageID == MessageID);
		}

		public void Insert(MessageEntity message)
		{
			MessageEntities!.Add(message);
		}

		public void Update(MessageEntity message)
		{
			//
		}

		public void Delete(int MessageID)
		{
			MessageEntity msg = MessageEntities!.Single(x => x.MessageID == MessageID);
			MessageEntities!.Remove(msg);
		}

		public void Save() => SaveChanges();
	}
}
