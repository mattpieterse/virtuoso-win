using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using ReactiveUI;

namespace Virtuoso.UI.Views.Decks.Components;

/// <summary>
/// Component for the <see cref="BoardControl"/>
/// </summary>
/// <seealso cref="DeckView"/>
public sealed partial class BoardItemControl
{
#region Variables

    public static readonly DependencyProperty ItemStateProperty = DependencyProperty.Register(
        nameof(ItemState),
        typeof(BoardItemState),
        typeof(BoardItemControl),
        new PropertyMetadata(
            propertyChangedCallback: OnItemStateChanged,
            defaultValue: null
        )
    );


    public BoardItemState? ItemState
    {
        get => (BoardItemState?) GetValue(ItemStateProperty);
        init => SetValue(ItemStateProperty, value);
    }

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="BoardItemControl"/>
    /// </summary>
    public BoardItemControl() {
        InitializeComponent();
        this.WhenActivated((disposables) => {
            var onItemMouseEnter = Observable
                .FromEventPattern(CardElement, nameof(CardElement.MouseEnter));

            var onItemMouseLeave = Observable
                .FromEventPattern(CardElement, nameof(CardElement.MouseLeave));

            var onPlaceholderMouseEnter = Observable
                .FromEventPattern(PlaceholderCard, nameof(PlaceholderCard.MouseEnter));

            var onPlaceholderMouseLeave = Observable
                .FromEventPattern(PlaceholderCard, nameof(PlaceholderCard.MouseLeave));

            var onMenuMouseEnter = Observable
                .FromEventPattern(Actions, nameof(Actions.MouseEnter));

            var onMenuMouseLeave = Observable
                .FromEventPattern(Actions, nameof(Actions.MouseLeave));

            onItemMouseEnter.Merge(onMenuMouseEnter)
                .Subscribe(_ => AnimateOpacity(Actions, OpacityProperty, 1.0))
                .DisposeWith(disposables);

            onItemMouseLeave.Merge(onMenuMouseLeave)
                .Subscribe(_ => AnimateOpacity(Actions, OpacityProperty, 0.0))
                .DisposeWith(disposables);

            onPlaceholderMouseEnter
                .Subscribe(_ => AnimateOpacity(PlaceholderIcon, OpacityProperty, 0.2, instant: true))
                .DisposeWith(disposables);

            onPlaceholderMouseLeave
                .Subscribe(_ => AnimateOpacity(PlaceholderIcon, OpacityProperty, 0.0, instant: true))
                .DisposeWith(disposables);

            this.WhenAnyValue(p => p.CardElement.ActualHeight)
                .Subscribe(size => {
                    var shouldShowText = ((size > 100) && !string.IsNullOrWhiteSpace(TextElement.Text));
                    TextLayout.Visibility = shouldShowText
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                })
                .DisposeWith(disposables);

            var commandParameter = this
                .WhenAnyValue(x => x.ItemState)
                .Select(state => state?.ItemId);

            this.BindCommand(
                ViewModel,
                bind => bind.UpdateItemCommand,
                view => view.UpdateAction,
                commandParameter
            ).DisposeWith(disposables);

            this.BindCommand(
                ViewModel,
                bind => bind.DeleteItemCommand,
                view => view.DeleteAction,
                commandParameter
            ).DisposeWith(disposables);
        });
    }

#endregion

#region Internals

    /// <summary>
    /// Configures the item component according to its state.
    /// </summary>
    private void UpdateItemDisplay() {
        if (ItemState == null) {
            CardElement.Visibility = Visibility.Collapsed;
            PlaceholderCard.Visibility = Visibility.Visible;
            Actions.Visibility = Visibility.Collapsed;
            return;
        }

        var isConfigured = ItemState.IsConfigured;

        CardElement.Visibility = isConfigured
            ? Visibility.Visible
            : Visibility.Collapsed;

        PlaceholderCard.Visibility = isConfigured
            ? Visibility.Collapsed
            : Visibility.Visible;

        Actions.Visibility = isConfigured
            ? Visibility.Visible
            : Visibility.Collapsed;

        if (!isConfigured) return;
        IconElement.Symbol = ItemState.Icon?.Symbol ?? default;
        TextElement.Text = ItemState.Text;
    }


    /// <summary>
    /// Animates the value of the opacity property of an element.
    /// </summary>
    private static void AnimateOpacity(
        UIElement targetElement,
        DependencyProperty property,
        double targetValue,
        bool instant = false
    ) {
        if (instant) {
            targetElement.Opacity = targetValue;
            return;
        }

        var animation = new DoubleAnimation {
            To = targetValue,
            Duration = TimeSpan.FromMilliseconds(200),
            EasingFunction = new CubicEase {
                EasingMode = EasingMode.EaseInOut
            }
        };

        targetElement.BeginAnimation(property, animation, HandoffBehavior.Compose);
    }


    private static void OnItemStateChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    ) {
        if (d is BoardItemControl control) {
            control.UpdateItemDisplay();
        }
    }

#endregion
}
