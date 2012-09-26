using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RandomStringGenerator.CharProviders;

namespace RandomStringGenerator
{
    public class StringGenerator
    {
        private const int DefaultStringLengthRange = 1000;
        private const int AvailableProvidersCount = 4;
        
        [ThreadStatic]
        private static readonly Random Rand;

        static StringGenerator()
        {
            Rand = new Random(DateTime.Now.Millisecond);
        }

        private readonly Dictionary<Type, RandomCharacterProvider> _characterProviders = new Dictionary<Type, RandomCharacterProvider>();

        private bool _isLengthSpecified = false;
        private int _expectedMinLength;
        private int _expectedMaxLength;

        private char[] _overridenSpecialChars;

        public string Generate()
        {
            int expectedStringLength = CalculateLength();
            List<RandomCharacterProvider> providers = GenerateProvidersForConfiguration(expectedStringLength);

            var resultChars = new List<char>();
            foreach (RandomCharacterProvider provider in providers)
            {
                char[] minCharsFromProvider = provider.GetRandomChars(provider.MinCount);
                resultChars.AddRange(minCharsFromProvider);
            }

            while (resultChars.Count < expectedStringLength)
            {
                providers.RemoveAll(prvd => prvd.IsCompleted); //remove providers we cannot use more
                if (providers.Count == 0)
                {
                    throw new InvalidOperationException(
                        "Something is wrong. Please contact developer of this library. This situation should not be possible");
                }

                int randomProviderIndex = Rand.Next(0, providers.Count);
                RandomCharacterProvider provider = providers[randomProviderIndex];
                char rangomCharFromRandomProvider = provider.GetNextRandomChar();
                resultChars.Add(rangomCharFromRandomProvider);                
            }

            resultChars = RandomizeList(resultChars);

            string result = new string(resultChars.ToArray());

            Debug.WriteLine("Random string generated " + result);
            return result;
        }        

        private int CalculateLength()
        {
            int totalMinPossible = SpecifiedCharacterProviders.Sum(provider => provider.MinCount);
            int totalMaxPossible = SpecifiedCharacterProviders.Sum(provider => provider.MaxCount);
            if(AllProvidersAreSpecifiles)
            {                
                if(_isLengthSpecified)
                {
                    return CalculateLengthIfAllProvidersAndIsLength(totalMinPossible, totalMaxPossible);
                }
                else
                {
                    return CalculateLengthIfAllProvidersAndNoLength(totalMinPossible, totalMaxPossible);
                }
            }
            else
            {
                if(_isLengthSpecified)
                {                    
                    return CalculateLengthIfNotAllProvidersAndIsLength(totalMinPossible);
                }
                else
                {
                    return CalculateLengthIfNotAllProvidersAndNoLength(totalMinPossible);
                }
            }
        }

        private int CalculateLengthIfAllProvidersAndIsLength(int totalMinPossible, int totalMaxPossible)
        {            
            if (totalMinPossible > _expectedMaxLength)
            {
                throw new InvalidOperationException(
                    "Configuration is invalid. Expected string length is to short to satisfy this criteria");
            }

            if (totalMaxPossible < _expectedMinLength)
            {
                throw new InvalidOperationException(
                    "Configuration is invalid. Expected string length is to large to satisfy this criteria");
            }

            int lowestLengthPossible = Math.Max(_expectedMinLength, totalMinPossible);
            int largestLengthPossible = Math.Min(_expectedMaxLength, totalMaxPossible);

            return Rand.Next(lowestLengthPossible, largestLengthPossible + 1);
        }

        private static int CalculateLengthIfAllProvidersAndNoLength(int totalMinPossible, int totalMaxPossible)
        {            
            return Rand.Next(totalMinPossible, totalMaxPossible + 1);            
        }

        private int CalculateLengthIfNotAllProvidersAndIsLength(int totalMinPossible)
        {
            if (totalMinPossible > _expectedMaxLength)
            {
                throw new InvalidOperationException(
                    "Configuration is invalid. Expected string length is to short to satisfy this criteria");
            }

            //we do not verify for max string length here as we can get any count of chars from not specified providers

            int lowestLengthPossible = Math.Max(_expectedMinLength, totalMinPossible);

            return Rand.Next(lowestLengthPossible, _expectedMaxLength + 1);            
        }

