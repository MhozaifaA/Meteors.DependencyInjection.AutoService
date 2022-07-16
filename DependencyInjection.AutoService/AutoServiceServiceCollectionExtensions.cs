using System.Reflection;
using DependencyInjection.AutoService;
using DependencyInjection.AutoService.Helper;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///  Extension methods for add AutoService Service
    /// </summary>
    public static class AutoServiceServiceCollectionExtensions
    {

        /// <summary>
        /// Inject all service wich used <see cref="AutoServiceAttribute"/> with specific <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyString">Loads an assembly given the long form of its name.</param>
        /// <exception cref="AmbiguousMatchException"></exception>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAutoService(this IServiceCollection services, params string[] assemblyString)
         => _AddAutoServiceInject(services, assemblyString);

        /// <summary>
        /// Inject all service wich used <see cref="AutoServiceAttribute"/> with specific <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="services"></param>
        /// <param name="anyClassInAssembly">Pass any time inside assemply</param>
        /// <exception cref="AmbiguousMatchException"></exception>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAutoService(this IServiceCollection services, params Type[] anyClassInAssembly)
             => _AddAutoServiceInject(services, anyClassInAssembly);


        /// <summary>
        /// Inject all service wich used <see cref="AutoServiceAttribute"/> with specific <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Assembly">Represents an assembly</param>
        /// <exception cref="AmbiguousMatchException"></exception>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAutoService(this IServiceCollection services, params Assembly[] Assembly)
            => _AddAutoServiceInject(services, Assembly);

        /// <summary>
        /// Inject all service wich used <see cref="AutoServiceAttribute"/> with specific <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoService(this IServiceCollection services)
            => _AddAutoServiceInject(services, DllDependencies.AbleNameSpaceToType().ToArray());


        /// <summary>
        /// Get all namespaces in same app luncher folder
        /// </summary>
        private static string[] DllDependencies => Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).
           Where(x => Path.GetExtension(x) == ".dll").Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();


        /// <summary>
        /// Generic array of <see langword="namespace"/> as <see cref="string"/> ,<see cref="Type"/> and <see cref="Assembly"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="any"></param>
        /// <returns></returns>
        private static IServiceCollection _AddAutoServiceInject<T>(IServiceCollection services, T[] any)
        {
            var Types = any.AbleNameSpaceToType();
            foreach (Type type in Types)
                InjectServices(services, type);
            return services;
        }



        /// <summary>
        /// Inject all <see cref="AutoServiceAttribute"/> users by <see cref="ServiceLifetime"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        private static void InjectServices(IServiceCollection services, Type type)
        {
            if (Attribute.IsDefined(type, typeof(AutoServiceAttribute)))
            {
                (ServiceLifetime lifetime,Type? interfaceType) = AutoServiceAttribute.GetProperties(type);

                if (interfaceType is null)
                {
                    interfaceType = type.GetInterface("I" + type.Name);
                    if (interfaceType is null)
                        throw new AmbiguousMatchException($"There is not match interface named {"I" + type.Name} \n please check {type.Name} .");
                }

                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(interfaceType, type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(interfaceType, type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(interfaceType, type);
                        break;
                }
            }

        }
    }
}
