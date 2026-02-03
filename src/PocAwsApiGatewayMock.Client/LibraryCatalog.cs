namespace PocAwsApiGatewayMock.Client;

public class LibraryCatalog
{
    private readonly AwsApiGatewayTransport _transport;

    public LibraryCatalog(AwsApiGatewayTransport transport)
    {
        _transport = transport;
    }

    public CatalogRecord GetCatalogRecordByIsbn(string isbn)
    {
        return new CatalogRecord
        {
            Isbn = isbn
        };
    }

    public List<CatalogRecord> GetCatalogRecordsByExactTitle(string title)
    {
        return new List<CatalogRecord>
        {
            new CatalogRecord
            {
                Title = title
            }
        };
    }

    
}
