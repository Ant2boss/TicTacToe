using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
	[FirstLaunch]
	
	Zadatak ove klase je da se pobrine da sve vanjske datoteke budu na svom mjestu, a ako
	ih nema (s obzirmo da su u pitanju jako male datoteke) da ih stvori kako bi ostale kolekcije mogle raditi
	ispravno

	Ovdje je, jer TextArt klasa nudi mogucnost vanjske pohrane i [ucitavanja] <- (uglavnom zbog toga :D )

 */

namespace KrizicKruzic
{
	public static class FirstLaunch
	{
		public static string BigXFile = "TA_BigX";
		public static string BigOFile = "TA_BigO";
		public static string BigPlayerFile = "TA_BigPlayer";

		public static void Initialize() {

			try
			{
				//Pokusaj otvoriti datoteku (Nije bitno koju, jer sve {trebaju} biti tu)
				FileStream tF = new FileStream($"{BigXFile}.txt", FileMode.Open);
				tF.Close();
			}
			catch
			{
				//Ukoliko otvaranje nije uspjelo stvori sve datoteke
				TextArt BigX = new TextArt(3, 3);

				BigX.SetCharAt('\\', 0, 0);
				BigX.SetCharAt('/', 2, 0);
				BigX.SetCharAt('X', 1, 1);
				BigX.SetCharAt('/', 0, 2);
				BigX.SetCharAt('\\', 2, 2);

				BigX.SaveToFile(BigXFile);

				TextArt BigO = new TextArt(3, 3);
				BigO.SetCharAt('/', 0, 0);
				BigO.SetCharAt('-', 1, 0);
				BigO.SetCharAt('\\', 2, 0);
				BigO.SetCharAt('|', 0, 1);
				BigO.SetCharAt('|', 2, 1);
				BigO.SetCharAt('\\', 0, 2);
				BigO.SetCharAt('-', 1, 2);
				BigO.SetCharAt('/', 2, 2);

				BigO.SaveToFile(BigOFile);

				TextArt BigPlayer = new TextArt(5, 5);
				BigPlayer.SetCharAt('#', 1, 1);
				BigPlayer.SetCharAt('#', 2, 1);
				BigPlayer.SetCharAt('#', 3, 1);
				BigPlayer.SetCharAt('#', 1, 2);
				BigPlayer.SetCharAt('#', 2, 2);
				BigPlayer.SetCharAt('#', 3, 2);
				BigPlayer.SetCharAt('#', 2, 3);
				BigPlayer.SetCharAt('#', 0, 4);
				BigPlayer.SetCharAt('#', 1, 4);
				BigPlayer.SetCharAt('#', 2, 4);
				BigPlayer.SetCharAt('#', 3, 4);
				BigPlayer.SetCharAt('#', 4, 4);

				BigPlayer.SaveToFile(BigPlayerFile);
			}
		}
	}
}
