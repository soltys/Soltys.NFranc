using System.Text.RegularExpressions;

namespace NFranc
{
    internal class LanguageFamily
    {
        public string LangaugeFamilyName { get; set; }
        public Regex RegularExpression { get; set; }
    }
}