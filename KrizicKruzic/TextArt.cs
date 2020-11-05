using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
	[TextArt]
	
	Klasa TextArt sluzi za cuvanje Info o "slici" koja se moze zalijepiti na Canvas
	
	Ideja je da se ucita iz datoteke, te da se lijepi okolo po ekranu preko prAttachTextArt() metode na [CanvasWindow]
	
 */

namespace KrizicKruzic
{
	class TextArt
	{
		//Konstruktori
		public TextArt(int xSize, int ySize) {
			this.mBufferSize = new Vec2i(xSize, ySize);
			this.mBuffer = new char[xSize * ySize];
		}
		public TextArt(string FileName) {
			this.LoadFromFile(FileName);
		}

		//Public metode
		public void SetCharAt(char C, int x, int y) {
			if (this.pOutOfBufferCheck(x, y))
				throw new Exception($"TextArt out of bounds! Trying to access ({x}, {y}) while size is {this.mBufferSize}");

			this.mBuffer[this.pIndexOf(x, y)] = C;
		}
		public void SetCharAt(char C, Vec2i V) => this.SetCharAt(C, V.X, V.Y);

		public char GetCharAt(int x, int y) {
			if (this.pOutOfBufferCheck(x, y))
				throw new Exception($"TextArt out of bounds! Trying to access ({x}, {y}) while size is {this.mBufferSize}");
			
			return this.mBuffer[this.pIndexOf(x, y)];
		}
		public char GetCharAt(Vec2i Index) => this.GetCharAt(Index.X, Index.Y);

		public void Clear(char C = ' ') {
			for (int i = 0; i < this.mBuffer.Length; ++i)
				this.mBuffer[i] = C;
		}

		public void SaveToFile(string FileName) {
			File.WriteAllText($"{FileName}.txt", $"{this.mBufferSize.X}\n{this.mBufferSize.Y}\n{new string(this.mBuffer)}");
		}
		public void LoadFromFile(string FileName) {
			string toParse;
			//Provjera ako postoji datoteka
			try
			{
				toParse = File.ReadAllText($"{FileName}.txt");
			}
			catch {
				throw new Exception($"Error opening File! {FileName}.txt doesn't exist!");
			}

			string[] parts = toParse.Split('\n');
			if (parts.Length != 3)
				throw new Exception($"Error parsing from file! {FileName}.txt has incorrect formating!");

			try
			{
				this.mBufferSize = new Vec2i(int.Parse(parts[0]), int.Parse(parts[1]));
			}
			catch {
				throw new Exception($"Error parsing from file! Excpecting 2 integers in file!");
			}

			this.mBuffer = new char[Size.X * Size.Y];
			for (int i = 0; i < this.mBuffer.Length; ++i) {
				this.mBuffer[i] = parts[2][i];	
				//Postoji sansa da ovo nece proci, medutim nakon 4 iznimno specifice provjere ako se provukla netocna datoteka onda stvarno
			}
		}

		//Private metode
		bool pOutOfBufferCheck(int x, int y) => (x < 0 || x >= this.mBufferSize.Y || y < 0 || y >= this.mBufferSize.Y);
		int pIndexOf(int x, int y) {
			return y * this.mBufferSize.X + x;
		}
		int pIndexOf(Vec2i Index) => this.pIndexOf(Index.X, Index.Y);

		//Properties
		public Vec2i Size { get => mBufferSize.GetCopy(); }

		//Privates
		char[] mBuffer;
		Vec2i mBufferSize;
	}
}
