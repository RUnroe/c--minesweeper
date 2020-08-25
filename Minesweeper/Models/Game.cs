using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class Game
    {
        private Board gameBoard;
        private int mineCounter;
        public void restart(int boardWidth, int boardHeight, int mineCount)
        {
            gameBoard = null;
            gameBoard = new Board(boardWidth, boardHeight, mineCount);
            mineCounter = mineCount;
        }

        public Board GameBoard {
            get { return gameBoard;  }
            set { gameBoard = value;  }
        }
    }
}
