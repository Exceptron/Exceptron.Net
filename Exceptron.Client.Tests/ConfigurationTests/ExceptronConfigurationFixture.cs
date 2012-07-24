using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }


        [Test]
        public void api_key_should_be_null_if_key_is_not_provided()
        {
            ExceptronConfiguration.ReadConfig("exceptron_noapikey").Should().NotBeNull();
            ExceptronConfiguration.ReadConfig("exceptron_noapikey").ApiKey.Should().BeNull();
        }

    }
}
