using Xunit;
using PocAwsApiGatewayMock.Client;
using PocAwsApiGatewayMock.Client.Models;

namespace PocAwsApiGatewayMock.Client.Tests;

// Demo-oriented contract tests for an API Gateway-backed Mock API.
// These tests intentionally hit a real API Gateway endpoint (not in-memory fakes)
// to prove: routing, request shaping (path/query/headers), and response mapping.
public class LibraryCatalogTests : IDisposable
{
    // Placeholder URI for the demo. Replace with a real API Gateway invoke URL
    // to run the live contract tests. Be mindful of usage and costs.
    private readonly Uri _baseUri = new("https://example.invalid");
    
    private readonly HttpClient _http;
    private readonly AwsApiGatewayTransport _transport;
    private readonly LibraryCatalog _catalog;

    public LibraryCatalogTests()
    {
        // Use a real HttpClient to exercise the full HTTP stack.
        // Handlers, headers, and timeouts can be added later as needed.
        _http = new HttpClient(); 

        // These tests require a live API Gateway endpoint.
        // Fail fast if the placeholder URL is still in use.
        if (_baseUri.Host.EndsWith(".invalid", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Set the API Gateway base URL in LibraryCatalogTests to run live contract tests.");

        _transport = new AwsApiGatewayTransport(_http, _baseUri);
        _catalog = new LibraryCatalog(_transport);
    }

    public void Dispose()
    {
        _http.Dispose();
    }

    // Proves: basic GET by identifier using a path parameter.
    // Exercises API Gateway resource routing, URL encoding, and 200 JSON mapping.
    [Fact]
    public void GetCatalogRecordByIsbn_ReturnsRecord_WithIsbn()
    {
        var record = _catalog.GetCatalogRecordByIsbn("9780765326355");

        Assert.NotNull(record);
        Assert.NotNull(record.Isbn);
        Assert.Equal("9780765326355", record.Isbn);
        Assert.Equal("The Way of Kings", record.Title);
        Assert.Equal("Brandon Sanderson", record.Author);
        Assert.Equal(2010, record.PublicationYear);
    }

    // Proves: query-string based lookup with exact-match semantics.
    // Exercises query parameter mapping and list response deserialization.
    [Fact]
    public void GetCatalogRecordsByExactTitle_ReturnsSingleRecord_WithTitle()
    {
        var records = _catalog.GetCatalogRecordsByExactTitle("The Way of Kings");
    
        Assert.NotNull(records);
        var record = Assert.Single(records);
        Assert.Equal("The Way of Kings", record.Title);

        // Representative fields to prove full-object mapping for list results.
        Assert.Equal("9780765326355", record.Isbn);
        Assert.Equal("Brandon Sanderson", record.Author);
    }
    
    // Proves: pagination mechanics using pageSize/nextToken and stable sorting across pages.
    [Fact]
    public void ListBooksByAuthor_Paginates_Accumulates_AllPages_AndTerminates()
    {
        // Arrange: author="Brandon Sanderson", pageSize small to force multiple pages
        var author = "Brandon Sanderson";
        var pageSize = 5;

        // Act: client.ListBooksByAuthor(author, pageSize: 5)
        var records = _catalog.ListBooksByAuthor(author, pageSize);

        // Assert:
        //   - returns all items across pages (accumulated)
        //   - ordering is stable (e.g., by Title then Isbn)
        //   - stops when nextToken is null/empty
        Assert.NotNull(records);
        Assert.True(records.Count > pageSize); // proves we crossed a page boundary

        // Minimal stable-order proof without baking in every expected title.
        var expectedOrder = records
            .OrderBy(r => r.Title, StringComparer.Ordinal)
            .ThenBy(r => r.Isbn, StringComparer.Ordinal)
            .ToList();

        Assert.Equal(
            expectedOrder.Select(r => (r.Title, r.Isbn)),
            records.Select(r => (r.Title, r.Isbn)));
    }

    // Proves: POST with a JSON body can be mocked in API Gateway without compute.
    // Exercises body mapping, 201 Created semantics, and JSON response mapping.
    [Fact]
    public void PlaceHold_ReturnsCreatedHold_WithHoldId()
    {
        // Arrange
        var isbn = "9780765326355";
        var patronId = "P-12345"; //This is an active Patron Id

        // Act
        var holdReply = _catalog.PlaceHold(isbn, patronId);

        // Assert (representative fields; not deep equals)
        Assert.NotNull(holdReply);
        Assert.False(string.IsNullOrWhiteSpace(holdReply.HoldId));
        Assert.Equal(isbn, holdReply.Isbn);
        Assert.Equal(patronId, holdReply.PatronId);
        Assert.Equal(HoldStatus.Placed, holdReply.Status);
    }

    // Proves: VTL conditional logic can model domain failure paths without a backend.
    // Exercises non-2xx behavior (409) and error response mapping.
    [Fact]
    public void PlaceHold_WhenPatronCardExpired_ReturnsRejectedHoldReply()
    {
        // Arrange: patronId value is used by the mock template to branch to a 409 response
        var isbn = "9780765326355";
        var patronId = "P-EXPIRED"; //This is an expired Patron Id

        // Act
        var holdReply = _catalog.PlaceHold(isbn, patronId);

        // Assert (representative fields; not deep equals)
        Assert.NotNull(holdReply);
        Assert.Null(holdReply.HoldId);
        Assert.Equal(isbn, holdReply.Isbn);
        Assert.Equal(patronId, holdReply.PatronId);
        Assert.Equal(HoldStatus.Rejected, holdReply.Status);
        Assert.Equal(HoldReasonCode.PatronCardExpired, holdReply.ReasonCode);
    }

}
