using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Virtuoso.UI.Views.Decks.Dialogs;

public sealed partial class DeckInsertContentModel
    : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string _name = string.Empty;


    [Reactive]
    private int? _gridSizeW;


    [Reactive]
    private int? _gridSizeH;


    public DeckInsertFinal? FormState { get; set; }


    private readonly ObservableAsPropertyHelper<bool> _isValid;
    public bool IsValid => _isValid.Value;


    public ReactiveCommand<Unit, DeckInsertFinal> SubmitCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }


    public DeckInsertContentModel() {
        _isValid = this.WhenAnyValue(
            p => p.Name,
            p => p.GridSizeW,
            p => p.GridSizeH,
            (title, sizeW, sizeH) => (
                !string.IsNullOrWhiteSpace(title)
                && (sizeW is > 0 and <= 6)
                && (sizeH is > 0 and <= 3)
            )
        ).ToProperty(this, nameof(IsValid));

        this.WhenAnyValue(p => p.GridSizeW)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(v => {
                switch (v) {
                case < 0: {
                    GridSizeW = 0;
                    return;
                }
                case > 6: {
                    GridSizeW = 6;
                    return;
                }
                }
            });

        this.WhenAnyValue(p => p.GridSizeH)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(v => {
                switch (v) {
                case < 0: {
                    GridSizeH = 0;
                    return;
                }
                case > 3: {
                    GridSizeH = 3;
                    return;
                }
                }
            });

        this.WhenAnyValue(
                p => p.GridSizeW,
                p => p.GridSizeH
            )
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(tuple => {
                switch (tuple) {
                case { Item1: >= 1, Item2: <= 0 or null }: {
                    GridSizeH = 1;
                    return;
                }
                case { Item2: >= 1, Item1: <= 0 or null }: {
                    GridSizeW = 1;
                    return;
                }
                }
            });

        // Commands

        SubmitCommand = ReactiveCommand
            .Create(
                execute: () => {
                    FormState = new DeckInsertFinal(
                        Name: Name,
                        GridSizeW: (int) GridSizeW!,
                        GridSizeH: (int) GridSizeH!
                    );

                    return FormState;
                },
                canExecute: Observable
                    .Return(_isValid.Value)
            );

        CancelCommand = ReactiveCommand
            .Create(() => { });
    }
}
