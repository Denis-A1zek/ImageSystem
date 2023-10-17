namespace ImageSystem.Web.Common;

public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException(string? message)
        : base($"Incorrect password. {message}") { }
}
