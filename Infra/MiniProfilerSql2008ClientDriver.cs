using System.Data.Common;
using NHibernate.Driver;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace MiniProfilerNhibernate5.Infra
{
    public class MiniProfilerSql2008ClientDriver: Sql2008ClientDriver
    {
        public override DbCommand CreateCommand()
        {
            return (DbCommand) new ProfiledDbCommand(
                base.CreateCommand(), 
                null,
                MiniProfiler.Current);
        }

        public override DbConnection CreateConnection()
        {
            return (DbConnection) new ProfiledDbConnection(
                base.CreateConnection(), 
                MiniProfiler.Current);
        }
    }
}