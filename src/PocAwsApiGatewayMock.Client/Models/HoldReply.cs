namespace PocAwsApiGatewayMock.Client.Models;

// Domain-facing response model for a hold placement attempt.
//
// This type represents the outcome of a PlaceHold operation as seen by
// calling code. It is intentionally protocol-agnostic: HTTP status codes
// and transport details are handled internally and mapped into domain
// concepts (status and reason).
public class HoldReply
{
    public string? HoldId { get; set; }
    public string? Isbn { get; set; }
    public string? PatronId { get; set; }

    public HoldStatus Status { get; set; }
    public HoldReasonCode? ReasonCode { get; set; }
}

// High-level outcome of a hold placement attempt.
public enum HoldStatus
{
    Placed,
    Rejected
}

// Domain reason for a rejected hold request.
// This is intentionally extensible as additional business rules are modeled.
public enum HoldReasonCode
{
    PatronCardExpired
}
