using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Kx.Toolx.AvaUi.ViewModels;
using Microsoft.Extensions.Logging;
using Splat;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Kx.Toolx.AvaUi.Views;

public partial class MainView : UserControl
{
    #region Injection

    private readonly ILogger? _logger = Locator.Current.GetService<ILogger<MainView>>();

    #endregion

    public MainView()
    {
        InitializeComponent();
        if (!Design.IsDesignMode)
        {
            DataContext = Locator.Current.GetService<MainViewModel>();
        }
    }

    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        _logger?.LogInformation("NavigationView_OnSelectionChanged");
    }
}
