using System.Configuration;
using Exceptron.Client.Message;

namespace Exceptron.Client.Configuration
{

    public class ExceptronConfiguration : ConfigurationSection
    {
        public static ExceptronConfiguration ReadConfig(string sectionName = "exceptron")
        {
            return ConfigurationManager.GetSection(sectionName) as ExceptronConfiguration;
        }

        public ExceptronConfiguration()
        {
            Host = "https://api.exceptron.com/v1/";
        }

        public string Host { get; internal set; }

        /// <summary>
        /// If ExceptronClinet should throw exceptions in case of an error. Default: <see cref="bool.False"/>
        /// </summary>
        /// <remarks>
        /// Its recommended that this flag is set to True during development and <see cref="bool.False"/> in production systems.
        /// If an exception is thrown while this flag is set to <see cref="bool.False"/> the thrown exception will be returned in <see cref="ExceptionResponse.Exception"/>
        /// </remarks>
        [ConfigurationProperty("throwExceptions", DefaultValue = false)]
        public bool ThrowExceptions { get; set; }

        [ConfigurationProperty("apiKey")]
        public string ApiKey { get; set; }

    }
}
