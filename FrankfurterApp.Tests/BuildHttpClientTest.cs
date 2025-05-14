using FluentAssertions;
using FrankfurterApp.Tests.Configuration;
using NUnit.Framework;

namespace FrankfurterApp.Tests
{
    [Order(1)]
    public class BuildHttpClientTest
    {
        [Test, Order(1)]
        public void Should_Successfully_Build_Http_Client()
        {
            var client = HttpClientHelper.GenerateHttpClient();

            client.Should().NotBeNull();
        }
    }
}