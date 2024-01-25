using FluentAvalonia.UI.Windowing;
using Kx.Toolx.AvaUi.ViewModels;

namespace Kx.Toolx.AvaUi.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }
}
