using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFranc
{
    public class NGram
    {
        private static readonly Func<string, IEnumerable<string>> _bigram = CreateNGram(2);
        public static Func<string, IEnumerable<string>> Bigram
        {
            get { return _bigram; }
        }

        private static readonly Func<string, IEnumerable<string>> _trigram = CreateNGram(3);
        public static Func<string, IEnumerable<string>> Trigram
        {
            get { return _trigram; }
        }


        public static Func<string, IEnumerable<string>> CreateNGram(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException("n", "n - must be positive");
            }

            return (value) =>
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                var nGrams = new Dictionary<int, string>();
                int index = value.Length - n + 1;

                if (index < 1)
                {
                    return nGrams.Values;
                }

                while (index-- > 0)
                {
                    nGrams.Add(index, value.Substring(index, n));
                }
                return nGrams.OrderBy(keyValue => keyValue.Key).Select(keyValue => keyValue.Value);
            };
        }
    }
}
