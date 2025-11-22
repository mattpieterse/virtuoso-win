using Wpf.Ui.Controls;

namespace Virtuoso.UI.Core.Models.Database;

public sealed class DeckItem
{
#region Variables

    public Guid Id { get; } = Guid.NewGuid();

    public string? Text { get; set; }
    public SymbolIcon? Icon { get; set; }

    public int PagePosition { get; set; }
    public int GridPosition { get; set; }

    public bool IsConfigured => Icon is not null;

#endregion
}
