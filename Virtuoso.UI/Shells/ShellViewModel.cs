using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Virtuoso.UI.Core.Caches;
using Virtuoso.UI.Core.Models.Database;
using Virtuoso.UI.Views.Decks;
using Virtuoso.UI.Views.Decks.Dialogs;
using Virtuoso.UI.Views.Home;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

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
                    scheduler: RxSchedulers.MainThreadScheduler,
                    value: true
                )
        );


    /// <summary>
    /// Command delegate for <see cref="CreateDeck"/>
    /// </summary>
    private ReactiveCommand<Unit, Unit> CreateDeckCommand { get; }

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
        CreateDeckCommand = ReactiveCommand
            .CreateFromTask(
                execute: async () => { await CreateDeck(); },
                canExecute: Observable
                    .Return(
                        scheduler: RxSchedulers.MainThreadScheduler,
                        value: true
                    )
            );

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
    /// before this method is run, custom color schemes will not automatically
    /// switch, only WPF-UI components.
    /// </remarks>
    private static void ToggleThemes() {
        ApplicationThemeManager.Apply(
            applicationTheme: ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark
                ? ApplicationTheme.Light
                : ApplicationTheme.Dark
        );
    }


    /// <summary>
    /// Invokes the <see cref="DeckInsertContentDialog"/> and creates a deck.
    /// </summary>
    private async Task CreateDeck() {
        var service = Ioc.Default
            .GetRequiredService<IContentDialogService>();

        var dialog = new DeckInsertContentDialog(service.GetDialogHost());
        var result = await dialog.ShowAsync();
        if (result is null) {
            return;
        }

        _deckCache.Insert(new Deck {
            Name = result.Name,
            Grid = new DeckGrid(
                colCount: result.GridSizeW,
                rowCount: result.GridSizeH
            )
        });

        NavigationHeaderItems = new ObservableCollection<INavigationViewItem>(GetNavigationHeaderItems());
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
                Content = "Create new deck",
                Command = CreateDeckCommand,
                Icon = new SymbolIcon() {
                    Symbol = SymbolRegular.AddSquare24
                }
            },
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
