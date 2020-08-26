using Minesweeper.Converters;
using Minesweeper.Models;
using System;
using System.Collections.Generic;
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
                }
            }
        }

        private void tileTapped(object sender, TappedRoutedEventArgs e)
        {
            string[] rawPos = ((Rectangle)sender).Name.Split('-');
            int vPos = int.Parse(rawPos[0]);
            int hPos = int.Parse(rawPos[1]);
            //MineCounter.Text = $"{vPos}-{hPos}";
            Tile clickedTile = game.GameBoard.getTile(vPos, hPos);
            if(!clickedTile.Revealed)
            {
                if(clickedTile.TileType == TileEnum.NORMAL)
                {
                    clickedTile.Revealed = true;
                    if (clickedTile.TileValue == 0) game.GameBoard.OpenPocket(clickedTile);
                    else if (clickedTile.TileValue == -1) EndGame();
                }
                //Check if tile type is normal
                //open tile
                //if pocket, expand pocket
                //if bomb, end game (show all bombs and disable clicks)
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
                //cycle through normal, flagged, and ?
                

                clickedTile.cycleType();
            }
            else
            {
                //Right-clicking a tile that has already been revealed and contains a number will 
                //unveil all surrounding non-flagged neighbors IF AND ONLY IF the number of flagged 
                //neighbors equals the number in the clicked tile AND there are no neighbors marked as “ambiguous”
            }
        }



        private void EndGame()
        {
            game.EndGame();
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
