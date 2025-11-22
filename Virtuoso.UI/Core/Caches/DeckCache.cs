using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Virtuoso.UI.Core.Models.Database;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Core.Caches;

public class DeckCache
    : IDeckCache
{
    private List<Deck> _items = [];


    public DeckCache() { }


    public Deck? FetchOne(Guid targetId) {
        return _items.FirstOrDefault(deck => deck.Id == targetId);
    }


    public void Insert(Deck deck) {
        _items.Add(deck);
    }


    public IEnumerable<Deck> FetchAll() {
        return _items;
    }


    public void Delete(Guid targetId) {
        _items.Remove(
            _items.First(deck => deck.Id == targetId)
        );
    }


    public void DeleteItem(
        Guid deckId,
        Guid itemId
    ) {
        _items
            .FirstOrDefault(deck => deck.Id == deckId)!
            .DelItem(itemId);
    }
}
