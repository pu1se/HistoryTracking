using HistoryTracking.DAL;
using Unity;
using Unity.Lifetime;

namespace HistoryTracking.BL
{
    public static class DependencyManager
    {
        private static readonly UnityContainer container = new UnityContainer();
        private static bool wasInitialized = false;

        public static void RegisterComponents()
        {
            if (wasInitialized)
                return;

            container.RegisterType<DataContext>(new HierarchicalLifetimeManager());
            wasInitialized = true;
        }

        public static T Resolve<T>() where T: class
        {
            return container.Resolve<T>();
        }
    }
}
