using ClaimsReservationService;
using ClaimsReservationService.Model;
using Microsoft.Practices.Unity;
using TinyCsvParser.Mapping;
using UnityAutoMoq;
using WTWTechnicalTestConsoleApp;

namespace ClaimsReservation.Tests
{
    [TestFixture]
    public class CumulativeDataCalculatorTest
    {
        private UnityAutoMoqContainer builder;
        private CumulativeClaim cumulativeClaim;

        [SetUp]
        public void Setup()
        {
            builder = new UnityAutoMoqContainer();
            builder.RegisterType<ICsvMapping<IncrementalDataModel>, IncrementalDataMapping>();
            builder.RegisterType<IIncrementalDataFileParser, IncrementalDataFileParser>();

            cumulativeClaim = builder.Resolve<CumulativeClaim>();
        }

        [Test]
        public void Generated_CumulativeDataTwoTriangles()
        {
            string filePath = Path.Combine(@"TestFiles\IncrementalDataTwoTriangles.csv");
            builder.GetMock<IAppConfig>().Setup(x => x.InputDataFilePath).Returns(Path.GetFullPath(filePath));

            var inputDataFileParser = builder.Resolve<IIncrementalDataFileParser>();
            var incrementalClaims = inputDataFileParser.Parse();

            var claimData = cumulativeClaim.GenerateCumulativeDataOutputContent(incrementalClaims);
            Assert.IsNotNull(claimData);
            Assert.That(claimData.OutputText[0], Is.EqualTo("1990, 4"));
            Assert.That(claimData.OutputText[1], Is.EqualTo("Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200"));
            Assert.That(claimData.OutputText[2], Is.EqualTo("Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100"));
        }

        [Test]
        public void Generate_CumulativeDataThreeTriangles()
        {
            string filePath = Path.Combine(@"TestFiles\IncrementalDataThreeTriangles.csv");
            builder.GetMock<IAppConfig>().Setup(x => x.InputDataFilePath).Returns(Path.GetFullPath(filePath));

            var inputDataFileParser = builder.Resolve<IIncrementalDataFileParser>();
            var incrementalClaims = inputDataFileParser.Parse();

            var claimData = cumulativeClaim.GenerateCumulativeDataOutputContent(incrementalClaims);
            Assert.IsNotNull(claimData);
            Assert.That(claimData.OutputText[0], Is.EqualTo("1990, 4"));
            Assert.That(claimData.OutputText[1], Is.EqualTo("Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200"));
            Assert.That(claimData.OutputText[2], Is.EqualTo("Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100"));
            Assert.That(claimData.OutputText[3], Is.EqualTo("Comp1, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200"));
        }

        [Test]
        public void Generate_CumulativeDataMoreThanFourOriginYears()
        {
            string filePath = Path.Combine(@"TestFiles\IncrementalDataMoreThanFourOriginYears.csv");
            builder.GetMock<IAppConfig>().Setup(x => x.InputDataFilePath).Returns(Path.GetFullPath(filePath));

            var inputDataFileParser = builder.Resolve<IIncrementalDataFileParser>();
            var incrementalClaims = inputDataFileParser.Parse();

            var claimData = cumulativeClaim.GenerateCumulativeDataOutputContent(incrementalClaims);
            Assert.IsNotNull(claimData);
            Assert.That(claimData.OutputText[0], Is.EqualTo("1990, 6"));
            Assert.That(claimData.OutputText[1], Is.EqualTo("Comp, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 110, 280, 280, 280, 200, 200, 200, 0, 0, 0"));
            Assert.That(claimData.OutputText[2], Is.EqualTo("Non-Comp, 45.2, 110, 110, 147, 147, 147, 50, 125, 150, 150, 150, 55, 140, 140, 140, 100, 100, 100, 0, 0, 0"));
            Assert.That(claimData.OutputText[3], Is.EqualTo("Comp1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200"));
        }

        [Test]
        public void Assess_File_Generation()
        {
            if (File.Exists(Path.Combine(@"TestFiles\Output.csv")))
            {
                File.Delete(Path.Combine(@"TestFiles\Output.csv"));
            }

            string filePath = Path.Combine(@"TestFiles\IncrementalDataTwoTriangles.csv");
            builder.GetMock<IAppConfig>().Setup(x => x.InputDataFilePath).Returns(Path.GetFullPath(filePath));

            string outPutPath = @"TestFiles";
            builder.GetMock<IAppConfig>().Setup(x => x.OutputDataFilePath).Returns(Path.GetFullPath(outPutPath));

            var inputDataFileParser = builder.Resolve<IIncrementalDataFileParser>();
            var incrementalClaims = inputDataFileParser.Parse();

            var claimData = cumulativeClaim.GenerateCumulativeDataOutputContent(incrementalClaims);
            var outputFileGenerator = new CumulativeDataFileGenerator(builder.Resolve<IAppConfig>());
            var generatedOutputFile = outputFileGenerator.GenerateOutputFile(claimData);

            Assert.IsTrue(File.Exists(Path.Combine(outPutPath, generatedOutputFile)));
        }
    }
}
