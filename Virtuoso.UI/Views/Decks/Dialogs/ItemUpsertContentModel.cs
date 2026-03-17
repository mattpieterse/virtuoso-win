using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Virtuoso.UI.Views.Decks.Dialogs;

public sealed partial class ItemUpsertContentModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private ItemUpsertFinal? _formState;

    public ReactiveCommand<Unit, ItemUpsertFinal?> SubmitCommand { get; }
    public ReactiveCommand<Unit, ItemUpsertFinal?> CancelCommand { get; }

    private readonly ObservableAsPropertyHelper<bool> _isValid;
    public bool IsValid => _isValid.Value;

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="ItemUpsertContentModel"/>
    /// </summary>
    public ItemUpsertContentModel() {
        _isValid = Observable.Return(true)
            .ToProperty(this, nameof(IsValid));

        SubmitCommand = ReactiveCommand
            .Create(
                execute: ItemUpsertFinal? () => null,
                canExecute: Observable
                    .Return(true)
            );

        CancelCommand = ReactiveCommand
            .Create(
                execute: ItemUpsertFinal? () => null,
                canExecute: Observable
                    .Return(true)
            );
    }

#endregion
}
