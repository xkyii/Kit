using FluentAvalonia.UI.Windowing;

namespace Kx.Toolx.AvaUi;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Title = "ffff";
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }
}
