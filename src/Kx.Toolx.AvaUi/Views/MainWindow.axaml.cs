using FluentAvalonia.UI.Windowing;

namespace Kx.Toolx.AvaUi.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Title = "Kx.Toolx.AvaUi";
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }
}
