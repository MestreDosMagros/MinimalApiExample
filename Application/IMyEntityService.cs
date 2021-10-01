using Domain;

namespace Application
{
    public interface IMyEntityService
    {
        Task<IEnumerable<MyEntity>> GetAll();
        Task<IEnumerable<MyEntity>> Delete(Guid id);
        Task<IEnumerable<MyEntity>> Add(MyEntity entity);
        Task<IEnumerable<MyEntity>> Update(MyEntity entity);
    }
}
