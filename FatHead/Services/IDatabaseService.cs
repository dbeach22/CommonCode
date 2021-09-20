using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FatHead.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<T> Get<T>(T model) where T : class, new();

        Task<IList<T>> GetList<T>();

        Task<int> Put<T>(T Model) where T : class, new();

        Task<int> Post<T>(T Model) where T : class, new();

        Task<int> Delete<T>(T Model) where T : class, new();
    }
}
