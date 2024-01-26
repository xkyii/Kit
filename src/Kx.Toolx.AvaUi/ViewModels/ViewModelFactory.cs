using System;
using Microsoft.Extensions.DependencyInjection;

namespace Kx.Toolx.AvaUi.ViewModels;

internal class ViewModelFactory<T>(IServiceProvider sp)
    where T : ViewModelBase
{
    public T GetViewModel()
    {
        var vm = sp.GetRequiredService<T>();
        vm.Initialize();
        return vm;
    }
}
