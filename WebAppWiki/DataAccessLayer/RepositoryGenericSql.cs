using Microsoft.EntityFrameworkCore;
using WebAppWiki.Abstract;
using WebAppWiki.Domains;

namespace WebAppWiki.DataAccessLayer
{
    public class RepositoryGenericSql<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : MyEntity<TKey>
    {
        private readonly DbContextWiki _context;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryGenericSql(DbContextWiki context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TKey id)
        {
            var entity = Read(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
                return;
            }
            throw new InvalidOperationException($"Entity with key '{id}' not found");
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public int GetCount()
        {
            return _dbSet.Count();
        }

        public TEntity Read(TKey id)
        {
            return _dbSet.Find(id); 
        }

        public void Update(TEntity model)
        {
            var key = model.GetId();
            var entity = Read(key);

            _context.Entry(entity).CurrentValues.SetValues(model);
            _context.SaveChanges();
        }

        public DbSet<TEntity> FromDbSet()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> Results(IQueryable<TEntity> query, int r)
        {
            var queryRes = query.Take(r);

            return queryRes;
        }
    }
}
