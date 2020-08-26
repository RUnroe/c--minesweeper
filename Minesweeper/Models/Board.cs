using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Wallet;

namespace Minesweeper.Models
{
    public class Board
    {

        private Tile[,] tileArray;

        public int Width{ get; set; }
        public int Height{ get; set; }
        public int MineCount { get; set; }
        public Tile[,] TileArray { get; set; }



        public Board(int width, int height, int mineCount)
        {
            Width = width;
            Height = height;
            MineCount = mineCount;
            CreateBoard();
        }

        private void CreateBoard()
        {
            tileArray = new Tile[Height, Width];
            FillBoardWithTiles();
            PlaceBombs();
        }

        private void FillBoardWithTiles()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    tileArray[i, j] = new Tile(i, j);
                }
            }
        }


        private void PlaceBombs()
        {
            Random rnd = new Random();
            int minesLeft = MineCount;
            while (minesLeft > 0)
            {
                int bombVPos = rnd.Next(Height);
                int bombHPos = rnd.Next(Width);
                if (tileArray[bombVPos, bombHPos].TileValue == 0)
                {
                    tileArray[bombVPos, bombHPos].TileValue = -1;
                    minesLeft--;
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
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
                if (bombHPos < (Width - 1)) addToValue(bombVPos - 1, bombHPos + 1);
            }
            if(bombVPos < (Height - 1))
            {
                addToValue(bombVPos + 1, bombHPos);
                if (bombHPos > 0) addToValue(bombVPos + 1, bombHPos - 1);
                if (bombHPos < (Width - 1)) addToValue(bombVPos + 1, bombHPos + 1);
            }
            if(bombHPos > 0) addToValue(bombVPos, bombHPos - 1);
            if (bombHPos < (Width - 1)) addToValue(bombVPos, bombHPos + 1);
        }

        private void addToValue(int vPos, int hPos)
        {
            if (tileArray[vPos, hPos].TileValue != -1) tileArray[vPos, hPos].TileValue += 1;
        }



        public void OpenPocket(Tile clickedTile)
        {
            List<Tile> tilesToCheck = new List<Tile>();
            tilesToCheck.Add(clickedTile);
            while(tilesToCheck.Count != 0)
            {
                Tile currentTile = tilesToCheck[0];
                for(int i = -1; i < 2; i++)
                {
                    for(int j = -1; j < 2; j++)
                    {
                        if (Math.Abs(i) == Math.Abs(j)) continue;
                        if (!tileExists(currentTile.VPos + i, currentTile.HPos + j)) continue;
                        
                        Tile observedTile = tileArray[currentTile.VPos + i, currentTile.HPos + j];
                        if (!observedTile.Revealed && observedTile.TileType == TileEnum.NORMAL)
                        {
                            if (observedTile.TileValue == 0) tilesToCheck.Add(observedTile);
                            else if (observedTile.TileValue > 0) observedTile.Revealed = true;
                        }
                        
                    }
                }
                currentTile.Revealed = true;
                tilesToCheck.RemoveAt(0);
            }
        }

        private bool tileExists(int vPos, int hPos)
        {
            return (vPos >= 0 && vPos <= (Height - 1) && hPos >= 0 && hPos <= (Width - 1)); 
        }


        public int CountSurroundingTileType(Tile selectedTile, TileEnum tileType)
        {
            int sum = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (tileExists(selectedTile.VPos + i, selectedTile.HPos + j))
                    {
                        if (tileArray[selectedTile.VPos + i, selectedTile.HPos + j].TileType == tileType) sum++;
                    }
                }
            }
            return sum;
        }
        public bool RevealSurroundingTiles(Tile selectedTile)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (tileExists(selectedTile.VPos + i, selectedTile.HPos + j))
                    {
                        Tile observedTile = tileArray[selectedTile.VPos + i, selectedTile.HPos + j];
                        if (observedTile.TileType == TileEnum.NORMAL && !observedTile.Revealed)
                        {
                            observedTile.Revealed = true;
                            if (observedTile.TileValue == -1) return false;
                            if (observedTile.TileValue == 0) OpenPocket(observedTile);
                        }
                    }
                }
            }
            return true;
        }

        public void ShowBoard()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    tileArray[i, j].Revealed = true;
                }
            }
        }


        public bool AllFlagsCoverBombs()
        {
            int counter = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (tileArray[i, j].TileValue == -1 && tileArray[i, j].TileType == TileEnum.FLAG) counter++;
                }
            }
            Debug.WriteLine($"{counter} == {MineCount}");
            return counter == MineCount;
        }

        public bool AllTilesRevealed()
        {
            int counter = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (tileArray[i, j].TileValue != -1 && tileArray[i, j].Revealed) counter++;
                }
            }
            return counter == ((Height * Width) - MineCount);
        }


        public Tile getTile(int vPos, int hPos)
        {
            return tileArray[vPos, hPos];
        }

    }
}
