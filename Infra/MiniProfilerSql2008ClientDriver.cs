﻿using System.Data.Common;
using NHibernate.Driver;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace MiniProfilerNhibernate5.Infra
{
    public class MiniProfilerSql2008ClientDriver: Sql2008ClientDriver
    {
        public override DbCommand CreateCommand()
        {
            return MiniProfiler.Current != null 
                ? new ProfiledDbCommand(base.CreateCommand(), null, MiniProfiler.Current) 
                : base.CreateCommand();
        }

        public override DbConnection CreateConnection()
        {
            return MiniProfiler.Current != null 
                ? new ProfiledDbConnection(base.CreateConnection(), MiniProfiler.Current) 
                : base.CreateConnection();
        }
    }
}