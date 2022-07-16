using DependencyInjection.AutoService.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.AutoService
{
    /// <summary>
    /// Custom attribute uses to inject all Servicers .
    /// <para>Warning: should inhernet all services from <see langword="I"/>[Name your repo] </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AutoServiceAttribute : Attribute
    {
        /// <summary>
        /// Inner lifetime  .
        /// </summary>
        public ServiceLifetime LifetimeType { get; private set; }

        /// <summary>
        /// Inner interface  .
        /// <para>Default value: will take same service(class name) with "I" letter at begin</para>
        /// </summary>
        public Type? InterfaceType { get; private set; }

     

        /// <summary>
        /// Defult constructor pass <see cref="ServiceLifetime"/> and <see cref="Type" langword="interface"/>.
        /// </summary>
        /// <param name="lifetime"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime, Type? interfaceType) => (LifetimeType, InterfaceType) = (lifetime, interfaceType);


        /// <summary>
        /// Default constructor pass not parameters inject <see cref="ServiceLifetime.Scoped"/> .
        /// </summary>
        public AutoServiceAttribute() : this(ServiceLifetime.Scoped) { }


        /// <summary>
        /// Defult constructor pass <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="lifetime"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime) : this(lifetime,default) { }


        /// <summary>
        /// Help constructor pass <see cref="string"/> of typo <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="lifetime"></param>
        public AutoServiceAttribute(string lifetime) : this(lifetime.ToEnum<ServiceLifetime>()) { }


        /// <summary>
        /// Default constructor pass <see cref="Type" langword="interface"/>.
        /// </summary>
        /// <param name="interfaceType">inteface to inject with this service</param>
        public AutoServiceAttribute(Type interfaceType) : this(ServiceLifetime.Scoped ,interfaceType) { }


        /// <summary>
        ///  Help constructor pass <see cref="string"/> of typo <see cref="ServiceLifetime"/> and pass  <see cref="Type" langword="interface"/>.
        /// </summary>
        /// <param name="lifetime"></param>
        public AutoServiceAttribute(string lifetime, Type interfaceType) : this(lifetime.ToEnum<ServiceLifetime>(), interfaceType) { }



        /// <summary>
        /// Reflection get <see cref="ServiceLifetime"/> which used with Services .
        /// </summary>
        /// <param name="type"></param>
        /// <returns>ServiceLifetime</returns>
        internal static ServiceLifetime GetLifetime(Type type)
            => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
                            .Cast<AutoServiceAttribute>().Select(x => x.LifetimeType)
                            .Single();


        /// <summary>
        /// Reflection get <see cref="Type" langword="interface"/>. which used with Services .
        /// </summary>
        /// <param name="type"></param>
        /// <returns> Type? </returns>
        internal static Type? GetInterface(Type type)
            => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
                            .Cast<AutoServiceAttribute>().Select(x => x.InterfaceType)
                            .Single();


        /// <summary>
        /// Reflection get <see cref="(ServiceLifetime,Type?)"/> which used with Services .
        /// </summary>
        /// <param name="type"></param>
        /// <returns>ServiceLifetime</returns>
        internal static (ServiceLifetime,Type?) GetProperties(Type type)
            => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
                            .Cast<AutoServiceAttribute>().Select(x => (x.LifetimeType,x.InterfaceType))
                            .Single();

    }
}
