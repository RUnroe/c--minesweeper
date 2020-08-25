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
            GameGrid.ColumnDefinitions.Clear();
            GameGrid.RowDefinitions.Clear();
            for (int i = 0; i < boardHeight; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < boardWidth; j++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    GameGrid.Children.Add(rectangle);
                    Grid.SetRow(rectangle, i);
                    Grid.SetColumn(rectangle, j);

                    Binding imageBinding = new Binding();
                    imageBinding.Source = game.GameBoard.getTile(i, j).TileName;
                    imageBinding.Mode = BindingMode.OneWay;
                    imageBinding.Converter = new TileToImageConverter();
                    rectangle.SetBinding(Rectangle.FillProperty, imageBinding);
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
