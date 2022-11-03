using ClaimsReservationService.Model;

namespace ClaimsReservationService
{
    public interface ICumulativeClaim
    {
        CumulativeClaimData GenerateCumulativeDataOutputContent(List<IncrementalDataModel> incrementValues);
    }
}