using System.Reflection;

namespace DependencyInjection.AutoService.Helper
{
    /// <summary>
    /// Basics extensions
    /// </summary>
    internal static class ExtensionMethods
    {

        /// <summary>
        /// Converts the string representation of <see cref="{T}"/>
        /// enumerated constants to an equivalent enumerated object. A parameter specifies
        /// whether the operation is case-insensitive.
        /// <para>ignoreCase is auto active <see langword="true"/> .</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static T ToEnum<T>(this string value)
            => (T)System.Enum.Parse(typeof(T), value, true);


        /// <summary>
        /// Get for each name-space as <see cref="String"/>, <see cref="Type"/>, <see cref="Assembly"/> all calsses type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="any"></param>
        /// <returns></returns>
        internal static List<Type> AbleNameSpaceToType<T>(this T[] any)
        => any.Aggregate(new List<Type>(), (all, next) => { all.AddRange(getOverWriteNext(next)); return all; });

#nullable disable
        /// <summary>
        /// Choice cast wise to implement <see langword="namespace"/> s
        /// </summary>
        internal static Func<object, Type[]> getOverWriteNext = (next) => next switch
        {
            string s => Assembly.Load(s).GetTypes(),
            Type t => t.Assembly.GetTypes(),
            Assembly a => a.GetTypes(),
            _ => default
        };
#nullable enable

    }
}
