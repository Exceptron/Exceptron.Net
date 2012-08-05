using System;
using Exceptron.Client.Configuration;

namespace Exceptron.Client.Sample
{
    class SampleApp
    {
        static void Main(string[] args)
        {
            ExceptronClient exceptron = null;

            //exceptron = GetClientConfiguredAtRuntime();
            exceptron = GetClientUsingConfigFile();

            try
            {
                CallToBrokenMethod();
            }
            catch (Exception e)
            {
                exceptron.SubmitException(e, "Main", ExceptionSeverity.Fatal, "Couldn't call the broken method", "User1");
            }

        }


        private static ExceptronClient GetClientUsingConfigFile()
        {
            return new ExceptronClient();
        }
         
        private static ExceptronClient GetClientConfiguredAtRuntime()
        {
            var exceptronConfig = new ExceptronConfiguration
            {
                ApiKey = "YOUR_API_TOKE",
                IncludeMachineName = true,
                ThrowExceptions = true
            };

            return new ExceptronClient(exceptronConfig);
        }

        private static int CallToBrokenMethod()
        {
            string nullString = null;

            return nullString.Length;
        }
    }
}
