using System.Drawing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Windows.Win32.Foundation;

using static Windows.Win32.PInvoke;


namespace Kx.Toolx.Clicky;

internal class HostedService : BackgroundService
{
	private readonly ILogger _logger;
	private readonly ClickyConfig _config;
	private Timer? _timer;

	public HostedService(ILogger<HostedService> logger, IOptions<ClickyConfig> options)
	{
		_logger = logger;
		_config = options.Value;

		_logger.LogInformation(_config.ToString());
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_config.TimerPeriod));
		return Task.CompletedTask;
	}

	private async void DoWork(object? state)
	{
		var point = RandomPoint();
		var hwnd = GetConfigHwnd();

		_logger.LogInformation($"点击 - 0x{hwnd.Value.ToString("X8")} - ({point.X}, {point.Y})");

		var pos = (LPARAM)(nint)(uint)(point.X | point.Y << 16);
		SendMessage(hwnd, WM_LBUTTONDOWN, (WPARAM)0, pos);
		await Task.Delay(50);
		SendMessage(hwnd, WM_LBUTTONUP, (WPARAM)0, pos);
	}

	private Point RandomPoint()
	{
		var x = (new Random().Next() % _config.Points.Count());
		//_logger.LogInformation($"x: {x}");
		//foreach (var p in _config.Points)
		//{
		//	_logger.LogInformation($"X: {p.X}, Y: {p.Y}");
		//}
		return _config.Points[x];
	}

	private HWND GetConfigHwnd()
	{
		if (_config == null || string.IsNullOrEmpty(_config.Hwnd))
		{
			return HWND.Null;
		}


		// 十六进制
		if (_config.Hwnd.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
		{
			if (long.TryParse(_config.Hwnd.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out var h))
			{
				return (HWND)h;
			}
		}

		// 十进制
		else 
		{
			if (long.TryParse(_config.Hwnd, out var h))
			{
				return (HWND)h;
			}
		}

		return HWND.Null;
	}

	public override void Dispose()
	{
		base.Dispose();
		_timer?.Dispose();
	}
}
