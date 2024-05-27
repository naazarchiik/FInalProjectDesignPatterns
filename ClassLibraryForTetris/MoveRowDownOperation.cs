using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryForTetris
{
    public class MoveRowDownOperation : IGridOperation
    {
        private readonly int _numRows;

        public MoveRowDownOperation(int numRows)
        {
            _numRows = numRows;
        }

        public void Execute(GameGrid grid, int row)
        {
            for (int i = 0; i < grid.Columns; i++)
            {
                grid[row + _numRows, i] = grid[row, i];
                grid[row, i] = 0;
            }
        }
    }
}
