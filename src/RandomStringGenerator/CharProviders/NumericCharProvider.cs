namespace RandomStringGenerator.CharProviders
{
	internal class NumericCharProvider : RandomCharacterProvider
	{
		public static readonly char[] Numeric = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

		public NumericCharProvider(int minCount, int maxCount)
			: base(minCount, maxCount)
		{
		}

		public override char[] Chracters
		{
			get { return Numeric; }
		}
	}
}
