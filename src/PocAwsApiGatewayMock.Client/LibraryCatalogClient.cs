namespace PocAwsApiGatewayMock.Client;

public class LibraryCatalogClient
{
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
