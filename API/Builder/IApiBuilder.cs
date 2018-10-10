using InstaBot.API.Logger;

namespace InstaBot.API.Builder
{
    public interface IApiBuilder
    {

        IApi Build();

        IApiBuilder SetUser(string userName, string password);

        IApiBuilder UseStockApi(bool useStockApi);

        IApiBuilder SetKeys(string applicationId, string secretKey);

        IApiBuilder UseLogger(ILogger logger);
    }
}