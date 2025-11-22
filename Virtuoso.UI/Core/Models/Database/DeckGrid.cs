namespace Virtuoso.UI.Core.Models.Database;

public class DeckGrid
{
    private readonly int _cols;
    private readonly int _rows;

    public int Cols => _cols;
    public int Rows => _rows;

    public int SlotCount => _cols * _rows;
    public int PageCount => 9;

    public DeckGrid(
        int colCount,
        int rowCount
    ) {
        _cols = colCount;
        _rows = rowCount;
    }

    public bool IsValidPosition(int gridPosition) =>
        gridPosition >= 0 && gridPosition < SlotCount;

    public (int row, int col) GetGridCoordinates(int gridPosition) =>
        gridPosition >= SlotCount
            ? throw new ArgumentOutOfRangeException(nameof(gridPosition))
            : (gridPosition / _cols, gridPosition % _cols);
}
