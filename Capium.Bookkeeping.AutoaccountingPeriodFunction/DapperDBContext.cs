using Capium.Bookkeeping.AutoaccountingPeriodFunction.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction
{
    public class DapperDBContext
    {
        private readonly string _connectionString;

        public DapperDBContext(MyConfiguration configuration)
        {
            _connectionString = configuration.Settings.AzureKeyVaultValues.BKConnectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
