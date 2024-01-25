using Avalonia.Controls;
using Kx.Toolx.AvaUi.ViewModels;

namespace Kx.Toolx.AvaUi.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel();

        NavigationView.SelectionChanged += NavigationView_SelectionChanged  ;
    }

    private void NavigationView_SelectionChanged(object? sender, FluentAvalonia.UI.Controls.NavigationViewSelectionChangedEventArgs e)
    {
        System.Console.WriteLine("On SelectionChanged");
    }
}
