using Autofac;
using CQRS_Simple.Infrastructure;

namespace CQRS_Simple.Modules
{
    public class IocManagerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // builder.RegisterType<IocManager>()
            //     .As<IIocManager>()
            //     .SingleInstance()
            //     ;
        }
    }
}