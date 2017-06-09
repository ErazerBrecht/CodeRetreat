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
            var result = (await client.GetAsync("http://mazeretreat.azurewebsites.net/mazes/51b8269c-00e2-4486-ac05-f4490942c0c9")).Content.ReadAsStringAsync().Result;

            string[] lines = result.Replace("\"", "").Split(new string[] { "\\r\\n" }, StringSplitOptions.None);
            var chars = lines.Select(l => l.ToCharArray()).ToArray();

            var lineLength = lines.ElementAt(0).Length;

            // Intiliaze map
            var map = new bool[lines.Length, lineLength];
            Point start = new Point(0,0);
            Point end = new Point(0,0);

            // Ugly loop to determine start, end and map!
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lineLength; x++)
                {
                    var test = chars[y][x];

                    if(test == 'S')
                        start = new Point(y, x);

                    if (test == 'F')
                        end = new Point(y, x);

                    if (test != '#')
                        map[y, x] = true;
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
                TeamName = "BramKen",
                MazeId = "51b8269c-00e2-4486-ac05-f4490942c0c9",
                Solution = lol
            };

            var jsonInString = JsonConvert.SerializeObject(a);

            //Post
            var error = await client.PostAsync(
                "http://mazeretreat.azurewebsites.net/solutions/51b8269c-00e2-4486-ac05-f4490942c0c9",
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));
        }
    }
}