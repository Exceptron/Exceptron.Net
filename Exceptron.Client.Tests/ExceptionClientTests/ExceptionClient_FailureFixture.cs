using System;
using System.Configuration;
using Exceptron.Client.Configuration;
using Exceptron.Client.Message;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exceptron.Client.Tests.ExceptionClientTests
{
    [TestFixture]
    public class ExceptionClient_FailureFixture : ClientTest
    {
        private ExceptionClient _clinet;
        private Mock<IRestClient> _fakeRestClient;
        private ExceptionData _validException;

        [SetUp]
        public void Setup()
        {
            _fakeRestClient = new Mock<IRestClient>();
            _clinet = new ExceptionClient(new ExceptronConfiguration { ApiKey = ApiKey }) { RestClient = _fakeRestClient.Object };
            _validException = new ExceptionData
                {
                    Exception = new TestException(),
                    Component = "Test",
                    Message = "Test",
                    Severity = ExceptionSeverity.None,
                    UserId = "12"
                };


        }

        [TestCase("")]
        [TestCase(null)]
        public void should_throw_if_new_instance_is_created_without_api_key(string apiKey)
        {
            Assert.Throws<ArgumentException>(() =>
                                             new ExceptionClient(new ExceptronConfiguration
                                                 {
                                                     ApiKey = apiKey
                                                 }));
        }

        [Test]
        public void should_throw_if_configuration_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ExceptionClient(null));
        }

        [Test]
        public void should_throw_exception_if_ThrowExceptions_flag_is_set()
        {
            _clinet.Configuration.ThrowExceptions = true;
            _fakeRestClient.Setup(c => c.Put<ExceptionResponse>(It.IsAny<string>(), It.IsAny<object>())).
                Throws(new TestException());

            Assert.Throws<TestException>(() => _clinet.SubmitException(FakeExceptionData));
        }


        [Test]
        public void should_return_exception_if_ThrowExceptions_flag_is_not_set()
        {
            _clinet.Configuration.ThrowExceptions = false;
            _fakeRestClient.Setup(c => c.Put<ExceptionResponse>(It.IsAny<string>(), It.IsAny<object>())).
                Throws(new TestException());

            var response = _clinet.SubmitException(FakeExceptionData);

            AssertFailedResponse(response);
        }


        [TestCase(true)]
        [TestCase(false)]
        public void null_ErrorData_should_cause_null_exception(bool throwsExceptions)
        {
            _clinet.Configuration.ThrowExceptions = throwsExceptions;
            MayThrow<ArgumentNullException>(() => _clinet.SubmitException(null), throwsExceptions);
        }

        [TestCase("", true)]
        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void missing_api_key_should_throw(string apiKey, bool throws)
        {
            _clinet.Configuration.ThrowExceptions = throws;
            _clinet.Configuration.ApiKey = apiKey;

            MayThrow<InvalidOperationException>(() => _clinet.SubmitException(_validException), throws);
        }


        [TestCase(true)]
        [TestCase(false)]
        public void null_exception_should_cause_argument_exception(bool throwsExceptions)
        {
            _clinet.Configuration.ThrowExceptions = throwsExceptions;
            var error = new ExceptionData { Exception = null };

            MayThrow<ArgumentException>(() => _clinet.SubmitException(error), throwsExceptions);
        }

        private static T MayThrow<T>(TestDelegate code, bool shouldThrow) where T : Exception
        {
            if (shouldThrow)
            {
                return Assert.Throws<T>(code);
            }
            else
            {
                code.Invoke();
                return null;
            }
        }

    }

}
