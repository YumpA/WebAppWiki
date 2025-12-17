using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppWiki.Domains;

namespace WebAppWiki.Abstract
{
    public interface IRepository<T, TKey> where T : MyEntity<TKey>
    {
        int GetCount();

        void Create(T entity);

        T Read(TKey id);

        void Update(T entity);

        void Delete(TKey id);

        IQueryable<T> GetAll();

        DbSet<T> FromDbSet();

        IQueryable<T> Results(IQueryable<T> query, int results);
    }
}
