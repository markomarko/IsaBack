using HospitalIsa.DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.DAL.Entites
{
    public class Repository<E> : IRepository<E> where E : class
    {

        private readonly DbSet<E> _dbSet;
        private readonly CenterContext _centerContext;

        public Repository(CenterContext centerContext)
        {
            _centerContext = centerContext;
            _dbSet = centerContext.Set<E>();
            
        }
       
        public async Task<bool> Create(E entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _centerContext.SaveChangesAsync();
            } catch (Exception e)
            {
                return false;
                throw e;
            }

            return true;
        }

        public void Delete(E entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<E> Find(Func<E, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(E entity)
        {
            throw new NotImplementedException();
        }
    }
}
