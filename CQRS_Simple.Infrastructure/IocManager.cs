using Autofac;

namespace CQRS_Simple.Infrastructure
{

    public interface IIocManager
    {
        ILifetimeScope AutofacContainer { get; set; }
        TService GetInstance<TService>();

    }

    public class IocManager : IIocManager
    {
        public IocManager(ILifetimeScope container)
        {
            AutofacContainer = container;
        }
        /// <summary>
        /// Autofac容器
        /// </summary>
        public ILifetimeScope AutofacContainer { get; set; }

        public TService GetInstance<TService>()
        {
            return AutofacContainer.Resolve<TService>();
        }
    }
}