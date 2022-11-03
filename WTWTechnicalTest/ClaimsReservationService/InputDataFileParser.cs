using ClaimsReservationService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace ClaimsReservationService
{
    public class IncrementalDataFileParser: IIncrementalDataFileParser
    {
        private readonly CsvParser<IncrementalDataModel> _csvParser;
        private readonly IAppConfig _appConfig;

        public IncrementalDataFileParser(
            ICsvMapping<IncrementalDataModel> mapping,
            IAppConfig config)
        {
            _appConfig = config;
            var csvParserOptions = new CsvParserOptions(true, ',');
            _csvParser = new CsvParser<IncrementalDataModel>(csvParserOptions, mapping);
        }

        public List<IncrementalDataModel> Parse()
        {
            try
            {
                var results = _csvParser.ReadFromFile(_appConfig.InputDataFilePath, Encoding.UTF8).Select(x => x.Result).ToList();
                ValidateData(results);
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing CSV file - {ex.Message}");
                throw;
            }

        }

        private void ValidateData(List<IncrementalDataModel> results)
        {
            if (results == null || results.Any(i => i == null))
            {
                throw new Exception("Missing data in file");
            }

            foreach (var result in results)
            {
                ValidateYearlyData(result);
            }
        }

        private void ValidateYearlyData(IncrementalDataModel result)
        {
            if(result.DevelopmentYear < result.OriginYear)
            {
                throw new Exception("Development Year " + result.DevelopmentYear + " is less than origin year " + result.OriginYear);
            }

            if(result.OriginYear > DateTime.Now.Year)
            {
                throw new Exception("The Origin Year " + result.OriginYear + " is newer than the current year - " + DateTime.Now.Year);
            }
        }
    }
}
