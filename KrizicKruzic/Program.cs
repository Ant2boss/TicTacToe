using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KrizicKruzic
{
	class Program
	{
		static void Main(string[] args)
		{
			FirstLaunch.Initialize();

			//MovingWindowExample();

			//TicTacExample();

			//MenuAndTextExample();

			Canvas Can = new Canvas(87, 32, "Tic-Tac-Toe");

			GameLoop GameOfTicTacToe = new GameLoop(Can);

			//Vrtimo se sve dok player ne izade
			while (true)
			{
				if (GameOfTicTacToe.DisplayMainMenuAndWaitForStart())
				{
					GameOfTicTacToe.ExecuteGame();
				}
				else
					break;
			}

		}

		private static void MenuAndTextExample()
		{
			Canvas Can = new Canvas(86, 32, "Testing-me");

			MenuWindow Menu = new MenuWindow(Can, 32, 16, 10, 10, "Menu-Test");

			Menu.AddMenuOption("123213123");
			Menu.AddMenuOption("Hello");
			Menu.AddMenuOption("Whats up");
			Menu.AddMenuOption("My nigga");

			Menu.SetMenuInFocus();
			Menu.LoopMode = false;

			int LoopOptionIndex = Menu.AddMenuOption($"LoopMode: {Menu.LoopMode}");
			int CurrIndex = Menu.GetSelectedIndex();

			TextWindow Text = new TextWindow(Can, 32, 8, 50, 5, "Text-test");
			Text.AddText("Hello world!\nsaiudgaskuzgdj\nashgfdjahsgdjhagsdjhasgd");

			char com;

			while (true)
			{
				Menu.Update();
				Can.Draw();

				com = Console.ReadKey(true).KeyChar;
				if (com == 'w' || com == 'W')
					CurrIndex = Menu.MoveMenuUP();
				else if (com == 's' || com == 'S')
					CurrIndex = Menu.MoveMenuDOWN();
				else if (com == ' ')
				{
					if (LoopOptionIndex == CurrIndex)
					{
						Menu.LoopMode = !Menu.LoopMode;
						Menu.ModifyMenuOptionAt(LoopOptionIndex, $"LoopMode: {Menu.LoopMode}");
					}
					else break;
				}

			}
		}

		private static void TicTacExample()
		{
			Canvas Can = new Canvas(86, 32, "Testing-purposes");

			TicTacBoard Board = new TicTacBoard(Can, 5, 5);

			Board.SelectAndPlaceO(2, 2);
			Board.SelectAndPlaceO(1, 1);
			Board.SelectAndPlaceO(0, 0);
			//Board.SelectTile(2, 1);

			Can.Draw();
			Console.ReadLine();
		}

		private static void MovingWindowExample()
		{
			Canvas Can = new Canvas(86, 32, "Example");

			FreeWindow MyWindow = new FreeWindow(Can, 32, 16, 10, 10, "Mirica");

			for (int y = 1; y < MyWindow.Size.Y - 1; ++y)
			{
				for (int x = 1; x < MyWindow.Size.X - 1; ++x)
				{
					MyWindow.SetCharAt((char)((x % 10) + '0'), x, y);
				}
			}

			FreeWindow SomeOtherWindow = new FreeWindow(Can, 32, 16, 5, 5, "Im just sitting here");

			TextArt BigO = new TextArt(FirstLaunch.BigOFile);
			TextArt BigPlayer = new TextArt(FirstLaunch.BigPlayerFile);

			SomeOtherWindow.AttachTextArt(BigO, 3, 3);
			SomeOtherWindow.AttachTextArt(BigO, 30, 14);

			SomeOtherWindow.AttachTextArt(BigPlayer, 25, 0);

			Can.SetCharAt('#', 0, 0);
			Can.SetCharAt('#', Can.Size.X - 1, 0);
			Can.SetCharAt('#', 0, Can.Size.Y - 1);
			Can.SetCharAt('#', Can.Size.X - 1, Can.Size.Y - 1);


			while (true)
			{
				SomeOtherWindow.Update();
				MyWindow.Update();

				Can.Draw();

				char com = Console.ReadKey(true).KeyChar;

				if (com == 'w' || com == 'W') { MyWindow.MoveWindow(0, -1); }
				else if (com == 's' || com == 'S') { MyWindow.MoveWindow(0, 1); }
				else if (com == 'a' || com == 'A') { MyWindow.MoveWindow(-1, 0); }
				else if (com == 'd' || com == 'D') { MyWindow.MoveWindow(1, 0); }
				else { break; }


			}
		}
	}
}
