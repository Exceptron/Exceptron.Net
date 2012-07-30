using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Exceptron.Client.Configuration;
using Exceptron.Client.Message;

namespace Exceptron.Client
{
    public class ExceptionClient : IExceptionClient
    {
        internal IRestClient RestClient { get; set; }

        /// <summary>
        /// Version of Client
        /// </summary>
        public string ClientVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }


        /// <summary>
        /// Name of Client
        /// </summary>
        public string ClientName
        {
            get { return "Official .NET"; }
        }

        /// <summary>
        /// Client Configuration
        /// </summary>
        public ExceptronConfiguration Configuration { get; private set; }

        /// <summary>
        /// Environment that the application is running in
        /// </summary>
        /// <example>
        /// Dev, Staging, Production
        /// </example>
        public string Enviroment { get; set; }

        /// <summary>
        /// Version of application executing. Default: Version of <see cref="Assembly.GetEntryAssembly()"/>
        /// </summary>
        public string ApplicationVersion { get; set; }



        /// <summary>
        /// Creates a new instance of <see cref="ExceptionClient"/>
        /// Loads <see cref="ExceptronConfiguration"/> from application config file.
        /// </summary>
        public ExceptionClient()
            : this(ExceptronConfiguration.ReadConfig())
        {
        }

        /// <param name="exceptronConfiguration">Exceptron client configuration</param>
        public ExceptionClient(ExceptronConfiguration exceptronConfiguration)
        {
            if (exceptronConfiguration == null)
                throw new ArgumentNullException("exceptronConfiguration");

            if (string.IsNullOrEmpty(exceptronConfiguration.ApiKey))
                throw new ArgumentException("An API Key was not provided");

            Configuration = exceptronConfiguration;

            RestClient = new RestClient();

            SetApplicationVersion();
        }

        /// <summary>
        /// Submit an exception to Exceptron Servers.
        /// </summary>
        /// <param name="exceptionData">Exception data to be reported to the server</param>
        public ExceptionResponse SubmitException(ExceptionData exceptionData)
        {
            try
            {
                if (string.IsNullOrEmpty(Configuration.ApiKey))
                    throw new InvalidOperationException("ApiKey has not been provided for this client.");

                if (exceptionData == null)
                    throw new ArgumentNullException("exceptionData");

                if (exceptionData.Exception == null)
                    throw new ArgumentException("ExceptionData.Exception Cannot be null.", "exceptionData");

                var report = new ExceptionReport();

                report.ap = Configuration.ApiKey;
                report.dn = ClientName;
                report.dv = ClientVersion;
                report.aver = ApplicationVersion;

                report.ext = exceptionData.Exception.GetType().FullName;
                report.stk = ConvertToFrames(exceptionData.Exception);
                report.exm = exceptionData.Exception.Message;

                report.cmp = exceptionData.Component;
                report.uid = exceptionData.UserId;
                report.env = Enviroment;
                report.msg = exceptionData.Message;
                report.cul = Thread.CurrentThread.CurrentCulture.Name;
                report.sv = (int)exceptionData.Severity;

                try
                {
                    report.os = Environment.OSVersion.VersionString;
                }
                catch (Exception)
                {
                    if (Configuration.ThrowExceptions) throw;
                }

                if (Configuration.IncludeMachineName)
                {
                    try
                    {
                        report.hn = Environment.MachineName;
                    }
                    catch (Exception)
                    {
                        if (Configuration.ThrowExceptions) throw;
                    }
                }

                var response = RestClient.Put<ExceptionResponse>(Configuration.Host, report);

                return response;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Unable to submit exception to exceptron. ", e.ToString());

                if (Configuration.ThrowExceptions)
                {
                    throw;
                }
                else
                {
                    return new ExceptionResponse { Exception = e };
                }
            }
        }


        private void SetApplicationVersion()
        {
            try
            {
                var entryAssembly = GetWebEntryAssembly();

                if (entryAssembly == null)
                {
                    entryAssembly = Assembly.GetEntryAssembly();
                }

                if (entryAssembly == null)
                {
                    entryAssembly = Assembly.GetCallingAssembly();
                }

                if (entryAssembly != null)
                {
                    ApplicationVersion = entryAssembly.GetName().Version.ToString();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Can't figure out application version.", e.ToString());
            }
        }

        static private Assembly GetWebEntryAssembly()
        {
            if (System.Web.HttpContext.Current == null ||
                System.Web.HttpContext.Current.ApplicationInstance == null)
            {
                return null;
            }

            var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }

        internal static List<Frame> ConvertToFrames(Exception exception)
        {
            if (exception == null) return null;

            var stackTrace = new StackTrace(exception, true);

            var frames = stackTrace.GetFrames();

            if (frames == null) return null;

            var result = new List<Frame>();

            for (int index = 0; index < frames.Length; index++)
            {
                var frame = frames[index];
                var method = frame.GetMethod();
                var declaringType = method.DeclaringType;

                var currentFrame = new Frame
                {
                    i = index,
                    fn = frame.GetFileName(),
                    ln = frame.GetFileLineNumber(),
                    m = method.ToString(),
                };


                currentFrame.m = currentFrame.m.Substring(currentFrame.m.IndexOf(' ')).Trim();


                if (declaringType != null)
                {
                    currentFrame.c = declaringType.FullName;
                }

                result.Add(currentFrame);
            }


            return result;
        }

    }
}
