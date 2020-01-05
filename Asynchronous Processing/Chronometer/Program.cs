using System;
using System.Linq;

namespace Chronometer
{
    class Program
    {
        static void Main(string[] args)
        {
            IChronometer chonometer = new Chronometer();

            string inputLine = string.Empty;

            while ((inputLine = Console.ReadLine()) != "exit")
            {
                switch (inputLine)
                {
                    case "start": chonometer.Start();
                        break;
                    case "stop": chonometer.Stop();
                        break;
                    case "lap":
                        Console.WriteLine(chonometer.Lap());
                        break;
                    case "time":
                        Console.WriteLine(chonometer.GetTime);
                        break;
                    case "reset":
                        chonometer.Reset();
                        break;
                    case "laps":
                        Console.WriteLine("Laps:" + (chonometer.Laps.Count == 0 
                            ? "no laps." 
                            : Environment.NewLine + string.Join(Environment.NewLine,  chonometer.Laps.Select((lap, index) => $"{index}. {lap}"))));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
