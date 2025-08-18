using iRechargeTestProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Domain.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(object id);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> Filter = null,
                                               Func<IQueryable<T>, IOrderedQueryable<T>> Order = null,
                                               string IncludeProperties = "");
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        bool Update(T Entity);
        void AddRange(IEnumerable<T> entities);
        void DeleteByObject(T entity);
        void DeleteByObjectId(object entity);
        void RemoveRange(IEnumerable<T> entities);
        Task SaveAsync();
    }
}
