using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Reversi {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);



            //ここでMainFormのオブジェクトを生成する．
            //MainFormのコンストラクタがGUIを組み立て，MainForm_Loadが呼び出される．
            //当然，ここより前にコードを書いて動作させることも可能．
			Application.Run(new MainForm());
		}
	}
}
