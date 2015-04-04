using System.Collections.Generic;

namespace NFranc
{
    internal class LanguageTrigramData
    {
        public string LanguageName { get; set; }
        public IEnumerable<Trigram> Trigrams { get; set; }
    }

    internal class Trigram
    {
        public string Value { get; set; }
        public int Weight { get; set; }
    }
}