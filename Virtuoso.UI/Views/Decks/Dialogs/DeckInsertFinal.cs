using JetBrains.Annotations;

namespace Virtuoso.UI.Views.Decks.Dialogs;

public sealed record DeckInsertFinal(
    string Name,
    int GridSizeW,
    int GridSizeH
);
