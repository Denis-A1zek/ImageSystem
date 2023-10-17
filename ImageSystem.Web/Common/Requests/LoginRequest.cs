namespace ImageSystem.Web.Common;

public record LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
