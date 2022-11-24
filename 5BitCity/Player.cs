using System;

namespace _4BitCity
{
	public static class Player
	{
		public static int Happiness = 50;
		public static int Money = 5000;
		public static int Population = 15;
		public static Building[,] Map = new Building[4,4];
		public static int[,] Age = new int[4,4];

		public static int Year = 2022; // tick count
	}
}
