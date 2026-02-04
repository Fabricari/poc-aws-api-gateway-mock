namespace PocAwsApiGatewayMock.Client;

public class LibraryCatalog
{
    private readonly AwsApiGatewayTransport _transport;

    public LibraryCatalog(AwsApiGatewayTransport transport)
    {
        _transport = transport;
    }

    public CatalogRecord GetCatalogRecordByIsbn(string isbn)
        => _transport.GetCatalogRecordByIsbn(isbn);

    public List<CatalogRecord> GetCatalogRecordsByExactTitle(string title)
        => _transport.GetCatalogRecordsByExactTitle(title);
}