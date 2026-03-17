using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using ReactiveUI;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Views.Decks.Dialogs;

public sealed partial class ItemUpsertContentDialog
    : IViewFor<ItemUpsertContentModel>
{
#region Variables

    [AllowNull]
    public ItemUpsertContentModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ItemUpsertContentModel?) value;
    }

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="ItemUpsertContentDialog"/>
    /// </summary>
    public ItemUpsertContentDialog(
        ContentPresenter? contentPresenter
    ) : base(contentPresenter) {
        ViewModel = new ItemUpsertContentModel();
        DataContext = ViewModel;

        InitializeComponent();
        this.WhenActivated((disposables) => {
            // Add your bindings here as needed
            // Example pattern from DeckInsertContentDialog:
            // this.Bind(
            //     ViewModel,
            //     bind => bind.SomeProperty,
            //     view => view.SomeControl.Text
            // ).DisposeWith(disposables);
        });
    }


    /// <summary>
    /// Complete override for <see cref="ContentDialog.ShowAsync"/>
    /// </summary>
    /// <remarks>
    /// WARNING: This declaration completely hijacks the default behavior of
    /// this method as defined in base classes. The documentations no longer
    /// accurately describe its behavior.
    /// </remarks>
    /// <returns>
    /// <see cref="ItemUpsertFinal"/> or null.
    /// </returns>
    public new async Task<ItemUpsertFinal?> ShowAsync(
        CancellationToken cancellationToken = default
    ) {
        await base.ShowAsync(cancellationToken);
        return ViewModel.FormState;
    }

#endregion

#region Internals

    /// <inheritdoc />
    protected override void OnButtonClick(
        ContentDialogButton button
    ) {
        if (button is ContentDialogButton.Primary) {
            if (!ViewModel.IsValid) return;
            ViewModel.SubmitCommand.Execute()
                .Subscribe();
        }

        ViewModel.CancelCommand.Execute()
            .Subscribe();

        base.OnButtonClick(button);
    }

#endregion
}
