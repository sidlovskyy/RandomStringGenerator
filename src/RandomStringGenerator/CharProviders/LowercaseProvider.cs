using System.Collections.Generic;

namespace RandomStringGenerator.CharProviders
{
	internal class LowercaseProvider : RandomCharacterProvider
	{
		private static readonly char[] LowercaseChars;

		static LowercaseProvider()
		{
			var lowercaseChars = new List<char>();
			for (char ch = 'a'; ch <= 'z'; ch++)
			{
				lowercaseChars.Add(ch);
			}
			LowercaseChars = lowercaseChars.ToArray();
		}

		public LowercaseProvider(int minCount, int maxCount)
			: base(minCount, maxCount)
		{
		}

		public override char[] Chracters
		{
			get { return LowercaseChars; }
		}
	}
}
