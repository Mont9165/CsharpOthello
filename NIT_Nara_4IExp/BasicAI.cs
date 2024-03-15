using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi {
	class BasicAI {

		public Place NextHand(Board board) {
			for (int row = 0; row < 8; row++)
				for (int column = 0; column < 8; column++)
					if (board.CanPlace(row, column, false))
						return new Place(row, column);

			//到達しないはずのコード
			return new Place(-1, -1);
		}
	}
}