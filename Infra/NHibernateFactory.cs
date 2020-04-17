﻿using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace MiniProfilerNhibernate5.Infra
{
    public class NHibernateFactory
    {
        private string ConnectionString { get; }

        public NHibernateFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(ConnectionString)
                        .Driver<MiniProfilerSql2008ClientDriver>()
#if DEBUG
                        .ShowSql()
#endif
                )

                .Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<Program>()
                    .Conventions.Setup(c => { c.Add(DefaultLazy.Never()); }))

                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty(Environment.BatchSize, "100");
                    cfg.SetProperty(Environment.Hbm2ddlKeyWords,
                        "none"); // https://groups.google.com/forum/#!topic/nhusers/F8IxCgYN038
                    // new SchemaExport(cfg);
                }).BuildSessionFactory();
        }
    }
}