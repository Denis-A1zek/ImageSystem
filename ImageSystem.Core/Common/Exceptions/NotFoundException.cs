namespace ImageSystem.Core;

public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message) { }

    public NotFoundException(string type, object key)
        : base($"Entity \"{type}\" ({key}) not found.") { }
}
