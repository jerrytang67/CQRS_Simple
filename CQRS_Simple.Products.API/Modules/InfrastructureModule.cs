using Autofac;
using Autofac.Extras.DynamicProxy;
using CQRS_Simple.Core;
using CQRS_Simple.Core.Dapper;
using CQRS_Simple.Core.MQ;
using CQRS_Simple.Core.Uow;
using CQRS_Simple.Products.API.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Simple.Products.API.Modules
{
    public class InfrastructureModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;
        //        private readonly ILoggerFactory _loggerFactory;

        public InfrastructureModule(string databaseConnectionString
        )
        {
            this._databaseConnectionString = databaseConnectionString;
            //            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RabbitMQClient>().SingleInstance();

            builder.Register(c => new SqlConnectionFactory(_databaseConnectionString))
                .As<ISqlConnectionFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DapperRepository<,>)).As(typeof(IDapperRepository<,>))
                .InstancePerLifetimeScope();

            var dbBuild = new DbContextOptionsBuilder<SimpleDbContext>();
            dbBuild.UseSqlServer(_databaseConnectionString);


            builder.Register(c => new SimpleDbContext(dbBuild.Options))
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope()
                .OnRelease(instance => instance.ReleaseCleanUp())
                ;

            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>))
                .InstancePerLifetimeScope()
                .InterceptedBy(typeof(CallLogger))
                .EnableInterfaceInterceptors();
            ;
        }
    }
}