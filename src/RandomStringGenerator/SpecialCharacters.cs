namespace RandomStringGenerator
{
    public static class SpecialCharacters
    {
        public static readonly char[] DefaultSpecialCharacters = new[]
        {
            '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', '-', '.', '/', ':', ';',
            '<', '=', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~', '}', ';'
        };

        public static char[] Default
        {
            get { return DefaultSpecialCharacters; }
        }
    }
}
