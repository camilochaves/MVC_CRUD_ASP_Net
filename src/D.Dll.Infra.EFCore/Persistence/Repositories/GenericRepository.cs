using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infra.EFCore.Persistence.Repository
{
  public class GenericRepository<T> : IGenericRepository<T> where T : class
  {
    protected readonly ApplicationContext _context;
    public GenericRepository(ApplicationContext context)
    {
      _context = context;
    }

    public async Task AddAsync(T entity)
    {
      await _context.Set<T>().AddAsync(entity);
    }
    public async Task<T> FindAsync(int id)
    {
      return await _context.Set<T>().FindAsync(id);
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
      return await _context.Set<T>().ToListAsync<T>();
    }
    public IQueryable<T> Find(Expression<Func<T, bool>> expression)
    {
      return _context.Set<T>().Where(expression);
    }

    public Task AddRangeAsync(IEnumerable<T> entities)
    {
      return _context.Set<T>().AddRangeAsync(entities);
    }
    public void Remove(T entity)
    {
      _context.Set<T>().Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
      _context.Set<T>().RemoveRange(entities);
    }

    public void Update(T entity)
    {
      var entry = _context.Set<T>().Update(entity);
      entry.State = EntityState.Modified;
    }

  }

}