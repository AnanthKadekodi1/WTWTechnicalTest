using ClaimsReservationService;
using ClaimsReservationService.Model;
using Microsoft.Practices.Unity;
using TinyCsvParser.Mapping;
using UnityAutoMoq;

namespace ClaimsResearvation.Tests
{
    [TestFixture]
    public class InputDataFileParserTest
    {
        private UnityAutoMoqContainer builder;
        private IncrementalDataFileParser inputDataFileParser;

        [SetUp]
        public void Setup()
        {
            builder = new UnityAutoMoqContainer();
            builder.RegisterType<ICsvMapping<IncrementalDataModel>, IncrementalDataMapping>();

            inputDataFileParser = builder.Resolve<IncrementalDataFileParser>();
        }

        [Test]
        public void Input_Data_Is_Parsed_Correctly()
        {
            string filePath = Path.Combine(@"TestFiles\IncrementalDataTwoTriangles.csv");
            builder.GetMock<IAppConfig>().Setup(x => x.InputDataFilePath).Returns(Path.GetFullPath(filePath));

            var incrementalClaims = inputDataFileParser.Parse();
            int productCount = 2;
            int numberOfRows = 12;
            int rowsCountForCompProduct = 3;
            int rowCountForNonCompProduct = 9;

            Assert.That(incrementalClaims.GroupBy(x => x.Product).Count(), Is.EqualTo(productCount));
            Assert.That(incrementalClaims.Count(), Is.EqualTo(numberOfRows));
            Assert.That(incrementalClaims.Count(x => x.Product == "Non-Comp"), Is.EqualTo(rowCountForNonCompProduct));
            Assert.That(incrementalClaims.Count(x => x.Product == "Comp"), Is.EqualTo(rowsCountForCompProduct));
        }

        [Test]
        public void Throw_Error_Reading_File_Missing_Column()
        {
            string filePath = Path.Combine(@"TestFiles\IncrementalDataMissingColumn.csv");
            builder.GetMock<IAppConfig>().Setup(x => x.InputDataFilePath).Returns(Path.GetFullPath(filePath));

            Assert.Throws<Exception>(() => inputDataFileParser.Parse());
        }

        [Test]
        public void Throw_Error_Reading_File_Missing_Delimiter()
        {
            string filePath = Path.Combine(@"TestFiles\IncrementalDataMissingDelimiter.csv");
            builder.GetMock<IAppConfig>().Setup(x => x.InputDataFilePath).Returns(Path.GetFullPath(filePath));

            Assert.Throws<Exception>(() => inputDataFileParser.Parse());
        }
    }
}
