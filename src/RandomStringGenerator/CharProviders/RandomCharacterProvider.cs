using System;
using System.Collections.Generic;

namespace RandomStringGenerator.CharProviders
{
    public abstract class RandomCharacterProvider
    {
        [ThreadStatic] 
        private static Random _rand;

        public Random Rand
        {
            get
            {
                return _rand ?? (_rand = new Random(DateTime.Now.Millisecond));
            }
        }

        private int _providedCharsCount;

        public int MinCount { get; protected set; }
        public int MaxCount { get; protected set; }

        protected RandomCharacterProvider(int minCount, int maxCount)
        {
            if(minCount < 0 || maxCount < 0)
            {
                throw new ArgumentException("Count should non negative value");
            }

            if(minCount > maxCount)
            {
                throw new ArgumentException("Min count should not be greater than max count");
            }

            MinCount = minCount;
            MaxCount = maxCount;
        }

        public bool IsCompleted
        {
            get { return _providedCharsCount >= MaxCount; }
        }

        public char[] GetRandomChars(int count)
        {
            if(count < 0)
            {
                throw new ArgumentException("Count should non negative value");
            }

            var chars = new List<char>();
            for(int i = 0; i < count; i++)
            {
                char ch = GetNextRandomChar();
                chars.Add(ch);
            }
            
            return chars.ToArray();
        }

        public char GetNextRandomChar()
        {
            char result = GenerateRandomChar();
            _providedCharsCount++;
            return result;
        }

        private char GenerateRandomChar()
        {
            int chracterLenght = Chracters.Length;
            int charIndex = Rand.Next(0, chracterLenght);
            return Chracters[charIndex];
        }

        public void Clear()
        {
            _providedCharsCount = 0;
        }

        public abstract char[] Chracters { get; }
    }
}
