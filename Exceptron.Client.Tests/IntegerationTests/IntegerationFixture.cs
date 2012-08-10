using System;
using System.Net;
using Exceptron.Client.Configuration;
using Exceptron.Client.Message;
using FluentAssertions;
using NUnit.Framework;

namespace Exceptron.Client.Tests.IntegerationTests
{
    [TestFixture(Category = "Integeration")]
    public class IntegerationFixture : ClientTest
    {
        private ExceptronClient _exceptronClient;

        [SetUp]
        public void Setup()
        {
            _exceptronClient = new ExceptronClient(new ExceptronConfiguration { ApiKey = ApiKey });
        
            if(InTeamCity())
            {
                _exceptronClient.Configuration.Host = Environment.GetEnvironmentVariable("ET.HostUrl");
            }
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

            AssertFailedResponse<ExceptronApiException>(response);

            AssertResponseCode(response,HttpStatusCode.BadRequest);
        }

        [Test]
        public void bad_token_should_return_401()
        {
            _exceptronClient.Configuration.ThrowExceptions = false;

            _exceptronClient.Configuration.ApiKey = "CB230C312E5C4FF38B4FB9644B05E000";
            var response = _exceptronClient.SubmitException(FakeExceptionData);

            AssertFailedResponse<ExceptronApiException>(response);

            AssertResponseCode(response, HttpStatusCode.Unauthorized);
        }

        [TestCase("http://www.somewrongdomain.com/")]
        public void communication_issue_should_return_faild_result(string url)
        {
            _exceptronClient.Configuration.Host = url;
            _exceptronClient.Configuration.ThrowExceptions = false;

            ExceptionResponse response = _exceptronClient.SubmitException(FakeExceptionData);

            AssertFailedResponse<WebException>(response);
        }



        private static void AssertResponseCode(ExceptionResponse response, HttpStatusCode statusCode)
        {
            response.Should().NotBeNull();
            response.Exception.Should().NotBeNull();
            response.Exception.InnerException.Should().BeOfType<WebException>();

            var httpResponse = ((WebException)response.Exception.InnerException).Response as HttpWebResponse;
            httpResponse.StatusCode.Should().Be(statusCode);
        }
    }
}
