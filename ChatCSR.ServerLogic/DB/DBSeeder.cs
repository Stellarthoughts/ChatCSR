using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.ServerLogic.DB
{
	public static class DBContextSeeder
	{
		public static void Seed(Repository context)
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
			context.SaveChanges();
		}
	}
}
