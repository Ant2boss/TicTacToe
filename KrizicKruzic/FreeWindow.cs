using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

	/*
		[FreeWindow]
		Klasa koja je prva nastala da nasljeduje [CanvasWindow]
		
		Ideja ove klase je da nudi nekakva Generic-Window entitet koji nudi van maksimalnu korisnost CanvasWindow bez nekakve
		ekstra modifikacije

		Originalno je poslužila za testiranje, međutim u samoj igri je iskorištena za drzanje "slika" / [TextArt] - a
	 */

namespace KrizicKruzic
{
	class FreeWindow : CanvasWindow
	{
		//Konstruktori
		public FreeWindow(Canvas MainCan, Vec2i Size, string WinName = "")
			: base(MainCan, Size, new Vec2i(0, 0), WinName) { }
		public FreeWindow(Canvas MainCan, int xSize, int ySize, string WinName = "")
			: base(MainCan, new Vec2i(xSize, ySize), new Vec2i(0, 0), WinName) { }
		public FreeWindow(Canvas MainCan, Vec2i Size, Vec2i Pos, string WinName = "")
			: base(MainCan, Size, Pos, WinName) { }
		public FreeWindow(Canvas MainCan, int xSize, int ySize, int xPos, int yPos, string WinName = "")
			: base(MainCan, new Vec2i(xSize, ySize), new Vec2i(xPos, yPos), WinName) { }

		//Public metode
		public void SetPosition(int x, int y) => base.prSetPos(x, y);
		public void SetPosition(Vec2i Pos) => base.prSetPos(Pos);
		public void SetCharAt(char C, int x, int y) => base.prSetCharAt(C, x, y);
		public void SetCharAt(char C, Vec2i Index) => base.prSetCharAt(C, Index);
		public char GetAt(int x, int y) => base.prGetCharAt(x, y);
		public char GetAt(Vec2i Index) => base.prGetCharAt(Index);

		public void MoveWindow(int x, int y) => base.prSetPos(base.Position.X + x, base.Position.Y + y);
		public void MoveWindow(Vec2i Off) => base.prSetPos(base.Position + Off);

		public void AttachTextArt(TextArt TxtArt, int x, int y) => base.prAttachTextArt(TxtArt, x, y);
		public void AttachTextArt(TextArt TxtArt, Vec2i Pos) => base.prAttachTextArt(TxtArt, Pos.X, Pos.Y);

		public void ClearWindow(char C) => base.prClear(C);

		public void Update() => base.prAttachToCanvas();
	}
}
