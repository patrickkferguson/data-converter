using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DataConverter.Extensions
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddTransientAllImplementations<T>(
            this IServiceCollection services) where T : class
        {
            foreach (var type in GetTypesWithInterface<T>())
            {
                services.AddTransient(typeof(T), type);
            }

            return services;
        }

        private static IEnumerable<Type> GetTypesWithInterface<T>() where T : class
        {
            var type = typeof(T);
            return type.Assembly.GetLoadableTypes()
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface);
        }

        private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
