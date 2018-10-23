using InstaBot.Logger;

namespace InstaBot.Builder
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