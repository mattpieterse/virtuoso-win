using Virtuoso.UI.Core.Models.Database;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Views.Decks.Components;

public sealed class BoardItemState
{
#region Class

    public Guid? ItemId { get; }
    public int GridPosition { get; }
    public string Text { get; }
    public SymbolIcon? Icon { get; }
    public bool IsConfigured { get; }

    public BoardItemState(DeckItem? deckItem, int gridPosition) {
        GridPosition = gridPosition;
        ItemId = deckItem?.Id;

        if (deckItem?.IsConfigured ?? false) {
            Text = deckItem.Text ?? string.Empty;
            Icon = deckItem.Icon;
            IsConfigured = true;
        }
        else {
            Text = string.Empty;
            Icon = null;
            IsConfigured = false;
        }
    }

#endregion
}
