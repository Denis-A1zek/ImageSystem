using ImageSystem.Core.Common.Utils;
using ImageSystem.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImageSystem.Core.Features;

public sealed record GetUserImagesQuery(Guid PersonalId, Guid FriendId, int Count) : IRequest<IEnumerable<ImageInfo>>;

public sealed class GetFriendImagesQueryHandler 
    : BaseFeature<Image>, IRequestHandler<GetUserImagesQuery, IEnumerable<ImageInfo>>
{
    private readonly IConfiguration _configuration;

    public GetFriendImagesQueryHandler(
        IConfiguration configuration,
        IUnitOfWork unitOfWork) : base(unitOfWork) 
        => _configuration = configuration;

    public async Task<IEnumerable<ImageInfo>> Handle(GetUserImagesQuery request, CancellationToken cancellationToken)
    {
        var userRepository = UnitOfWork.GetRepository<User>();
        var currentUser = await userRepository.GetFirstOrDefaultAsync(
            u => u.Id == request.PersonalId,
            include: i => i.Include(r => r.ReceivedFriendships));
        if (currentUser is null) throw new NotFoundException($"Пользователь {request.PersonalId} не найден");

        if(request.FriendId != request.PersonalId)
        {
            var itsMySubscriber = currentUser.SubscriberVerification(request.FriendId);

            if (!itsMySubscriber)
                throw new AccessException("У вас нет прав просматривать фотографии данного пользователя");
        }

        var image = Repository.GetQueryable().Where(i => i.UserId == request.FriendId).Take(request.Count);

        var remoteUrl = _configuration[ImageConstant.RemotePath];
        var userImages = image.Select(user => new ImageInfo(request.FriendId, user.Name, $"{remoteUrl}/{user.Path}"));

        return userImages;
    }
}
