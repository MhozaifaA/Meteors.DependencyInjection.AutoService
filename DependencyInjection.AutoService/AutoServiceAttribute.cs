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
        public ServiceLifetime LifetimeTypes { get; private set; }

        /// <summary>
        /// Defult constructor pass not parameters inject <see cref="ServiceLifetime.Scoped"/> .
        /// </summary>
        public AutoServiceAttribute() => LifetimeTypes = ServiceLifetime.Scoped;

        /// <summary>
        /// Defult constructor pass <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="lifetime"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime) => LifetimeTypes = lifetime;

        /// <summary>
        /// Help constructor pass <see cref="string"/> of typo <see cref="ServiceLifetime"/> .
        /// </summary>
        /// <param name="lifetime"></param>
        public AutoServiceAttribute(string lifetime) : this(lifetime.ToEnum<ServiceLifetime>()) { }

        /// <summary>
        /// Reflection get <see cref="ServiceLifetime"/> which used with Services .
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static ServiceLifetime GetLifetime(Type type)
            => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
                            .Cast<AutoServiceAttribute>().Select(x => x.LifetimeTypes)
                            .Single();
    }
}
