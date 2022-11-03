namespace ClaimsReservationService
{
    public interface IAppConfig
    {
        string InputDataFilePath { get; set; }
        string OutputDataFilePath { get; set; }
    }
}