public enum ErrorType
{
    /// <summary>
    /// Input failed validation rules — malformed data, missing required fields,
    /// values out of range, business invariants violated by the caller's input.
    /// Maps to HTTP 400 Bad Request.
    /// Examples: "Email is not a valid format", "Amount must be positive",
    /// "Order must contain at least one line item".
    /// </summary>
    Validation,

    /// <summary>
    /// The requested resource or entity does not exist.
    /// Maps to HTTP 404 Not Found.
    /// Examples: "Customer with ID 123 not found", "No order matches that reference".
    /// Use this when absence is a legitimate outcome the caller should handle —
    /// not when something that *should* exist is mysteriously missing (that's a bug).
    /// </summary>
    NotFound,

    /// <summary>
    /// The operation conflicts with the current state of the system.
    /// The request is well-formed but can't be applied right now.
    /// Maps to HTTP 409 Conflict.
    /// Examples: "Email already registered", "Order already shipped and cannot be cancelled",
    /// "Concurrency conflict — entity was modified by another user",
    /// "Insufficient stock to fulfill order".
    /// </summary>
    Conflict,

    /// <summary>
    /// The caller is not permitted to perform this operation.
    /// Covers both authentication failures (no/invalid identity) and authorization
    /// failures (valid identity but insufficient permissions). If you need to
    /// distinguish, split into Unauthenticated (401) and Forbidden (403).
    /// Maps to HTTP 401 or 403.
    /// Examples: "Must be logged in", "Only the order owner can cancel this order",
    /// "Admin role required".
    /// </summary>
    Unauthorized,

    /// <summary>
    /// A failure that wasn't anticipated by the domain — typically used as a
    /// fallback when wrapping infrastructure exceptions into a Result at a boundary.
    /// Maps to HTTP 500 Internal Server Error.
    /// Examples: "Database connection failed", "Third-party API returned malformed response".
    /// Prefer letting these remain as exceptions when possible; only use this variant
    /// when you have a specific reason to convert an exception into a Result.
    /// </summary>
    Unexpected
}