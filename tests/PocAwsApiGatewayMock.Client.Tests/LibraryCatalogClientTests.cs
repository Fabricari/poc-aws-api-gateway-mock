using Xunit;
using PocAwsApiGatewayMock.Client;

namespace PocAwsApiGatewayMock.Client.Tests;

public class LibraryCatalogClientTests
{
    [Fact]
    public void GetCatalogRecordByIsbn_ReturnsRecord_WithIsbn()
    {
        var client = new LibraryCatalogClient();

        var record = client.GetCatalogRecordByIsbn("9780765326355");

        Assert.NotNull(record);
        Assert.NotNull(record.Isbn);
        Assert.Equal("9780765326355", record.Isbn);
    }

    [Fact]
    public void GetCatalogRecordsByExactTitle_ReturnsSingleRecord_WithTitle()
    {
        var client = new LibraryCatalogClient();

        var records = client.GetCatalogRecordsByExactTitle("The Way of Kings");

        Assert.NotNull(records);
        var record = Assert.Single(records);
        Assert.Equal("The Way of Kings", record.Title);
    }
}
