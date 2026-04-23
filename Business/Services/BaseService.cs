using System.Collections.Generic;
using System.Linq;
using DataAccess;

namespace Business
{
    public class BaseService<TEntity> where TEntity : class, new()
    {
        protected IBaseModel<TEntity> _baseModel;

        public BaseService(IBaseModel<TEntity> baseModel)
        {
            _baseModel = baseModel;
        }

        #region Repository

        /// <summary>
        /// Gets all entities.
        /// </summary>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return _baseModel.GetAll.ToList();
        }

        /// <summary>
        /// Creates an entity and persists it.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Create(TEntity entity)
        {
            return _baseModel.Create(entity);
        }

        /// <summary>
        /// Updates the entity and persists it.
        /// </summary>
        /// <param name="id">Primary key of the entity to update.</param>
        /// <param name="editedEntity">Edited entity.</param>
        /// <param name="changed">Whether the entity was modified.</param>
        /// <returns></returns>
        public virtual TEntity Update(object id, TEntity editedEntity, out bool changed)
        {
            TEntity originalEntity = _baseModel.FindById(id);
            return _baseModel.Update(editedEntity, originalEntity, out changed);
        }

        /// <summary>
        /// Deletes an entity and persists it.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Delete(TEntity entity)
        {
            return _baseModel.Delete(entity);
        }

        #endregion
    }
}
