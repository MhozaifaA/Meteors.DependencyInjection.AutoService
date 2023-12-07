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

using Meteors.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace Meteors
{
    /// <summary>
    /// Custom attribute uses to inject all Servicers .
    /// <para>Warning: better inhernet all services from <see langword="I"/>[Name your repo] </para>
    /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
    /// <list type="number">
    /// <item>[AutoService()]</item>*
    /// <item>[AutoService(<see cref="LifetimeType"/>)]</item>*
    /// <item>[AutoService(<see cref="ImplementationType"/>)]</item>*
    /// <item>[AutoService(<see cref="UseImplementation"/>)]</item>*
    /// <item>[AutoService(<see cref="LifetimeType"/>,<see cref="ImplementationType"/>)]</item>*
    /// <item>[AutoService(<see cref="LifetimeType"/>,<see cref="UseImplementation"/>)]</item>*
    /// <item>[AutoService(<see cref="ImplementationType"/>,<see cref="UseImplementation"/>)]</item> 
    /// <item>[AutoService(<see cref="LifetimeType"/>,<see cref="ImplementationType"/>,<see cref="UseImplementation"/>)]</item>*
    /// </list>
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
        public Type? ImplementationType { get; private set; }

        /// <summary>
        ///  Use implementation default/(only once) Interface.
        ///  <para></para>
        ///  Works only if <see cref="ImplementationType"/> equal null(false, true, null),
        ///  <list type="bullet">
        ///  <item>  <see langword="true"/>: use to service type with implementation only Interface </item> 
        ///  <item>  <see langword="false"/>: use to service type without implementation </item> 
        ///  <item>  <see langword="null"/>: use to service type with implementation only Interface but if ImplementationType stil null will be false work with condition as true </item> 
        /// </list>
        /// </summary>
        public bool? UseImplementation { get; private set; }

        /// <summary>
        /// Get the key of the service, if applicable.
        /// </summary>
        public object? ServiceKey { get; private set; }


        /// <summary>
        /// Defult constructor pass <see cref="ServiceLifetime"/> and <see cref="ImplementationType"/> and <see cref="UseImplementation"/>.
        /// Custom attribute uses to inject all Servicers .
        /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        /// <para>when <see cref="UseImplementation" langword="false"/>  will igonre implement and inject service </para>
        /// </summary>
        /// <param name="useImplementation"></param>
        /// <param name="interfaceType"></param>
        /// <param name="lifetime"></param>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime, Type interfaceType, bool useImplementation, object? serviceKey = null) => (LifetimeType, ImplementationType, UseImplementation,ServiceKey) = (lifetime, interfaceType, useImplementation, serviceKey);


        /// <summary>
        /// Defult constructor pass <see cref="ServiceLifetime"/> and <see cref="Type" langword="interface"/>.
        /// Custom attribute uses to inject all Servicers .
        /// <para>Warning: better inhernet all services from <see langword="I"/>[Name your repo] </para>
        /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="interfaceType"></param>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime, Type interfaceType, object? serviceKey = null) => (LifetimeType, ImplementationType, ServiceKey) = (lifetime, interfaceType, serviceKey);



        /// <summary>
        /// Defult constructor pass <see cref="ServiceLifetime"/> and <see cref="UseImplementation"/>.
        /// Custom attribute uses to inject all Servicers .
        /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        /// <para>when <see cref="UseImplementation" langword="false"/>  will igonre implement and inject service </para>
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="useImplementation"></param>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime, bool useImplementation, object? serviceKey = null) => (LifetimeType, UseImplementation, ServiceKey) = (lifetime, useImplementation, serviceKey);



        /// <summary>
        /// <para>lifetime <see cref="ServiceLifetime.Scoped"/></para>
        /// Defult constructor pass <see cref="ImplementationType"/> and <see cref="UseImplementation"/>.
        /// Custom attribute uses to inject all Servicers .
        /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        /// <para>when <see cref="UseImplementation" langword="false"/>  will igonre implement and inject service </para>
        /// </summary>
        /// <param name="useImplementation"></param>
        /// <param name="interfaceType"></param>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(Type interfaceType, bool useImplementation, object? serviceKey = null) : this(ServiceLifetime.Scoped, interfaceType, useImplementation, serviceKey) { }



        /// <summary>
        /// Default constructor pass not parameters inject <see cref="ServiceLifetime.Scoped"/> .
        /// Custom attribute uses to inject all Servicers .
        /// <para>Warning: better inhernet all services from <see langword="I"/>[Name your repo] </para>
        /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        /// </summary>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(object? serviceKey = null) : this(ServiceLifetime.Scoped, serviceKey) { }


        /// <summary>
        /// Defult constructor pass <see cref="ServiceLifetime"/> .
        /// Custom attribute uses to inject all Servicers .
        /// <para>Warning: better inhernet all services from <see langword="I"/>[Name your repo] </para>
        /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(ServiceLifetime lifetime, object? serviceKey = null) => (LifetimeType, ServiceKey) = (lifetime, serviceKey);





        /// <summary>
        /// Default constructor pass <see cref="Type" langword="interface"/>.
        /// </summary>
        /// <param name="interfaceType">inteface to inject with this service</param>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(Type interfaceType, object? serviceKey = null) : this(ServiceLifetime.Scoped, interfaceType, serviceKey) { }


        /// <summary>
        /// <para>lifetime <see cref="ServiceLifetime.Scoped"/></para>
        /// Defult constructor pass <see cref="UseImplementation"/>.
        /// Custom attribute uses to inject all Servicers .
        /// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        /// <para>when <see cref="UseImplementation" langword="false"/>  will igonre implement and inject service </para>
        /// </summary>
        /// <param name="useImplementation"></param>
        /// <param name="serviceKey"></param>
        public AutoServiceAttribute(bool useImplementation, object? serviceKey = null) : this(ServiceLifetime.Scoped, useImplementation, serviceKey) { }




        ///// <summary>
        ///// Help constructor pass <see cref="string"/> of typo <see cref="ServiceLifetime"/> .
        ///// Custom attribute uses to inject all Servicers .
        ///// <para>Warning: better inhernet all services from <see langword="I"/>[Name your repo] </para>
        ///// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        ///// </summary>
        ///// <param name="lifetime"></param>
        ///// <param name="serviceKey"></param>
        //public AutoServiceAttribute(string lifetime, object? serviceKey = null) : this(lifetime.ToEnum<ServiceLifetime>(), serviceKey) { }



        ///// <summary>
        /////  Help constructor pass <see cref="string"/> of typo <see cref="ServiceLifetime"/> and pass  <see cref="Type" langword="interface"/>.
        ///// Custom attribute uses to inject all Servicers .
        ///// <para>Warning: better inhernet all services from <see langword="I"/>[Name your repo] </para>
        ///// <para> will work default when  see same service type same interface name With I at start/else will work by <see cref="ImplementationType"/> then <see cref="UseImplementation"/> </para>
        ///// </summary>
        ///// <param name="lifetime"></param>
        ///// <param name="interfaceType"></param>
        ///// <param name="serviceKey"></param>
        //public AutoServiceAttribute(string lifetime, Type interfaceType, object? serviceKey = null) : this(lifetime.ToEnum<ServiceLifetime>(), interfaceType, serviceKey) { }



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
                            .Cast<AutoServiceAttribute>().Select(x => x.ImplementationType)
                            .Single();


        /// <summary>
        /// Reflection get All which used with Services .
        /// </summary>
        /// <param name="type"></param>
        /// <returns>ServiceLifetime</returns>
        internal static (ServiceLifetime, Type?, bool?,object?) GetProperties(Type type)
            => type.GetCustomAttributes(typeof(AutoServiceAttribute), false)
                            .Cast<AutoServiceAttribute>().Select(att => (att.LifetimeType, att.ImplementationType, att.UseImplementation,att.ServiceKey))
                            .Single();

        /// <inheritdoc/>

        public override string ToString() => $"{nameof(LifetimeType)}:{LifetimeType}" +
            $",    {nameof(ImplementationType)}:{ImplementationType?.ToString() ?? "null"}" +
            $",    {nameof(UseImplementation)}:{UseImplementation?.ToString() ?? "null"}" +
            $",  {nameof(ServiceKey)}:{ServiceKey?.ToString() ?? "null"}";
    }
}
