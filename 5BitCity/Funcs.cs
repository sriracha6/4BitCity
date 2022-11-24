using System;
using System.Xml;

namespace _4BitCity
{
    public static class Functions
    {
        public static readonly (int x, int y) ESCAPE_POS = (int.MaxValue, int.MaxValue);

        public static void OutL(object s, ConsoleColor c = ConsoleColor.White)
        {
            Console.ForegroundColor = c;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public static void Out(object s, ConsoleColor c = ConsoleColor.White)
        {
            Console.ForegroundColor = c;
            Console.Write(s);
            Console.ResetColor();
        }

        public static void DisplayMessage(string text, int width, int height)
        {
            (int x, int y) pos = (Console.WindowWidth / 2 - (width / 2), Console.WindowHeight / 2 - (height / 2));
            Console.SetCursorPosition(pos.x, pos.y);
            int spos = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Console.SetCursorPosition(pos.x + i, pos.y + j);
                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                        Out("&", ConsoleColor.DarkYellow);
                    if (i == 1 && j == 1) Out("Press [Space] to close", ConsoleColor.Red);
                    if (i == 1 && j >= 2 && spos < text.Length)
                    {
                        int startSpos = spos;
                        for (int k = spos; k < (startSpos + width - 2 >= text.Length ? startSpos + text.Length - startSpos : startSpos + width - 2); k++)
                        {
                            if (text[k] != '\n')
                                Out(text[k]);
                            else
                            {
                                spos++;
                                break;
                            }
                            spos++;
                        }
                        //Out(text.Substring(spos, spos + width - 2 >= text.Length ? text.Length - spos : width - 2));
                    }
                }
            }
            Game.CurMode = Mode.InMessage;
        }

        public static void DisplayQuickMessage(string text)
        {
            Console.SetCursorPosition(Console.WindowWidth - text.Length, Console.WindowHeight - 1);
            Out(text, ConsoleColor.Yellow);
        }

        public static (int x, int y) GetPos()
        {
            while (true)
            {
                string input = "";
                Console.SetCursorPosition(0,Console.WindowHeight-1);
                while(true)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Enter) break;
                    if (key.Key == ConsoleKey.Escape) return ESCAPE_POS;
                    input += key.KeyChar;
                }
                (int x, int y) pos;
                var spl = input.Split(',');

                if (spl.Length != 2) continue;

                if (int.TryParse(spl[0], out int inx)) pos.x = inx;
                else continue;

                if (int.TryParse(spl[1], out int iny)) pos.y = iny;
                else continue;

                if (pos.x < 0 || pos.x > 4 || pos.y < 0 || pos.y > 4) continue;

                return pos;
            }
        }

        public static void WriteEl(this XmlWriter x, string name, object value)
        {
            x.WriteStartElement(name);
            x.WriteValue(value);
            x.WriteEndElement();
        }
    }
}