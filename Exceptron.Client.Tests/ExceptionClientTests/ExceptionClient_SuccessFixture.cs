using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using Exceptron.Client.Configuration;
using Exceptron.Client.Message;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exceptron.Client.Tests.ExceptionClientTests
{
    [TestFixture]
    public class ExceptionClient_SuccessFixture : ClientTest
    {
        private ExceptionClient _clinet;
        private Mock<IRestClient> _fakeRestClient;

        [SetUp]
        public void Setup()
        {
            _fakeRestClient = new Mock<IRestClient>();
            _clinet = new ExceptionClient(new ExceptronConfiguration { ApiKey = ApiKey }) { RestClient = _fakeRestClient.Object };
        }


        [Test]
        public void message_should_contain_api_key()
        {
            _clinet.SubmitException(FakeExceptionData);

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.ap == _clinet.Configuration.ApiKey)), Times.Once());
        }


        [Test]
        public void message_should_contain_driver_info()
        {
            _clinet.SubmitException(FakeExceptionData);

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.dn == _clinet.ClientName)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.dv == _clinet.ClientVersion)), Times.Once());
        }

        [Test]
        public void message_should_contain_exception_data()
        {
            _clinet.SubmitException(FakeExceptionData);

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.cmp == FakeExceptionData.Component)), Times.Once());

            _fakeRestClient
                 .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.aver == _clinet.ApplicationVersion )), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.env == _clinet.Enviroment)), Times.Once());
        

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.exm == FakeExceptionData.Exception.Message)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.ext == "Exceptron.Client.Tests.TestException")), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.msg == FakeExceptionData.Message)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.stk.Any())), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.sv == (int)FakeExceptionData.Severity)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.uid == FakeExceptionData.UserId)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.cul == Thread.CurrentThread.CurrentCulture.Name)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.os == Environment.OSVersion.VersionString)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.hn == Environment.MachineName)), Times.Once());

        }








    }

}
