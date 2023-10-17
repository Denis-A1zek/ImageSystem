using ImageSystem.Domain;

namespace ImageSystem.Core.Features;

public class BaseFeature<T> where T : Identity
{
    public BaseFeature(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    protected IUnitOfWork UnitOfWork { get; init; }
    protected IRepository<T> Repository => UnitOfWork.GetRepository<T>();

    protected async Task<bool> IsEntityExsistById(Guid id)
    {
        var entity = await Repository.GetByIdAsync(id);
        return entity is not null;
    }
}
