namespace ChatCSR.ServerLogic.DB
{
	public static class DBContextSeeder
	{
		public static void Seed(ChatContext context)
		{
			context.Database.EnsureCreated();
			context.SaveChanges();
		}
	}
}
