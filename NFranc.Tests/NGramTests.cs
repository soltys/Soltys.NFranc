using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NFranc.Tests
{
    [TestFixture]
    public class NGramTests
    {
        private Func<string, IEnumerable<string>> _monoGram = NGram.CreateNGram(1);

        [Test]
        public void NegativeOrZeroThrowsRangeOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => NGram.CreateNGram(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => NGram.CreateNGram(-1));
        }

        [Test]
        public void MonoGramReturnsOneCharacterWhenOneCharacterGiven()
        {
            var unigrams = _monoGram("a").ToList();
            Assert.AreEqual(1, unigrams.Count);
            Assert.AreEqual("a", unigrams.ElementAt(0));


        }

        [Test]
        public void MonoGramReturnsTwoUnigramsWhenTwoCharactersGriven()
        {
            var unigrams = _monoGram("ab").ToList();
            Assert.AreEqual(2, unigrams.Count);
            Assert.AreEqual("a", unigrams[0]);
            Assert.AreEqual("b", unigrams[1]);
        }

        [Test]
        public void MonoGramTest()
        {
            CollectionAssert.AreEqual(new[] { "t", "e", "s", "t" }, _monoGram("test"));
            var result = string.Join("|", _monoGram("test"));
            Assert.AreEqual("t|e|s|t", result);
        }

        [Test]
        public void EmptyStringTest()
        {
            var result = _monoGram("");

            Assert.AreEqual(0, result.Count());

        }

        [Test]
        public void NullStringTest()
        {
            Assert.Throws<ArgumentNullException>(() => _monoGram(null));
        }

        [Test]
        public void BigramTest()
        {
            CollectionAssert.AreEqual(new []{"te", "es", "st"}, NGram.Bigram("test"));
        }

        [Test]
        public void TrigramTest()
        {
            CollectionAssert.AreEqual(new[] { "tes", "est" }, NGram.Trigram("test"));
        }

        [Test]
        public void TenGramTest()
        {
            var tenGram = NGram.CreateNGram(10);
            CollectionAssert.AreEqual(new[] { "abcdefghij", "bcdefghijk" }, tenGram("abcdefghijk"));
        }
    }
}
