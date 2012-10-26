using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Exceptron.Client.Message;
using FizzWare.NBuilder;
using FluentAssertions;
using NUnit.Framework;

namespace Exceptron.Client.Tests
{
    public abstract class ClientTest
    {
        protected const string Url = "https://api.exceptron.com/v1/";
        protected const string ApiKey = "9c95215de676416a96cbfbc20915839f";

        static readonly ConsoleTraceListener ConsoleTraceListener = new ConsoleTraceListener(false);

        protected ExceptionData FakeExceptionData { get; set; }


        protected readonly Version appVersion = new Version(1,0,1,5);

        [SetUp]
        [DebuggerStepThrough]
        public void ClientTestSetup()
        {
            if (InTeamCity() && !Trace.Listeners.Contains(ConsoleTraceListener))
            {
                Trace.Listeners.Add(ConsoleTraceListener);
            }

            Console.WriteLine("Client version: {0}", typeof(ExceptronClient).Assembly.GetName().Version);

            FakeExceptionData = Builder<ExceptionData>.CreateNew()
                .With(c => c.Exception = GetThrownException(new TestException()))
                .Build();

        }

        private static Exception GetThrownException(Exception exception)
        {
            try
            {
                ThrowException(exception);
            }
            catch (Exception e)
            {
                return e;
            }

            return null;
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        private static void ThrowException(Exception exception)
        {
            throw exception;
        }

        protected static void AssertFailedResponse<T>(ExceptionResponse response) where T : Exception
        {
            response.Should().NotBeNull();
            response.Exception.Should().BeOfType<T>();
            response.RefId.Should().BeNull();
            response.Successful.Should().BeFalse();
        }

        protected static void AssertSuccessfulResponse(ExceptionResponse response)
        {
            response.Should().NotBeNull();
            response.Successful.Should().BeTrue("ExceptionResponse.Successful");
            response.RefId.Should().HaveLength(8, "ExceptionResponse.RefId");
            response.Exception.Should().BeNull("ExceptionResponse.Exception");
        }

        protected static bool InTeamCity()
        {
            return !String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));
        }
    }
}