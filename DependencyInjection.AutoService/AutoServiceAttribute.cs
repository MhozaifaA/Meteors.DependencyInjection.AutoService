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

using Microsoft.Extensions.DependencyInjection;

namespace Meteors
{
    /// <summary>
    /// Custom attribute uses to inject all Servicers .
    /// <para>Warning: better inhernet all services from <see langword="I"/>[Name your repo] </para>
    /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
    /// <list type="number">
    /// <item>[AutoService()]</item>*
    /// <item>[AutoService(<see cref="Lifetime"/>)]</item>*
    /// <item>[AutoService(<see cref="SupportLazy"/>)]</item>*
    /// <item>[AutoService(<see cref="ImplementationType"/>)]</item>*
    /// <item>[AutoService(<see cref="UseImplementation"/>)]</item>*
    /// <item>[AutoService(<see cref="Lifetime"/>,<see cref="ImplementationType"/>)]</item>*
    /// <item>[AutoService(<see cref="Lifetime"/>,<see cref="UseImplementation"/>)]</item>*
    /// <item>[AutoService(<see cref="ImplementationType"/>,<see cref="UseImplementation"/>)]</item> 
    /// <item>[AutoService(<see cref="Lifetime"/>,<see cref="ImplementationType"/>,<see cref="UseImplementation"/>)]</item>*
    /// </list>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class AutoServiceAttribute : Attribute
    {
        /// <summary>
        /// Inner lifetime  .
        /// </summary>
        public ServiceLifetime Lifetime { get; private set; }

        /// <summary>
        /// Inner interface  .
        /// <para>Default value: will take same service(class name) with "I" letter at begin</para>
        /// </summary>
        public Type? ImplementationType { get; private set; }

        /// <summary>
        ///  Use implementation default/(only once) Interface.
        ///  <para></para>
        ///  Works only if <see cref="ImplementationType"/> equal null(false, true, null),
        ///  <list type="bullet">
        ///  <item>  <see langword="true"/>: use to service type with implementation only Interface </item> 
        ///  <item>  <see langword="false"/>: use to service type without implementation </item> 
        /// </list>
        /// </summary>
        public bool UseImplementation { get; private set; }

        /// <summary>
        /// Get the key of the service, if applicable.
        /// </summary>
        public object? ServiceKey { get; private set; }


        /// <summary>
        /// Register extra service for Lazy as Services.Add(provider => new Lazy(() => provider.GetRequiredService()));
        /// </summary>
        public bool SupportLazy { get; private set; }

        /// <summary>
        /// main ctor
        /// <para> default life <see cref="ServiceLifetime.Scoped"/></para>
        /// <para>default <paramref name="useImplementation"/> is take I+ or( first if <paramref name="interfaceType"/> <see langword="null"/>) </para>
        /// </summary>
        /// <param name="lifetime"> </param>
        /// <param name="interfaceType"></param>
        /// <param name="useImplementation"></param>
        /// <param name="serviceKey"></param>
        /// <param name="supportLazy"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped, Type? interfaceType = null, bool useImplementation = true, object? serviceKey = null, bool supportLazy = false) => (Lifetime, ImplementationType, UseImplementation, ServiceKey, SupportLazy) = (lifetime, interfaceType, useImplementation, serviceKey, supportLazy);



        ///// <summary>
        ///// Reflection get <see cref="ServiceLifetime"/> which used with Services .
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns>ServiceLifetime</returns>
        //internal static ServiceLifetime GetLifetime(Type type)
        //    => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
        //                    .Cast<AutoServiceAttribute>().Select(x => x.LifetimeType)
        //                    .Single();


        ///// <summary>
        ///// Reflection get <see cref="Type" langword="interface"/>. which used with Services .
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns> Type? </returns>
        //internal static Type? GetInterface(Type type)
        //    => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
        //                    .Cast<AutoServiceAttribute>().Select(x => x.ImplementationType)
        //                    .Single();


        ///// <summary>
        ///// Reflection get All which used with Services .
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns>ServiceLifetime</returns>
        //internal static (ServiceLifetime, Type?, bool, object?, bool) GetProperties(Type type)
        //    => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
        //                    .Cast<AutoServiceAttribute>().Select(att => (att.LifetimeType, att.ImplementationType, att.UseImplementation, att.ServiceKey, att.SupportLazy))
        //                    .Single();

        /// <inheritdoc/>

        public override string ToString() => $"{nameof(Lifetime)}:{Lifetime}" +
            $", {nameof(ImplementationType)}:{ImplementationType?.ToString() ?? "null"}" +
            $", {nameof(UseImplementation)}:{UseImplementation}" +
            $", {nameof(ServiceKey)}:{ServiceKey?.ToString() ?? "null"}" +
            $", {nameof(SupportLazy)}:{SupportLazy}";


        /// <summary>
        /// Get all properties of <see cref="AutoServiceAttribute"/> in one deconstruct method.
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="implementationType"></param>
        /// <param name="useImplementation"></param>
        /// <param name="serviceKey"></param>
        /// <param name="supportLazy"></param>
        public void Deconstruct(
            out ServiceLifetime lifetime,
            out Type? implementationType,
            out bool useImplementation,
            out object? serviceKey,
            out bool supportLazy)
        {
            lifetime = Lifetime;
            implementationType = ImplementationType;
            useImplementation = UseImplementation;
            serviceKey = ServiceKey;
            supportLazy = SupportLazy;
        }
    }
}
