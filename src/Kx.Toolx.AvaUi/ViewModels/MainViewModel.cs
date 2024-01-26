using System.Reactive;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Kx.Toolx.AvaUi.ViewModels;

public class MainViewModel : ViewModelBase, IScreen
{
    private readonly ILogger? _logger = Locator.Current.GetService<ILogger<MainViewModel>>();

    public MainViewModel()
    {
        CheckBindingCommand = ReactiveCommand.Create(CheckBinding);
        GoNext = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new FirstViewModel(this))
        );
    }

    public RoutingState Router { get; } = new();

    // The command that navigates a user to first view model.
    public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => Router.NavigateBack;

    public string Greeting { get; set; } = "Welcome to Avalonia!";

    #region Commands

    public ReactiveCommand<Unit, Unit>? CheckBindingCommand { get; private set; }
    private void CheckBinding()
    {
        _logger?.LogInformation("Greeting: {Greeting}", Greeting);
    }

    #endregion

}
