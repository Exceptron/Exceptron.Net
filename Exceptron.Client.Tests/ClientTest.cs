using System;
using System.Diagnostics;
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
        protected const string Url = "http://localhost:57674/v1/";
        protected const string ApiKey = "9c95215de676416a96cbfbc20915839f";


        protected ExceptionData FakeExceptionData { get; set; }

        [SetUp]
        [DebuggerStepThrough]
        public void ClientTestSetup()
        {
            Console.WriteLine("Client version: {0}", Assembly.GetExecutingAssembly().GetName().Version);
            
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
            response.RefId.Should().BeNull();
            response.Successful.Should().BeFalse();
            response.Exception.Should().NotBeNull();
            response.Exception.Should().BeOfType<T>();
        }

        protected static void AssertSuccessfulResponse(ExceptionResponse response)
        {
            response.Should().NotBeNull();
            response.Successful.Should().BeTrue("API call failed");
            response.RefId.Should().HaveLength(8, "Invalid RefId");
            response.Exception.Should().BeNull("Exception object was not excepted");
        }

        protected static bool InTeamCity()
        {
            return !String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));
        }
    }
}