using NUnit.Framework;

namespace NFranc.Tests
{
    [TestFixture]
    class NFrancTests
    {
        [Test]
        public void NFrancPolishTest()
        {
            Soltys.NFranc franc = new Soltys.NFranc(10);

            var polTvn24Sample =
                @"Policjanci zakończyli przesłuchanie dwóch mężczyzn zatrzymanych w zawiązku z napadem na ratowników. Obaj usłyszeli po pięć zarzutów: czynnej napaści na funkcjonariusza publicznego, znieważenia funkcjonariusza, pobicia, używania gróźb karalnych i zniszczenia mienia.";
           
        }

        [Test]
        public void NFrancEnglishTest()
        {
            Soltys.NFranc franc = new Soltys.NFranc(10);
            var bbcSample =
                @"Those sharing it were moved by the fear in the child's eyes, as she seems to staring into the barrel of a gun. It wasn't a gun, of course, but a camera, and the moment was captured for all to see. But who took the picture and what is the story behind it? BBC Trending have tracked down the original photographer - Osman Sağırlı - and asked him how the image came to be.";

            var detectedLanuages = franc.DetectLanguage(
               bbcSample
               );
            Assert.AreEqual("eng", detectedLanuages[0].LanguageName);


        }
    }
}
