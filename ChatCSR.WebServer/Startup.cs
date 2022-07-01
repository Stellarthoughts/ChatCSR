using ChatCSR.ServerLogic.DB;
using ChatCSR.ServerLogic.Handlers;
using Microsoft.EntityFrameworkCore;

namespace ChatCSR.WebServer
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			Directory.CreateDirectory("C:\\temp\\database\\");
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddWebSocketManager();
			services.AddDbContext<ChatContext>(options =>
			{
				options.UseSqlite(Configuration.GetConnectionString("Default"));
			});
			services.AddScoped<IRepository, Repository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			app.UseWebSockets();
			app.MapWebSocketManager("/message", serviceProvider.GetService<ChatMessageHandler>()!);
			app.UseStaticFiles();
		}

	}
}
