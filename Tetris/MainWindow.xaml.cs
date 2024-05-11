﻿using System;
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
        private List<int> scoreList = new List<int>();
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayIncrease = 25;

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
                    imageControls[i, j].Opacity = 1;
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (BlockPosition p in block.TilePosition())
            {                
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
                imageControls[p.Row, p.Column].Opacity = 1;
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHoldBlock(Block holdBlock) 
        {
            if (holdBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[holdBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (BlockPosition p in block.TilePosition())
            {
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.1;
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHoldBlock(gameState.HoldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";           
        }
        
        private async Task GameLoop()
        {          
            Draw(gameState);
            
            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - ((gameState.Score / 100) * delayIncrease));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";         
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
                case Key.C:
                    gameState.HoldingBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
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
            scoreList.Add(gameState.Score);
            HighScore.Visibility = Visibility.Visible;
            HighScore.Text = $"Highest score:  {GetHighestScore()}";          
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();           
        }

        private int GetHighestScore()
        {
            return scoreList.Max();
        }
    }
}
