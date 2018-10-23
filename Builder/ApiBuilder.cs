using InstaBot.Logger;
using System;

namespace InstaBot.Builder
{
    public class ApiBuilder : IApiBuilder
    {
        private string userName;
        private string password;

        private bool useStockApi;
        private string applicationId;
        private string secretKey;
        private ILogger logger;


        private ApiBuilder()
        {
        }


        public IApi Build()
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException($"UserName must be specified");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException($"Password must be specified");

            if (useStockApi)
            {
                if (String.IsNullOrEmpty(applicationId))
                    throw new ArgumentNullException($"UserName must be specified");
                if (String.IsNullOrEmpty(secretKey))
                    throw new ArgumentNullException($"Password must be specified");

                return new Api(logger, userName, password, applicationId, secretKey);
            }

            return new Api(logger, userName, password);
        }

        public IApiBuilder SetUser(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
            return this;
        }

        public IApiBuilder UseStockApi(bool useStockApi)
        {
            this.useStockApi = useStockApi;
            return this;
        }

        public IApiBuilder SetKeys(string applicationId, string secretKey)
        {
            this.applicationId = applicationId;
            this.secretKey = secretKey;
            return this;
        }

        public IApiBuilder UseLogger(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        public static IApiBuilder CreateBuilder()
        {
            return new ApiBuilder();
        }
    }
}