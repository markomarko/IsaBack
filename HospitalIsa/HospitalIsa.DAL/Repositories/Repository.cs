using HospitalIsa.DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace HospitalIsa.DAL.Repositories
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
            }
            catch (Exception e)
            {
                return false;
                throw e;
            }

            return true;
        }

        public void Delete(E entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<E> Find(Func<E, bool> predicate)
        {
            return _dbSet.Where(predicate);
        }

       

        public  IEnumerable<E> GetAll()
        {
            return _dbSet;
        }

        public void Update(E entity)
        {
            throw new NotImplementedException();
        }
    }
}
