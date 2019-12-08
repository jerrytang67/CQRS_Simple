﻿using System;
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
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                var find = await db.QueryFirstOrDefaultAsync<T>(
                    "SELECT * FROM " + _tableName + " WHERE Id=@Id", new { Id = id });
                return find;
            }
        }

        public async Task AddAsync(T item)
        {
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                //                var parameters = (object)Mapping(item);
                item.Id = await db.InsertAsync<TC>(_tableName, item);
            }
        }

        public async Task RemoveAsync(T item)
        {
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                await db.ExecuteAsync("DELETE FROM " + _tableName + " WHERE Id=@Id", new { Id = item.Id });
            }
        }

        public async Task UpdateAsync(T item)
        {
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                await db.UpdateAsync(_tableName, item);
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> items = new List<T>();
            var result = DynamicQuery.GetDynamicQuery(_tableName, predicate);
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                items = await db.QueryAsync<T>(result.Sql, (object)result.Param);
            }
            return items;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> items = new List<T>();
            using (var db = _sqlConnectionFactory.GetOpenConnection())
            {
                items = await db.QueryAsync<T>("SELECT * FROM " + _tableName);
            }
            return items;
        }

        //        internal virtual dynamic Mapping(T item)
        //        {
        //            return item;
        //        }
    }
}