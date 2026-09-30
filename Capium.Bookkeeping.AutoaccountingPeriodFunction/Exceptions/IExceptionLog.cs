namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Exceptions
{
    public interface IExceptionLog
    {
        Task LogInformations(string message);

        Task LogExceptions(Exception exception);

        Task LogExceptions(Exception exception, string message);

        Task LogRequestResponse(int statusCode, string url, string request, string response);
    }
}
