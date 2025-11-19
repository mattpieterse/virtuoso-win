using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Virtuoso.UI.Core.Models.Intents;

namespace Virtuoso.UI.Views.Decks;

/// <summary>
/// View model for <see cref="DeckView"/>
/// </summary>
public sealed partial class DeckViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();


    [Reactive]
    private string _debugText = string.Empty;

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="DeckViewModel"/>
    /// </summary>
    public DeckViewModel(
        IMessageBus bus
    ) {
        var latestIntent = bus.Listen<DeckNavigationIntent>()
            .Replay(1)
            .RefCount();

        this.WhenActivated((disposables) => {
            latestIntent
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(intent => DebugText = intent.Argument)
                .DisposeWith(disposables);
        });
    }

#endregion
}
