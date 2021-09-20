using System.Collections.Generic;
using System.Data;

namespace FatHead.Converters.Interfaces
{
    public interface IDataConverter
    {
        IList<T> ConvertModelFromDataTable<T>(DataTable dataTable) where T : class, new();

        void ConvertModelFromModel<T, U>(T modelToCopy, U modelToCopyTo) 
            where T : class, new() 
            where U : class, new();
    }
}
