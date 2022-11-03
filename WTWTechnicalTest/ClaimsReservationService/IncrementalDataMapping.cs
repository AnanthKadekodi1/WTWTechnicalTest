using ClaimsReservationService.Model;
using TinyCsvParser.Mapping;

namespace ClaimsReservationService
{
    public class IncrementalDataMapping: CsvMapping<IncrementalDataModel>
    {
        public IncrementalDataMapping()
        {
            MapProperty(0, x => x.Product);
            MapProperty(1, x => x.OriginYear);
            MapProperty(2, x => x.DevelopmentYear);
            MapProperty(3, x => x.Increment);
        }
    }
}
