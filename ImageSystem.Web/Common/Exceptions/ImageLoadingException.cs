namespace ImageSystem.Web.Common.Exceptions;

public class ImageLoadingException : Exception
{
    public ImageLoadingException(string? message) : base(message) { }
}
