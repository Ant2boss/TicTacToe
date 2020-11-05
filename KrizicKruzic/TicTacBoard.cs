using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/*
	[TicTacBoard]

	Ova klasa je zaduzena za prikaz polja od krizic kruzica, te sve stvari vezane uz upravljanje tim poljem

 */

namespace KrizicKruzic
{
	enum Direction { 
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
	class TicTacBoard : CanvasWindow
	{
		public TicTacBoard(Canvas MainCan, int xPos = 0, int yPos = 0)
			: base(MainCan, xSize, ySize, xPos, yPos, "TicTacToe")
		{

			int xOff = 2;
			int yOff = 1;

			this.mBoardOffset = new Vec2i(xOff, yOff);
			this.mTileSize = new Vec2i(7, 5);

			int[] xLine = new int[] { 7, 15 };
			int[] yLine = new int[] { 5, 11 };

			//Stavljanje Grid-a
			for (int y = 0; y < base.Size.Y - yOff * 2; ++y)
			{
				if (y == yLine[0] || y == yLine[1])
				{
					for (int x = 0; x < base.Size.X - xOff * 2; ++x)
					{
						if (x == xLine[0] || x == xLine[1])
							base.prSetCharAt('+', x + xOff, y + yOff);
						else
							base.prSetCharAt('-', x + xOff, y + yOff);
					}
				}
				else
				{
					this.prSetCharAt('|', xLine[0] + xOff, y + yOff);
					this.prSetCharAt('|', xLine[1] + xOff, y + yOff);
				}
			}

		}

		//Public metode
		public void SelectTile(int x, int y)
		{
			if (x < 0 || x >= 3 || y < 0 || y >= 3) return;
			if (!this.pSelectedIsNotValid())
			{
				this.pClearTile(mSelectedTile, ' ');
			}
			this.mSelectedTile = new Vec2i(x, y);
			this.pClearTile(mSelectedTile, ':');
		}
		public void SelectTile(Vec2i Index) => this.SelectTile(Index.X, Index.Y);
		public bool PlaceXOnSelected() => this.pPlaceOnSelected('X');
		public bool PlaceOOnSelected() => this.pPlaceOnSelected('O');
		public void SelectAndPlaceX(int x, int y)
		{
			this.SelectTile(x, y);
			this.PlaceXOnSelected();
		}
		public void SelectAndPlaceX(Vec2i Index) => this.SelectAndPlaceX(Index.X, Index.Y);
		public void SelectAndPlaceO(int x, int y)
		{
			this.SelectTile(x, y);
			this.PlaceOOnSelected();
		}
		public void SelectAndPlaceO(Vec2i Index) => this.SelectAndPlaceO(Index.X, Index.Y);

		public Vec2i GetSelectedTile()
		{
			return this.mSelectedTile.GetCopy();
		}
		public char GetStateAt(int x, int y)
		{
			if (x < 0 || x >= 3 || y < 0 || y >= 3)
				throw new Exception($"TicTacToe board is 3x3! Cannot read element from Index({x}, {y})");

			return this.mBoardState[this.pIndexOf(x, y)];
		}

		public void ClearBoard()
		{
			for (int y = 0; y < 3; ++y)
			{
				for (int x = 0; x < 3; ++x)
				{
					this.mSelectedTile.SetUp(x, y);
					this.mBoardState[this.pIndexOf(this.mSelectedTile)] = ' ';
					this.pClearTile(mSelectedTile);
				}
			}
			this.mSelectedTile.SetUp(-1, -1);
		}

		public void Update() => base.prAttachToCanvas();

		public void MoveSelection(Direction Dir) {
			if (this.pSelectedIsNotValid())
				return;

			Vec2i tNewPos = this.mSelectedTile.GetCopy();

			switch (Dir) {
				case Direction.UP:
					--tNewPos.Y;
					if (tNewPos.Y < 0)
						tNewPos.Y = 0;
					break;
				case Direction.DOWN:
					++tNewPos.Y;
					if (tNewPos.Y >= 3)
						tNewPos.Y = 2;
					break;
				case Direction.LEFT:
					--tNewPos.X;
					if (tNewPos.X < 0)
						tNewPos.X = 0;
					break;
				case Direction.RIGHT:
					++tNewPos.X;
					if (tNewPos.X >= 3)
						tNewPos.X = 2;
					break;
			}
			this.SelectTile(tNewPos);
		}

		//Constante
		public const int xSize = 27;
		public const int ySize = 19;

		//Private metode
		void pClearTile(Vec2i TileIndex, char ClearWith = ' ')
		{
			Vec2i tStartOff = new Vec2i(((this.mTileSize.X + 1) * TileIndex.X) + this.mBoardOffset.X,
				((this.mTileSize.Y + 1) * TileIndex.Y) + this.mBoardOffset.Y);

			Vec2i tOff = new Vec2i();

			for (int y = 0; y < this.mTileSize.Y; ++y)
			{
				for (int x = 0; x < this.mTileSize.X; ++x)
				{
					tOff.X = tStartOff.X + x;
					tOff.Y = tStartOff.Y + y;

					base.prSetCharAt(ClearWith, tOff);
				}
			}
			tStartOff.Offset((this.mTileSize.X - this.mBigO.Size.X) / 2, (this.mTileSize.Y - this.mBigO.Size.Y) / 2);

			switch (this.mBoardState[this.pIndexOf(TileIndex)])
			{
				case 'X':
					base.prAttachTextArt(this.mBigX, tStartOff.X, tStartOff.Y);
					break;
				case 'O':
					base.prAttachTextArt(this.mBigO, tStartOff.X, tStartOff.Y);
					break;
				default:
					break;
			}
		}

		private bool pSelectedIsNotValid()
		{
			return this.mSelectedTile.X == -1 && this.mSelectedTile.Y == -1;
		}
		bool pPlaceOnSelected(char C)
		{
			if (this.pSelectedIsNotValid())
				return false;

			if (this.mBoardState[this.pIndexOf(this.mSelectedTile)] == 'O' || this.mBoardState[this.pIndexOf(this.mSelectedTile)] == 'X')
				return false;

			switch (C)
			{
				case 'X':
					this.mBoardState[this.pIndexOf(this.mSelectedTile)] = 'X';
					break;
				case 'O':
					this.mBoardState[this.pIndexOf(this.mSelectedTile)] = 'O';
					break;
				default:
					break;
			}
			this.pClearTile(this.mSelectedTile);
			this.mSelectedTile.SetUp(-1, -1);
			return true;
		}

		int pIndexOf(int x, int y)
		{
			if (x < 0 || x >= 3 || y < 0 || y >= 3)
				throw new Exception($"Out of bounds! Trying to acces tile ({x}, {y}), while size is (3, 3)");

			return y * 3 + x;
		}
		int pIndexOf(Vec2i Index) => pIndexOf(Index.X, Index.Y);


		//Privates
		char[] mBoardState = new char[9];

		Vec2i mSelectedTile = new Vec2i(-1, -1);
		Vec2i mBoardOffset;
		Vec2i mTileSize;

		TextArt mBigO = new TextArt(FirstLaunch.BigOFile);
		TextArt mBigX = new TextArt(FirstLaunch.BigXFile);
	}
}
