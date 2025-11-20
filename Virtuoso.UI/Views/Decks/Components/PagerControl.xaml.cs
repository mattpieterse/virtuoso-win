using System.Reactive.Disposables.Fluent;
using ReactiveUI;

namespace Virtuoso.UI.Views.Decks.Components;

/// <summary>
/// Component for the <see cref="DeckView"/>
/// </summary>
public sealed partial class PagerControl
{
#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="PagerControl"/>
    /// </summary>
    public PagerControl() {
        InitializeComponent();
        this.WhenActivated((disposables) => {
            this.OneWayBind(
                ViewModel,
                bind => bind.PagerOptions,
                view => view.Paginator.ItemsSource
            ).DisposeWith(disposables);
        });
    }

#endregion
}
