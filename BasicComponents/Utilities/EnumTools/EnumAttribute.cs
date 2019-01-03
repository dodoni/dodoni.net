/* MIT License
Copyright (c) 2011-2019 Markus Wendt (http://www.dodoni-project.net)

All rights reserved.

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
 
Please see http://www.dodoni-project.net/ for more information concerning the Dodoni.net project. 
*/
using System;
using System.Reflection;

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Serves as factory for attributes of enumerations.
    /// </summary>
    public static class EnumAttribute
    {
        #region public (static) methods

        /// <summary>Gets the attribute of a specific enumeration type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute class.</typeparam>
        /// <param name="enumType">An enumeration type.</param>
        /// <returns>The first <typeparamref name="TAttribute"/> instance of the enumeration or <c>null</c> if no such attribute is available.</returns>
        public static TAttribute Create<TAttribute>(Type enumType)
            where TAttribute : Attribute
        {
            object[] customAttributes = enumType.GetCustomAttributes(typeof(TAttribute), false);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                for (int k = 0; k < customAttributes.Length; k++)
                {
                    if (customAttributes[k] is TAttribute)  // return the first attribute which corresponds to the given attribute type
                    {
                        return customAttributes[k] as TAttribute;
                    }
                }
            }
            return null;
        }

        /// <summary>Gets the attribute of a specific <see cref="System.Enum"/> object.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute class.</typeparam>
        /// <param name="value">The value, i.e. the item of the enumeration.</param>
        /// <returns>The first <typeparamref name="TAttribute"/> instance of <paramref name="value"/> or <c>null</c> if no such attribute is available.</returns>
        public static TAttribute Create<TAttribute>(Enum value)
           where TAttribute : Attribute
        {
            Type type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());
            if ((memInfo != null) && (memInfo.Length >= 1))
            {
                for (int j = 0; j < memInfo.Length; j++)
                {
                    object[] customAttributes = memInfo[j].GetCustomAttributes(typeof(TAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        for (int k = 0; k < customAttributes.Length; k++)
                        {
                            if (customAttributes[k] is TAttribute)  // return the first attribute which corresponds to the given attribute type
                            {
                                return customAttributes[k] as TAttribute;
                            }
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region internal (static) methods

        /// <summary>Gets the attribute of a specific enumeration item.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute class.</typeparam>
        /// <param name="value">The value, i.e. the item of the enumeration.</param>
        /// <returns>The first <typeparamref name="TAttribute"/> instance of <paramref name="value"/> or <c>null</c> if no such attribute is available.</returns>
        /// <remarks>Here, we use the <see cref="IConvertible"/> representation of a type-safe enumeration representation.</remarks>
        internal static TAttribute Create<TAttribute>(IConvertible value)
            where TAttribute : Attribute
        {
            Type type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());
            if ((memInfo != null) && (memInfo.Length >= 1))
            {
                for (int j = 0; j < memInfo.Length; j++)
                {
                    object[] customAttributes = memInfo[j].GetCustomAttributes(typeof(TAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        for (int k = 0; k < customAttributes.Length; k++)
                        {
                            if (customAttributes[k] is TAttribute)  // return the first attribute which corresponds to the given attribute type
                            {
                                return customAttributes[k] as TAttribute;
                            }
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
}