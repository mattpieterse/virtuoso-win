using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Virtuoso.UI.Core.Caches;
using Virtuoso.UI.Core.Models.Database;
using Virtuoso.UI.Core.Models.Intents;
using Virtuoso.UI.Views.Decks.Components;
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


    private Deck _currentDeck;


    [Reactive]
    private int _selectedIndex = 0;


    [Reactive]
    private ObservableCollection<MenuItem> _pagerOptions;


    [Reactive]
    private ObservableCollection<BoardItemState> _boardItems = [];


    public ReactiveCommand<int, Unit> SelectPageCommand { get; }
    public ReactiveCommand<Guid, Unit> UpdateItemCommand { get; }
    public ReactiveCommand<Guid, Unit> DeleteItemCommand { get; }


    public int GridCols => _currentDeck.Grid.Cols;
    public int GridRows => _currentDeck.Grid.Rows;

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="DeckViewModel"/>
    /// </summary>
    public DeckViewModel(
        IMessageBus bus,
        DeckCache cache
    ) {
        var cache1 = cache;
        _currentDeck = DeckCacheSeeder.CreateEmptyDeck();

        var latestIntent = bus
            .Listen<DeckNavigationIntent>()
            .Replay(1)
            .RefCount();

        SelectPageCommand = ReactiveCommand
            .Create<int>(i => SelectedIndex = (i - 1));

        UpdateItemCommand = ReactiveCommand
            .Create<Guid>((id) => { System.Diagnostics.Debug.WriteLine($"Update command executed: {id}"); });

        DeleteItemCommand = ReactiveCommand
            .Create<Guid>((id) => {
                System.Diagnostics.Debug.WriteLine($"Delete command executed: {id}");
                cache.DeleteItem(
                    deckId: _currentDeck.Id,
                    itemId: id
                );

                RefreshBoardItems();
            });

        PagerOptions = new ObservableCollection<MenuItem>(GetPagerOptions());

        this.WhenActivated((disposables) => {
            latestIntent
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(intent => {
                    var cachedDeck = cache1
                        .FetchOne(targetId: new Guid(intent.Argument));

                    System.Diagnostics.Debug.WriteLine(intent.Argument);
                    ArgumentNullException.ThrowIfNull(cachedDeck);
                    _currentDeck = cachedDeck;

                    RefreshBoardItems();

                    this.RaisePropertyChanged(nameof(GridCols));
                    this.RaisePropertyChanged(nameof(GridRows));
                })
                .DisposeWith(disposables);

            this.WhenAnyValue(x => x.SelectedIndex)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(selectedIndex => {
                    for (var i = 0; i < _pagerOptions.Count; i++) {
                        var isSelected = i == selectedIndex;
                        PagerOptions[i].FontWeight = isSelected
                            ? FontWeight.FromOpenTypeWeight(750)
                            : FontWeight.FromOpenTypeWeight(400);
                    }
                })
                .DisposeWith(disposables);

            this.WhenAnyValue(x => x.SelectedIndex)
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(_ => RefreshBoardItems())
                .DisposeWith(disposables);

            RefreshBoardItems();
        });
    }

#endregion

#region Internals

    /// <summary>
    /// Sets the board items to the latest from the collection source.
    /// </summary>
    private void RefreshBoardItems() {
        var items = new ObservableCollection<BoardItemState>();
        var pageIndex = _selectedIndex;

        for (var position = 0; position < _currentDeck.Grid.SlotCount; position++) {
            var deckItem = _currentDeck.GetItem(pageIndex, position);
            var itemState = new BoardItemState(deckItem, position);
            items.Add(itemState);
        }

        BoardItems = new ObservableCollection<BoardItemState>(items);
    }


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
