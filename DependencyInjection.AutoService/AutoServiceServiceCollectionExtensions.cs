/*
MIT License

Copyright (c) 2022 Huzaifa Aseel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */


using System.Reflection;
using Meteors;
using Meteors.Helper;

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
            => _AddAutoServiceInject(services, ExtensionMethods.DllDependencies.AbleNameSpaceToType().ToArray());



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
        /// <list type="bullet">
        /// <item>[AutoService()]</item>
        /// <item>[AutoService(supportLazy)]</item>
        /// <item>[AutoService(lifetime)]</item>
        /// <item>[AutoService(implementationType)]</item>
        /// <item>[AutoService(useImplementation)]</item>
        /// <item>[AutoService(lifetime,implementationType)]</item>
        /// <item>[AutoService(lifetime,useImplementation)]</item>
        /// <item>[AutoService(implementationType,useImplementation)]</item>
        /// <item>[AutoService(lifetime,implementationType,useImplementation)]</item>
        /// </list>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        private static void InjectServices(IServiceCollection services, Type type)
        {
            if (!Attribute.IsDefined(type, typeof(AutoServiceAttribute)))
                return;

            var attrs = type.GetCustomAttributes<AutoServiceAttribute>();

            foreach (var attr in attrs)
            {
                (ServiceLifetime lifetime, Type? implementationType, bool useImplementation, object? serviceKey, bool supportLazy) = attr;

                implementationType ??= GetImplementationType(type, useImplementation);
                if (implementationType is null && useImplementation == true)
                    throw new AmbiguousMatchException($"There is not a matching interface named 'I{type.Name}'. Please check '{type.Name}' or ensure there is a default interface.");

                var serviceType = useImplementation ? implementationType : type;
                var descriptor = new ServiceDescriptor(serviceType, serviceKey, type, lifetime);
                services.Add(descriptor);

                if (supportLazy)
                {
                    var descripte = CreateLazyServiceDescriptor(serviceType, lifetime);
                    services.Add(descripte);
                }

            }

        }





        private static Type? GetImplementationType(Type type, bool useImplementation)
        {
            if (!useImplementation)
                return null;

            var interfaceName = "I" + type.Name;
            var implementationType = type.GetInterface(interfaceName);

            if (implementationType == null)
            {
                var allInterfaces = type.GetInterfaces();
                var directInterfaces = allInterfaces.Except(allInterfaces.SelectMany(i => i.GetInterfaces()));

                if (directInterfaces.Any())
                    implementationType = directInterfaces.First();
            }

            return implementationType;
        }

        public static ServiceDescriptor CreateLazyServiceDescriptor(Type serviceType, ServiceLifetime lifetime)
        {

            var lazyServiceType = typeof(Lazy<>).MakeGenericType(serviceType);

            var lazyServiceImplementationType = typeof(LazyService<>).MakeGenericType(serviceType);

            var descripte = new ServiceDescriptor(lazyServiceType, lazyServiceImplementationType, lifetime);
            return descripte;
        }

        internal class LazyService<T> : Lazy<T> where T : class
        {
            public LazyService(IServiceProvider serviceProvider)
                : base(() =>
                 serviceProvider.GetRequiredService<T>()
                )
            { }

            //public LazyService(IServiceScopeFactory scopeFactory)
            //  : base(() =>
            //  {
            //      var scope = scopeFactory.CreateScope();
            //      return scope.ServiceProvider.GetRequiredService<T>();
            //  }) { }
        }

    }
}
