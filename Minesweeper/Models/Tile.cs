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

        public Tile(int tileVPos, int tileHPos)
        {
            VPos = tileVPos;
            HPos = tileHPos;
        }

        public int VPos { get; set; }
        public int HPos { get; set; }

        public bool Revealed
        {
            get { return revealed; }
            set { 
                revealed = value;
                updateTileName();
            }
        }
        public TileEnum TileType
        {
            get { return tileType; }
            set
            {
                tileType = value;
                updateTileName();
            }
        }
        public int TileValue
        {
            get { return tileValue; }
            set
            {
                tileValue = value;
                updateTileName();
                
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

        private void updateTileName()
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

        public void cycleType()
        {
            switch (tileType)
            {
                case TileEnum.NORMAL:
                    TileType = TileEnum.FLAG;
                    break;
                case TileEnum.FLAG:
                    TileType = TileEnum.AMBIGUOUS;
                    break;
                case TileEnum.AMBIGUOUS:
                    TileType = TileEnum.NORMAL;
                    break;
            }
            
        }
        private void FieldChanged([CallerMemberName] string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }


    }
}
