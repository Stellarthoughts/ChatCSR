using ServiceServer;

IHost host = Host.CreateDefaultBuilder(args)
	.UseWindowsService(options =>
	{
		options.ServiceName = "ChatCSR Server";
	})
	.ConfigureServices(services =>
	{
		services.AddHostedService<Worker>();
	})
	.Build();

await host.RunAsync();
