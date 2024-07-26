using FF.Cockpit.Entity.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity
{
    public interface IGenericRepository<T> : IDisposable where T : new()
    {
        Task<IEnumerable<T>> ExcuteProcedureWithMultiResult(string procedureName, object parameters);
        Task<T> ExcuteProcedureWithSingleResult(string procedureName, object parameters);


        IEnumerable<T> ExcuteProcedureWithMultiResult_sync(string procedureName, object parameters);
        T ExcuteProcedureWithSingleResult_sync(string procedureName, object parameters,bool isCommandTimeout);
    }
}
