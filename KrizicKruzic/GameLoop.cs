using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*
	[GameLoop]

	Ultimativna kulminacija svih klasi i metodica koje su izradene

	Igra krizic kruzic :D 

	(Nadam se)
 */

namespace KrizicKruzic
{
	class GameLoop
	{
		public GameLoop(Canvas GameCanvas)
		{
			this.mGameCan = GameCanvas;
		}

		public bool DisplayMainMenuAndWaitForStart()
		{

			this.mGameCan.Clear();

			TextWindow GreetingText = new TextWindow(mGameCan, mGameCan.Size.X, 7, 0, 0);

			GreetingText.AddText("Welcome to TicTacToe, find a friend and get ready to play a Game!\n\nX| |O\n-+-+-\nX|O|\n-+-+-\n | |");

			MenuWindow StartMenu = new MenuWindow(
				mGameCan,
				mGameCan.Size.X / 3, 15,
				(this.mGameCan.Size.X / 2) - ((mGameCan.Size.X / 3) / 2),
				GreetingText.Size.Y + 3,
				"Main menu"
				);

			int indexStartGame = StartMenu.AddMenuOption("Start game");
			int indexTutorial = StartMenu.AddMenuOption("How to play");
			int indexExitGame = StartMenu.AddMenuOption("Exit game");

			StartMenu.SetMenuInFocus();
			StartMenu.LoopMode = true;

			TextWindow SelectedOption = new TextWindow(
				mGameCan,
				mGameCan.Size.X / 3,
				5,
				StartMenu.Position.X,
				GreetingText.Size.Y + 14,
				"[Selected]"
				);

			char com;
			int CurrentIndex = StartMenu.GetSelectedIndex();

			while (true)
			{
				GreetingText.Update();
				StartMenu.Update();

				SelectedOption.ClearText();
				if (CurrentIndex == indexStartGame)
					SelectedOption.AddText("Start a new game of\nTic-Tac-Toe and prove your\ndominance!");
				else if (CurrentIndex == indexTutorial)
					SelectedOption.AddText("Check out the basics\nof how to play Tic-Tac-Toe\nusing this application!");
				else if (CurrentIndex == indexExitGame)
					SelectedOption.AddText("Leave the game");
				else
					SelectedOption.AddText("Wait! How did you get here! COME BACK HERE!! HEY STOP RESISTING!!!!");

				SelectedOption.Update();

				this.mGameCan.Draw();

				com = Console.ReadKey(true).KeyChar;
				if (com == 'W' || com == 'w')
				{
					CurrentIndex = StartMenu.MoveMenuUP();
				}
				else if (com == 's' || com == 'S')
				{
					CurrentIndex = StartMenu.MoveMenuDOWN();
				}
				else if (com == ' ')
				{
					if (CurrentIndex == indexStartGame)
					{
						return true;
					}
					else if (CurrentIndex == indexTutorial)
					{
						StartMenu.SetPosition(2, StartMenu.Position.Y);
						SelectedOption.SetPosition(2, SelectedOption.Position.Y);

						this.mGameCan.Clear();

						TextWindow Tutorial = new TextWindow(
							this.mGameCan,
							mGameCan.Size.X - StartMenu.Size.X - StartMenu.Position.X - 5,
							StartMenu.Size.Y + 1,
							StartMenu.Position.X + StartMenu.Size.X + 3,
							StartMenu.Position.Y,
							"Tutorial"
							);

						Tutorial.AddText("A game of TicTacToe is played between 2 players!\n\n");
						Tutorial.AddText("On the right it is displayd whoose turn it is!\n\n");
						Tutorial.AddText("The player whooes turn it is moves using:\n\n");
						Tutorial.AddText("   [W]\n[A][S][D]\n\n");
						Tutorial.AddText("The tile which is selected will be highlited!\n");
						Tutorial.AddText("Press [ Space ] to confirm\nthe placement of your symbol!\n\n");
						Tutorial.AddText("The first player to make a 3 in a row\nof their symbol is automaticaly decalred winner!");
					}
					else if (CurrentIndex == indexExitGame)
					{
						return false;
					}
				}


			}
		}

