using System.Collections.Generic;

namespace Business
{
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        IEnumerable<TEntity> GetAll();
        TEntity Create(TEntity entity);
        TEntity Update(object id, TEntity editedEntity, out bool changed);
        TEntity Delete(TEntity entity);
        void SaveChanges();
    }
}
