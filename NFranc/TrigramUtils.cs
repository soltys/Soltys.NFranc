using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NFranc
{
    static class TrigramUtils
    {
        private readonly static Regex ExpressionSymbols = new Regex(@"[\u0021-\u0040]+");

        public static string Clean(string value)
        {
            if (value == null)
            {
                value = "";
            }

            string step1 = ExpressionSymbols.Replace(value, " ");
            string step2 = Regex.Replace(step1, @"\s+", " ");
            return step2.Trim().ToLower();
        }

        public static IEnumerable<string> GetCleanTrigrams(string value)
        {
            return NGram.Trigram(" " + value + " ");
        }

        /// <summary>
        /// Get an `Object` with trigrams as its attributes, and
        /// their occurence count as their values
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Dictionary containing weighted trigrams</returns>
        public static Dictionary<string, int> GetCleanTrigramsAsDictionary(string value)
        {
            var trigrams = GetCleanTrigrams(value).ToList();
            var dict =  trigrams.ToDictionary(keySelector => keySelector,
                valueSelector => trigrams.Count(t => t == valueSelector));

            return dict;
        }


        public static IEnumerable<Tuple<string, int>> GetCleanTrigramsAsTuples(string value)
        {
            var dictionary = GetCleanTrigramsAsDictionary(value);
            return dictionary.OrderBy(kv => kv.Value).Select(kv => new Tuple<string, int>(kv.Key, kv.Value));
        }
    }
}
