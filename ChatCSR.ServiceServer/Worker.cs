using System.Diagnostics;

namespace ServiceServer
{
	public class Worker : BackgroundService
	{
		private ProcessStartInfo? _processInfo;
		private Process? _process;

		public Worker()
		{
			SetupProcess();
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)

		{
			if (_processInfo != null)
				_process = Process.Start(_processInfo);
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			if (_process != null)
				_process.Kill();
			_process = null;
			_processInfo = null;
		}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

		private void SetupProcess()
		{
			var serverRelativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				@".\..\..\..\..\..\ChatCSR.WebServer\bin\Debug\net6.0\ChatCSR.WebServer.exe");
			var serverProcessPath = Path.GetFullPath(serverRelativePath);

			_processInfo = new ProcessStartInfo(serverProcessPath)
			{
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				CreateNoWindow = true,
				ErrorDialog = false,
				WindowStyle = ProcessWindowStyle.Hidden
			};
		}
	}
}