using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryForTetris
{
    public class ClearRowOperation : IGridOperation
    {
        public void Execute(GameGrid grid, int row)
        {
            for (int i = 0; i < grid.Columns; i++)
            {
                grid[row, i] = 0;
            }
        }
    }
}
