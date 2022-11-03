using ClaimsReservationService;
using ClaimsReservationService.Model;
using Microsoft.Practices.Unity;
using System.Configuration;
using TinyCsvParser.Mapping;

namespace WTWTechnicalTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new UnityContainer();
            try
            {
                builder.RegisterInstance<IAppConfig>(new AppConfig
                {
                    InputDataFilePath = ConfigurationManager.AppSettings["InputDataFilePath"],
                    OutputDataFilePath = ConfigurationManager.AppSettings["OutputDataFilePath"]
                });

                builder.RegisterType<ICsvMapping<IncrementalDataModel>, IncrementalDataMapping>();
                builder.RegisterType<IIncrementalDataFileParser, IncrementalDataFileParser>();
                builder.RegisterType<ICumulativeClaim, CumulativeClaim>();
                builder.RegisterType<ICumulativeDataFileGenerator, CumulativeDataFileGenerator>();

                var incrementalDataFileParser = builder.Resolve<IncrementalDataFileParser>();
                var cumulativeClaim = builder.Resolve<CumulativeClaim>();
                var outputFileGenerator = builder.Resolve<CumulativeDataFileGenerator>();

                List<IncrementalDataModel> incrementalData = incrementalDataFileParser.Parse();
                CumulativeClaimData cumulativeClaimData = cumulativeClaim.GenerateCumulativeDataOutputContent(incrementalData);
                var fileGenerated = outputFileGenerator.GenerateOutputFile(cumulativeClaimData);
                Console.WriteLine("Generated file - " + fileGenerated);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Quitting Program");
            }
        }
    }
}