using System;
using Kx.Toolx.AvaUi.ViewModels;
using ReactiveUI;

namespace Kx.Toolx.AvaUi.Views;

public class MainViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        FirstViewModel context => new FirstView { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}
