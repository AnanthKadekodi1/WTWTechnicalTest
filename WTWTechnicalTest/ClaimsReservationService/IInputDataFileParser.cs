using ClaimsReservationService.Model;

namespace ClaimsReservationService
{
    public interface IIncrementalDataFileParser
    {
        List<IncrementalDataModel> Parse();
    }
}