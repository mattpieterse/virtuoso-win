using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using Virtuoso.UI.Core.Models.Decorators;

namespace Virtuoso.UI.Views.Decks.Components;

/// <summary>
/// Component for the <see cref="DeckView"/>
/// </summary>
public sealed partial class BoardControl
{
#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="BoardControl"/>
    /// </summary>
    public BoardControl() {
        InitializeComponent();
        this.WhenActivated((disposables) => {
            ViewModel
                .WhenAnyValue(p => p.BoardItems)
                .DistinctUntilChanged()
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(_ => BuildContentGrid())
                .DisposeWith(disposables);

            ViewModel
                .WhenAnyValue(p => p.GridCols, p => p.GridRows)
                .DistinctUntilChanged()
                .ObserveOn(RxSchedulers.MainThreadScheduler)
                .Subscribe(_ => BuildContentGrid())
                .DisposeWith(disposables);

            BuildContentGrid();
        });
    }

#endregion

#region Internals

    /// <summary>
    /// TODO
    /// </summary>
    private void BuildContentGrid() {
        ClearContentGrid();
        ArgumentNullException.ThrowIfNull(ViewModel);
        var cols = ViewModel.GridCols;
        var rows = ViewModel.GridRows;
        const double spacing = 6;

        for (var i = 0; i < cols; i++) {
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition {
                Width = new GridLength(1, GridUnitType.Star)
            });

            if (i < cols - 1) {
                ContentGrid.ColumnDefinitions.Add(new ColumnDefinition {
                    Width = new GridLength(spacing)
                });
            }
        }

        for (var i = 0; i < rows; i++) {
            ContentGrid.RowDefinitions.Add(new RowDefinition {
                Height = new GridLength(1, GridUnitType.Star)
            });

            if (i < rows - 1) {
                ContentGrid.RowDefinitions.Add(new RowDefinition {
                    Height = new GridLength(spacing)
                });
            }
        }

        foreach (
            var item in ViewModel.BoardItems
        ) {
            var col = item.GridPosition % cols;
            var row = item.GridPosition / cols;

            var control = new AspectLayout {
                AspectRatio = 1,
                Child = new BoardItemControl {
                    ViewModel = ViewModel,
                    ItemState = item
                }
            };

            Grid.SetColumn(control, col * 2);
            Grid.SetRow(control, row * 2);

            ContentGrid.Children.Add(control);
        }
    }


    /// <summary>
    /// TODO
    /// </summary>
    private void ClearContentGrid() {
        ContentGrid.ColumnDefinitions.Clear();
        ContentGrid.RowDefinitions.Clear();
        ContentGrid.Children.Clear();
    }

#endregion
}
