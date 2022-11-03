using ClaimsReservationService;
using ClaimsReservationService.Model;
using System.Text;

namespace WTWTechnicalTestConsoleApp
{
    public class CumulativeDataFileGenerator : ICumulativeDataFileGenerator
    {
        private readonly IAppConfig _appConfig;

        public CumulativeDataFileGenerator(IAppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public string GenerateOutputFile(CumulativeClaimData cumulativeClaimData)
        {
            string outputFileName = "CumulativeClaimsDataOutput" + DateTime.Now.ToFileTime() + ".csv";
            try
            {
                var outputFilePath = $@"{_appConfig.OutputDataFilePath}\{outputFileName}";
                using(StreamWriter sw = new StreamWriter(outputFilePath))
                {
                    foreach(string line in cumulativeClaimData.OutputText)
                    {
                        sw.WriteLine(line);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error writing to output file - {ex.Message}");
                throw;
            }

            return outputFileName;
        }
    }
}