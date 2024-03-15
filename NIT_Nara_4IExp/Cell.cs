using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    public class Cell
    {
        public int Row;
        public int Column;
        public bool IsBlank;
        public bool IsBlack;
        public bool CanBlackPlace;
        public bool CanWhitePlace;

        public Cell(int row, int column)
        {
            if(row > -1 && row < 8 && column > -1 && column < 8)
            {
                this.Row = row;
                this.Column = column;
            }
            else
            {
                this.Row = -1;
                this.Column = -1;
            }

            this.IsBlank = true;
            this.IsBlack = false;
            this.CanBlackPlace = false;
            this.CanWhitePlace = false;

        }

        public void set(bool isBlack)
        {
            this.IsBlack = isBlack;
            this.IsBlank = false;
        }

    }

   
    

}
