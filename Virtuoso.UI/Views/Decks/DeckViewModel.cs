using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Virtuoso.UI.Core.Models.Intents;
using Virtuoso.UI.Core.Services.Appearance.Toasts;
using Virtuoso.UI.Views.Decks.Components;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Views.Decks;

/// <summary>
/// View model for <see cref="DeckView"/>
/// </summary>
public sealed partial class DeckViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();


    [Reactive]
    private int _selectedIndex = 1;


    [Reactive]
    private ObservableCollection<MenuItem> _pagerOptions;


    private ReactiveCommand<int, Unit> SelectPageCommand { get; }

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="DeckViewModel"/>
    /// </summary>
    public DeckViewModel() {
        SelectPageCommand = ReactiveCommand
            .Create<int>(i => SelectedIndex = i);

        PagerOptions = new ObservableCollection<MenuItem>(GetPagerOptions());

        this.WhenActivated((disposables) => {
            this.WhenAnyValue(x => x.SelectedIndex)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(selectedIndex => {
                    for (var i = 0; i < _pagerOptions.Count; i++) {
                        var pageNumber = (i + 1);
                        var isSelected = pageNumber == selectedIndex;

                        PagerOptions[i].FontWeight = isSelected
                            ? FontWeight.FromOpenTypeWeight(750)
                            : FontWeight.FromOpenTypeWeight(400);
                    }
                })
                .DisposeWith(disposables);
        });
    }

#endregion

#region Internals

    /// <summary>
    /// Constructs and returns the menu items for the pagination component.
    /// </summary>
    /// <seealso cref="PagerControl"/>
    /// <seealso cref="MenuItem"/>
    private List<MenuItem> GetPagerOptions() {
        var collection = new List<MenuItem>();
        for (var i = 0; i < 9; i++) {
            var page = (i + 1);
            collection.Add(
                new MenuItem {
                    Header = page.ToString(),
                    Command = SelectPageCommand,
                    CommandParameter = page
                }
            );
        }

        collection.Add(
            new MenuItem {
                IsEnabled = false,
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Add24
                }
            }
        );

        return collection;
    }

#endregion
}
