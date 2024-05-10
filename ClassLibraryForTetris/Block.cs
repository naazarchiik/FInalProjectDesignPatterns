using System.Collections.Generic;

namespace ClassLibraryForTetris
{
    public abstract class Block
    {
        protected abstract BlockPosition[][] Tiles { get; }
        protected abstract BlockPosition StartOffset { get; }
        public abstract int Id { get; }

        private int rotationState;
        private BlockPosition offset;

        public Block()
        {
            offset = new BlockPosition(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<BlockPosition> TilePosition()
        {
            foreach (BlockPosition p in Tiles[rotationState])
            {
                yield return new BlockPosition(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Move(int rows , int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
