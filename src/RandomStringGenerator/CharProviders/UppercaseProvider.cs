using System.Collections.Generic;

namespace RandomStringGenerator.CharProviders
{
    internal class UppercaseProvider : RandomCharacterProvider
    {
        private static readonly char[] UppercaseChars;

        static UppercaseProvider()
        {
            var uppercaseChars = new List<char>();
            for(char ch = 'A'; ch <= 'Z'; ch++)
            {
                uppercaseChars.Add(ch);
            }
            UppercaseChars = uppercaseChars.ToArray();
        }

        public UppercaseProvider(int minCount, int maxCount) 
            : base(minCount, maxCount)
        {
        }

        public override char[] Chracters
        {
            get { return UppercaseChars; }
        }
    }
}
