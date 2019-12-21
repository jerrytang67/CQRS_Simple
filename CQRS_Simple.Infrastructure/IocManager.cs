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
        static IocManager()
        {
            Instance = new IocManager();
        }
        public static IocManager Instance { get; private set; }

        /// <summary>
        /// Autofac容器
        /// </summary>
        public ILifetimeScope AutofacContainer { get; set; }

        public TService GetInstance<TService>()
        {
            return Instance.AutofacContainer.Resolve<TService>();
        }
    }
}