using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NFranc;

namespace Soltys
{
    public class NFranc
    {
        private int minLength = 10;
        private readonly LanguageFamilyTestDictionary _languageFamilies = new LanguageFamilyTestDictionary();
        private readonly LanguageData _languagesData = new LanguageData();
        public int MaxDiffrence { get { return 300; } }
        public NFranc(int minimumLength)
        {

        }

        public IList<LanguageDistance> DetectLanguage(string testString)
        {
            if (testString.Length < 10)
            {
                return new LanguageDistance[]
                {
                    new LanguageDistance
                    {
                        Distance = 1,
                        LanguageName = "und"
                    }
                };
            }

            var languageFamilyWithCount = GetLanguageFamily(testString);

            if (!_languagesData.ContainsLanguage(languageFamilyWithCount.Item1.LangaugeFamilyName))
            {
                if (languageFamilyWithCount.Item2 == 0)
                {
                    return new LanguageDistance[]
                    {
                        new LanguageDistance
                        {
                            Distance = 1,
                            LanguageName = "und"
                        }
                    };
                }
                else
                {
                    return new LanguageDistance[]
                    {
                        new LanguageDistance
                        {
                            Distance = 1,
                            LanguageName = languageFamilyWithCount.Item1.LangaugeFamilyName
                        }
                    };

                }
            }

            /*
            * Get all distances for a given script, and
            * normalize the distance values.
            */

            var cleanTrigrams = TrigramUtils.GetCleanTrigramsAsTuples(testString);
            var languageTrigrams = _languagesData.GetLanuageFamillyTrigrams(languageFamilyWithCount.Item1.LangaugeFamilyName);
            var distances = GetDistances(cleanTrigrams.ToList(), languageTrigrams);
            return Normalize(testString, distances);


        }

        private IList<LanguageDistance> Normalize(string testString, IList<LanguageDistance> distances)
        {
            double min = distances.First().Distance;
            double max = (testString.Length * MaxDiffrence) - min;
            int index = -1;
            int length = distances.Count;

            while (++index < length)
            {
                distances[index].Distance = 1 - ((distances[index].Distance - min) / max);
            }

            return distances;
        }

        private IList<LanguageDistance> GetDistances(IList<Tuple<string, int>> cleanTrigrams, IEnumerable<LanguageTrigramData> languageTrigrams)
        {
            var distances = new List<LanguageDistance>();

            foreach (var languageTrigram in languageTrigrams)
            {
                distances.Add(new LanguageDistance
                {
                    LanguageName = languageTrigram.LanguageName,
                    Distance = GetDistance(cleanTrigrams, languageTrigram.Trigrams.ToList())
                });
            }

            return distances.OrderBy(x => x.Distance).ToList();
        }

        private int GetDistance(IList<Tuple<string, int>> testTrigrams, IList<Trigram> languageTrigrams)
        {
            int distance = 0;
            int index = testTrigrams.Count;
            int difference;
            while (index-- > 0)
            {
                var testTrigram = testTrigrams[index];
                var languageTrigram = languageTrigrams.FirstOrDefault(x => x.Value == testTrigram.Item1);
                difference = languageTrigram != null ? Math.Abs(testTrigram.Item2 - languageTrigram.Weight) : MaxDiffrence;
                distance += difference;
            }
            return distance;
        }



        /// <summary>
        /// Returns most used expression with count
        /// </summary>
        /// <param name="testString"></param>
        /// <returns>Returns most used expression with count</returns>
        private Tuple<LanguageFamily, int> GetLanguageFamily(string testString)
        {

            var expressionWithUseCount = _languageFamilies.Select(x =>
                new
                {
                    LanguageFamily = x,
                    ExpressionCount = x.RegularExpression.Matches(testString).Count
                }

                ).OrderByDescending(x => x.ExpressionCount).First();

            return new Tuple<LanguageFamily, int>(expressionWithUseCount.LanguageFamily, expressionWithUseCount.ExpressionCount);
        }
    }

    public class LanguageDistance
    {
        public string LanguageName { get; set; }
        public double Distance { get; set; }
    }

    internal enum Language
    {
        Undefined,
        Polish,
        English
    }
}
