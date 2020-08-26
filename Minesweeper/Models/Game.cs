using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Board GameBoard { get; set; }

        private int mineCounter;
        public int MineCounter
        {
            get { return mineCounter; }
            set 
            { 
                mineCounter = value;
                FieldChanged();
            }
        }

        public void ChangeMineCounter(int delta)
        {
            MineCounter = (mineCounter + delta);
        }

        public void restart(int boardWidth, int boardHeight, int mineCount)
        {
            GameBoard = null;
            GameBoard = new Board(boardWidth, boardHeight, mineCount);
            mineCounter = mineCount;
        }

        public void EndGame()
        {
            GameBoard.ShowBoard();
        }

        private void FieldChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

    }
}
