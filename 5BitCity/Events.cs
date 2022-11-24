using System;

namespace _4BitCity
{
	public static class Events
	{
		public static Event Birth = new Event("Someone was born.", () => { Player.Population++; });
		public static Event Death = new Event("Someone died.", () => { if(Player.Population > 3) Player.Population--; });
		public static Event Marriage = new Event("Some people got married.", () => { Player.Happiness+=10; });
		public static Event Divorce = new Event("Some people got divorced.", () => { Player.Happiness-=10; });
		public static Event Disaster = new Event("A natural disaster occurred.", () => { Player.Happiness-=10; if(Player.Population > 3) Player.Population--; });

		public static Event[] AllEvents = { Birth, Death, Marriage, Divorce, Disaster };
	}
}
