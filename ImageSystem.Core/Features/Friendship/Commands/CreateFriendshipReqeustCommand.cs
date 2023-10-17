using ImageSystem.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImageSystem.Core.Features;

public sealed record CreateFriendshipReqeustCommand(Guid Sender, Guid Reciever) : IRequest<Guid>;

public sealed class CreateFriendshipReqeustCommandHandler 
    : BaseFeature<User>, IRequestHandler<CreateFriendshipReqeustCommand, Guid>
{
    public CreateFriendshipReqeustCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork) { }

    public async Task<Guid> Handle(CreateFriendshipReqeustCommand request, CancellationToken cancellationToken)
    {
        var user = await Repository.GetFirstOrDefaultAsync(
            u => u.Id == request.Sender,
            include: i => i.Include(u => u.SendFriendships)
                            .Include(u => u.ReceivedFriendships),
            disableTracking: false);

        if (user is null) 
            throw new NotFoundException($"Пользователь {request.Sender} не найден");

        var reciever = await Repository.GetByIdAsync(request.Reciever);
        if (reciever is null) 
            throw new NotFoundException($"Ошибка при добавлении пользователя в друзья. Пользователь {request.Reciever} не был найден");

        user.AddFriendRequest(request.Reciever);
        Repository.Update(user);

        await UnitOfWork.SaveChangesAsync();
        return request.Reciever;
    }
}
