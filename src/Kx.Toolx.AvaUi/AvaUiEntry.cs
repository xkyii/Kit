using System;
using Kx.Toolx.AvaUi.ViewModels;
using Kx.Toolx.AvaUi.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Kx.Toolx.AvaUi;

public static class AvaUiEntry
{
    public static void AddAvaUi(this IServiceCollection services)
    {
        services.AddSingleton(typeof(ViewModelFactory<>));
        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();
    }
}

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
