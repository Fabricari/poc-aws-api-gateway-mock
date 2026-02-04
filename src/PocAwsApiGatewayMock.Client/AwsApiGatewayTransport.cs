using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PocAwsApiGatewayMock.Client;

public class AwsApiGatewayTransport
{
    private readonly HttpClient _http;
    private readonly Uri _baseUri;
    private readonly JsonSerializerOptions _jsonOptions;


    public AwsApiGatewayTransport(HttpClient http, Uri baseUri, JsonSerializerOptions? jsonOptions = null)
    {
        _http = http;
        _baseUri = baseUri;
        _jsonOptions = jsonOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }


    // Transport-level API (still “about AWS gateway resources”, not JSON/URI)
    public CatalogRecord GetCatalogRecordByIsbn(string isbn)
        => Get200Json<CatalogRecord>($"/catalog/{EscapePath(isbn)}");

    public List<CatalogRecord> GetCatalogRecordsByExactTitle(string title)
        => Get200Json<List<CatalogRecord>>($"/catalog?title={EscapeQuery(title)}&match=exact");

    // Generic primitive (reusable across endpoints)
    private T Get200Json<T>(string relativePath)
    {
        var requestUri = new Uri(_baseUri, EnsureLeadingSlash(relativePath));

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

    private static string EnsureLeadingSlash(string path)
        => path.StartsWith("/", StringComparison.Ordinal) ? path : "/" + path;

    private static string EscapePath(string segment)
        => Uri.EscapeDataString(segment);

    private static string EscapeQuery(string value)
        => Uri.EscapeDataString(value);
}
