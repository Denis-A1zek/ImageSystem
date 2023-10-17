namespace ImageSystem.Web.Common.Models;

public record ErrorResponse
{
    public ErrorResponse() { }
    public ErrorResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        ErrorReason = message;
    }

    public int StatusCode { get; set; }
    public string ErrorReason { get; set; }

    public static ErrorResponse Create(int statusCode, string errorReason)
        => new(statusCode, errorReason);
}
