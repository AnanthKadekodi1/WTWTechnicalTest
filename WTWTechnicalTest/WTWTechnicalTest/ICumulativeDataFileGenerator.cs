using ClaimsReservationService.Model;

namespace WTWTechnicalTestConsoleApp
{
    public interface ICumulativeDataFileGenerator
    {
        string GenerateOutputFile(CumulativeClaimData cumulativeClaimData);
    }
}