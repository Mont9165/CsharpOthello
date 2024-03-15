namespace Reversi
{
    class GameController
    {
        private Board Board;
        private MainForm Form;
        private BasicAI Ai;


        /// <summary>
        /// AIのセットとボードの初期化をする．
        /// </summary>
        public GameController(MainForm form)
        {
            this.Form = form;
            this.Ai = new BasicAI();

            Board = new Board();
            
            Board.Set(3, 3, false);     //3,3初期値    0,1テスト
            Board.Set(4, 4, false);     //4,4初期値    1,0テスト
            Board.Set(3, 4, true);      //3,4初期値    0,0テスト
            Board.Set(4, 3, true);      //4,3初期値    1,1テスト
            
        }


        /// <summary>
        /// ゲームを開始する．黒（プレイヤー）が先攻．
        /// </summary>
        internal void Play()
        {
            Form.SetMessage("ゲーム開始! 黒のターン");
            Form.Refresh(Board);
        }


        /// <summary>
        /// クリックを処理．MainFormのイベントハンドラから呼び出される．
        /// </summary>
        /// <param name="row">クリックされたますの行番号</param>
        /// <param name="column">クリックされたますの列番号</param>
        internal void Clicked(int row, int column)
        {

            if (Board.CanPlace(true))
            {
                if (Board.CanPlace(row, column, true))
                {
                    Board.Set(row, column, true);
                    Form.Refresh(Board);
                    Form.SetMessage("黒のターン: " + ((char)('A' + column)).ToString() + (row + 1).ToString());
                    System.Threading.Thread.Sleep(200);
                }
                else
                {
                    Form.SetMessage(((char)('A' + column)).ToString() + (row + 1).ToString() + "に黒は置けません!!!");
                    
                    return;

                }
            }
            else
            {
                Form.SetMessage("黒に置けるところがないので白に移ります");
            }

            if (Board.CanPlace(false))
            {
                while (true)
                {
                    PlaceAiHand();
                    if (Board.CanPlace(true) == true || Board.CanPlace(false) == false )   //黒に置く所があるとき or 白に置く所がなくなった時
                    {
                        break;
                    }
                }

            }
            else
            {
                Form.SetMessage("白に置けるところがないので黒に移ります");
            }

            if(Board.CanPlace(true) == false && Board.CanPlace(false) == false)
            {
                ShowResult();
            }
            
        }


        /// <summary>
        /// AIが次の手を考えて置く．
        /// </summary>
        private void PlaceAiHand()
        {
            Place place = Ai.NextHand(Board);
            Board.Set(place.Row, place.Column, false);
            Form.Refresh(Board);
            Form.SetMessage("白(AI)のターン: " + ((char)('A' + place.Column)).ToString() + (place.Row + 1).ToString());
            System.Threading.Thread.Sleep(500);
        }


        /// <summary>
        /// ゲーム結果を表示する．
        /// </summary>
        private void ShowResult()
        {
            int black = Board.GetPieceNum(true);
            int white = Board.GetPieceNum(false);

            if (black > white)
                Form.SetMessage("黒:" + black + " 白:" + white + "でプレイヤーの勝ち!!");
            else if (black < white)
                Form.SetMessage("黒:" + black + " 白:" + white + "でプレイヤーの負け・・");
            else
                Form.SetMessage("黒:" + black + " 白:" + white + "で引き分け");
        }
    }
}