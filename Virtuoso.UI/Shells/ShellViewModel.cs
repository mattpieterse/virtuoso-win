using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
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

    /// <summary>
    /// Constructor for <see cref="ShellViewModel"/>
    /// </summary>
    public ShellViewModel() {
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
    private static IReadOnlyCollection<INavigationViewItem> GetNavigationHeaderItems() {
        return [
            new NavigationViewItem {
                Content = "Home",
                TargetPageType = typeof(HomeView),
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Home24
                }
            },
            new NavigationViewItem {
                Content = "Decks",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Grid24
                },
                MenuItems = {
                    new NavigationViewItem {
                        Content = "Debug 3",
                        TargetPageType = typeof(DeckView),
                        TargetPageTag = "XYZ",
                        Icon = new SymbolIcon {
                            Symbol = SymbolRegular.Code24
                        }
                    },
                    new NavigationViewItem {
                        Content = "Debug 4",
                        TargetPageType = typeof(DeckView),
                        TargetPageTag = "ZYX",
                        Icon = new SymbolIcon {
                            Symbol = SymbolRegular.Code24
                        }
                    }
                }
            },
            new NavigationViewItem {
                Content = "Debug 1",
                TargetPageType = typeof(DeckView),
                TargetPageTag = "XXX",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Code24
                }
            },
            new NavigationViewItem {
                Content = "Debug 2",
                TargetPageType = typeof(DeckView),
                TargetPageTag = "YYY",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Code24
                }
            }
        ];
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
