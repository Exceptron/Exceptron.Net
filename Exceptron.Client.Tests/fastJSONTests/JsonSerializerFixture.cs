using System.Linq;
using Exceptron.Client.Message;
using Exceptron.Client.fastJSON;
using FizzWare.NBuilder;
using FluentAssertions;
using NUnit.Framework;
using ServiceStack.Text;

namespace Exceptron.Client.Tests.fastJSONTests
{
    [TestFixture]
    public class JsonSerializerFixture
    {
        private ExceptionReport _report;

        [SetUp]
        public void Setup()
        {
            _report = Builder<ExceptionReport>.CreateNew()
                .With(c => c.stk = Builder<Frame>.CreateListOfSize(3).Build().ToList())
                .Build();
        }

        [Test]
        public void should_be_able_to_do_a_serilization_roundtrip()
        {
            var json = JSON.Instance.ToJSON(_report);
            var hydratedObject = JSON.Instance.ToObject<ExceptionReport>(json);

            hydratedObject.ShouldHave().AllPropertiesBut(c => c.stk).EqualTo(_report);
            hydratedObject.stk.Should().HaveSameCount(_report.stk);
            hydratedObject.stk.Should().NotContain(c => string.IsNullOrEmpty(c.c));
            hydratedObject.stk.Should().NotContain(c => string.IsNullOrEmpty(c.fn));
            hydratedObject.stk.Should().NotContain(c => string.IsNullOrEmpty(c.m));
            hydratedObject.stk.Should().NotContain(c => c.ln < 0);
        }

        [Test]
        public void should_be_able_to_do_a_serilization_roundtrip_using_servicestack_text()
        {
            var json = JSON.Instance.ToJSON(_report);
            var hydratedObject = JsonSerializer.DeserializeFromString<ExceptionReport>(json);

            hydratedObject.ShouldHave().AllPropertiesBut(c => c.stk).EqualTo(_report);
            hydratedObject.stk.Should().HaveSameCount(_report.stk);
            hydratedObject.stk.Should().NotContain(c => string.IsNullOrEmpty(c.c));
            hydratedObject.stk.Should().NotContain(c => string.IsNullOrEmpty(c.fn));
            hydratedObject.stk.Should().NotContain(c => string.IsNullOrEmpty(c.m));
            hydratedObject.stk.Should().NotContain(c => c.ln < 0);
        }


        [Test]
        public void should_not_serilize_backing_fields()
        {
            var json = JSON.Instance.ToJSON(_report);

            json.Should().NotContain("BackingField");
        }


        [Test]
        public void servicestack_serilized_message_and_fastjson_serilized_messages_should_be_the_same_size()
        {
            var fastJsonText = JSON.Instance.ToJSON(_report);
            var serviceStackText = JsonSerializer.SerializeToString(_report);

            fastJsonText.Should().HaveLength(serviceStackText.Length);
        }

    }

}
