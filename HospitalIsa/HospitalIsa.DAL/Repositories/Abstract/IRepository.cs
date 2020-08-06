using HospitalIsa.DAL.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.DAL.Repositories.Abstract
{
    public interface IRepository<E> where E : class
        
    {
        IEnumerable<E> GetAll();
        Task<bool> Create(E entity);
        void Update(E entity);
        void Delete(E entity);
        IEnumerable<E> Find(Func<E, bool> predicate);
    }
}
