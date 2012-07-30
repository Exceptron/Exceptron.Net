using Exceptron.Client.Configuration;
using Exceptron.Client.Message;
using NUnit.Framework;

namespace Exceptron.Client.Tests.IntegerationTests
{
    public class V1IntegerationTests : ClientTest
    {
        private ExceptionClient _exceptionClient;

        [SetUp]
        public void Setup()
        {
            _exceptionClient = new ExceptionClient(new ExceptronConfiguration { ApiKey = ApiKey });
        }


        [Test]
        public void should_be_able_to_submit_exception()
        {
            var response = _exceptionClient.SubmitException(FakeExceptionData);

            AssertSuccessfulResponse(response);
        }

        [Test]
        public void invalid_request_should_return_faild_result()
        {
            _exceptionClient.Configuration.ThrowExceptions = false;

            FakeExceptionData.Component = "";
            var response = _exceptionClient.SubmitException(FakeExceptionData);

            AssertFailedResponse(response);
        }

        [TestCase("http://www.google.com/")]
        [TestCase("http://www.somewrongdomain.com/")]
        public void communication_issue_should_return_faild_result(string url)
        {
            _exceptionClient.Configuration.Host = url;
            _exceptionClient.Configuration.ThrowExceptions = false;

            ExceptionResponse response = _exceptionClient.SubmitException(FakeExceptionData);

            AssertFailedResponse(response);
        }
    }
}
