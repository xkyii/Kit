using System;
using Kx.Toolx.AvaUi.ViewModels;
using Kx.Toolx.AvaUi.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Kx.Toolx.AvaUi;

public static class AvaUiEntry
{
    public static void AddAvaUi(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();
    }
}
