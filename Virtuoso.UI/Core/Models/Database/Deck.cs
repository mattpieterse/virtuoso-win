namespace Virtuoso.UI.Core.Models.Database;

public class Deck
{
    public Guid Id { get; set; } = Guid.NewGuid();


    public string Name { get; set; }
    public DeckGrid Grid { get; init; }


    public IReadOnlyList<DeckItem> Items => _items.ToList().AsReadOnly();
    private readonly List<DeckItem> _items = [];


    public DeckItem? GetItem(
        int pagePosition,
        int gridPosition
    ) {
        if (!Grid.IsValidPosition(gridPosition))
            throw new ArgumentOutOfRangeException(nameof(gridPosition));

        return _items.FirstOrDefault(x =>
            x.PagePosition == pagePosition && x.GridPosition == gridPosition);
    }


    public void SetItem(
        DeckItem instance
    ) {
        if (!Grid.IsValidPosition(instance.GridPosition))
            throw new ArgumentOutOfRangeException(nameof(instance.GridPosition));

        var existing = GetItem(instance.PagePosition, instance.GridPosition);
        if (existing != null)
            _items.Remove(existing);

        _items.Add(instance);
    }


    public void DelItem(Guid targetId) {
        var item = _items.FirstOrDefault(x => x.Id == targetId);
        if (item != null)
            _items.Remove(item);
    }


    public IEnumerable<DeckItem> GetPageItems(int pageIndex) =>
        _items.Where(x => x.PagePosition == pageIndex).OrderBy(x => x.GridPosition);
}
