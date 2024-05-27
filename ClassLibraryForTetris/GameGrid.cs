namespace ClassLibraryForTetris
{
    public class GameGrid
    {
        private readonly int[,] _grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => _grid[r, c];
            set => _grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _grid = new int[rows, columns];
        }

        public bool IsInside(int r, int c) => r >= 0 && r < Rows && c >= 0 && c < Columns;

        public bool IsEmpty(int r, int c) => IsInside(r, c) && _grid[r, c] == 0;

        public bool IsRowFull(int r)
        {
            for (int i = 0; i < Columns; i++)
            {
                if (_grid[r, i] == 0) return false;
            }
            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int i = 0; i < Columns; i++)
            {
                if (_grid[r, i] != 0) return false;
            }
            return true;
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int i = Rows - 1; i >= 0; i--)
            {
                if (IsRowFull(i))
                {
                    PerformOperation(new ClearRowOperation(), i);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    PerformOperation(new MoveRowDownOperation(cleared), i);
                }
            }

            return cleared;
        }

        private void PerformOperation(IGridOperation operation, int row)
        {
            operation.Execute(this, row);
        }
    }

}