using Minesweeper.Converters;
using Minesweeper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Minesweeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Game game = new Game();
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            createGameView(9, 9, 10);
        }
        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            createGameView(16, 16, 40);
        }
        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            createGameView(30, 16, 99);
        }

        private void createGameView(int boardWidth, int boardHeight, int mineCount)
        {
            ChangeView(true);
            outputTextBlock.Text = "";
            game.restart(boardWidth, boardHeight, mineCount);
            GameGrid.Children.Clear();
            GameGrid.ColumnDefinitions.Clear();
            GameGrid.RowDefinitions.Clear();
            for (int i = 0; i < boardHeight; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(32);
                GameGrid.RowDefinitions.Add(rd);
            }
            for (int j = 0; j < boardWidth; j++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(32);
                GameGrid.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    GameGrid.Children.Add(rectangle);
                    Grid.SetRow(rectangle, i);
                    Grid.SetColumn(rectangle, j);
                    rectangle.Name = $"{i}-{j}";
                    Binding imageBinding = new Binding();
                    imageBinding.Path = new PropertyPath("TileName");
                    imageBinding.Source = game.GameBoard.getTile(i, j);
                    imageBinding.Mode = BindingMode.OneWay;
                    imageBinding.Converter = new TileToImageConverter();
                    rectangle.SetBinding(Rectangle.FillProperty, imageBinding);

                    rectangle.RightTapped += tileRightTapped;
                    rectangle.Tapped += tileTapped;

                    Binding mineCountBinding = new Binding();
                    mineCountBinding.Path = new PropertyPath("MineCounter");
                    mineCountBinding.Source = game;
                    mineCountBinding.Mode = BindingMode.OneWay;
                    MineCounterText.SetBinding(TextBlock.TextProperty, mineCountBinding);
                }
            }
        }

        private void tileTapped(object sender, TappedRoutedEventArgs e)
        {
            string[] rawPos = ((Rectangle)sender).Name.Split('-');
            int vPos = int.Parse(rawPos[0]);
            int hPos = int.Parse(rawPos[1]);
            Tile clickedTile = game.GameBoard.getTile(vPos, hPos);
            if(!clickedTile.Revealed)
            {
                if(clickedTile.TileType == TileEnum.NORMAL)
                {
                    clickedTile.Revealed = true;
                    if (clickedTile.TileValue == 0) game.GameBoard.OpenPocket(clickedTile);
                    CheckForWin();
                    if (clickedTile.TileValue == -1) EndGame();
                }
            }

        }
        private void tileRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            string[] rawPos = ((Rectangle)sender).Name.Split('-');
            int vPos = int.Parse(rawPos[0]);
            int hPos = int.Parse(rawPos[1]);
            Tile clickedTile = game.GameBoard.getTile(vPos, hPos);
            if(!clickedTile.Revealed)
            {
                CycleType(clickedTile);
            }
            else
            {
                if(clickedTile.TileValue > 0)
                {
                    if(clickedTile.TileValue == game.GameBoard.CountSurroundingTileType(clickedTile, TileEnum.FLAG))
                    {
                        if(game.GameBoard.CountSurroundingTileType(clickedTile, TileEnum.AMBIGUOUS) == 0)
                        {
                            if (!game.GameBoard.RevealSurroundingTiles(clickedTile)) EndGame();
                        }
                    }
                }
            }
        }

        private void CycleType(Tile selectedTile)
        {
            switch (selectedTile.TileType)
            {
                case TileEnum.NORMAL:
                    selectedTile.TileType = TileEnum.FLAG;
                    game.ChangeMineCounter(-1);
                    break;
                case TileEnum.FLAG:
                    selectedTile.TileType = TileEnum.AMBIGUOUS;
                    game.ChangeMineCounter(1);
                    break;
                case TileEnum.AMBIGUOUS:
                    selectedTile.TileType = TileEnum.NORMAL;
                    break;
            }
            CheckForWin();
        }


        private void EndGame()
        {
            game.EndGame();
            RemoveTappedListeners();
            outputTextBlock.Text = "You lose :(";
        }
        private void CheckForWin()
        {
            if (game.WinGame()) WinGame();
        }
        private void WinGame()
        {
            RemoveTappedListeners();
            outputTextBlock.Text = "You win!";
        }

        private void RemoveTappedListeners()
        {
            for (int i = 0; i < game.GameBoard.Height; i++)
            {
                for (int j = 0; j < game.GameBoard.Width; j++)
                {
                    Rectangle rect = FindName($"{i}-{j}") as Rectangle;
                    rect.RightTapped -= tileRightTapped;
                    rect.Tapped -= tileTapped;

                }
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeView(false);
        }

        private void ChangeView(bool changeToGame)
        {
            if(changeToGame)
            {
                TitlePage.Visibility = Visibility.Collapsed;
                GamePage.Visibility = Visibility.Visible;
            }
            else
            {
                GamePage.Visibility = Visibility.Collapsed;
                TitlePage.Visibility = Visibility.Visible;
            }
        }
    }
}
