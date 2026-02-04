using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PocAwsApiGatewayMock.Client;

// Transport responsible for talking directly to AWS API Gateway.
//
// This layer owns HTTP mechanics, URI construction, and JSON
// serialization concerns. It intentionally exposes methods that
// reflect API Gateway resources and query shapes, not domain concepts.
//
// Higher-level clients (e.g., LibraryCatalog) depend on this class
// so that AWS- and HTTP-specific details remain contained here.
public class AwsApiGatewayTransport
{
    private readonly HttpClient _http;
    private readonly Uri _baseUri;
    
    //Serialization policy is part of the transport contract. If it needs to vary, we’ll introduce a seam then.
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public AwsApiGatewayTransport(HttpClient http, Uri baseUri)
    {
        _http = http;
        _baseUri = baseUri;
    }

    // Transport-level operations that map directly to API Gateway resources.
    public CatalogRecord GetCatalogRecordByIsbn(string isbn)
        => Get200Json<CatalogRecord>($"/dev/catalog/{EscapePath(isbn)}");

    public List<CatalogRecord> GetCatalogRecordsByExactTitle(string title)
        => Get200Json<List<CatalogRecord>>($"/dev/catalog?title={EscapeQuery(title)}&match=exact");

    public CatalogRecordPage ListBooksByAuthorPage(string author, int limit, string? nextToken)
    {
        var query = $"author={EscapeQuery(author)}&limit={limit}";
        if (!string.IsNullOrWhiteSpace(nextToken))
            query += $"&nextToken={EscapeQuery(nextToken)}";

        return Get200Json<CatalogRecordPage>($"/dev/catalog/page?{query}");
    }

    // Low-level primitive:
    // - issues a GET
    // - requires 200 OK
    // - deserializes JSON
    //
    // This keeps error handling and serialization rules consistent
    // across all transport methods.
    private T Get200Json<T>(string relativePath)
    {
        var requestUri = new Uri(_baseUri, relativePath);

        using var response = _http.GetAsync(requestUri).GetAwaiter().GetResult();

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new HttpRequestException($"GET {requestUri} failed: {(int)response.StatusCode} {response.ReasonPhrase}. Body: {body}");
        }

        var result = response.Content.ReadFromJsonAsync<T>(_jsonOptions).GetAwaiter().GetResult();
        if (result is null)
            throw new InvalidOperationException($"GET {requestUri} returned empty JSON body.");

        return result;
    }

    // Small helpers to keep URI construction explicit and safe.
    private static string EscapePath(string segment)
        => Uri.EscapeDataString(segment);

    private static string EscapeQuery(string value)
        => Uri.EscapeDataString(value);
}
