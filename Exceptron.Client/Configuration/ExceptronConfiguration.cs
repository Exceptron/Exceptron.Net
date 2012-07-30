using System.Configuration;
using Exceptron.Client.Message;

namespace Exceptron.Client.Configuration
{

    public class ExceptronConfiguration : ConfigurationSection
    {
        public ExceptronConfiguration()
        {
            Host = "https://api.exceptron.com/v1/";
            IncludeMachineName = true;
        }

        public static ExceptronConfiguration ReadConfig(string sectionName = "exceptron")
        {
            return ConfigurationManager.GetSection(sectionName) as ExceptronConfiguration;
        }

        internal string Host { get; set; }

        /// <summary>
        /// If ExceptronClinet should throw exceptions in case of an error. Default: <see cref="bool.False"/>
        /// </summary>
        /// <remarks>
        /// Its recommended that this flag is set to True during development and <see cref="bool.False"/> in production systems.
        /// If an exception is thrown while this flag is set to <see cref="bool.False"/> the thrown exception will be returned in <see cref="ExceptionResponse.Exception"/>
        /// </remarks>
        [ConfigurationProperty("throwExceptions", DefaultValue = false)]
        public bool ThrowExceptions { get; set; }

        /// <summary>
        /// The API of this application. Can find your API key in application settings page.
        /// </summary>
        [ConfigurationProperty("apiKey")]
        public string ApiKey { get; set; }


        /// <summary>
        /// If the machine name should be attached to the exception report
        /// </summary>
        /// <remarks>Machine name can be usefull in webfarm enviroments when multiple
        /// servers are running the same app and the issue could be machine specific.
        /// Hoewever, You might want to disable this feature for privacy reasons.</remarks>
        [ConfigurationProperty("includeMachineName", DefaultValue = true)]
        public bool IncludeMachineName { get; set; }

    }
}
