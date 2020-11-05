using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
	[Canvas]
	
	Canvas je zaduzen za oblikovanje konzole kako bi se postavila u stanje navedeno u konstruktori
	Prema van nudi Metode koje su zaduzene za "postavi slovo na mjesto", te metodu za iscrtavanje i praznjenje ekrana
 */

namespace KrizicKruzic
{
	class Canvas
	{
		//Konstruktori
		public Canvas(int xSize, int ySize, string CanvasName = "")
		{
			//Zbog zanimljivih svojstava promjene velicine konzole, potrebno je Interni buffer napraviti
			//vecim nego sto se izvana moze pristupiti

			this.mRealSize = new Vec2i(xSize + 6, ySize + 3);
			this.mCanBuffer = new char[this.mRealSize.X * this.mRealSize.Y - 1];

			Console.SetWindowSize(this.mRealSize.X, this.mRealSize.Y);
			Console.SetBufferSize(this.mRealSize.X, this.mRealSize.Y);
			Console.CursorVisible = false;

			this.Size = new Vec2i(xSize, ySize);

			Vec2i tVec = new Vec2i();

			//Crtanje okvira kako bi korisnik mogao vidjeti podrucje rada aplikacije
			for (int y = 0; y < this.mRealSize.Y; ++y)
			{
				tVec.X = this.mRealSize.X - 1;
				tVec.Y = y;
				//Na kraju svakog retka staviti \n u slucaju da se konzola full-screen-a da ne dode do raspada sustava
				if (y != this.mRealSize.Y - 1) this.mCanBuffer[this.pIndexOf(tVec)] = '\n';

				//Ukoliko smo na samom vrhu ili dnu
				if (y == this.mOffset.Y - (this.mOffset.Y / 2) || y == (this.mRealSize.Y - (this.mOffset.Y / 2)))
				{
					//Ukoliko smo na samom vrhu i Canvas-u je dodijeljeno ime
					int tOff = 0;
					if (CanvasName.Length != 0 && y == this.mOffset.Y - (this.mOffset.Y / 2))
					{
						//Dodaj ime na vrh
						for (int i = 0; i < CanvasName.Length; ++i)
						{
							tVec.X = this.mOffset.X + i + 1;
							this.mCanBuffer[this.pIndexOf(tVec)] = CanvasName[i];
						}
						//Dodaj mali '-' da izgleda kul
						tVec.X = this.mOffset.X;
						this.mCanBuffer[this.pIndexOf(tVec)] = this.mTopBottomChar;
						tOff = CanvasName.Length + 1;
					}

					//Postavi '-' kako bi se mogao vidjeti obrub canvas-a
					for (int x = this.mOffset.X + tOff; x < this.mRealSize.X - (mOffset.X / 2); ++x)
					{
						tVec.X = x;
						this.mCanBuffer[this.pIndexOf(tVec)] = this.mTopBottomChar;
					}
				}
				//Ukoliko nismo kod samog vrha ili dna postavi samo rubove
				else if (y > this.mOffset.Y - (this.mOffset.Y / 2))
				{
					tVec.X = this.mOffset.X - 1;
					this.mCanBuffer[this.pIndexOf(tVec)] = this.mLeftRightChar;

					tVec.X += this.Size.X + 1;
					this.mCanBuffer[this.pIndexOf(tVec)] = this.mLeftRightChar;
				}
				//Metoda SetCharAt se ovdje nemoze koristiti jer ovaj dio koda pise po "nedostupnom" dijelu
				//Okvir se nalazi na (-1, y) i (x, -1) sto se tice SetCharAt; On za takve koordinate vraca Exception

			}

		}

		//Public metode
		public void SetCharAt(char C, int x, int y)
		{
			this.pCheckExternalAccess(x, y);
			this.mCanBuffer[this.pIndexOf(x + this.mOffset.X, y + this.mOffset.Y)] = C;
		}
		public void SetCharAt(char C, Vec2i Index) => this.SetCharAt(C, Index.X, Index.Y);


		public char GetCharAt(int x, int y)
		{
			this.pCheckExternalAccess(x, y);
			return this.mCanBuffer[this.pIndexOf(x + this.mOffset.X, y + this.mOffset.Y)];
		}
		public char GetCharAt(Vec2i Index) => this.GetCharAt(Index.X, Index.Y);

		public void Clear(char C = ' ')
		{
			for (int y = 0; y < this.Size.Y; ++y)
			{
				for (int x = 0; x < this.Size.X; ++x)
				{
					this.SetCharAt(C, x, y);
				}
			}
		}

		public void Draw()
		{
			//Draw je brzi od ToString metode, jer se ne mora u pozadini pretvarati char[] u string
			Console.SetCursorPosition(0, 0);
			Console.Write(this.mCanBuffer);
		}

		//Properties
		public Vec2i Size { get; }

		//Override od Base
		public override string ToString()
		{
			return new string(this.mCanBuffer);
		}

		//Private metode
		void pCheckExternalAccess(int x, int y)
		{
			if (x < 0 || x >= this.Size.X || y < 0 || y >= this.Size.Y)
			{
				throw new Exception($"Trying to access ({x}, {y}) while size is {this.Size}!");
			}
		}
		int pIndexOf(Vec2i V)
		{
			return this.pIndexOf(V.X, V.Y);
		}
		int pIndexOf(int x, int y)
		{
			return y * this.mRealSize.X + x;
		}

		//Privtes
		char mTopBottomChar = '-';
		char mLeftRightChar = '|';

		char[] mCanBuffer;
		Vec2i mRealSize;

		Vec2i mOffset = new Vec2i(4, 2);

	}
}
