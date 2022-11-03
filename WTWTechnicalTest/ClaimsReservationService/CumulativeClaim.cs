using ClaimsReservationService.Model;
using System.Text;

namespace ClaimsReservationService
{
    public class CumulativeClaim : ICumulativeClaim
    {
        private int _earliestOriginYear;
        private int _numberOfDevelopmentYears;
        private int _maxDevelopmentYear;

        public CumulativeClaimData GenerateCumulativeDataOutputContent(List<IncrementalDataModel> incrementalValues)
        {
            IList<string> cumulativeDataOutputText = new List<string>();
            try
            {
                cumulativeDataOutputText = generateOutputText(cumulativeDataOutputText, incrementalValues);
            }catch (Exception ex)
            {
                Console.WriteLine($"Error generating output text - {ex.Message}");
                throw;
            }
            
            return new CumulativeClaimData
            {
                OutputText = cumulativeDataOutputText
            };
        }


        private IList<string> generateOutputText(IList<string> outputText, List<IncrementalDataModel> incrementalValues)
        {
            IEnumerable<int> originYears = incrementalValues.Select(x => x.OriginYear).Distinct().OrderBy(x => x);
            _earliestOriginYear = originYears.First();
            _numberOfDevelopmentYears = originYears.Last() - _earliestOriginYear + 1;
            _maxDevelopmentYear = originYears.Last();

            outputText.Add(AddFirstRow(_numberOfDevelopmentYears, _earliestOriginYear));
            outputText = AddSubsequentRows(outputText, incrementalValues, _earliestOriginYear, _maxDevelopmentYear, _numberOfDevelopmentYears );
            return outputText;
        }


        private string AddFirstRow(int developmentYears, int earliestOriginYear)
        {
            return String.Concat(earliestOriginYear.ToString(), ", ", developmentYears.ToString());
        }

        private IList<string> AddSubsequentRows(IList<string> cumulativeDataOutputText, List<IncrementalDataModel> incrementalValues, int earliestOriginYear, int maxDevelopmentYear, int numberOfDevelopmentYears)
        {
            try
            {
                var products = from p in incrementalValues
                               group p by p.Product into x
                               select x;
                StringBuilder sb = new StringBuilder();

                foreach (var product in products)
                {
                    sb = sb.Clear();
                    sb.Append(product.Key);
                    for (int row = 0; row <= numberOfDevelopmentYears; row++)
                    {
                        int originYear = earliestOriginYear + row;
                        int devYear = earliestOriginYear + row;
                        double accumTotal = 0;
                        while (devYear <= maxDevelopmentYear)
                        {
                            IEnumerable<double> claimedYearsForProduct = incrementalValues.Where(t => t.Product == product.Key
                                                            && t.OriginYear == originYear
                                                            && t.DevelopmentYear == devYear)
                                                            .Select(t => t.Increment);
                            accumTotal += claimedYearsForProduct.FirstOrDefault();
                            sb.Append("," + " " + accumTotal.ToString("###0.##"));
                            devYear++;
                        }
                    }
                    cumulativeDataOutputText.Add(sb.ToString());
                }
                return cumulativeDataOutputText;
            }catch (Exception ex)
            {
                Console.WriteLine($"Error adding subsequent rows - {ex.Message}");
                throw;
            }
            
        }
    }
}