		public void ExecuteGame()
		{
			mGameCan.Clear();

			TextArt tPlayerArt = new TextArt(FirstLaunch.BigPlayerFile);

			int xTotalSize = (tPlayerArt.Size.X + 5) * 2 + TicTacBoard.xSize;

			TextWindow Helpdesk = new TextWindow(mGameCan,
				xTotalSize,
				3,
				(mGameCan.Size.X / 2) - (xTotalSize / 2),
				2,
				"Info"
				);
			FreeWindow WinPlayer1 = new FreeWindow(mGameCan,
				tPlayerArt.Size.X + 2,
				tPlayerArt.Size.Y + 2,
				Helpdesk.Position.X,
				Helpdesk.Position.Y + Helpdesk.Size.Y + 5,
				"P1--X"
				);
			TicTacBoard WindBorad = new TicTacBoard(mGameCan,
				WinPlayer1.Position.X + WinPlayer1.Size.X + 3,
				WinPlayer1.Position.Y - 3
				);
			FreeWindow WinPlayer2 = new FreeWindow(mGameCan,
				tPlayerArt.Size.X + 2,
				tPlayerArt.Size.Y + 2,
				WindBorad.Position.X + WindBorad.Size.X + 3,
				WinPlayer1.Position.Y,
				"P2--O"
				);

			TextArt tBigX = new TextArt(FirstLaunch.BigXFile);
			FreeWindow WinBigX = new FreeWindow(mGameCan,
				tBigX.Size.X + 2,
				tBigX.Size.Y + 2,
				WinPlayer1.Position.X + 1,
				WinPlayer1.Position.Y + WinPlayer1.Size.Y + 2
				);
			WinBigX.AttachTextArt(tBigX, 1, 1);

			TextArt tBigO = new TextArt(FirstLaunch.BigOFile);
			FreeWindow WinBigO = new FreeWindow(mGameCan,
				tBigO.Size.X + 2,
				tBigO.Size.Y + 2,
				WinPlayer2.Position.X + 1,
				WinPlayer2.Position.Y + WinPlayer2.Size.Y + 2
				);
			WinBigO.AttachTextArt(tBigO, 1, 1);


			WinPlayer1.AttachTextArt(tPlayerArt, 1, 1);
			WinPlayer2.AttachTextArt(tPlayerArt, 1, 1);

			Random tr = new Random();

			mWhooseTurn = (tr.Next(1, 10) >= 5) ? ('O') : ('X');
			pDisplayTurnInfo(Helpdesk);

			Vec2i tSelTile = new Vec2i(0, 0);
			char com;

			bool tB;
			int WhoWon;

			WindBorad.SelectTile(tSelTile);


			//GameLoop
			while (true)
			{

				this.pHighlightCurrentPlayer(WinPlayer1, WinPlayer2, '.', ' ', '#');

				Helpdesk.Update();
				WindBorad.Update();
				WinPlayer1.Update();
				WinPlayer2.Update();

				mGameCan.Draw();

				com = Console.ReadKey(true).KeyChar;

				switch (com)
				{
					case 'w':
					case 'W':
						WindBorad.MoveSelection(Direction.UP);
						break;
					case 's':
					case 'S':
						WindBorad.MoveSelection(Direction.DOWN);
						break;
					case 'a':
					case 'A':
						WindBorad.MoveSelection(Direction.LEFT);
						break;
					case 'd':
					case 'D':
						WindBorad.MoveSelection(Direction.RIGHT);
						break;
					case ' ':
						tSelTile = WindBorad.GetSelectedTile();
						tB = false;
						if (mWhooseTurn == 'O')
						{
							if (WindBorad.PlaceOOnSelected())
							{
								mWhooseTurn = 'X';
								WindBorad.SelectTile(tSelTile);
								tB = true;
							}
						}
						else if (mWhooseTurn == 'X')
						{
							if (WindBorad.PlaceXOnSelected())
							{
								mWhooseTurn = 'O';
								WindBorad.SelectTile(tSelTile);
								tB = true;
							}
						}

						if (tB)
						{
							this.pDisplayTurnInfo(Helpdesk);
						}

						if (tB && this.pIsGameOver(WindBorad, out WhoWon))
						{

							Helpdesk.ClearText();
							mGameCan.Clear();

							Helpdesk.Update();
							WindBorad.Update();

							if (WhoWon == 0)
							{
								Helpdesk.AddText("\nBoard is full! It's a Tie!");
							}
							else
							{
								Helpdesk.AddText($"\nGame over! Player {((WhoWon == 1) ? ('X') : ('O'))} won!");

								if (WhoWon == 1)
								{
									WinPlayer1.Update();
									WinBigX.Update();
								}
								else
								{
									WinPlayer2.Update();
									WinBigO.Update();
								}
							}

							mGameCan.Draw();
							Console.ReadKey(true);
							return;
						}
						break;
					default:
						break;

				}

			}
		}

		private void pDisplayTurnInfo(TextWindow Helpdesk)
		{
			Helpdesk.ClearText();
			Helpdesk.AddText($"\nIt is currently {((mWhooseTurn == 'O') ? ("Player 2 [O] ") : ("Player 1 [X] "))}s' turn!");
		}

		private void pHighlightCurrentPlayer(FreeWindow xPlayer, FreeWindow oPlayer, char Highlight, char NonHighlight, char IgnoreChar)
		{
			for (int y = 0; y < xPlayer.Size.Y; ++y)
			{
				for (int x = 0; x < xPlayer.Size.X; ++x)
				{
					if (xPlayer.GetAt(x, y) != IgnoreChar)
					{
						if (mWhooseTurn == 'X')
							xPlayer.SetCharAt(Highlight, x, y);
						else
							xPlayer.SetCharAt(NonHighlight, x, y);
					}
					if (oPlayer.GetAt(x, y) != IgnoreChar)
					{
						if (mWhooseTurn == 'O')
							oPlayer.SetCharAt(Highlight, x, y);
						else
							oPlayer.SetCharAt(NonHighlight, x, y);
					}
				}
			}
		}

