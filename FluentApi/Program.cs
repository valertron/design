using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace FluentTask
{
	internal class Program
	{
		private static void Main()
		{
			var acts = new List<Action>();
			var behaviour = new Behavior(acts)
				.Say("Привет мир!")
				.UntilKeyPressed(b => b
					.Say("Ля-ля-ля!")
					.Say("Тру-лю-лю"))
				.Jump(JumpHeight.High)
				.UntilKeyPressed(b => b
					.Say("Aa-a-a-a-aaaaaa!!!")
					.Say("[набирает воздух в легкие]"))
				.Say("Ой!")
				.Delay(TimeSpan.FromSeconds(1))
				.Say("Кто здесь?!")
				.Delay(TimeSpan.FromMilliseconds(2000));
			behaviour.Execute();

			Console.ReadLine();
		}
	}

	interface IBehavior
	{
		Behavior Say(string input);
		Behavior Delay(TimeSpan time);
		Behavior UntilKeyPressed(Func<Behavior, Behavior> myFunc);
		Behavior Jump(JumpHeight jumpHeight);
		void Execute();
	}

	internal class Behavior : IBehavior
	{

		private readonly List<Action> actions;

		public Behavior(IEnumerable<Action> actions)
		{
			this.actions = actions.ToList();
		}

		public void Execute()
		{
			foreach (var action in actions)
			{
				action();
			}
		}

		public Behavior Say(string input)
		{
			actions.Add(() => Console.WriteLine(input));
			return new Behavior(actions);
		}

		public Behavior Delay(TimeSpan time)
		{
			actions.Add(() => Thread.Sleep(time));
			return new Behavior(actions);
		}

		public Behavior UntilKeyPressed(Func<Behavior, Behavior> myFunc)
		{
			actions.Add(() =>
			{

				var beh = new Behavior(new List<Action>());
				while (Console.KeyAvailable == false)
				{
					myFunc(beh);
					beh.Execute();
					Thread.Sleep(250); // Loop until input is entered.
				}
			});
			return new Behavior(actions);
		}

		public Behavior Jump(JumpHeight jumpHeight)
		{
				actions.Add(() => Console.ReadKey(true));

			return new Behavior(actions);
		}

	}
}