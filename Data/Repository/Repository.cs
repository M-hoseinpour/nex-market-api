using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using market.Models.Common;
using market.Extensions;

namespace market.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly ApplicationDbContext DbContext;
        public DbSet<TEntity> Entities { get; }

        public virtual IQueryable<TEntity> Table
            => typeof(TEntity).IsSubclassOf(typeof(EntityBase))
                ? Entities.Cast<EntityBase>().Where(q => !q.IsDeleted).Cast<TEntity>()
                : Entities;

        public virtual IQueryable<TEntity> TableNoTracking => typeof(TEntity).IsSubclassOf(typeof(EntityBase))
            ? Entities.AsNoTracking().Cast<EntityBase>().Where(q => !q.IsDeleted).Cast<TEntity>()
            : Entities.AsNoTracking();

        public Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>();
        }

        #region Async Methods

        public virtual async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            var entity = await Entities.FindAsync(ids, cancellationToken);
            if (entity is EntityBase e)
                return e.IsDeleted ? null : entity;
            return entity;
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await Table.ToListAsync(cancellationToken);

            return entities;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            entity.NotNull(nameof(entity));

            if (entity is EntityBase e)
                e.CreateMoment = DateTime.UtcNow;

            await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);

            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            entities.NotNull(nameof(entities));

            foreach (var entity in entities)
                if (entity is EntityBase e)
                    e.CreateMoment = DateTime.UtcNow;

            await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            entity.NotNull(nameof(entity));

            if (entity is EntityBase e)
                e.UpdateMoment = DateTime.UtcNow;

            Entities.Update(entity);

            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            entities.NotNull(nameof(entities));

            foreach (var entity in entities)
                if (entity is EntityBase e)
                    e.UpdateMoment = DateTime.UtcNow;

            Entities.UpdateRange(entities);

            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            entity.NotNull(nameof(entity));

            if (entity is EntityBase e)
            {
                e.DeleteMoment = DateTime.UtcNow;
                e.IsDeleted = true;
            }

            return UpdateAsync(entity, cancellationToken, saveNow);
        }

        public virtual async Task DropAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            entity.NotNull(nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            entities.NotNull(nameof(entities));

            foreach (var entity in entities)
            {
                if (entity is EntityBase e)
                {
                    e.DeleteMoment = DateTime.UtcNow;
                    e.IsDeleted = true;
                }
            }

            return UpdateRangeAsync(entities, cancellationToken, saveNow);
        }

        public virtual async Task DropRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken,
            bool saveNow = true)
        {
            entities.NotNull(nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Sync Methods

        public virtual TEntity GetById(params object[] ids)
        {
            var entity = Entities.Find(ids);
            if (entity is EntityBase e)
                return e.IsDeleted ? null : entity;
            return entity;
        }

        public virtual TEntity Add(TEntity entity, bool saveNow = true)
        {
            entity.NotNull(nameof(entity));

            if (entity is EntityBase e)
                e.CreateMoment = DateTime.UtcNow;

            Entities.Add(entity);

            if (saveNow)
                DbContext.SaveChanges();

            return entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            entities.NotNull(nameof(entities));

            foreach (var entity in entities)
                if (entity is EntityBase e)
                    e.CreateMoment = DateTime.UtcNow;

            Entities.AddRange(entities);

            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity, bool saveNow = true)
        {
            entity.NotNull(nameof(entity));

            if (entity is EntityBase e)
                e.UpdateMoment = DateTime.UtcNow;

            Entities.Update(entity);

            DbContext.SaveChanges();
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            entities.NotNull(nameof(entities));

            foreach (var entity in entities)
                if (entity is EntityBase e)
                    e.UpdateMoment = DateTime.UtcNow;

            Entities.UpdateRange(entities);

            if (saveNow)
                DbContext.SaveChanges();
        }

        public void Delete(TEntity entity, bool saveNow = true)
        {
            entity.NotNull(nameof(entity));

            if (entity is EntityBase e)
            {
                e.DeleteMoment = DateTime.UtcNow;
                e.IsDeleted = true;
            }

            Update(entity, saveNow);
        }

        public virtual void Drop(TEntity entity, bool saveNow = true)
        {
            entity.NotNull(nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            entities.NotNull(nameof(entities));

            foreach (var entity in entities)
            {
                if (entity is EntityBase e)
                {
                    e.DeleteMoment = DateTime.UtcNow;
                    e.IsDeleted = true;
                }
            }

            UpdateRange(entities, saveNow);
        }

        public virtual void DropRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            entities.NotNull(nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                DbContext.SaveChanges();
        }

        #endregion

        #region Attach & Detach

        public virtual void Detach(TEntity entity)
        {
            entity.NotNull(nameof(entity));
            var entry = DbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Detached;
        }

        public virtual void Attach(TEntity entity)
        {
            entity.NotNull(nameof(entity));
            if (DbContext.Entry(entity).State == EntityState.Detached)
                Entities.Attach(entity);
        }

        #endregion

        #region Explicit Loading

        public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);

            var collection = DbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                await collection.LoadAsync(cancellationToken).ConfigureAwait(false);

            if (collection.CurrentValue != null)
            {
                var collectionType = collection.CurrentValue.GetType();
                if (collectionType.IsGenericType &&
                    collectionType.GetGenericArguments().First().IsSubclassOf(typeof(EntityBase)))
                {
                    var castedCollection = collection.CurrentValue.Cast<EntityBase>();
                    castedCollection = castedCollection.Where(collectionEntity => !collectionEntity.IsDeleted);
                    collection.CurrentValue = castedCollection.Cast<TProperty>().ToList();
                }
            }
        }

        public virtual void LoadCollection<TProperty>(TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
            where TProperty : class
        {
            Attach(entity);
            var collection = DbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                collection.Load();

            if (collection.CurrentValue != null)
            {
                var collectionType = collection.CurrentValue.GetType();
                if (collectionType.IsGenericType &&
                    collectionType.GetGenericArguments().First().IsSubclassOf(typeof(EntityBase)))
                {
                    collection.CurrentValue =
                        collection.CurrentValue.Cast<EntityBase>()
                            .Where(collectionEntity => !collectionEntity.IsDeleted)
                            .Cast<TProperty>();
                }
            }
        }

        public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity,
            Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                await reference.LoadAsync(cancellationToken).ConfigureAwait(false);

            if (reference.CurrentValue is EntityBase { IsDeleted: true })
                reference.CurrentValue = null;
        }

        public virtual void LoadReference<TProperty>(TEntity entity,
            Expression<Func<TEntity, TProperty>> referenceProperty)
            where TProperty : class
        {
            Attach(entity);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                reference.Load();
            if (reference.CurrentValue is EntityBase { IsDeleted: true })
                reference.CurrentValue = null;
        }

        #endregion
    }
}