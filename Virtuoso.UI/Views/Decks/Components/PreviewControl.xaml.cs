using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using ReactiveUI;
using Virtuoso.UI.Core.Models.Decorators;
using Virtuoso.UI.Views.Decks.Dialogs;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Views.Decks.Components;

/// <summary>
/// Component for the <see cref="InsertContentDialog"/>
/// </summary>
public sealed partial class PreviewControl
{
#region Lifecycle

    /// <summary>
    /// Constructor for the <see cref="PreviewControl"/>
    /// </summary>
    public PreviewControl() {
        InitializeComponent();
        this.WhenActivated((disposables) => {
            this.WhenAnyValue(
                    p => p.ViewModel!.GridSizeW,
                    p => p.ViewModel!.GridSizeH
                )
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => RebuildPreview())
                .DisposeWith(disposables);

            RebuildPreview();
        });
    }

#endregion

#region Internals

    /// <summary>
    /// Rebuilds the preview grid based on current width and height values.
    /// </summary>
    private void RebuildPreview() {
        ClearPreview();
        var cols = ViewModel?.GridSizeW ?? 0;
        var rows = ViewModel?.GridSizeH ?? 0;
        if (cols <= 0 || rows <= 0) {
            return;
        }

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

        for (var row = 0; row < rows; row++) {
            for (var col = 0; col < cols; col++) {
                var card = new CardAction {
                    IsChevronVisible = false,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    MaxHeight = 80,
                    MaxWidth = 80,
                    MinHeight = 0,
                    MinWidth = 0
                };

                var control = new AspectLayout {
                    AspectRatio = 1,
                    Child = card
                };

                Grid.SetColumn(control, col * 2);
                Grid.SetRow(control, row * 2);

                ContentGrid.Children.Add(control);
            }
        }
    }


    /// <summary>
    /// Clears all grid definitions and children.
    /// </summary>
    [UsedImplicitly]
    private void ClearPreview() {
        ContentGrid.ColumnDefinitions.Clear();
        ContentGrid.RowDefinitions.Clear();
        ContentGrid.Children.Clear();
    }

#endregion
}
