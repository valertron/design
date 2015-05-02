using System;
using System.Linq;
using System.Diagnostics;

namespace PerfLogger
{
	class Program
	{
		static void Main(string[] args)
		{
			var perf = new PerfLogger();
			var sum = 0.0;
			using (PerfLogger.Measure(t => Console.WriteLine("for: {0}", t)))
				for (var i = 0; i < 100000000; i++) sum += i;

			using (PerfLogger.Measure(t => Console.WriteLine("linq: {0}", t)))
				sum -= Enumerable.Range(0, 100000000).Sum(i => (double)i);
			Console.WriteLine(sum);

			Console.ReadLine();
		}

		public class PerfLogger: IDisposable
		{
			private static Action<double> Action;
			private static Stopwatch stopwatch;
			public static PerfLogger instance;
			public PerfLogger()
			{
				instance = this;
			}

			public static PerfLogger Measure(Action<double> action)
			{
				stopwatch = new Stopwatch();
				stopwatch.Start();
				Action = action;
				return instance;
			}

			public void Dispose()
			{
				stopwatch.Stop();
				var elps = stopwatch.Elapsed.TotalMilliseconds;
				Action(elps);
			}
		}
		
		
	}
}
