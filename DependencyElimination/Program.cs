using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DependencyElimination
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var sw = Stopwatch.StartNew();
			using (var http = new HttpClient())
			{
				using (var output = new StreamWriter("links.txt", false))
					for (int page = 1; page < 6; page++)
					{
						var url = "http://habrahabr.ru/top/page" + page;
						Console.WriteLine(url);
						var habrResponse = http.GetAsync(url).Result;
						if (habrResponse.IsSuccessStatusCode)
							ParseResponse(habrResponse, output).Wait();
						else
						{
							Console.WriteLine("Error: " + habrResponse.StatusCode + " " + habrResponse.ReasonPhrase);
						}
					}
			}
			Console.WriteLine("Total links found: {0}", totalLinks);
			Console.WriteLine("Finished");
			Console.WriteLine(sw.Elapsed);
		}

		private static int totalLinks = 0;

		private static async Task ParseResponse(HttpResponseMessage response, StreamWriter output)
		{
			string content = await response.Content.ReadAsStringAsync();
			var matches = Regex.Matches(content, @"\Whref=[""'](.*?)[""'\s>]").Cast<Match>();
			var count = 0;
			foreach (var match in matches)
			{
				output.WriteLine(match.Groups[1].Value);
				totalLinks++;
				count++;
			}
			Console.WriteLine("found {0} links", count);
		}
	}
}