using System.Reflection;
using System.Web;
using Exceptron.Client.Configuration;
using Exceptron.Client.Message;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exceptron.Client.Tests.ExceptionClientTests
{
    [TestFixture]
    public class ExceptionClient_HttpExceptions : ClientTest
    {
        private ExceptronClient _client;
        private Mock<IRestClient> _fakeRestClient;
        private ExceptionReport _submittedReport;
        private HttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _fakeRestClient = new Mock<IRestClient>();
            _fakeRestClient
                        .Setup(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.IsAny<ExceptionReport>()))
                        .Callback<string, object>((target, report) => _submittedReport = (ExceptionReport)report);

            _client = new ExceptronClient(new ExceptronConfiguration { ApiKey = ApiKey }, appVersion) { RestClient = _fakeRestClient.Object };

            var httpResponse = new HttpResponse(null);
            var httpRequest = new HttpRequest("", "http://www.somebrokensite/folder?string=query","");

            _httpContext = new HttpContext(httpRequest, httpResponse);

            FakeExceptionData.HttpContext = _httpContext;

        }
        
        [Test]
        public void http_exception_should_submit_http_info()
        {
            _client.SubmitException(FakeExceptionData);

            _submittedReport.Should().NotBeNull();

            _submittedReport.url.Should().Be("http://www.somebrokensite/folder?string=query");
        }
    }

}
