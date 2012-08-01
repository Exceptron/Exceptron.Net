using System.Reflection;
using Exceptron.Client.Configuration;
using Exceptron.Client.Message;

namespace Exceptron.Client
{
    public interface IExceptronClient
    {
        /// <summary>
        /// Client Configuration
        /// </summary>
        ExceptronConfiguration Configuration { get; }

        /// <summary>
        /// Version of application executing. Default: Version of <see cref="Assembly.GetEntryAssembly()"/>
        /// </summary>
        string ApplicationVersion { get; set; }

        /// <summary>
        /// Submit an exception to Exceptron Servers.
        /// </summary>
        /// <param name="exceptionData">Exception data to be reported to the server</param>
        ExceptionResponse SubmitException(ExceptionData exceptionData);
    }
}