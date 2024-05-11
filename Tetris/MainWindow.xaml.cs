using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibraryForTetris;
using Block = ClassLibraryForTetris.Block;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;

        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int  j = 0; j < grid.Columns; j++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (i - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, j * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[i, j] = imageControl;
                }
            }

            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int i = 0; i < grid.Rows;i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    int id = grid[i, j];
                    imageControls[i, j].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (BlockPosition p in block.TilePosition())
            {
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                await Task.Delay(500);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch(e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down: 
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                default: return;
            }
            
            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
