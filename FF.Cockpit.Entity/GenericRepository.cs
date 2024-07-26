using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity
{
    public class GenericRepository<T> : IGenericRepository<T> where T : new()
    {
        public static SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder();
        private readonly IDbConnection con;

        public GenericRepository()
        {
            try
            {
                con = new SqlConnection(DatabaseHelper.ConnectionStringBuilder.ConnectionString);
                con.Open();
            }
            finally
            {
                con.Close();
            }
        }

        public async Task<IEnumerable<T>> ExcuteProcedureWithMultiResult(string procedureName, object parameters)
        {
            return await con.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> ExcuteProcedureWithSingleResult(string procedureName, object parameters)
        {
            return await con.QueryFirstOrDefaultAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }


        public IEnumerable<T> ExcuteProcedureWithMultiResult_sync(string procedureName, object parameters)
        {
            return con.Query<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public T ExcuteProcedureWithSingleResult_sync(string procedureName, object parameters, bool iscommandTimeout = false)
        {
            if (iscommandTimeout)
                return con.QueryFirstOrDefault<T>(procedureName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            else
                return con.QueryFirstOrDefault<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);

        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    con.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
