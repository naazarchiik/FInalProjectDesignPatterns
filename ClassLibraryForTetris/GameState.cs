namespace ClassLibraryForTetris
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get { return currentBlock; }
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++) 
                {
                    CurrentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        CurrentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block HoldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        private bool BlockFits()
        {
            foreach (BlockPosition p in CurrentBlock.TilePosition())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public void HoldingBlock()
        {
            if(!CanHold)return;

            if(HoldBlock == null)
            {
                HoldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Swap(CurrentBlock, HoldBlock);
            }

            CanHold = false;
        }
        private void Swap(Block block1, Block block2)
        {
            Block tmp = block1;
            block1 = block2;
            block2 = tmp;
        }
        private void RotateBlock(Action rotateAction, Action undoAction)
        {
            rotateAction();
            if (!BlockFits()) undoAction();
        }
        private void MoveBlock(int rowDelta, int colDelta, Action onFail = null)
        {
            CurrentBlock.Move(rowDelta, colDelta);
            if (!BlockFits())
            {
                CurrentBlock.Move(-rowDelta, -colDelta);
                onFail?.Invoke();
            }
        }
        public void RotateBlockCW()
        {
            RotateBlock(() => CurrentBlock.RotateCW(), () => CurrentBlock.RotateCCW());
        }

        public void RotateBlockCCW()
        {
            RotateBlock(() => CurrentBlock.RotateCCW(), () => CurrentBlock.RotateCW());
        }

        public void MoveBlockLeft()
        {
            MoveBlock(0, -1);
        }

        public void MoveBlockRight()
        {
            MoveBlock(0, 1);
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (BlockPosition p in CurrentBlock.TilePosition())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows() * 100;

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            MoveBlock(1, 0, PlaceBlock);
        }

        private int TileDropDistance(BlockPosition p)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (BlockPosition p in CurrentBlock.TilePosition())
            {
                drop = Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
