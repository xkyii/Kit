using System.Reactive;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Kx.Toolx.AvaUi.ViewModels;

public class MainViewModel : ViewModelBase
{

    private readonly ILogger logger;

    public MainViewModel(ILogger<MainViewModel> logger)
    {
        this.logger = logger;
        CheckBindingCommand = ReactiveCommand.Create(CheckBinding);
    }

    public string Greeting { get; set; } = "Welcome to Avalonia!";

    #region Commands

    public ReactiveCommand<Unit, Unit> CheckBindingCommand { get; }
    private void CheckBinding()
    {
        logger.LogInformation("Greeting: {Greeting}", Greeting);
    }

    #endregion
}
