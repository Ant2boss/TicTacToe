using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/*
	[MenuWindow]
	Sluzi za izradu jednostavnog izbornika omedenog prozorom

	Nudi dodavanje opcija, odabiranje opcija, te par Utilit-a glede izbornika

	Nije autonomna, ali nude metode da se koristi uz minimalno "razmisljanja"

 */

namespace KrizicKruzic
{
	class MenuWindow : CanvasWindow
	{
		//Konstruktori
		public MenuWindow(Canvas MainCan, Vec2i Size, Vec2i Pos, string WindowName = "")
			: base(MainCan, Size, Pos, WindowName) { }
		public MenuWindow(Canvas MainCan, int xSize, int ySize, int xPos, int yPos, string WindowName = "")
			: base(MainCan, xSize, ySize, xPos, yPos, WindowName) { }

		//Public metode
		public void SetPosition(int x, int y) => base.prSetPos(x, y);
		public void SetPosition(Vec2i Pos) => base.prSetPos(Pos);

		public int AddMenuOption(string Option)
		{
			this.mOptionsBuffer.Add(Option);
			this.pDisplayOptions();

			//Vrati nazad index dodane opcije (primjer korisnosti moze se vidjeti u [GameLoop])
			return this.GetOptionCount() - 1;
		}
		public void ModifyMenuOptionAt(int index, string NewOption)
		{
			if (index < 0 || index >= this.GetOptionCount())
				throw new Exception($"Error trying to modify option! Cannot modify option of Index {index}," +
					$" while size is {this.GetOptionCount()}");
			
			this.pAddLineToWindow((index * 3) + 1, ' ');
			this.mOptionsBuffer[index] = NewOption;
			this.pDisplayOptions();
		}

		public void SetMenuInFocus()
		{
			this.mCurrentIndex = 0;
			this.pDisplayOptions();
		}
		public void RemoveMenuFromFocus()
		{
			this.mCurrentIndex = -1;
			this.pDisplayOptions();
		}

		public int MoveMenuDOWN()
		{
			if (this.mCurrentIndex == -1)
				return -1;

			++this.mCurrentIndex;
			if (this.mCurrentIndex >= this.GetOptionCount())
				this.mCurrentIndex = this.LoopMode ? (0) : (this.mCurrentIndex - 1);

			this.pDisplayOptions();
			return this.mCurrentIndex;
		}
		public int MoveMenuUP()
		{
			if (this.mCurrentIndex == -1)
				return -1;

			--this.mCurrentIndex;
			if (this.mCurrentIndex < 0)
				this.mCurrentIndex = this.LoopMode ? (this.GetOptionCount() - 1) : (0);

			this.pDisplayOptions();

			return this.mCurrentIndex;
		}

		public int GetOptionCount() => this.mOptionsBuffer.Count;
		public int GetSelectedIndex() => this.mCurrentIndex;
		public string GetSelectedOption() => this.GetOptionAt(this.mCurrentIndex);
		public string GetOptionAt(int index) => (index < 0 || index >= this.mOptionsBuffer.Count) ? ("") : (this.mOptionsBuffer[index]);

		public void ClearOptions()
		{
			this.mOptionsBuffer.Clear();
			this.mCurrentIndex = -1;
			this.pDisplayOptions();
		}

		public void Update() => base.prAttachToCanvas();

		//Private methods
		void pDisplayOptions()
		{
			for (int i = 0; i < this.GetOptionCount(); ++i)
			{
				//Svaku opciju koja je dodana potrebno je poljepiti na ekran
				this.pAddWordToWindow(this.mOptionsBuffer[i], 0, (i * 3) + 1);
				//Odabranu opciju okruzi sa --- kako bi se moglo odrediti sto je oznaceno
				if (this.mCurrentIndex == i)
				{
					this.pAddLineToWindow(i * 3, mLineSelected);
					this.pAddLineToWindow((i * 3) + 2, mLineSelected);
				}
				else
				{
					this.pAddLineToWindow(i * 3, ' ');
					this.pAddLineToWindow((i * 3) + 2, ' ');
				}

			}
		}
		void pAddWordToWindow(string str, int x, int y)
		{
			if (y >= base.Size.Y)
				return;

			for (int tx = 0; tx < str.Length; ++tx)
			{
				if (tx + x >= base.Size.X)
					continue;

				base.prSetCharAt(str[tx], x + tx, y);
			}
		}
		//Ova metoda generira rijec; u ovom slucaju ta rijec je jedan znak koji se razvuce preko cijelog [CanvasWindow]
		void pAddLineToWindow(int y, char LineContent)
		{
			char[] line = new char[base.Size.X];	//<- Neznam kako da resizeam string stoga je ovo brze

			for (int i = 0; i < line.Length; ++i)
				line[i] = LineContent;

			this.pAddWordToWindow(new string(line), 0, y);
		}

		//Property
		public bool LoopMode { set; get; }	//LoopMode odreduje da ako se pozove DOWN dok je na dnu da ode na vrh

		//Privates
		IList<string> mOptionsBuffer = new List<string>();
		int mCurrentIndex = -1;

		const char mLineSelected = '-';
	}
}
