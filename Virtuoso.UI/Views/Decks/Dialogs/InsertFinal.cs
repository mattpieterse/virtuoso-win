using JetBrains.Annotations;

namespace Virtuoso.UI.Views.Decks.Dialogs;

public sealed record InsertFinal(
    string Name,
    int GridSizeW,
    int GridSizeH
);
