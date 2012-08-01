using Exceptron.Client.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Exceptron.Client.Tests.ConfigurationTests
{
    [TestFixture]
    public class ExceptronConfigurationFixture
    {
        [Test]
        public void missing_config_section_should_not_have()
        {
            ExceptronConfiguration.ReadConfig("missing_section").Should().BeNull();
        }

        [Test]
        public void should_be_able_to_read_the_default_config_section()
        {
            ExceptronConfiguration.ReadConfig().Should().NotBeNull();
            ExceptronConfiguration.ReadConfig().ApiKey.Should().Be("ABCD");
            ExceptronConfiguration.ReadConfig().ThrowExceptions.Should().BeTrue();
            ExceptronConfiguration.ReadConfig().IncludeMachineName.Should().BeTrue();
        }


        [Test]
        public void api_key_should_be_null_if_key_is_not_provided()
        {
            ExceptronConfiguration.ReadConfig("exceptron_noapikey").Should().NotBeNull();
            ExceptronConfiguration.ReadConfig("exceptron_noapikey").ApiKey.Should().BeEmpty();
        }


        [Test]
        public void url_should_point_to_correct_address()
        {
            ExceptronConfiguration.ReadConfig().Host.Should().StartWith("https://api.exceptron.com/v");
        }


        [Test]
        public void config_should_have_proper_default_values()
        {
            ExceptronConfiguration.ReadConfig().IncludeMachineName.Should().Be(true);
        }

    }
}
