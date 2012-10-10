using System;

namespace RandomStringGenerator.SampleConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Started.");
			try
			{
				var generator = new StringGenerator()
					.WithNumericCount(2)
					.WithSpecialCharacters(new[] { '@', '#', '!' })
					.WithSpecialCount(1)
					.WithUppercaseCountBetween(1, 3)
					.WithLengthBetween(10, 15);

				Console.WriteLine(generator.Generate());
				Console.WriteLine(generator.Generate());
				Console.WriteLine(generator.Generate());

				Console.Write("Done.");
			}
			catch (Exception ex)
			{
				Console.Write("Exceptiom {0}", ex);
			}
			Console.ReadKey();
		}
	}
}
