using Xunit;
using PocAwsApiGatewayMock.Client;

namespace PocAwsApiGatewayMock.Client.Tests;

public class HelloClientTests
{
    [Fact]
    public void GetGreeting_ReturnsHelloWorld()
    {
        var client = new HelloClient();

        var result = client.GetGreeting();

        Assert.Equal("Hello, world!", result);
    }
}
