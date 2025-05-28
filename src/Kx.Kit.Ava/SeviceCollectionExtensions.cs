using Kx.Kit.Ava.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Kx.Kit.Ava;

public static class ServiceCollectionExtensions {
    public static void AddCommonServices(this IServiceCollection collection) {
        collection.AddTransient<MainWindowViewModel>();
    }
}
