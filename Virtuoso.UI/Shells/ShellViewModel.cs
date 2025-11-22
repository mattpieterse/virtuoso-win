using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Virtuoso.UI.Core.Caches;
using Virtuoso.UI.Views.Decks;
using Virtuoso.UI.Views.Home;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Shells;

/// <summary>
/// View model for <see cref="Shell"/>
/// </summary>
public sealed partial class ShellViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();


    [Reactive]
    private ObservableCollection<INavigationViewItem> _navigationHeaderItems;


    [Reactive]
    private ObservableCollection<INavigationViewItem> _navigationFooterItems;


    /// <summary>
    /// Command delegate for <see cref="ToggleThemes"/>
    /// </summary>
    private ReactiveCommand<Unit, Unit> ToggleThemesCommand { get; } = ReactiveCommand
        .Create(
            execute: ToggleThemes,
            canExecute: Observable
                .Return(
                    scheduler: RxApp.MainThreadScheduler,
                    value: true
                )
        );

#endregion

#region Lifecycle

    private readonly DeckCache _deckCache;


    /// <summary>
    /// Constructor for <see cref="ShellViewModel"/>
    /// </summary>
    public ShellViewModel(
        DeckCache deckCache
    ) {
        _deckCache = deckCache;
        NavigationHeaderItems = new ObservableCollection<INavigationViewItem>(GetNavigationHeaderItems());
        NavigationFooterItems = new ObservableCollection<INavigationViewItem>(GetNavigationFooterItems());
    }

#endregion

#region ICommands

    /// <summary>
    /// Toggles the theme to the inverse variant of the currently applied theme.
    /// </summary>
    /// <remarks>
    /// If the <see cref="Core.Services.Appearance.Theme.IThemeService.Listen()"/> method has not been invoked
    /// before this method is run, custom colour schemes will not automatically
    /// switch, only WPF-UI components.
    /// </remarks>
    private static void ToggleThemes() {
        ApplicationThemeManager.Apply(
            applicationTheme: ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark
                ? ApplicationTheme.Light
                : ApplicationTheme.Dark
        );
    }

#endregion

#region Internals

    /// <summary>
    /// Constructs and returns the navigation items for the sidebar header area.
    /// </summary>
    /// <seealso cref="NavigationViewItem"/>
    /// <seealso cref="NavigationView"/>
    private List<INavigationViewItem> GetNavigationHeaderItems() {
        var options = new List<INavigationViewItem> {
            new NavigationViewItem {
                Content = "Home",
                TargetPageType = typeof(HomeView),
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Home24
                }
            }
        };

        var context = _deckCache.FetchAll();
        options.AddRange(context
            .Select(item => new NavigationViewItem {
                    Content = item.Name,
                    TargetPageType = typeof(DeckView),
                    TargetPageTag = item.Id.ToString(),
                    Icon = new SymbolIcon {
                        Symbol = SymbolRegular.Code24
                    }
                }
            )
        );

        return options;
    }


    /// <summary>
    /// Constructs and returns the navigation items for the sidebar footer area.
    /// </summary>
    /// <seealso cref="NavigationViewItem"/>
    /// <seealso cref="NavigationView"/>
    private IReadOnlyCollection<INavigationViewItem> GetNavigationFooterItems() {
        return [
            new NavigationViewItem() {
                Content = "Themes",
                Command = ToggleThemesCommand,
                Icon = new SymbolIcon() {
                    Symbol = SymbolRegular.Color24
                }
            }
        ];
    }

#endregion
}
