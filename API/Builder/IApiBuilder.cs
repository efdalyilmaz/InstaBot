using System.Net.Http;
using InstaSharper.Classes;
using InstaSharper.Classes.Android.DeviceInfo;
using InstaSharper.Logger;

namespace InstaBot.API.Builder
{
    public interface IApiBuilder
    {

        IApi Build();

        IApiBuilder SetUser(string userName, string password);

        IApiBuilder UseStockApi(bool useStockApi);

        IApiBuilder SetKeys(string applicationId, string secretKey);
    }
}