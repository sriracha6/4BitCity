using System;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace _4BitCity
{
	using static Functions;
	public enum Mode { None, InMessage, BuildingPlace, BuildingRemove, BuildingView }

	public static class Game
	{
		public static Mode CurMode;
		public static Building BuildingToPlace;
		public static Event CurrentEvent;

		public static void StartNewGame()
		{
			DisplayMessage("Welcome Mayor! We need you to run the island.Your forefathers before you have done so. Good luck.", 25, 10);
			Loop();
		}

		public static void Loop()
		{
            while (true)
            {
                if (CurMode == Mode.BuildingPlace)
                {
                    DisplayQuickMessage("Input location to build. i.e 1,1");

                    (int x, int y) pos = GetPos();
                    Console.WriteLine($"{pos.x},{pos.y}");
                    if (pos == ESCAPE_POS)
                        CurMode = Mode.None;
                    else
                    {
						if (Player.Money >= BuildingToPlace.Cost || Player.Map[pos.x, pos.y] == null)
						{
							Player.Age[pos.x, pos.y] = 0;
							Player.Map[pos.x, pos.y] = BuildingToPlace;
							CurMode = Mode.None;
							if (BuildingToPlace.TickRate == -1) BuildingToPlace.TickAction();
							Tick();
							Player.Money -= BuildingToPlace.Cost;
						}
						else
						{
							if (Player.Money < BuildingToPlace.Cost)
								DisplayQuickMessage("Not enough money");
							else
								DisplayQuickMessage("Spot is taken. Remove the building at the tile first.");
						}
                    }
                    Redraw();
                }

                if (CurMode == Mode.BuildingView)
                {
                    DisplayQuickMessage("Input location to view. i.e 1,1");

                    (int x, int y) pos = GetPos();
                    if (pos == ESCAPE_POS)
                        CurMode = Mode.None;
                    else
                    {
                        Building b = Player.Map[pos.x, pos.y];
                        if (b != null)
                            DisplayMessage($"{b.Name}\n{new string('-', b.Name.Length)}\nPosition:{pos.x},{pos.y}\nAge:{Player.Age[pos.x, pos.y]}\nCost: {b.Cost}\nDescription: {b.Description}\nTick Rate: {b.TickRate}", 50, 25);
                        else DisplayQuickMessage("There is nothing on that tile");
                    }
                }

                if (CurMode == Mode.BuildingRemove)
                {
                    DisplayQuickMessage("Input location to remove (50% refund). i.e 1,1");

                    (int x, int y) pos = GetPos();
                    if (pos == ESCAPE_POS)
                        CurMode = Mode.None;
                    else
                    {
                        Building b = Player.Map[pos.x, pos.y];
                        if (b != null)
                        {
                            Player.Money += b.Cost / 2;
                            Player.Map[pos.x, pos.y] = null;
                            Player.Age[pos.x, pos.y] = 0;
                        }
                        else DisplayQuickMessage("There is nothing on that tile");
                        CurMode = Mode.None;
                        Tick();
                    }
                }

                Console.SetCursorPosition(0, Console.WindowHeight - 1);

                var key = Console.ReadKey();

                if (CurMode == Mode.InMessage && key.Key == ConsoleKey.Spacebar)
                {
                    Redraw();
                    CurMode = Mode.None;
                }

                if (key.Key == ConsoleKey.B)
                    BuildMenu();
                if (key.Key == ConsoleKey.V)
                {
                    CurMode = Mode.BuildingView;
                    continue;
                }
                if (key.Key == ConsoleKey.R)
                {
                    CurMode = Mode.BuildingRemove;
                    continue;
                }
                if (key.Key == ConsoleKey.P)
                    Tick();
            }
        }

		public static void Tick()
		{
			Player.Year++;
			for(int i = 0; i < 4; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					Building b = Player.Map[i, j];
					if (b == null) continue;
					if (Player.Age[i, j] % b.TickRate == 0)
						b.TickAction();
				}
			}
			// taxes
			Player.Money += Player.Population * (Math.Min(Player.Happiness,100) / 100);
			if (Player.Happiness <= 35 && new Random().Next(0, 101) >= 50) Player.Population--;
			if (Player.Population < 0) Player.Population = 0;

			DoRandomEvent();
            Program.SaveGame();

			Redraw();
			if(CurrentEvent != null)
            DisplayQuickMessage(CurrentEvent.Description);
		}

		public static void DoRandomEvent()
		{
			if (new Random().Next(0, 101) >= 75)
			{
				Event ev = Events.AllEvents[new Random().Next(0, Events.AllEvents.Length)];
				ev.Effect();
				CurrentEvent = ev;
			}
			else CurrentEvent = null;
        }

		public static void Redraw()
		{
			Console.Clear();
			DrawAmounts();
			DrawMap();
		}

		public static void DrawAmounts()
		{
			Console.SetCursorPosition(0,0);
			Out("$" + Player.Money, ConsoleColor.Green);
			Out(" | ");
			Out("☺ " + Math.Max(Math.Min(Player.Happiness, 100), 0), ConsoleColor.DarkYellow);
			Out(" | ");
			Out("Pop:" + Player.Population);
			Out(" | ");
			Out(Player.Year);
			Out(" | [b] Build | [v] View | [r] Remove | [p] Do Nothing");
		}

		public static void DrawMap()
		{
			Console.SetCursorPosition(0,2);
			for(int i = 0; i < 4; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					Console.SetCursorPosition(i*2 + 2,j+3);
					Building b = Player.Map[i, j];
					if (b != null) Out(b.Char, b.Color);
					else Out("?", ConsoleColor.Green);
				}
			}

			for (int i = 0; i < 4; i++)
			{
				Console.SetCursorPosition(0, i + 3);
				Out(i);
			}

			for(int i = 0; i < 4; i++)
			{
				Console.SetCursorPosition(i * 2 + 2, 2);
				Out(i);
			}
		}

		public static void BuildMenu()
		{
			string buildText = "";
			int i = 0;
			foreach(Building b in Buildings.AllBuildings)
			{
				buildText += $"ID:{i} | {b.Name} | {b.Cost} | {b.Description} | TR:{b.TickRate}\n";
				i++;
			}
			buildText += "Enter the ID of the building to build:";
			DisplayMessage(buildText, 70, 20);
			while(true)
			{
				Console.SetCursorPosition(0,Console.WindowHeight - 1);
				string id = Console.ReadLine();
				if(int.TryParse(id, out int iid))
				{
					if (iid < 0 || iid >= Buildings.AllBuildings.Length) continue;
					else
					{
						CurMode = Mode.BuildingPlace;
						BuildingToPlace = Buildings.AllBuildings[iid];
						Redraw();
						break;
					}
				}
			}
		}
	}
}