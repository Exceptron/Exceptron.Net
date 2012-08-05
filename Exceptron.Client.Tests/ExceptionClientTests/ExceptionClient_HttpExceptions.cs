using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
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
        private ExceptionReport _submitedReport;
        private HttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _fakeRestClient = new Mock<IRestClient>();
            _fakeRestClient
                        .Setup(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.IsAny<ExceptionReport>()))
                        .Callback<string, object>((target, report) => _submitedReport = (ExceptionReport)report);

            _clinet = new ExceptronClient(new ExceptronConfiguration { ApiKey = ApiKey }) { RestClient = _fakeRestClient.Object };

            var httpResponse = new HttpResponse(null);
            var httpRequest = new HttpRequest("", "http://www.somebrokensite/folder", "?string=query");
            _httpContext = new HttpContext(httpRequest, httpResponse);

            FakeExceptionData.HttpContext = _httpContext;

        }


        [Test]
        public void status_code_should_be_set_to_context_statuscode()
        {
            _httpContext.Response.StatusCode = 401;
            _clinet.SubmitException(FakeExceptionData);

            _submitedReport.sc.Should().Be(401);

        }
    }

}
