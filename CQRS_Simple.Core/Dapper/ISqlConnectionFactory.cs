using System.Data;

namespace CQRS_Simple.Core
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}