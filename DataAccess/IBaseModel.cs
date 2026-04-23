using System.Linq;

namespace DataAccess
{
    public interface IBaseModel<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll { get; }
        TEntity FindById(object id);
        TEntity Create(TEntity entity);
        TEntity Update(TEntity editedEntity, TEntity originalEntity, out bool changed);
        TEntity Delete(TEntity entity);
        void SaveChanges();
    }
}
