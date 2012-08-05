using System;
using System.Linq;
using System.Net;
using Exceptron.Client.Message;
using FizzWare.NBuilder;
using FluentAssertions;
using NUnit.Framework;

namespace Exceptron.Client.Tests.RestClientTests
{
    [TestFixture]
    public class RestClientFixture: ClientTest
    {
        private ExceptionReport _exceptionReport;
        private RestClient _restClient;

        [SetUp]
        public void Setup()
        {
            _exceptionReport = Builder<ExceptionReport>.CreateNew()
                .With(c => c.stk = Builder<Frame>.CreateListOfSize(2).Build().ToList())
                .With(c => c.ap = ApiKey).Build();

            _restClient = new RestClient();
        }

        [Test]
        public void valid_request_should_return_response()
        {
            var result = _restClient.Put<ExceptionResponse>(Url, _exceptionReport);
            result.Should().NotBeNull();
        }


        [Test]
        public void bad_token_should_throw_401()
        {
            _exceptionReport.ap = "CB230C312E5C4FF38B4FB9644B05EXXX";
            var exception = Assert.Throws<ExceptronApiException>(() => _restClient.Put<ExceptionResponse>(Url, _exceptionReport));
            
            ((exception.Response)).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void missing_stack_should_throw_400()
        {
            _exceptionReport.stk = null;
            var exception = Assert.Throws<ExceptronApiException>(() => _restClient.Put<ExceptionResponse>(Url, _exceptionReport));

            exception.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public void invalid_url_should_throw()
        {
            Assert.Throws<WebException>(() => _restClient.Put<ExceptionResponse>("http://somebadurl.com", _exceptionReport));
        }

        [Test]
        public void null_url_should_throw()
        {
            Assert.Throws<ArgumentNullException>(() => _restClient.Put<ExceptionResponse>(null, _exceptionReport));
        }

        [Test]
        public void blank_url_should_throw()
        {
            Assert.Throws<ArgumentNullException>(() => _restClient.Put<ExceptionResponse>("", _exceptionReport));
        }

        [Test]
        public void null_report_should_throw()
        {
            Assert.Throws<ArgumentNullException>(() => _restClient.Put<ExceptionResponse>(Url, null));
        }

    }

}
