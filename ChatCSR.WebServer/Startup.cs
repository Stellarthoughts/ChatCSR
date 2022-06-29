using ChatCSR.ServerLogic.Handlers;

namespace ChatCSR.WebServer
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddWebSocketManager();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
			var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

			app.UseWebSockets();
			app.MapWebSocketManager("/message", serviceProvider.GetService<ChatMessageHandler>()!);
			app.UseStaticFiles();
		}
	}
}
