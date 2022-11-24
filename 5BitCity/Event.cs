using System;

namespace _4BitCity
{
	public class Event
	{
		public string Description;
		public Action Effect;

		public Event(string description, Action effect)
		{
			Description = description;
			Effect = effect;
		}
	}
}
