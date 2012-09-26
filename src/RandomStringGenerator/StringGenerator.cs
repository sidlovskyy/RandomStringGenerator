using System;

namespace RandomStringGenerator
{
    public class StringGenerator
    {
        private const int MaxDefaultStringLength = 1000;

        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);
        private static readonly char[] Numeric = new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        private int _length;
        private int _minLength;
        private int _maxLength;

        private int _numericCount = -1;

        public string Generate()
        {
            if (LengthRangeIsSpecified)
            {
                _length = Rand.Next(_minLength, _maxLength);
            }
            else
            if (!LengthIsSpecified)
            {
                int minLenght = NumericCountSpecified && (_numericCount > 0) ? _numericCount : 1;
                _length = Rand.Next(minLenght, MaxDefaultStringLength);
            }

            if(NumericCountSpecified && _numericCount > _length)
            {
                throw new InvalidOperationException("Numeric count should not be greater than length");
            }

            string result = string.Empty;
            for (int i = 0; i < _numericCount; i++)
            {
                int numericIndex = Rand.Next(0, Numeric.Length - 1);
                result += Numeric[numericIndex];
            }

            while(result.Length < _length)
            {
                result += ' ';
            }

            return result;
        }        

        # region Fluid Configuration Functions
        public StringGenerator WithLength(int length)
        {
            if(length <= 0)
            {
                throw new ArgumentOutOfRangeException("length", length, "Length should be greater then zero");
            }

            _length = length;
            ClearLengthRange();

            return this;
        }

        public StringGenerator WithLengthRange(int min, int max)
        {
            if (min <= 0 || max <= 0)
            {
                throw new ArgumentException("Range elements should be greater then zero");
            }

            if(min > max)
            {
                throw new ArgumentException("Lower boundary should be less or equals than higher");
            }

            _minLength = min;
            _maxLength = max;
            ClearLength();

            return this;
        }

        public StringGenerator WithNumericCount(int numericCount)
        {
            if(numericCount < 0)
            {
                throw  new  ArgumentOutOfRangeException("numericCount", numericCount, "Numeric count should not be negative");
            }

            _numericCount = numericCount;
            
            return this;
        }
        #endregion

        #region Helper Function
        private bool LengthRangeIsSpecified
        {
            get
            {
                return (_minLength > 0) && (_maxLength > 0);
            }
        }

        private bool LengthIsSpecified
        {
            get
            {
                return _length > 0;
            }
        }

        protected bool NumericCountSpecified
        {
            get { return _numericCount != -1; }
        }

        private void ClearLength()
        {
            _length = 0;
        }

        private void ClearLengthRange()
        {
            _minLength = 0;
            _maxLength = 0;
        }
        #endregion
    }
}
