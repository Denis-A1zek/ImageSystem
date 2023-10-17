namespace ImageSystem.Domain;

public class Friendship 
{
    public Guid SenderId { get; set; }    
    public User? Sender { get; set; }
    public Guid RecieverId { get; set; }
    public User? Reciever { get; set; }
    public DateTime CreatedAt { get; set; }

    public static Friendship Create
        (Guid sender, Guid receiver)
    {
        return new Friendship
        {
            SenderId = sender,
            RecieverId = receiver,
            CreatedAt = DateTime.UtcNow
        };
    }

}
