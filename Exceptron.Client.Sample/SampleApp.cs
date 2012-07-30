using System;
using Exceptron.Client.Configuration;

namespace Exceptron.Client.Sample
{
    class SampleApp
    {
        static void Main(string[] args)
        {

            

            var exceptronConfig = new ExceptronConfiguration
                                      {
                                          ApiKey = "YOUR_API_TOKE",
                                          IncludeMachineName = true,
                                          ThrowExceptions = true
                                      };

            //Create new instance of exceptron client
            //Configuration will be read from config file if no configuration is passed in.
            var exceptron = new ExceptronClient(exceptronConfig);

            try
            {
                CallToBrokenMethod();
            }
            catch (Exception e)
            {
                exceptron.SubmitException(e, "Main", ExceptionSeverity.Fatal, "Couldn't call the broken method", "User1");
            }

        }

        private static int CallToBrokenMethod()
        {
            string nullString = null;

            return nullString.Length;
        }
    }
}
