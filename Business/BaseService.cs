using System.Collections.Generic;
using System.Linq;
using DataAccess;

namespace Business
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        protected IBaseModel<TEntity> _baseModel;

        public BaseService(IBaseModel<TEntity> baseModel)
        {
            _baseModel = baseModel;
        }

        #region Repository


        /// <summary>
        /// Consulta todas las entidades
        /// </summary>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return _baseModel.GetAll.ToList();
        }

        /// <summary>
        /// Crea un entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Create(TEntity entity)
        {
            return _baseModel.Create(entity);
        }

        /// <summary>
        /// Actualiza la entidad (GUARDA)
        /// </summary>
        /// <param name="editedEntity">Entidad editada</param>
        /// <param name="originalEntity">Entidad Original sin cambios</param>
        /// <param name="changed">Indica si se modifico la entidad</param>
        /// <returns></returns>
        public virtual TEntity Update(object id, TEntity editedEntity, out bool changed)
        {
            TEntity originalEntity = _baseModel.FindById(id);
            return _baseModel.Update(editedEntity, originalEntity, out changed);
        }

        /// <summary>
        /// Elimina una entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Delete(TEntity entity)
        {
            return _baseModel.Delete(entity);
        }

        /// <summary>
        /// Guardar cambios
        /// </summary>
        public virtual void SaveChanges()
        {
            _baseModel.SaveChanges();
        }
        #endregion
    }
}
