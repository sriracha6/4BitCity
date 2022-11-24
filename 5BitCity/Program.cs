using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace _4BitCity
{
    using static Functions;
    using static Game;

    internal class Program
    {
        static void Main(string[] args)
        {
            MainMenu();
        }

        static void MainMenu()
        {
            Console.Clear();
            OutL("4BitCity\n", ConsoleColor.Red);
            OutL("[n] Start Game");
            OutL("[l] Load Game");
            OutL("[i] Info");

            var key = Console.ReadKey();

            if(key.Key == ConsoleKey.N)
                StartNewGame();
            if (key.Key == ConsoleKey.L)
                LoadSaveGame();
            if (key.Key == ConsoleKey.I)
                Tutorial();
        }

        static void Tutorial()
        {
            Console.Clear();
            int index = -1;
            string[] Pages = {
@"Stats
----
There are a few stats. They are all displayed on the top of the map.

* Population - how many people live in your village. This affects tax amount.
* Money - how much money you have. Money allows you to buy buildings.
* Happiness - how happy your people are. Affects population and tax amount.",
@"Buildings
------
Buildings can be placed anywhere in the 4x4 map. Empty tiles in the map do nothing. Buildings can have many effects, including
tax amount changes, happiness, population, etc. Buildings have a cost to buy and a tick rate. Tickrate is how many years before
these affects happen. -1 Tickrate means it is a one off thing.
",
@"Actions
------
There are 4 actions.
[b] Build - build a building
[v] View - view the stats of a building at a specific tile.
[r] Remove - remove a building from the map. You get a 50% refund
[p] Do Nothing - skip doing anything for the next year.",
@"Events
------
Every year, there is a chance a random event will occurr.
These events can change happiness, population, etc.
"
            };
            DisplayMessage("Welcome to the tutorial. Use the left and right arrow keys to navigate.", 50, 25);
            while(true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.LeftArrow && index >= 1) index--;
                if (key.Key == ConsoleKey.RightArrow && index < Pages.Length - 1) index++;
                if (key.Key == ConsoleKey.Spacebar) break;
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Out($"Page {index+1}/{Pages.Length}");
                DisplayMessage(Pages[index], 50,25);
            }
            MainMenu();
        }

        static void LoadSaveGame()
        {
            XmlDocument x = new XmlDocument();
            x.Load("save.4b");
            Player.Year = int.Parse(x.SelectSingleNode("//Year").InnerText);
            Player.Money = int.Parse(x.SelectSingleNode("//Money").InnerText);
            Player.Population = int.Parse(x.SelectSingleNode("//Population").InnerText);
            Player.Happiness = int.Parse(x.SelectSingleNode("//Happiness").InnerText);
            var mapnode = x.SelectSingleNode("//Map").ChildNodes;
            for(int i = 0; i < 16; i++)
            {
                int xx = i % 4;
                int yy = i / 4;
                Player.Map[xx, yy] = Buildings.AllBuildings.FirstOrDefault(x => x.Name == mapnode[i].InnerText);
            }
            var agenode = x.SelectSingleNode("//Ages").ChildNodes;
            for (int i = 0; i < 16; i++)
            {
                int xx = i % 4;
                int yy = i / 4;
                Player.Age[xx, yy] = int.Parse(agenode[i].InnerText);
            }
            Console.Clear();
            Game.Redraw();
            Game.Loop();
        }

        public static void SaveGame()
        {
            XmlWriter x = XmlWriter.Create("save.4b");
            x.WriteStartDocument();
            x.WriteStartElement("Save");

            x.WriteEl("Money",Player.Money);
            x.WriteEl("Population",Player.Population);
            x.WriteEl("Happiness",Player.Happiness);
            x.WriteEl("Year",Player.Year);

            x.WriteStartElement("Map");
            for(int i = 0; i < 4; i++)
                for(int j = 0; j < 4; j++)
                    x.WriteEl("B", Player.Map[i,j] != null ? Player.Map[i,j].Name : "Null");
            x.WriteEndElement();

            x.WriteStartElement("Ages");
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    x.WriteEl("A", Player.Age[i,j]);
            x.WriteEndElement();

            x.WriteEndElement();
            x.WriteEndDocument();
            x.Close();
        }
    }
}