using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reversi {
	public partial class MainForm : Form {

		static int LabelFont = 24;
        static int PieceFont = 28;

        GameController Game;

        private Label[,] Labels = new Label[8, 8];

        public MainForm() {
			//GUIデザイナーで作った画面を作成．変更しないこと!!
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e) {
			//ちらつきを押さえるためのダブルバッファリングを設定
			System.Type myType = typeof(DataGridView);
			System.Reflection.PropertyInfo myPropertyInfo = myType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			myPropertyInfo.SetValue(mainBoard, true, null);

            //ボードのGUIを用意
            MakeMainBoard();
            MakeBoardSide();

            //GameControllerを用意&ゲーム開始
            Game = new GameController(this);
            Game.Play();
            DoCellTest();
            DoCalcConditionTest();
        }

        private void DoCellTest() { 
        //Cellクラスのコンストラクタ，setメソッドのテスト

            int[] Row = new int[4] {-1,0,7,8};
            int[] Column = new int[4] { -1, 0, 7, 8 };
            Console.WriteLine();
            Console.WriteLine("DoCellTest");
            for (int i = 0; i < Row.Length; i++)
            {
                for(int j = 0; j < Column.Length; j++)
                {
                    Cell c = new Cell(Row[i], Column[j]);
                    Console.WriteLine("(" + c.Row + "," + c.Column + "), isBlank:" + c.IsBlank + ", isBlack:" + c.IsBlack + ", CanBlackPlace:" + c.CanBlackPlace + ", CamWhitePlace:" + c.CanWhitePlace);

                    c.set(true);
                    Console.WriteLine("(" + c.Row + "," + c.Column + "), isBlank:" + c.IsBlank + ", isBlack:" + c.IsBlack);
                    Console.WriteLine();
                }
            }
        }
        private void DoCalcConditionTest()
        {
            Board board = new Board();
            Console.WriteLine("DoCalcConditionTest");

            LeftCorner(true, board);     //左 黒
            RightCorner(true, board);    //右 黒  計8方向の黒のテスト
            Console.WriteLine();

            LeftCorner(false, board);    //左 白
            RightCorner(false, board);   //右 白　計8方向の白のテスト
            Console.WriteLine();

            Middle(true, board);
            Console.WriteLine();
        }

        private void LeftCorner(bool isBlack, Board board)
        {
            board.Set(6, 1, !isBlack);
            board.Set(5, 2, isBlack);
            CanPlaceJudge(7, 0, isBlack, board);    //右斜め上の1方向で挟める

            board = new Board();
            board.Set(0, 1, !isBlack);
            board.Set(0, 2, isBlack);
            CanPlaceJudge(0, 0, isBlack, board);    //右の1方向で挟める

            board = new Board();
            board.Set(1, 1, !isBlack);
            board.Set(2, 2, isBlack);
            CanPlaceJudge(0, 0, isBlack, board);    //右斜め下の1方向で挟める

            board = new Board();
            board.Set(1, 0, !isBlack);
            board.Set(2, 0, isBlack);
            CanPlaceJudge(0, 0, isBlack, board);    //下の1方向で挟める
            board = new Board();
        }

        private void RightCorner(bool isBlack, Board board)
        {
            board.Set(1, 6, !isBlack);
            board.Set(2, 5, isBlack);
            CanPlaceJudge(0, 7, isBlack, board);    //左斜め下の1方向で挟める

            board = new Board();
            board.Set(7, 6, !isBlack);
            board.Set(7, 5, isBlack);
            CanPlaceJudge(7, 7, isBlack, board);    //左の1方向で挟める

            board = new Board();
            board.Set(6, 6, !isBlack);
            board.Set(5, 5, isBlack);
            CanPlaceJudge(7, 7, isBlack, board);    //左斜め上の1方向で挟める

            board = new Board();
            board.Set(6, 7, !isBlack);
            board.Set(5, 7, isBlack);
            CanPlaceJudge(7, 7, isBlack, board);    //上の1方向で挟める
            board = new Board();
        }

        private void Middle(bool isBlack, Board board)
        {
            board = new Board();
            CanPlaceJudge(3, 3, isBlack, board);    //周り8か所空白で挟めない

            board = new Board();
            board.Set(3, 3, isBlack);
            CanPlaceJudge(3, 3, isBlack, board);    //既に駒が置かれているため置けないはず
            board = new Board();

            board.Set(3, 2, isBlack);
            CanPlaceJudge(3, 3, isBlack, board);    // ＿黒□ で挟めないはず
            board = new Board();

            board.Set(3, 2, isBlack);
            board.Set(3, 1, isBlack);
            CanPlaceJudge(3, 3, isBlack, board);    // 黒黒□ で挟めないはず
            board = new Board();

            board.Set(3, 2, !isBlack);
            board.Set(3, 1, isBlack);
            CanPlaceJudge(3, 4, isBlack, board);    // 黒白＿□ で挟めないはず
            board = new Board();

            board.Set(3, 2, isBlack);
            board.Set(3, 1, isBlack);
            CanPlaceJudge(3, 4, isBlack, board);    // 黒黒＿□ で挟めないはず
            board = new Board();

            board.Set(2, 2, !isBlack);
            board.Set(1, 1, isBlack);
            board.Set(4, 4, isBlack);
            board.Set(5, 5, isBlack);
            CanPlaceJudge(3, 3, isBlack, board);    // 黒白□黒黒 で挟めるはず
            board = new Board();
        }

        private void CanPlaceJudge(int row, int column, bool isBlack, Board board)
        {
            Console.WriteLine("isBlack:" + isBlack + " , CanPlace:" + board.CanPlace(isBlack) + " , CanPlace(" + row + "," + column + "):" + board.CanPlace(row, column, isBlack) + ", isBlank:" + board.IsBlank(row, column));
        }


        /// <summary>
        /// ボードの縁に表示する数字とアルファベットを作成する．
        /// </summary>
        /// 
        private void MakeBoardSide()
        {
            Font font = new System.Drawing.Font("ＭＳ ゴシック", LabelFont);

            //上下の端にアルファベット
            for (int i = 1; i < 9; i++)
            {
                Label topLabel = new Label();
                topLabel.Font = font;
                topLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
                topLabel.TextAlign = ContentAlignment.MiddleCenter;
                topLabel.Text = ((char)('A' + i - 1)).ToString();
                mainBoard.Controls.Add(topLabel, i, 0);

                Label bottomLabel = new Label();
                bottomLabel.Font = font;
                bottomLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
                bottomLabel.TextAlign = ContentAlignment.MiddleCenter;
                bottomLabel.Text = ((char)('A' + i - 1)).ToString();
                mainBoard.Controls.Add(bottomLabel, i, 9);
            }

            //左右の端に数字
            for (int i = 1; i < 9; i++)
            {
                Label leftLabel = new Label();
                leftLabel.Font = font;
                leftLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
                leftLabel.TextAlign = ContentAlignment.MiddleCenter;
                leftLabel.Text = i.ToString();
                mainBoard.Controls.Add(leftLabel, 0, i);

                Label rightLabel = new Label();
                rightLabel.Font = font;
                rightLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
                rightLabel.TextAlign = ContentAlignment.MiddleCenter;
                rightLabel.Text = i.ToString();
                mainBoard.Controls.Add(rightLabel, 9, i);
            }
        }


        /// <summary>
        /// 8x8のボードを作成する．
        /// </summary>
        private void MakeMainBoard()
        {
            Font boardFont = new System.Drawing.Font("ＭＳ ゴシック", PieceFont);

            //セルを表すラベルをHEIGHTxWIDTH個作成
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Labels[r, c] = new Label();
                    Labels[r, c].Font = boardFont;
                    Labels[r, c].Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
                    Labels[r, c].TextAlign = ContentAlignment.MiddleCenter;

                    //ラベルがクリックされたときに処理をするメソッドを指定
                    Labels[r, c].Click += new EventHandler(Cell_Click);

                    mainBoard.Controls.Add(Labels[r, c], c + 1, r + 1);
                }
            }
        }


        /// <summary>
        /// セルのクリックを処理するメソッド（イベントハンドラ）．行番号と列番号を調べてGameControllerに処理させる．
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Cell_Click(object sender, EventArgs e)
        {
            //senderがイベントの発信源（ここではセルのラベル）なので型変換
            Label tmp = (Label)sender;

            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (Labels[r, c] == tmp)
                        Game.Clicked(r, c);
        }


        /// <summary>
        /// Window上部のメッセージを更新する
        /// </summary>
        /// <param name="message">表示するメッセージ</param>
        internal void SetMessage(string message)
        {
            systemMessageLabel.Text = message;
            systemMessageLabel.Refresh();
        }


        /// <summary>
        /// ボードの状態にあわせて表示を更新する
        /// </summary>
        /// <param name="board">表示するボード</param>
        internal void Refresh(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (board.IsBlank(r, c))
                        Labels[r, c].Text = "";
                    else if (board.IsBlack(r, c))
                        Labels[r, c].Text = "●";
                    else
                        Labels[r, c].Text = "○";
                }
            }

            mainBoard.Refresh();
        }
    }
}