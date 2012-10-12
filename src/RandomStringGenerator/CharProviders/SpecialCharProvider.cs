namespace RandomStringGenerator.CharProviders
{
    internal class SpecialCharProvider : RandomCharacterProvider
    {
        public char[] SpecialCharacters { get; set; }

        public SpecialCharProvider(int minCount, int maxCount, char[] specialChars = null)
            : base(minCount, maxCount)
        {
            SpecialCharacters = specialChars;
        }

        public override char[] Chracters
        {
            get
            {
                return (SpecialCharacters != null) && (SpecialCharacters.Length > 0)
                           ? SpecialCharacters
                           : RandomStringGenerator.SpecialCharacters.Default;
            }
        }
    }
}
