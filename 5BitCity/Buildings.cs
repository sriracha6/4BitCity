using System;

namespace _4BitCity
{
	public static class Buildings
	{
		public static Building HousingComplex = new Building("Housing Complex", () => { Player.Population += 10; }, -1, '#', ConsoleColor.Gray, 1000, "+10 Pop");
		public static Building Condo = new Building("Condo", () => { Player.Population += 8; Player.Happiness += 2; }, -1, '\\', ConsoleColor.DarkGray, 5000, "+8 Pop, +2 ☺");
		public static Building Apartments = new Building("Apartments", () => { Player.Population += 3; Player.Happiness--; }, -1, '<', ConsoleColor.Gray, 375, "+3 Pop, -1 ☺");
		public static Building Park = new Building("Park", () => { Player.Happiness += 15; }, -1, '%', ConsoleColor.Green, 750, "+15 ☺");
		public static Building School = new Building("School", () => { Player.Money += (int)(Player.Population * 0.2f); }, 1, '"', ConsoleColor.Red, 2500, "+20% Tax");
		public static Building CoalPlant = new Building("Coal Plant", () => { Player.Money += (int)(Player.Population * 0.1f); Player.Happiness -= 10; }, 1, '!', ConsoleColor.Red, 2500, "-10% ☺ +10% Tax");

		public static Building[] AllBuildings = { HousingComplex, Condo, Apartments, Park, School, CoalPlant };
	}
}
