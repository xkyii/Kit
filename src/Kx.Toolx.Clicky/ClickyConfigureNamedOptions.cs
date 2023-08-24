using System.Drawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Kx.Toolx.Clicky;

internal class ClickyConfigureNamedOptions : ConfigureNamedOptions<ClickyConfig>
{
	public ClickyConfigureNamedOptions(string? name, IConfiguration configuration)
		: base(name, cc => Bind(cc, configuration))
	{
	}

	private static void Bind(ClickyConfig config, IConfiguration configuration)
	{
		if (config == null)
		{
			return;
		}

		if (configuration == null)
		{
			return;
		}

		config.Hwnd = configuration[nameof(config.Hwnd)] ?? string.Empty;
		config.TimerPeriod = double.TryParse(configuration[nameof(config.TimerPeriod)], out var tp) ? tp : 5;
		config.Points = configuration.GetSection(nameof(config.Points))
			.GetChildren()
			.Select(p => new Point((int.TryParse(p["X"], out var x) ? x : 0), (int.TryParse(p["Y"], out var y) ? y : 0)))
			.ToList()
			;
	}
}
