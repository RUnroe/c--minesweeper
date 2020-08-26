using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Wallet;

namespace Minesweeper.Models
{
    public class Board
    {
        private int width;
        private int height;
        private int mineCount;
        private Tile[,] tileArray;

        public int Width{ get; set; }
        public int Height{ get; set; }
        public int MineCount { get; set; }
        public Tile[,] TileArray { get; set; }



        public Board(int width, int height, int mineCount)
        {
            this.width = width;
            this.height = height;
            this.mineCount = mineCount;
            CreateBoard();
        }

        private void CreateBoard()
        {
            tileArray = new Tile[height, width];
            FillBoardWithTiles();
            PlaceBombs();
        }

        private void FillBoardWithTiles()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tileArray[i, j] = new Tile();
                }
            }
        }


        private void PlaceBombs()
        {
            Random rnd = new Random();
            int minesLeft = mineCount;
            while (minesLeft > 0)
            {
                int bombVPos = rnd.Next(height);
                int bombHPos = rnd.Next(width);
                if (tileArray[bombVPos, bombHPos].TileValue == 0)
                {
                    tileArray[bombVPos, bombHPos].TileValue = -1;
                    minesLeft--;
                }
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (tileArray[i, j].TileValue == -1) changeTileValues(i, j);
                }
            }
        }

        private void changeTileValues(int bombVPos, int bombHPos)
        {
            if(bombVPos > 0)
            {
                addToValue(bombVPos - 1, bombHPos);
                if (bombHPos > 0) addToValue(bombVPos - 1, bombHPos - 1);
                if (bombHPos < (width - 1)) addToValue(bombVPos - 1, bombHPos + 1);
            }
            if(bombVPos < (height - 1))
            {
                addToValue(bombVPos + 1, bombHPos);
                if (bombHPos > 0) addToValue(bombVPos + 1, bombHPos - 1);
                if (bombHPos < (width - 1)) addToValue(bombVPos + 1, bombHPos + 1);
            }
            if(bombHPos > 0) addToValue(bombVPos, bombHPos - 1);
            if (bombHPos < (width - 1)) addToValue(bombVPos, bombHPos + 1);
        }

        private void addToValue(int vPos, int hPos)
        {
            if (tileArray[vPos, hPos].TileValue != -1) tileArray[vPos, hPos].TileValue += 1;
        }

        public void OpenPocket(int vPos, int hPos)
        {
            
        }

        public void ShowBombs()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (tileArray[i, j].TileValue == -1) tileArray[i, j].Revealed = true;
                }
            }
        }
        public Tile getTile(int vPos, int hPos)
        {
            return tileArray[vPos, hPos];
        }

    }
}
