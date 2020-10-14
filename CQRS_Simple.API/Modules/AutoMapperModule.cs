using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;

namespace CQRS_Simple.API.Modules
{
    public class AutoMapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
                {
                    //cfg.ConstructServicesUsing(ServiceConstructor);

                    foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                    {
                        cfg.AddProfile(profile);
                    }
                }))
                .AsSelf()
                .AutoActivate()
                .SingleInstance();

            builder.Register(c =>
                {
                    // these are the changed lines
                    var scope = c.Resolve<ILifetimeScope>();
                    return new Mapper(c.Resolve<MapperConfiguration>(), scope.Resolve);
                })
                .As<IMapper>()
                .SingleInstance();
        }
    }
}