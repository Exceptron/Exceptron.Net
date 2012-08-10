using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Exceptron.Client.Message;
using FizzWare.NBuilder;
using FluentAssertions;
using NUnit.Framework;

namespace Exceptron.Client.Tests
{
    public abstract class ClientTest
    {
        protected const string Url = "http://api.exceptron.com/v1/";
        protected const string ApiKey = "CB230C312E5C4FF38B4FB9644B05E60G";


        protected ExceptionData FakeExceptionData { get; set; }

        [SetUp]
        [DebuggerStepThrough]
        public void ClientTestSetup()
        {
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
            response.Successful.Should().BeTrue();
            response.RefId.Should().HaveLength(8);
            response.Exception.Should().BeNull();
        }

        protected static bool InTeamCity()
        {
            return !String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));
        }
    }
}