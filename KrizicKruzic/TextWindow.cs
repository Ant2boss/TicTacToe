using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
	[TextWindow]

	TextWindow je prozor zaduzen za prikazivanje teksta unutar okvira CanvasWindow-a
 */

namespace KrizicKruzic
{
	class TextWindow : CanvasWindow
	{
		//Konstruktori
		public TextWindow(Canvas MainCan, Vec2i Size, Vec2i Pos, string WindowName = "")
			: base(MainCan, Size, Pos, WindowName) { }

		public TextWindow(Canvas MainCan, int xSize, int ySize, int xPos, int yPos, string WindowName = "")
			: base(MainCan, xSize, ySize, xPos, yPos, WindowName) { }

		//Public metode
		public void SetPosition(int x, int y) => base.prSetPos(x, y);
		public void SetPosition(Vec2i Pos) => base.prSetPos(Pos.X, Pos.Y);

		public void SetCursorPosition(int x, int y)
		{
			this.mCursorPos.SetUp(x, y);
			this.pCheckAndModifyCursor();
		}
		public void SetCursorPosition(Vec2i Pos) => this.SetCursorPosition(Pos.X, Pos.Y);

		public void AddText(string Text)
		{
			for (int i = 0; i < Text.Length; ++i)
			{
				if (Text[i] == '\n')
				{
					this.SetCursorPosition(0, this.mCursorPos.Y + 1);
					continue;
				}
				base.prSetCharAt(Text[i], this.mCursorPos);
				this.pOffsetCursor();
			}
		}

		public void ClearText()
		{
			for (int y = 0; y < base.Size.Y; ++y)
			{
				for (int x = 0; x < base.Size.X; ++x)
				{
					base.prSetCharAt(' ', x, y);
				}
			}
			this.SetCursorPosition(0, 0);
		}

		public void Update() => base.prAttachToCanvas();

		//Public props
		public Vec2i CursorPosition { get => mCursorPos.GetCopy(); }

		//Private metode
		void pOffsetCursor()
		{
			++this.mCursorPos.X;
			this.pCheckAndModifyCursor();
		}
		void pCheckAndModifyCursor()
		{
			if (this.mCursorPos.X >= base.Size.X)
			{
				this.mCursorPos.X = 0;
				++this.mCursorPos.Y;
			}
			if (this.mCursorPos.Y >= base.Size.Y)
			{
				this.mCursorPos.SetUp(0, 0);
			}
		}

		//Privates
		Vec2i mCursorPos = new Vec2i(0, 0);
	}
}
