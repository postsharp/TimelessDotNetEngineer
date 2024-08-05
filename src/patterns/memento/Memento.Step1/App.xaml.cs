// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NameGenerator;
using NameGenerator.Generators;

namespace Memento.Step1;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public sealed partial class App : Application
{
    public static IHost? Host { get; private set; }

    public App()
    {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(
                ( context, services ) =>
                {
                    // Add windows
                    services.AddSingleton<MainWindow>();

                    // Add root view models
                    services.AddSingleton<MainViewModel>();

                    // Add services
                    // [<snippet ServiceRegistration>]
                    services.AddSingleton<IMementoCaretaker, MementoCaretaker>();

                    // [<endsnippet ServiceRegistration>]
                    services.AddSingleton<IFishGenerator, FishGenerator>();
                    services.AddSingleton<GeneratorBase, RealNameGenerator>();
                } )
            .Build();
    }

    protected override async void OnStartup( StartupEventArgs e )
    {
        await Host!.StartAsync();

        var mainWindow = Host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup( e );
    }

    protected override async void OnExit( ExitEventArgs e )
    {
        await Host!.StopAsync();
        base.OnExit( e );
    }
}