        private static int CalculateLengthIfNotAllProvidersAndNoLength(int totalMinPossible)
        {
            return Rand.Next(totalMinPossible, totalMinPossible + DefaultStringLengthRange);
        }

        private List<RandomCharacterProvider> GenerateProvidersForConfiguration(int stringLength)
        {
            //we take all specified prviders plus
            //not included providers but with configuration which can satisfy length requirements
            var providers = new List<RandomCharacterProvider>(SpecifiedCharacterProviders);
            if(!providers.OfType<NumericCharProvider>().Any())
            {
                providers.Add(new NumericCharProvider(0, stringLength));
            }

            if (!providers.OfType<SpecialCharProvider>().Any())
            {
                providers.Add(new SpecialCharProvider(0, stringLength));
            }

            if (!providers.OfType<LowercaseProvider>().Any())
            {
                providers.Add(new LowercaseProvider(0, stringLength));
            }

            if (!providers.OfType<UppercaseProvider>().Any())
            {
                providers.Add(new UppercaseProvider(0, stringLength));
            }

            //replace ovveriden special chracters
            SpecialCharProvider specialCharProvider = providers.OfType<SpecialCharProvider>().First();
            specialCharProvider.SpecialCharacters = _overridenSpecialChars;

            //clear provider if need to call generate few times
            foreach (RandomCharacterProvider provider in providers)
            {
                provider.Clear();
            }

            return providers;
        }

        private ICollection<RandomCharacterProvider> SpecifiedCharacterProviders
        {
            get
            {
                return _characterProviders.Values;
            }
        }

        private bool AllProvidersAreSpecifiles
        {
            get { return AvailableProvidersCount == _characterProviders.Count; }
        }

        private List<T> RandomizeList<T>(List<T> list)
        {
            return list.OrderBy(x => Rand.Next()).ToList();
        }

        # region Fluid Configuration Functions
        public StringGenerator WithLength(int length)
        {
            WithLengthBetween(length, length);
            return this;
        }

        public StringGenerator WithLengthBetween(int minLength, int maxLength)
        {
            if (minLength < 0 || maxLength < 0)
            {
                throw new ArgumentException("Length should be non negative number");
            }

            if(minLength > maxLength)
            {
                throw new ArgumentException("Lower boundary should be less or equals than higher");
            }

            _expectedMinLength = minLength;
            _expectedMaxLength = maxLength;
            _isLengthSpecified = true;

            return this;
        }

        public StringGenerator WithNumericCount(int count)
        {
            WithnumericCountBetween(count, count);
            return this;
        }

        public StringGenerator WithnumericCountBetween(int minCount, int maxCount)
        {
            RandomCharacterProvider provider = new NumericCharProvider(minCount, maxCount);
            AddOrUpdateChracterProvider(provider);
            return this;
        }

        public StringGenerator WithSpecialCount(int count)
        {
            WithSpecialCountBetween(count, count);
            return this;
        }

        public StringGenerator WithSpecialCountBetween(int minCount, int maxCount)
        {
            RandomCharacterProvider provider = new SpecialCharProvider(minCount, maxCount);
            AddOrUpdateChracterProvider(provider);
            return this;
        }

        public StringGenerator WithSpecialCharacters(char[] specialChars)
        {
            _overridenSpecialChars = specialChars;

            return this;
        }

        public StringGenerator WithLowercaseCount(int count)
        {
            WithLowercaseCountBetween(count, count);
            return this;
        }

        public StringGenerator WithLowercaseCountBetween(int minCount, int maxCount)
        {
            RandomCharacterProvider provider = new LowercaseProvider(minCount, maxCount);
            AddOrUpdateChracterProvider(provider);
            return this;
        }

        public StringGenerator WithUppercaseCount(int count)
        {
            WithUppercaseCountBetween(count, count);
            return this;
        }

        public StringGenerator WithUppercaseCountBetween(int minCount, int maxCount)
        {
            RandomCharacterProvider provider = new UppercaseProvider(minCount, maxCount);
            AddOrUpdateChracterProvider(provider);
            return this;
        }
        #endregion
       
        private void AddOrUpdateChracterProvider(RandomCharacterProvider characterProvider)
        {
            if(_characterProviders.ContainsKey(characterProvider.GetType()))
            {
                _characterProviders[characterProvider.GetType()] = characterProvider;
            }
            else
            {
                _characterProviders.Add(characterProvider.GetType(), characterProvider);
            }
        }        
    }
}
