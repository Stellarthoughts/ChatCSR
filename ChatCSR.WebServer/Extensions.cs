using ChatCSR.ServerLogic.Handlers;
using ChatCSR.ServerLogic.Managers;
using ChatCSR.ServerLogic.Middleware;
using System.Reflection;

namespace ChatCSR.WebServer
{
	public static class Extensions
	{
		public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
														PathString path,
														WebSocketHandler handler)
		{
			return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
		}

		public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
		{
			services.AddTransient<ConnectionManager>();
			services.AddTransient<ChatMessageHandler>();

			foreach (var type in Assembly.GetExecutingAssembly()!.ExportedTypes)
			{
				if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
				{
					services.AddSingleton(type);
				}
			}

			return services;
		}
	}
}
