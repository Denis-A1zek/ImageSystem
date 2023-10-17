using ImageSystem.Core.Features.ViewModels;
using ImageSystem.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImageSystem.Core.Features;

public sealed record ViewFriendRequestsQuery(Guid UserId) : IRequest<FriendRequest[]>;

public sealed class ViewFriendRequestsQueryHandler
    : BaseFeature<User>, IRequestHandler<ViewFriendRequestsQuery, FriendRequest[]>
{
    public ViewFriendRequestsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<FriendRequest[]> Handle(ViewFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        var user = await Repository
            .GetFirstOrDefaultAsync(
                u => u.Id == request.UserId, 
                include: i => i.Include(r => r.ReceivedFriendships)
                                .ThenInclude(f => f.Sender)
                                .Include(s => s.SendFriendships)
                                .ThenInclude(s => s.Reciever));

        if (user is null) throw new NotFoundException("Пользователь не найден");

        var friendRequests = user.ReceivedFriendships
                .Select(r => new FriendRequest(r.SenderId, r.Sender.Name));

        return friendRequests.ToArray();
    }
}
