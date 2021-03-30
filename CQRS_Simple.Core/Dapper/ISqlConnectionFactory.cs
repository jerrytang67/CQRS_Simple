using System.Data;

namespace CQRS_Simple.Core.Dapper
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}