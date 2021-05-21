using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HistoryTracking.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Unity;
using Unity.Lifetime;

namespace HistoryTracking.BL
{
    public static class DependencyManager
    {
        private static readonly UnityContainer services = new UnityContainer();
        private static bool wasInitialized = false;

        public static void RegisterComponents()
        {
            if (wasInitialized)
                return;

            services.RegisterType<DataContext>(new TransientLifetimeManager());
            AddTransientServices();
            wasInitialized = true;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects, // to prevent json Circular Object Reference issues
                FloatFormatHandling = FloatFormatHandling.String,
                FloatParseHandling = FloatParseHandling.Decimal                
            };
        }

        public static T Resolve<T>() where T: class
        {
            return services.Resolve<T>();
        }

        private static IEnumerable<Type> _allTypes;
        private static IEnumerable<Type> AllTypes => _allTypes ?? (_allTypes = Assembly.GetExecutingAssembly().GetTypes());

        private static IEnumerable<Type> _serviceTypes;
        private static void AddTransientServices()
        {
            if (_serviceTypes == null)
            {
                _serviceTypes = AllTypes.Where(type => type.BaseType == typeof(BaseService));
            }
            
            foreach (var type in _serviceTypes)
            {
                services.RegisterType(type, new TransientLifetimeManager());
            }
        }
    }
}
