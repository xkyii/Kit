using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kx.Toolx.AvaUi.ViewModels;
using ReactiveUI;

namespace Kx.Toolx.AvaUi.Views;

public partial class FirstView : ReactiveUserControl<FirstViewModel>
{
    public FirstView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
