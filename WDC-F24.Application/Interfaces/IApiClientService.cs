


namespace WDC_F24.Application.Interfaces
{
    public interface IApiClientService
    {
        Task<bool> IsValidApiKey(string apiKey);
        Task<string> GenerateApiKey(string appName);
    }
}
