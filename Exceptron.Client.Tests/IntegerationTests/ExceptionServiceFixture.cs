using Exceptron.Client.Configuration;
using Exceptron.Client.Message;
using NUnit.Framework;

namespace Exceptron.Client.Tests.IntegerationTests
{
    public class V1IntegerationTests : ClientTest
    {
        private ExceptronClient _exceptronClient;

        [SetUp]
        public void Setup()
        {
            _exceptronClient = new ExceptronClient(new ExceptronConfiguration { ApiKey = ApiKey });
        }


        [Test]
        public void should_be_able_to_submit_exception()
        {
            var response = _exceptronClient.SubmitException(FakeExceptionData);

            AssertSuccessfulResponse(response);
        }

        [Test]
        public void invalid_request_should_return_faild_result()
        {
            _exceptronClient.Configuration.ThrowExceptions = false;

            FakeExceptionData.Component = "";
            var response = _exceptronClient.SubmitException(FakeExceptionData);

            AssertFailedResponse(response);
        }

        [TestCase("http://www.google.com/")]
        [TestCase("http://www.somewrongdomain.com/")]
        public void communication_issue_should_return_faild_result(string url)
        {
            _exceptronClient.Configuration.Host = url;
            _exceptronClient.Configuration.ThrowExceptions = false;

            ExceptionResponse response = _exceptronClient.SubmitException(FakeExceptionData);

            AssertFailedResponse(response);
        }
    }
}
