using System.Reactive;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Kx.Toolx.AvaUi.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ILogger? _logger = Locator.Current.GetService<ILogger<MainViewModel>>();

    public MainViewModel()
    {
        CheckBindingCommand = ReactiveCommand.Create(CheckBinding);
    }

    public string Greeting { get; set; } = "Welcome to Avalonia!";

    #region Commands

    public ReactiveCommand<Unit, Unit>? CheckBindingCommand { get; private set; }
    private void CheckBinding()
    {
        _logger?.LogInformation("Greeting: {Greeting}", Greeting);
    }

    #endregion

}
