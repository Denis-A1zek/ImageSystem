namespace ImageSystem.Domain;

public class Image : Identity
{
    public Image(string name, string path, Guid userId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Path = path;
        UserId = userId;
    }

    public string Name { get; set; }
    public string Path { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
