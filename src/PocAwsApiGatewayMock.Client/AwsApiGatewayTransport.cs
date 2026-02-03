namespace PocAwsApiGatewayMock.Client;

public class AwsApiGatewayTransport
{
    private readonly HttpClient _http;
    private readonly Uri _baseUri;

    public AwsApiGatewayTransport(HttpClient http, Uri baseUri)
    {
        _http = http;
        _baseUri = baseUri;
    }

    public CatalogRecord GetCatalogRecordByIsbn(string isbn)
    {
        // placeholder for now; next step would be async HTTP GET and deserialize
        return new CatalogRecord { Isbn = isbn };
    }
}
