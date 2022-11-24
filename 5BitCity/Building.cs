using System;

namespace _4BitCity
{
	public class Building
	{
		public string Name;
		public Action TickAction;
		public int TickRate; // 5 would be 1 in every 5 moves
		public char Char;
		public ConsoleColor Color;
		public int Cost;
		public string Description;
											//		if tickrate is -1, it is a one off thing
		public Building(string name, Action tickAction, int tickRate, char @char, ConsoleColor color, int Cost, string desc)
		{
			Name = name;
			TickAction = tickAction;
			TickRate = tickRate;
			Char = @char;
			Color = color;
			this.Cost = Cost;
			this.Description = desc;
		}
	}
}