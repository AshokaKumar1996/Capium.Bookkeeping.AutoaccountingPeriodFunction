using Capium.Bookkeeping.AutoaccountingPeriodFunction.Interface;
using Capium.Bookkeeping.AutoaccountingPeriodFunction.Model.AccountingPeriod;
using Microsoft.Data.SqlClient;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Repositories
{
    public class AccountingPeriodRepository : IAccountingPeriodRepository
    {
        private readonly DapperDBContext _dapperDBContext;

        public AccountingPeriodRepository(DapperDBContext dapperDBContext)
        {
            _dapperDBContext = dapperDBContext;
        }

        public async Task<AccountingPeriodModel?> GetLatestByCompanyIdAsync(Guid companyId)
        {
            const string query = @"SELECT TOP (1)
                [Id],
                [FromDate],
                [ToDate],
                [CompanyId],
                [IsLocked],
                [IsCeased],
                [IsComparative],
                [IsDeleted]
                FROM [dbo].[AccountingPeriod]
                WHERE [CompanyId] = @CompanyId
                AND [IsDeleted] = 0
                ORDER BY [ToDate] DESC";

            using var connection = _dapperDBContext.CreateConnection();
            if (connection is not SqlConnection sqlConnection)
            {
                throw new InvalidOperationException("Database connection is not a SQL connection.");
            }

            await sqlConnection.OpenAsync();
            using var command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@CompanyId", companyId);

            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new AccountingPeriodModel
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                FromDate = reader.GetDateTime(reader.GetOrdinal("FromDate")),
                ToDate = reader.GetDateTime(reader.GetOrdinal("ToDate")),
                CompanyId = reader.GetGuid(reader.GetOrdinal("CompanyId")),
                IsLocked = reader.GetBoolean(reader.GetOrdinal("IsLocked")),
                IsCeased = reader.GetBoolean(reader.GetOrdinal("IsCeased")),
                IsComparative = reader.GetBoolean(reader.GetOrdinal("IsComparative")),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
            };
        }

        public async Task<Guid> CreateAsync(AccountingPeriodModel model)
        {
            const string query = @"INSERT INTO [dbo].[AccountingPeriod]
            ([FromDate], [ToDate], [CompanyId], [IsLocked], [IsCeased], [IsComparative], [IsDeleted], [CreatedBy])
            OUTPUT inserted.[Id]
            VALUES
            (@FromDate, @ToDate, @CompanyId, @IsLocked, @IsCeased, @IsComparative, @IsDeleted, @CreatedBy)";

            using var connection = _dapperDBContext.CreateConnection();
            if (connection is not SqlConnection sqlConnection)
            {
                throw new InvalidOperationException("Database connection is not a SQL connection.");
            }

            await sqlConnection.OpenAsync();
            using var command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@FromDate", model.FromDate.Date);
            command.Parameters.AddWithValue("@ToDate", model.ToDate.Date);
            command.Parameters.AddWithValue("@CompanyId", model.CompanyId);
            command.Parameters.AddWithValue("@IsLocked", model.IsLocked);
            command.Parameters.AddWithValue("@IsCeased", model.IsCeased);
            command.Parameters.AddWithValue("@IsComparative", model.IsComparative);
            command.Parameters.AddWithValue("@IsDeleted", model.IsDeleted);
            command.Parameters.AddWithValue("@CreatedBy", (object?)model.CreatedBy ?? DBNull.Value);

            var insertedId = await command.ExecuteScalarAsync();
            return insertedId is Guid id ? id : Guid.Empty;
        }
    }
}
