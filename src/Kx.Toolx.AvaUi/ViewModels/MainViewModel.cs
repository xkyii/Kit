using System.Reactive;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Kx.Toolx.AvaUi.ViewModels;

public class MainViewModel(ILogger<MainViewModel> logger) : ViewModelBase
{
    internal override void Initialize()
    {
        CheckBindingCommand = ReactiveCommand.Create(CheckBinding);
    }

    public string Greeting { get; set; } = "Welcome to Avalonia!";

    #region Commands

    public ReactiveCommand<Unit, Unit>? CheckBindingCommand { get; private set; }
    private void CheckBinding()
    {
        logger.LogInformation("Greeting: {Greeting}", Greeting);
    }

    #endregion

}
