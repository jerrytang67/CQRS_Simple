using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Simple.Core.Extensions
{
    public static class EfCoreExtensions
    {
        public static async Task<IList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int skip, int pagesize, CancellationToken token)
        {
            return await query.Skip(skip).Take(pagesize).ToListAsync(token);
        }
    }
}