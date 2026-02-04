namespace PocAwsApiGatewayMock.Client;

// Domain-facing client for the library catalog.
//
// This intentionally sits above the transport layer to keep HTTP and
// API Gateway details out of calling code. For now it is a thin
// pass-through; domain rules, validation, caching, or error mapping
// could be added here later without changing consumers.
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