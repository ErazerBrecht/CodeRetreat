using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CodeRetreat
{
    class Program
    {
        private static SearchParameters parameters { get; set; }

        // Documentation: http://blog.two-cats.com/2014/06/a-star-example/
        // private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation) => Deze functie doet digionaal voor de rest werkt het!!!!!
        static void Main(string[] args)
        {
            // Run async code
            Run().GetAwaiter().GetResult();

 
        }

        static async Task Run()
        {
            await ParseMap();
            PathFinder pathFinder = new PathFinder(parameters);
            List<Point> path = pathFinder.FindPath();
            await SendMap(path);
            Console.WriteLine();
        }

        static async Task ParseMap()
        {
            var client = new HttpClient();

            // Get level
            var result = (await client.GetAsync("http://mazeretreat.azurewebsites.net/mazes/09a655dd-6cbd-4793-89cf-604d2b0a8330")).Content.ReadAsStringAsync().Result;

            string[] lines = result.Replace("\"", "").Split(new string[] { "\\r\\n" }, StringSplitOptions.None);
            var chars = lines.Select(l => l.ToCharArray()).ToArray();

            // Intiliaze map
            var map = new bool[lines.Length, lines.Length];
            Point start = new Point(0,0);
            Point end = new Point(0,0);

            // Ugly loop to determine start, end and map!
            for (int i = 0; i < lines.Length; i++)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    var test = chars[i][k];

                    if(test == 'S')
                        start = new Point(i, k);
                    else if (test == 'F')
                        end = new Point(i, k);

                    if (test != '#')
                        map[i, k] = true;
                }
            }

            parameters = new SearchParameters(start, end, map);
        }

        static async Task SendMap(List<Point> path)
        {
            var client = new HttpClient();

            string lol = "";
            foreach (var p in path)
            {
                lol += $"{p.Y},{p.X};";
            }

            var a = new
            {
                TeamName = "YOLO",
                MazeId = "09a655dd-6cbd-4793-89cf-604d2b0a8330",
                Solution = lol
            };

            var jsonInString = JsonConvert.SerializeObject(a);

            //Post
            var error = await client.PostAsync(
                "http://mazeretreat.azurewebsites.net/solutions/09a655dd-6cbd-4793-89cf-604d2b0a8330",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));
        }
    }
}