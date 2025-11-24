using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows.Controls;
using ReactiveUI;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Views.Decks.Dialogs;

public sealed partial class InsertContentDialog
    : IViewFor<InsertContentModel>
{
#region Variables

    [AllowNull]
    public InsertContentModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (InsertContentModel?) value;
    }

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="InsertContentDialog"/>
    /// </summary>
    public InsertContentDialog(
        ContentPresenter? contentPresenter
    ) : base(contentPresenter) {
        ViewModel = new InsertContentModel();
        DataContext = ViewModel;

        InitializeComponent();
        this.WhenActivated((disposables) => {
            this.Bind(
                ViewModel,
                bind => bind.Name,
                view => view.NameInput.Text
            ).DisposeWith(disposables);

            this.Bind(
                ViewModel,
                bind => bind.GridSizeH,
                view => view.GridSizeHInput.Text
            ).DisposeWith(disposables);

            this.Bind(
                ViewModel,
                bind => bind.GridSizeW,
                view => view.GridSizeWInput.Text
            ).DisposeWith(disposables);

            this.WhenAnyValue(p => p.ViewModel.IsValid)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(v => {
                    PrimaryButtonAppearance = v
                        ? ControlAppearance.Primary
                        : ControlAppearance.Transparent;
                })
                .DisposeWith(disposables);

            this.WhenAnyValue(
                    p => p.DialogWidth,
                    p => p.Padding
                )
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => {
                    const double internalContentPadding = 48;
                    var horizontalPadding = Padding.Left + Padding.Right;
                    ContentGrid.MaxWidth = (DialogWidth - horizontalPadding - internalContentPadding);
                    ContentGrid.MinWidth = (DialogWidth - horizontalPadding - internalContentPadding);
                })
                .DisposeWith(disposables);
        });
    }

#endregion

#region Functions

    /// <summary>
    /// Complete override for <see cref="ContentDialog.ShowAsync"/>
    /// </summary>
    /// <remarks>
    /// WARNING: This declaration completely hijacks the default behavior of
    /// this method as defined in base classes. The documentations no longer
    /// accurately describe its behavior.
    /// </remarks>
    /// <returns>
    /// <see cref="InsertFinal"/> or null.
    /// </returns>
    public new async Task<InsertFinal?> ShowAsync(
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
