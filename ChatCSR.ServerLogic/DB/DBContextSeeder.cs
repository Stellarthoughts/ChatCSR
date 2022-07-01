namespace ChatCSR.ServerLogic.DB
{
	public static class DBContextSeeder
	{
		public static void Seed(ChatContext context)
		{
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
			context.SaveChanges();
		}
	}
}
