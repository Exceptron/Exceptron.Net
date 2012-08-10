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
        private ExceptronClient _clinet;
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

            _clinet = new ExceptronClient(new ExceptronConfiguration { ApiKey = ApiKey }) { RestClient = _fakeRestClient.Object };

            var httpResponse = new HttpResponse(null);
            var httpRequest = new HttpRequest("", "http://www.somebrokensite/folder", "?string=query");
            _httpContext = new HttpContext(httpRequest, httpResponse);

            FakeExceptionData.HttpContext = _httpContext;

        }
    }

}
