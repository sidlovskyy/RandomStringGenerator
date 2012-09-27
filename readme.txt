This library (of almost single public class :)) allows to generate random strings using fluent configuration.
It may be used for example for random password generation.

Example of code:
-------------------------------------------------------
var generator = new StringGenerator()
	.WithNumericCount(2)
	.WithSpecialCharacters(new[] { '@', '#', '!' })
	.WithSpecialCount(1)
	.WithUppercaseCountBetween(1, 3)
	.WithLengthBetween(10,15);

Console.WriteLine(generator.Generate());
Console.WriteLine(generator.Generate());
Console.WriteLine(generator.Generate());

Output:
-------------------------------------------------------
Started.
hPZg9lya@3fblkO
Arkiq@vaZ75
dyHNu5O2m#
Done.

Please let me know if you have found any issues or would like to support more configurations.