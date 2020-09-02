using HospitalIsa.DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace HospitalIsa.DAL.Repositories
{
    public class Repository<E> : IRepository<E> where E : class
    {

        private readonly DbSet<E> _dbSet;
        private readonly CenterContext _centerContext;
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
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

        public async Task<bool> Delete(E entity)
        {
            lock (_lock)
            {
                try
                {
                    _dbSet.Remove(entity);
                    _centerContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return false;
                    throw e;
                }

                return true;
            }
           
        }

        public IEnumerable<E> Find(Func<E, bool> predicate)
        {
            lock (_lock)
            {
                return _dbSet.Where(predicate);
            }

        }

        public  IEnumerable<E> GetAll()
        {
            lock (_lock)
            {
                return _dbSet;
            }
            
        }

        public async Task<bool> Update(E entity)
        {
            lock ( _lock)
            {
                try
                {
                    _centerContext.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                    throw e;
                }

            }

        }
    }
}
