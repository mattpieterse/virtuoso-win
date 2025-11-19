using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using Virtuoso.UI.Shells;
using Wpf.Ui.Abstractions.Controls;

namespace Virtuoso.UI.Views.Decks;

/// <summary>
/// Backing class for a view within the <see cref="Shell"/>
/// </summary>
public sealed partial class DeckView
    : IViewFor<DeckViewModel>, INavigableView<DeckViewModel>
{
#region Variables

    [AllowNull]
    public DeckViewModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (DeckViewModel?) value;
    }

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="DeckView"/>
    /// </summary>
    public DeckView(
        DeckViewModel viewModel
    ) {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
        this.WhenActivated((disposables) => {
            this.Bind(
                ViewModel,
                bind => bind.DebugText,
                view => view.DebugText.Content
            ).DisposeWith(disposables);
        });
    }

#endregion
}
