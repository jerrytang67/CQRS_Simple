using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CQRS_Simple.Infrastructure.Dapper
{
    public static class DapperExtensions
    {
        public static async Task<T> InsertAsync<T>(this IDbConnection db, string tableName, object param)
        {
            IEnumerable<T> result = await db.QueryAsync<T>(DynamicQuery.GetInsertQuery(tableName, param), param);
            return result.First();
        }

        public static async Task<int> UpdateAsync(
            this IDbConnection db,
            string tableName, object param)
        {
            return await db.ExecuteAsync(DynamicQuery.GetUpdateQuery(tableName, param), param);
        }
    }
}