using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.Client;
using Microsoft.Data.SqlClient;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Repositories
{
    public class ClientDetailsRepository : IClientDetailsRepository
    {
        private readonly DapperDBContext _dapperDBContext;

        public ClientDetailsRepository(DapperDBContext dapperDBContext)
        {
            _dapperDBContext = dapperDBContext;
        }

        public async Task<List<ClientDetailModel>> GetAutoAccountingPeriodClientsAsync()
        {
            const string query = @"SELECT [ClientId]
              ,[UpdatedBy]
               FROM [dbo].[ClientDetails]
               WHERE [IsAutoAccountingPeriod] = 1";

            using var connection = _dapperDBContext.CreateConnection();
            if (connection is not SqlConnection sqlConnection)
            {
                throw new InvalidOperationException("Database connection is not a SQL connection.");
            }

            var result = new List<ClientDetailModel>();
            await sqlConnection.OpenAsync();
            using var command = new SqlCommand(query, sqlConnection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new ClientDetailModel
                {
                    ClientId = reader.IsDBNull(reader.GetOrdinal("ClientId")) ? null : reader.GetGuid(reader.GetOrdinal("ClientId")),
                    UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetGuid(reader.GetOrdinal("UpdatedBy"))
                });
            }

            return result;
        }
    }
}
