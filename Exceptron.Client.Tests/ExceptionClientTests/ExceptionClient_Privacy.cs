using System;
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
    public class ExceptionClient_Privacy : ClientTest
    {
        private ExceptronClient _client;
        private Mock<IRestClient> _fakeRestClient;
        private ExceptionReport _submittedReport;

        [SetUp]
        public void Setup()
        {
            _fakeRestClient = new Mock<IRestClient>();
            _fakeRestClient
                        .Setup(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.IsAny<ExceptionReport>()))
                        .Callback<string, object>((target, report) => _submittedReport = (ExceptionReport)report);

            _client = new ExceptronClient(new ExceptronConfiguration { ApiKey = ApiKey }, appVersion) { RestClient = _fakeRestClient.Object };
        }


        [Test]
        public void private_machine_key_should_not_be_sent()
        {
            _client.Configuration.IncludeMachineName = false;

            _client.SubmitException(FakeExceptionData);

            _submittedReport.Should().NotBeNull();
            _submittedReport.hn.Should().BeNull();

        }


        [Test]
        public void message_should_contain_driver_info()
        {
            _client.SubmitException(FakeExceptionData);

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.dn == _client.ClientName)), Times.Once());

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.dv == _client.ClientVersion)), Times.Once());
        }

        [Test]
        public void message_should_contain_exception_data()
        {
            _client.SubmitException(FakeExceptionData);

            _fakeRestClient
                .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.cmp == FakeExceptionData.Component)), Times.Once());

            _fakeRestClient
                 .Verify(r => r.Put<ExceptionResponse>(It.IsAny<string>(), It.Is<ExceptionReport>(report => report.aver == appVersion.ToString())), Times.Once());

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
