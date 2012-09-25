using System;

namespace RandomStringGenerator
{
    public class StringGenerator
    {
        private const int MAX_DEFAULT_STRING_LENGTH = 1000;

        private static readonly Random _rand = new Random(DateTime.Now.Millisecond);

        private int _length;
        private int _minLength;
        private int _maxLength;        

        public string Generate()
        {
            if (LengthRangeIsSpecified)
            {
                _length = _rand.Next(_minLength, _maxLength);
            }
            else
            if (!LengthIsSpecified)
            {
                _length = _rand.Next(1, MAX_DEFAULT_STRING_LENGTH);
            }

            string result = string.Empty;

            for (int i = 0; i < _length; i++)
            {
                result += ' ';
            }

            return result;
        }        

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

        private void ClearLength()
        {
            _length = 0;
        }

        private void ClearLengthRange()
        {
            _minLength = 0;
            _maxLength = 0;
        }

        private void SetSameMinMaxLength(int lenth)
        {
            _maxLength = _minLength = lenth;
        }
    }
}
