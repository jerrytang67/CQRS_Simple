using Autofac;
using CQRS_Simple.EntityFrameworkCore;
using CQRS_Simple.Infrastructure;
using CQRS_Simple.Infrastructure.Dapper;
using CQRS_Simple.Infrastructure.MQ;
using CQRS_Simple.Infrastructure.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CQRS_Simple.Modules
{
    public class InfrastructureModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;
        //        private readonly ILoggerFactory _loggerFactory;

        public InfrastructureModule(string databaseConnectionString
            //            , ILoggerFactory loggerFactory
            )
        {
            this._databaseConnectionString = databaseConnectionString;
            //            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RabbitMQClient>()
                .SingleInstance();

            builder.Register(c => new SqlConnectionFactory(_databaseConnectionString))
                .As<ISqlConnectionFactory>()
                // .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DapperRepository<,>)).As(typeof(IDapperRepository<,>))
                .InstancePerLifetimeScope();

            var dbBuild = new DbContextOptionsBuilder<SimpleDbContext>();
            dbBuild.UseSqlServer(_databaseConnectionString);

            //            dbBuild.UseLoggerFactory(_loggerFactory);

            builder.Register(c => new SimpleDbContext(dbBuild.Options))
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope()
                .OnRelease(instance => instance.CleanUp())
                ;

            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();
        }
    }
}