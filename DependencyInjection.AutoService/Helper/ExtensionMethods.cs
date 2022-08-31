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

namespace Meteors.Helper
{
    /// <summary>
    /// Basics extensions
    /// </summary>
    internal static class ExtensionMethods
    {

        /// <summary>
        /// Converts the string representation of <see cref="Type"/>
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


        /// <summary>
        /// Get all namespaces in same app luncher folder
        /// </summary>
        internal static string[] DllDependencies => Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).
           Where(x => Path.GetExtension(x) == ".dll").Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();


    }
}
