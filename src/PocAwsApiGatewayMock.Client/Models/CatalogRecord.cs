namespace PocAwsApiGatewayMock.Client.Models;

// Data model representing a catalog record as returned by the API.
//
// This type mirrors the wire format and is intentionally lightweight.
// It carries data across the HTTP boundary without enforcing domain
// invariants or behavior.
public class CatalogRecord
{
    public string? Isbn { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? PublicationYear { get; set; }
    public string? Edition { get; set; }
    public string? Format { get; set; }
    public string? Description { get; set; }
}