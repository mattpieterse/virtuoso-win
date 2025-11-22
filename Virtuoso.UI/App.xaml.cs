using System.Globalization;
using System.Reflection;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using JetBrains.Annotations;
using Lepo.i18n.DependencyInjection;
using Lepo.i18n.Yaml;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Virtuoso.UI.Core.Caches;
using Virtuoso.UI.Core.Services.Appearance.Toasts;
using Virtuoso.UI.Shells;
using Virtuoso.UI.Views.Decks;
using Virtuoso.UI.Views.Home;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.DependencyInjection;
using IThemeService = Virtuoso.UI.Core.Services.Appearance.Theme.IThemeService;
using ThemeService = Virtuoso.UI.Core.Services.Appearance.Theme.ThemeService;

namespace Virtuoso.UI;

/// <summary>
/// The main entry-point for the application.
/// </summary>
public sealed partial class App
{
#region Lifecycle

    /// <summary>
    /// The main entry-point for the application.
    /// </summary>
    [UsedImplicitly]
    private void Construct(
        object sender,
        StartupEventArgs e
    ) {
        InjectServices();
        UseApplicationThemes();

        var deck1 = DeckCacheSeeder.CreateDemoDeck1();
        var deck2 = DeckCacheSeeder.CreateDemoDeck2();
        var cache = Ioc.Default.GetRequiredService<DeckCache>();
        cache.Insert(deck1);
        cache.Insert(deck2);

        Ioc.Default
            .GetRequiredService<Shell>()
            .Show();
    }

#endregion

#region Internals

#region Internals >> Injection

    /// <summary>
    /// Injects services into the <see cref="Ioc"/> container.
    /// </summary>
    /// <seealso cref="ConstructServices"/>
    private static void InjectServices() {
        Ioc.Default.ConfigureServices(
            serviceProvider: ConstructServices()
                .BuildServiceProvider()
        );
    }


    /// <summary>
    /// Constructs and returns the registered dependencies within this project
    /// with their lifetime configurations. The dependencies in this collection
    /// will be built and injected at runtime.
    /// </summary>
    /// <returns>
    /// <see cref="ServiceCollection"/>
    /// </returns>
    private static ServiceCollection ConstructServices() {
        var services = new ServiceCollection();

        // Services
        services
            .AddStringLocalizer((builder) => {
                    var app = Assembly.GetExecutingAssembly();
                    builder.FromYaml(app, "/Assets/Languages/Translations-en-US.yaml", new CultureInfo("en-US"));
                    builder.FromYaml(app, "/Assets/Languages/Translations-en-GB.yaml", new CultureInfo("en-GB"));
                    builder.SetCulture(CultureInfo.GetCultureInfo("en-GB"));
                }
            )
            .AddNavigationViewPageProvider()
            .AddSingleton<IContentDialogService, ContentDialogService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<ISnackbarService, SnackbarService>()
            .AddSingleton<IThemeService, ThemeService>()
            .AddSingleton<IToastService, ToastService>()
            .AddSingleton<IMessageBus, MessageBus>()
            .AddSingleton<IDeckCache, DeckCache>()
            .AddSingleton<DeckCache>();

        // MVVM
        services
            .AddScoped<ShellViewModel>()
            .AddScoped<Shell>()
            .AddTransient<HomeViewModel>()
            .AddTransient<HomeView>()
            .AddTransient<DeckViewModel>()
            .AddTransient<DeckView>();

        return services;
    }

#endregion

    /// <summary>
    /// Ensures that the custom <see cref="IThemeService"/> is observing changes
    /// and then sets the application theme to its current state to coerce valid
    /// synchronized initial states for custom theme resources.
    /// </summary>
    /// <remarks>
    /// This must be called after the service collection is built.
    /// </remarks>
    private static void UseApplicationThemes() {
        Ioc.Default
            .GetRequiredService<IThemeService>()
            .Listen();

        ApplicationThemeManager.Apply(
            ApplicationThemeManager.GetAppTheme()
        );
    }

#endregion
}