		private bool pIsGameOver(TicTacBoard ThicTac, out int StatusMessage)
		{
			//StatusMessage
			//0 -> Board full (no winners) / 1 -> X Wins / 2 ->O Wins

			char tCheck;
			int tCount;

			//Provjerava se svaki redak
			for (int y = 0; y < 3; ++y)
			{
				tCount = 0;
				tCheck = ThicTac.GetStateAt(0, y);
				//Ako na pocetku retka postoji znak
				if (tCheck == 'O' || tCheck == 'X')
				{
					++tCount;
					//Provjeri se ako u sljedeca dva retka se nalazi isti taj znak
					for (int x = 1; x < 3; ++x)
					{
						if (ThicTac.GetStateAt(x, y) == tCheck)
							++tCount;
					}
					//Ako su prebrojana 3 ista znaka
					if (tCount == 3)
					{
						ThicTac.ClearBoard();
						for (int x = 0; x < 3; ++x)
						{
							if (tCheck == 'O')
								ThicTac.SelectAndPlaceO(x, y);
							else
								ThicTac.SelectAndPlaceX(x, y);
						}
						StatusMessage = (tCheck == 'X') ? (1) : (2);
						return true;
					}
				}


			}

			//Provjerava se svaki stupac
			for (int x = 0; x < 3; ++x)
			{
				tCount = 0;
				tCheck = ThicTac.GetStateAt(x, 0);
				//Ako na pocetku retka postoji znak
				if (tCheck == 'O' || tCheck == 'X')
				{
					++tCount;
					//Provjeri se ako u sljedeca dva retka se nalazi isti taj znak
					for (int y = 1; y < 3; ++y)
					{
						if (ThicTac.GetStateAt(x, y) == tCheck)
							++tCount;
					}
					//Ako su prebrojana 3 ista znaka
					if (tCount == 3)
					{
						ThicTac.ClearBoard();
						for (int y = 0; y < 3; ++y)
						{
							if (tCheck == 'O')
								ThicTac.SelectAndPlaceO(x, y);
							else
								ThicTac.SelectAndPlaceX(x, y);
						}
						StatusMessage = (tCheck == 'X') ? (1) : (2);
						return true;
					}
				}


			}

			//Provjera \ dijagonale
			tCheck = ThicTac.GetStateAt(0, 0);
			if (tCheck == 'X' || tCheck == 'O')
			{
				if (ThicTac.GetStateAt(0, 0) == ThicTac.GetStateAt(1, 1) && ThicTac.GetStateAt(1, 1) == ThicTac.GetStateAt(2, 2))
				{
					ThicTac.ClearBoard();
					if (tCheck == 'X')
					{
						ThicTac.SelectAndPlaceX(0, 0);
						ThicTac.SelectAndPlaceX(1, 1);
						ThicTac.SelectAndPlaceX(2, 2);
					}
					else
					{
						ThicTac.SelectAndPlaceO(0, 0);
						ThicTac.SelectAndPlaceO(1, 1);
						ThicTac.SelectAndPlaceO(2, 2);
					}
					StatusMessage = (tCheck == 'X') ? (1) : (2);
					return true;
				}
			}

			//Provjera / dijagonale
			tCheck = ThicTac.GetStateAt(0, 2);
			if (tCheck == 'X' || tCheck == 'O')
			{
				if (ThicTac.GetStateAt(0, 2) == ThicTac.GetStateAt(1, 1) && ThicTac.GetStateAt(1, 1) == ThicTac.GetStateAt(2, 0))
				{
					ThicTac.ClearBoard();
					if (tCheck == 'X')
					{
						ThicTac.SelectAndPlaceX(0, 2);
						ThicTac.SelectAndPlaceX(1, 1);
						ThicTac.SelectAndPlaceX(2, 0);
					}
					else
					{
						ThicTac.SelectAndPlaceO(0, 2);
						ThicTac.SelectAndPlaceO(1, 1);
						ThicTac.SelectAndPlaceO(2, 0);
					}

					StatusMessage = (tCheck == 'X') ? (1) : (2);
					return true;
				}
			}

			//Provjera pune ploce
			tCount = 0;
			for (int y = 0; y < 3; ++y)
			{
				for (int x = 0; x < 3; ++x)
				{
					tCheck = ThicTac.GetStateAt(x, y);
					if (tCheck == 'X' || tCheck == 'O')
						++tCount;
				}
			}
			if (tCount == 9)
			{
				StatusMessage = 0;
				return true;
			}

			StatusMessage = -1;
			return false;
		}

		Canvas mGameCan;
		char mWhooseTurn = ' ';
	}
}
