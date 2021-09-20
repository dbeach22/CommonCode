using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FatHead.DAL.Interfaces
{
    public interface IDatabaseAccess
    {
        int Execute();

        Task<int> ExecuteAsync();

        DataTable QueryDataTable();

        Task<DataTable> QueryDataTableAsync();

        IList<T> QueryList<T>() where T : class, new();

        Task<IList<T>> QueryListAsync<T>() where T : class, new();
    }
}
