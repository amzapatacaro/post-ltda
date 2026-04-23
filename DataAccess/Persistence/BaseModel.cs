using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess
{
    public class BaseModel<TEntity> : IBaseModel<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Database context.
        /// </summary>
        JujuTestContext _context;

        /// <summary>
        /// Entity set for this model.
        /// </summary>
        protected DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the model.
        /// </summary>
        /// <param name="context"></param>
        public BaseModel(JujuTestContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Gets all entities as a queryable set.
        /// </summary>
        public virtual IQueryable<TEntity> GetAll
        {
            get { return _dbSet; }
        }

        /// <summary>
        /// Gets an entity by primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity FindById(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Creates an entity and persists it.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Updates the entity and persists it.
        /// </summary>
        /// <param name="editedEntity">Edited entity.</param>
        /// <param name="originalEntity">Original entity before changes.</param>
        /// <param name="changed">Whether the entity was modified.</param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity editedEntity, TEntity originalEntity, out bool changed)
        {
            _context.Entry(originalEntity).CurrentValues.SetValues(editedEntity);

            changed = _context.Entry(originalEntity).State == EntityState.Modified;

            _context.SaveChanges();

            return originalEntity;
        }

        /// <summary>
        /// Deletes an entity and persists it.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Delete(TEntity entity)
        {
            _dbSet.Remove(entity);

            _context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Persists pending changes.
        /// </summary>
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
