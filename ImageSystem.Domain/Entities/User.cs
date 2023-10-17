using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ImageSystem.Domain;

public class User : Identity
{
    public string Name { get; set; }
    public string Password { get; set; }
    
    private List<Image> _images = new();

    [BackingField(nameof(_images))]
    public IReadOnlyCollection<Image> Images => _images;

    public ICollection<Friendship> SendFriendships { get; set; }
    
    public ICollection<Friendship> ReceivedFriendships { get; set; }

    public bool SubscriberVerification(Guid userId)
    {
        var requestExsist = ReceivedFriendships
            .Where(sender => sender.SenderId == userId).FirstOrDefault();
        return requestExsist is not null;
    }

    public void AddFriendRequest(Guid reciever)
    {
        if(reciever == Guid.Empty) 
            throw new ValidationException("Идентефиктор пользователя не может быть пустым");

        var requestExsist = SendFriendships.Where(r => r.RecieverId == reciever).FirstOrDefault();
        if (requestExsist is not null) throw new ArgumentException("Вы уже подписали на данного пользователя");

        SendFriendships.Add(Friendship.Create(Id, reciever));
    }

    public void AddImage(string name, string path)
    {
        if (string.IsNullOrEmpty(name))
            throw new ValidationException("Название картинки не может быть пустым");
        if (string.IsNullOrEmpty(path))
            throw new ValidationException("Путь к картинке не может быть пустым");
        
        var image = new Image(name, path, Id);

        _images.Add(image);
    }
}
