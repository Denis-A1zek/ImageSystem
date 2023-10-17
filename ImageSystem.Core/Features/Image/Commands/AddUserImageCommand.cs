using ImageSystem.Domain;
using MediatR;

namespace ImageSystem.Core.Features;

public sealed record AddUserImageCommand(Guid UserId, string FileName, string FilePath) : IRequest<string>;

public sealed class AddUserImageCommandHandler 
    : BaseFeature<User>, IRequestHandler<AddUserImageCommand, string>
{
    public AddUserImageCommandHandler
        (IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<string> Handle(AddUserImageCommand request, CancellationToken cancellationToken)
    {
        var userDb = await Repository.FindAsync(u => u.Id == request.UserId, i => i.Images);
        var user = userDb.Any() ? userDb.FirstOrDefault() : null;

        if(user == null) 
            throw new NotFoundException($"Пользователь {request.UserId} не найден");

        user.AddImage(request.FileName,request.FilePath);

        Repository.Update(user);
        await UnitOfWork.SaveChangesAsync();

        return request.FileName;
    }
}
