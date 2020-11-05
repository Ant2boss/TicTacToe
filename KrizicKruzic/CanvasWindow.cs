using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrizicKruzic
{
	/*
		[CanvasWindow]

		Elementi se mogu stavljati direktno na Canvas, ali CanvasWindow je zaduzen da pruzi neki stupanj zaštite na
		razini crtanja, micanja, brisanja.
		
		Ideja je da ako se zeli dodati neki element na Canvas da se to radi preko CanvasWindow-a
				(Nezgodno je opisati, ali ideja se moze vidjeti preko nasljednika ove klase);
			Stoga CanvasWindow nudi mnogobrojne prefab funkcije koje nude zaštitu od stavlja gdje se ne smije, pomicanja, ispisvanje,
			crtanje...

		Abstraktan je jer su sve metode koje su potrebne za rad sa CanvasWindow protected te nema smisla stvarati CanvasWindow
	*/

	abstract class CanvasWindow
	{
		//Konstruktori
		public CanvasWindow(Canvas MainCan, Vec2i Size, Vec2i Pos, string WindowName = "")
			: this(MainCan, Size.X, Size.Y, Pos.X, Pos.Y, WindowName) { }
		public CanvasWindow(Canvas MainCan, int xSize, int ySize, int xPos, int yPos, string WindowName = "")
		{
			this.Border = true;

			this.mWindowBuffer = new char[ySize * xSize];
			this.mPos = new Vec2i();

			this.mCan = MainCan;
			this.Size = new Vec2i(xSize, ySize);
			this.mWindowName = WindowName;

			this.prSetPos(xPos, yPos);
		}

		//Protected -> Onaj koji nasljeduje mora moci uredivati window, ali mozda ne zeli da korisnik iz vana moze pristupati buffer-u
		protected void prSetPos(int x, int y)
		{
			this.pUndrawBorder();

			this.mPos.X = x;
			this.mPos.Y = y;

			this.prAttachToCanvas();
		}
		protected void prSetPos(Vec2i Pos) => this.prSetPos(Pos.X, Pos.Y);

		protected void prSetCharAt(char C, int x, int y)
		{
			if (this.pOutOfWindow(x, y))
				throw new Exception($"Trying to access ({x}, {y}) while size is {this.Size}");

			this.mWindowBuffer[this.pIndexOf(x, y)] = C;

			this.prAttachToCanvas();
		}
		protected void prSetCharAt(char C, Vec2i Index) => this.prSetCharAt(C, Index.X, Index.Y);
		protected char prGetCharAt(int x, int y)
		{
			if (this.pOutOfWindow(x, y))
				throw new Exception($"Trying to read from ({x}, {y}) while size is {this.Size}");

			return this.mWindowBuffer[this.pIndexOf(x, y)];
		}
		protected char prGetCharAt(Vec2i Index) => this.prGetCharAt(Index.X, Index.Y);

		protected void prClear(char C = ' ')
		{
			for (int y = 0; y < this.Size.Y; ++y)
			{
				for (int x = 0; x < this.Size.X; ++x)
				{
					this.mWindowBuffer[this.pIndexOf(x, y)] = C;
				}
			}
		}
		protected void prAttachToCanvas()
		{
			this.pDrawBorder();

			Vec2i tVec = new Vec2i();
			for (int y = 0; y < this.Size.Y; ++y)
			{
				for (int x = 0; x < this.Size.X; ++x)
				{
					//tVec je mjesto gdje se charachter stavlja, u ovom slucaju to je (x, y) + pozicija
					tVec.SetUp(x, y);
					tVec += this.Position;
					if (!this.pOutOfCanvas(tVec))
						mCan.SetCharAt(this.mWindowBuffer[this.pIndexOf(x, y)], tVec);
				}
			}
		}
		protected void prAttachTextArt(TextArt TxtArt, int xPos, int yPos)
		{
			Vec2i tOff = new Vec2i(xPos, yPos);
			Vec2i tVec = new Vec2i();

			for (int y = 0; y < TxtArt.Size.Y; ++y)
			{
				for (int x = 0; x < TxtArt.Size.X; ++x)
				{
					tVec.SetUp(x, y);
					tVec += tOff;
					if (!this.pOutOfWindow(tVec))
						this.prSetCharAt(TxtArt.GetCharAt(x, y), tVec);
				}
			}
		}

		//Public property
		public bool Border { set; get; }
		public Vec2i Position
		{
			get => mPos.GetCopy();
		}
		public Vec2i Size { get; }


		//Private methods
		bool pOutOfWindow(int x, int y)
		{
			return (x < 0 || x >= this.Size.X || y < 0 || y >= this.Size.Y);
		}
		bool pOutOfWindow(Vec2i Index) => pOutOfWindow(Index.X, Index.Y);
		bool pOutOfCanvas(int x, int y)
		{
			return (x < 0 || x >= this.mCan.Size.X || y < 0 || y >= this.mCan.Size.Y);
		}
		bool pOutOfCanvas(Vec2i Index) => this.pOutOfCanvas(Index.X, Index.Y);

		int pIndexOf(int x, int y) => y * this.Size.X + x;
		int pIndexOf(Vec2i Index) => pIndexOf(Index.X, Index.Y);

		void pDrawBorder()
		{
			if (!this.Border)
				return;

			//Crtanje okvira oko prozora
			Vec2i tVec = new Vec2i();

			for (int y = -1; y < this.Size.Y + 1; ++y)
			{
				tVec.Y = this.Position.Y + y;

				//Vrh ili dno
				if (y == -1 || y == this.Size.Y)
				{
					int tOff = 0;
					//Ukoliko smo na vrhu
					if (y == -1)
					{
						//Napisi ime ukoliko je na Canvasu
						for (int i = 0; i < this.mWindowName.Length; ++i)
						{
							tVec.X = this.Position.X + i + 1;

							if (this.pOutOfCanvas(tVec)) continue;

							this.mCan.SetCharAt(this.mWindowName[i], tVec);
						}
						tVec.X = this.Position.X;

						if (!this.pOutOfCanvas(tVec))
							this.mCan.SetCharAt(this.mTopBottomChar, tVec);

						tOff = this.mWindowName.Length + 1;
					}
					//Nacrtaj donji ili gornji okvir do kraja ukoliko je na Canvasu
					for (int x = 0 + tOff; x < this.Size.X; ++x)
					{
						tVec.X = this.Position.X + x;

						if (this.pOutOfCanvas(tVec)) 
							continue;

						this.mCan.SetCharAt(this.mTopBottomChar, tVec);
					}
				}
				else
				{
					//Postavi krajni lijevi i krajnji desni okvir ukoliko su na Canvas-u
					tVec.X = this.Position.X - 1;
					if (!this.pOutOfCanvas(tVec))
						this.mCan.SetCharAt(this.mLeftRightChar, tVec);

					tVec.X = this.Position.X + this.Size.X;
					if (!this.pOutOfCanvas(tVec))
						this.mCan.SetCharAt(this.mLeftRightChar, tVec);
				}
			}
		}
		void pUndrawBorder()
		{
			Vec2i tVec = new Vec2i();

			for (int y = -1; y < this.Size.Y + 1; ++y)
			{
				tVec.Y = this.Position.Y + y;

				//Ako smo na vrhu ili dnu
				if (y == -1 || y == this.Size.Y)
				{
					for (int x = 0; x < this.Size.X; ++x)
					{
						tVec.X = this.Position.X + x;

						if (this.pOutOfCanvas(tVec))
							continue;

						this.mCan.SetCharAt(this.mEmptyChar, tVec);
					}
				}
				else
				{
					tVec.X = this.Position.X - 1;
					if (!this.pOutOfCanvas(tVec))
						this.mCan.SetCharAt(this.mEmptyChar, tVec);

					tVec.X = this.Position.X + this.Size.X;
					if (!this.pOutOfCanvas(tVec))
						this.mCan.SetCharAt(this.mEmptyChar, tVec);
				}
			}
		}


		//Privates
		Canvas mCan;
		char[] mWindowBuffer;

		readonly Vec2i mPos;
		string mWindowName;

		char mTopBottomChar = '-';
		char mLeftRightChar = '|';
		char mEmptyChar = ' ';

	}

}
