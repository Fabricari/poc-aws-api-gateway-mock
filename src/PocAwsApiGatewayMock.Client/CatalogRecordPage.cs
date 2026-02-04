using PocAwsApiGatewayMock.Client;

// Transport-level page envelope returned by list endpoints.
//
// This type models the API Gateway response shape for paginated
// resources (items + nextToken). It is not a domain concept; it
// exists solely to carry paging state across the HTTP boundary.
public class CatalogRecordPage
{
    public List<CatalogRecord>? Items { get; set; }
    public string? NextToken { get; set; }
}