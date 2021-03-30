using Autofac;
using CQRS_Simple.Core;

namespace CQRS_Simple.Products.API.Modules
{
    public class IocManagerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var scope = c.Resolve<ILifetimeScope>();
                    return new IocManager(scope);
                })
                .As<IIocManager>()
                ;
        }
    }
}