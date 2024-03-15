using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi {
    /// <summary>
    /// ボードの状態を表す
    /// </summary>
    public class Board
    {
        private Cell[,] Cells;

        private bool CanBlackPlace = false;
        private bool CanWhitePlace = false;


        /// <summary>
        /// コンストラクタ．8x8のCellの配列を用意
        /// </summary>
        public Board()
        {
            Cells = new Cell[8, 8];

            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    Cells[r, c] = new Cell(r, c);
        }


        /// <summary>
        /// 指定した色のピース数を返す
        /// </summary>
        /// <param name="isBlack">trueなら黒，falseなら白</param>
        public int GetPieceNum(bool isBlack)
        {
            int num = 0;
            for (int row = 0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                    if (!Cells[row, column].IsBlank && Cells[row, column].IsBlack == isBlack)
                        num++;

            return num;
        }


        /// <summary>
        /// ボード上に指定の色をおけるか返す
        /// </summary>
        /// <param name="isBlack">trueなら黒，falseなら白</param>
        public bool CanPlace(bool isBlack)
        {
            if (isBlack)
                return CanBlackPlace;
            else
                return CanWhitePlace;
        }


        /// <summary>
        /// 指定のセルに指定の色をおけるか返す
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="isBlack">trueなら黒，falseなら白</param>
        public bool CanPlace(int row, int column, bool isBlack)
        {
            if (isBlack)
                return Cells[row, column].CanBlackPlace;
            else
                return Cells[row, column].CanWhitePlace;
        }


        /// <summary>
        /// 指定のセルが黒かどうか返す．セルが空白の場合は常にfalseを返す．
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public bool IsBlack(int row, int column)
        {
            if (Cells[row, column].IsBlank)
                return false;
            return Cells[row, column].IsBlack;
        }


        /// <summary>
        /// 指定のセルが空いているか返す．
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public bool IsBlank(int row, int column)
        {
            return Cells[row, column].IsBlank;
        }


        /// <summary>
        /// 指定のセルに指定の色を置く．裏返しと盤面の再計算もする．
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="isBlack">trueなら黒，falseなら白</param>
        public void Set(int row, int column, bool isBlack)
        {
            Cells[row, column].set(isBlack);

            Reverse(row, column, isBlack);
            CalcCondition();
        }


        /// <summary>
        /// 指定のセルに置かれたピースの8方向について，挟まれたピースを裏返す
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="isBlack">trueなら黒，falseなら白</param>
        private void Reverse(int row, int column, bool isBlack)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (x == 0 && y == 0 || !CanFlip(row, column, x, y, isBlack))
                        continue;

                    //挟む
                    int z = 1;
                    //以降が盤の外ではなく，かつ他色の間
                    while (row + y * z > -1 && column + x * z > -1 && row + y * z < 8 && column + x * z < 8 && Cells[row + y * z, column + x * z].IsBlack != isBlack)
                    {
                        Cells[row + y * z, column + x * z].set(isBlack);
                        z++;
                    }
                }
            }
        }


        /// <summary>
        /// ボード上の全てのセルについて，黒と白それぞれがおけるかどうか計算
        /// </summary>
        private void CalcCondition()
        {
            this.CanBlackPlace = false;
            this.CanWhitePlace = false;
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Cells[row, column].CanBlackPlace = false;
                    Cells[row, column].CanWhitePlace = false;

                    if (IsBlank(row, column))   //マスが開いてる
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (!(x == 0 && y == 0) && CanFlip(row, column, x, y, true) == true)  //黒が置けるとき
                                {
                                    this.CanBlackPlace = true;  //どこかのマスに黒が置ける
                                    Cells[row, column].CanBlackPlace = true;
                                }
                                if (!(x == 0 && y == 0) && CanFlip(row, column, x, y, false) == true) //白が置けるとき
                                {
                                    this.CanWhitePlace = true;  //どこかのマスに白が置ける
                                    Cells[row, column].CanWhitePlace = true;
                                }
                            }
                        }
                        
                    }
                    else  //マスが開いてない
                    {
                        Cells[row, column].CanBlackPlace = false;
                        Cells[row, column].CanWhitePlace = false;
                    }
                }
            }
        }


        /// <summary>
        /// 指定のセルから指定の方向に挟めるか返す
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="x">挟む方向のx軸</param>
        /// <param name="y">挟む方向のy軸</param>
        /// <param name="isBlack">trueなら黒，falseなら白</param>
        private bool CanFlip(int row, int column, int x, int y, bool isBlack)
        {
            //盤の外は見ない
            if (row + y < 0 || column + x < 0 || row + y > 8 - 1 || column + x > 8 - 1)
                return false;

            //^ は xor
            if (!Cells[row + y, column + x].IsBlank && Cells[row + y, column + x].IsBlack ^ isBlack)
            {
                int z = 2;
                //以降が盤の外ではなく，かつ空白ではない間
                while (row + y * z > -1 && column + x * z > -1 && row + y * z < 8 && column + x * z < 8 && !Cells[row + y * z, column + x * z].IsBlank)
                {
                    if (Cells[row + y * z, column + x * z].IsBlack == isBlack)
                        return true;
                    z++;
                }
            }

            return false;
        }
    }
}
