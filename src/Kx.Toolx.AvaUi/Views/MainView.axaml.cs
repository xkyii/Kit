using Avalonia.Controls;
using Kx.Toolx.AvaUi.ViewModels;
using Splat;

namespace Kx.Toolx.AvaUi.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        if (!Design.IsDesignMode)
        {
            DataContext = Locator.Current.GetService<ViewModelFactory<MainViewModel>>()?.GetViewModel();
        }
    }
}
