using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi {
	public class Place {
		public int Row;
		public int Column;


		public Place(int row, int column) {
			this.Row = row;
			this.Column = column;
		}

		public override string ToString() {
			return ((char)('A' + Column)).ToString() + (Row + 1).ToString();
		}
	}
}