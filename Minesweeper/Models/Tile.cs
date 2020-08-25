using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Models
{
    public class Tile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool revealed = false;
        private TileEnum tileType = TileEnum.NORMAL;
        private int tileValue = 0;
        private string tileName = "unopened";

        public bool Revealed
        {
            get { return revealed; }
            set { 
                revealed = value;
                updateTitleName();
            }
        }
        public TileEnum TileType
        {
            get { return tileType; }
            set
            {
                tileType = value;
                updateTitleName();
            }
        }
        public int TileValue
        {
            get { return tileValue; }
            set
            {
                tileValue = value;
                updateTitleName();
                
            }
        }
        public string TileName
        {
            get { return tileName; }
            set
            {
                tileName = value;
                FieldChanged();
            }
        }

        private void updateTitleName()
        {
            if(revealed)
            {
                if (tileValue == -1) TileName = "bomb";
                else TileName = $"{tileValue}";
            }
            else
            {
                switch(tileType)
                {
                    case TileEnum.NORMAL:
                        TileName = "unopened";
                        break;
                    case TileEnum.FLAG:
                        TileName = "flag";
                        break;
                    case TileEnum.AMBIGUOUS:
                        TileName = "questionmark";
                        break;
                }
            }
        }

        private void FieldChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }


    }
}
