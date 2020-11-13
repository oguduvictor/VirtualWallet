using System;

namespace VirtualWallet.Application
{
    public static class EnvironmentHelper
    {
        public static string GetEnvironment()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Process)?.ToLower();
            if (string.IsNullOrEmpty(environmentName))
            {
                return "Production";
            }

            return environmentName;
        }
    }
}