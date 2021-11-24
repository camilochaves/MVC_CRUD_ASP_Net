using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Wrappers.Response;

namespace Application.Services.Interfaces
{
  public interface IService<T,U,V> where T:class where U:class where V:class
  {
    Task<CustomServiceResultWrapper<V>> AddAsync (T entity);
    Task<CustomServiceResultWrapper<IEnumerable<V>>> GetAllAsync();
    Task<CustomServiceResultWrapper<V>> GetWithIdAsync(int id);
    Task<CustomServiceResultWrapper<V>> RemoveWithIdAsync (int id);
    Task<CustomServiceResultWrapper<V>> UpdateAsync(int id, U _new);
  }
}