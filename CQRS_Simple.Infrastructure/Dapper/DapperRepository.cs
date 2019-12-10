using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;

namespace CQRS_Simple.Infrastructure.Dapper
{
    public class DapperRepository<T, TC> : IDapperRepository<T, TC> where T : Entity<TC>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly string _tableName;

        public DapperRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            var attr = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
            _tableName = (attr as TableAttribute)?.Name;
        }

        public async Task<T> GetByIdAsync(TC id)
        {
            using var db = _sqlConnectionFactory.GetOpenConnection();
            return await db.QueryFirstOrDefaultAsync<T>(
                $"SELECT * FROM {_tableName} WHERE Id=@Id",
                new { Id = id });
        }

        public async Task<TC> AddAsync(T item)
        {
            using var db = _sqlConnectionFactory.GetOpenConnection();
            return await db.InsertAsync<TC>(_tableName, item);
        }

        public async Task<int> RemoveAsync(T item)
        {
            using var db = _sqlConnectionFactory.GetOpenConnection();
            return await db.ExecuteAsync(
                $"DELETE FROM {_tableName} WHERE Id=@Id",
                new { Id = item.Id });
        }

        public async Task<int> UpdateAsync(T item)
        {
            using var db = _sqlConnectionFactory.GetOpenConnection();
            return await db.UpdateAsync(_tableName, item);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> items;
            var result = DynamicQuery.GetDynamicQuery(_tableName, predicate);
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                items = await db.QueryAsync<T>(result.Sql, (object)result.Param);
            }

            return items;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> items;
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                items = await db.QueryAsync<T>(
                    $"SELECT * FROM {_tableName}"
                );
            }

            return items;
        }
    }
}