using Virtuoso.UI.Core.Models.Database;
using Wpf.Ui.Controls;

namespace Virtuoso.UI.Core.Caches;

public static class DeckCacheSeeder
{
#region Variables

    private static readonly Guid DemoDeck1Id = new("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoDeck2Id = new("22222222-2222-2222-2222-222222222222");

#endregion

#region Functions

    /// <summary>
    /// Creates the first demo deck with sample items (6x3 grid).
    /// </summary>
    public static Deck CreateDemoDeck1() {
        var deck = new Deck {
            Id = DemoDeck1Id,
            Name = "Demo Deck",
            Grid = new DeckGrid(6, 3)
        };

        var sampleItems = new List<DeckItem> {
            new() {
                PagePosition = 0,
                GridPosition = 0,
                Text = "Discord",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Chat24
                }
            },
            new() {
                PagePosition = 0,
                GridPosition = 1,
                Text = "Mute",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.MicOff24
                }
            },
            new() {
                PagePosition = 0,
                GridPosition = 4,
                Text = "Stream",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.RadioButton24
                }
            },
            new() {
                PagePosition = 0,
                GridPosition = 8,
                Text = "Settings",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Settings24
                }
            }
        };

        foreach (var item in sampleItems) {
            deck.SetItem(item);
        }

        return deck;
    }


    /// <summary>
    /// Creates the second demo deck with a 3x3 grid, multiple pages, and sparse items.
    /// </summary>
    public static Deck CreateDemoDeck2() {
        var deck = new Deck {
            Id = DemoDeck2Id,
            Name = "Compact Deck",
            Grid = new DeckGrid(3, 3)
        };

        var sampleItems = new List<DeckItem> {
            new() {
                PagePosition = 0,
                GridPosition = 0,
                Text = "Home",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Home24
                }
            },
            new() {
                PagePosition = 0,
                GridPosition = 2,
                Text = "Search",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Search24
                }
            },
            new() {
                PagePosition = 0,
                GridPosition = 4,
                Text = "Mail",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Mail24
                }
            },
            new() {
                PagePosition = 0,
                GridPosition = 7,
                Text = "Calendar",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Calendar24
                }
            },
            new() {
                PagePosition = 1,
                GridPosition = 1,
                Text = "Music",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.MusicNote124
                }
            },
            new() {
                PagePosition = 1,
                GridPosition = 5,
                Text = "Camera",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Camera24
                }
            },
            new() {
                PagePosition = 1,
                GridPosition = 8,
                Text = "Games",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Games24
                }
            },
            new() {
                PagePosition = 2,
                GridPosition = 3,
                Text = "Files",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Folder24
                }
            },
            new() {
                PagePosition = 2,
                GridPosition = 6,
                Text = "Notes",
                Icon = new SymbolIcon {
                    Symbol = SymbolRegular.Document24
                }
            }
        };

        foreach (var item in sampleItems) {
            deck.SetItem(item);
        }

        return deck;
    }


    /// <summary>
    /// Creates an empty deck with a 3x3 grid.
    /// </summary>
    public static Deck CreateEmptyDeck(string name = "New Deck") {
        return new Deck {
            Name = name,
            Grid = new DeckGrid(3, 3)
        };
    }

#endregion
}
