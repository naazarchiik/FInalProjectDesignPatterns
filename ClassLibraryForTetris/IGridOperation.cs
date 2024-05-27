using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryForTetris
{
    public interface IGridOperation
    {
        void Execute(GameGrid grid, int row);
    }
}
