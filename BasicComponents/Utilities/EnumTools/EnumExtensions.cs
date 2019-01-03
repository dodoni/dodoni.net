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
using System.Text;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Represents extensions for <see cref="System.Enum"/> objects.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class EnumExtensions
    {
        #region public (static) methods

        /// <summary>Gets a <see cref="System.String"/> representation that takes into account <see cref="LanguageResourceAttribute"/>,
        /// <see cref="LanguageStringAttribute"/> or <see cref="StringAttribute"/>; if available.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the <see cref="System.String"/> representation.</param>
        /// <returns>A <see cref="System.String"/> representation of <paramref name="value"/> with respect to <paramref name="enumStringRepresentationUsage"/>, i.e. 
        /// <see cref="LanguageResourceAttribute"/>, <see cref="LanguageStringAttribute"/>, <see cref="StringAttribute"/>
        /// or the <c>ToString()</c> method is used to generate the string representation.</returns>
        public static string ToFormatString(this Enum value, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            if (enumStringRepresentationUsage == EnumStringRepresentationUsage.ToStringMethod)
            {
                return value.ToString();
            }
            else if (enumStringRepresentationUsage == EnumStringRepresentationUsage.StringAttribute)
            {
                if (Enum.IsDefined(value.GetType(), value) == true)
                {
                    StringAttribute stringAttribute = EnumAttribute.Create<StringAttribute>(value);
                    if (stringAttribute != null)
                    {
                        return stringAttribute.StringRepresentation;
                    }
                    return value.ToString();
                }
                else // is a flag, i.e. a|b|c etc. which is not a member of the enumeration, but perhaps 'b|c' is in the enumeration!
                {
                    StringBuilder strBuilder = new StringBuilder();

                    foreach (Enum enumValue in GetEnumComponents(value))
                    {
                        if (strBuilder.Length > 0)
                        {
                            strBuilder.Append(EnumString.FlagsEnumSeparatorChar);
                        }
                        StringAttribute stringAttribute = EnumAttribute.Create<StringAttribute>(enumValue);
                        if (stringAttribute != null)
                        {
                            strBuilder.Append(stringAttribute.StringRepresentation);
                        }
                        else
                        {
                            strBuilder.Append(enumValue.ToString());
                        }

                    }
                    return strBuilder.ToString();
                }
            }

            // otherwise take into account a language depended string representation, if available:
            Type enumType = value.GetType();
            LanguageResourceAttribute languageResourceFile = Attribute.GetCustomAttribute(enumType, typeof(LanguageResourceAttribute)) as LanguageResourceAttribute;

            if (Enum.IsDefined(value.GetType(), value) == true)
            {
                if (languageResourceFile != null)
                {
                    LanguageStringAttribute languageStringAttribute = EnumAttribute.Create<LanguageStringAttribute>(value);
                    if (languageStringAttribute != null)
                    {
                        ResourceManager resourceManager = new ResourceManager(languageResourceFile.FullResourceName, enumType.Assembly);
                        return resourceManager.GetString(languageStringAttribute.ResourcePropertyName, Thread.CurrentThread.CurrentUICulture);
                    }
                }
                StringAttribute stringAttribute = EnumAttribute.Create<StringAttribute>(value);
                if (stringAttribute != null)
                {
                    return stringAttribute.StringRepresentation;
                }
            }
            else  // the enumeration is some [Flag] and only the parts contains a attribute but not the bitwise combination; we use ',' as separator
            {
                StringBuilder strBuilder = new StringBuilder();

                foreach (Enum enumValue in GetEnumComponents(value))
                {
                    if (strBuilder.Length > 0)
                    {
                        strBuilder.Append(EnumString.FlagsEnumSeparatorChar);
                    }

                    if (languageResourceFile != null)
                    {
                        LanguageStringAttribute languageStringAttribute = EnumAttribute.Create<LanguageStringAttribute>(enumValue);
                        if (languageStringAttribute != null)
                        {
                            ResourceManager resourceManager = new ResourceManager(languageResourceFile.FullResourceName, enumType.Assembly);
                            strBuilder.Append(resourceManager.GetString(languageStringAttribute.ResourcePropertyName, Thread.CurrentThread.CurrentUICulture));
                            continue;  // to to next enumValue
                        }
                    }
                    StringAttribute stringAttribute = EnumAttribute.Create<StringAttribute>(enumValue);
                    if (stringAttribute != null)
                    {
                        strBuilder.Append(stringAttribute.StringRepresentation);
                    }
                    else
                    {
                        strBuilder.Append(enumValue.ToString());
                    }
                }
                return strBuilder.ToString();
            }
            return value.ToString();
        }

        /// <summary>Gets a description of the argument taken into account <see cref="DescriptionAttribute"/> or <see cref="LanguageDescriptionAttribute"/>;
        /// if no description available <see cref="String.Empty"/> will be returned.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The description of <paramref name="value"/>; <see cref="System.String.Empty"/> if no description available.</returns>
        /// <remarks>The <see cref="LanguageDescriptionAttribute"/> will be preferred if <see cref="DescriptionAttribute"/> is given as well.</remarks>
        public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            if (Attribute.GetCustomAttribute(enumType, typeof(LanguageResourceAttribute)) is LanguageResourceAttribute languageResourceFile)
            {
                ResourceManager resourceManager = new ResourceManager(languageResourceFile.FullResourceName, enumType.Assembly);

                LanguageDescriptionAttribute languageDescriptionAttribute = EnumAttribute.Create<LanguageDescriptionAttribute>(value);
                if (languageDescriptionAttribute != null)
                {
                    return String.Format(resourceManager.GetString(languageDescriptionAttribute.ResourcePropertyName, Thread.CurrentThread.CurrentUICulture), languageDescriptionAttribute.Arg0);
                }
                // perhaps some language independent description is given:
                DescriptionAttribute descriptionAttribute = EnumAttribute.Create<DescriptionAttribute>(value);
                if (descriptionAttribute != null)
                {
                    return descriptionAttribute.Description;
                }
            }
            else // no language resource file given:
            {
                DescriptionAttribute descriptionAttribute = EnumAttribute.Create<DescriptionAttribute>(value);
                if (descriptionAttribute != null)
                {
                    return descriptionAttribute.Description;
                }
            }
            return String.Empty;
        }

        /// <summary>Determines whether a specific <see cref="System.Enum"/> object represents the value '0'.
        /// </summary>
        /// <param name="value">The <see cref="System.Enum"/> object.</param>
        /// <returns><c>true</c> if the underlying value of <paramref name="value"/> is equal to <c>0</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNullValue(this Enum value)
        {
            Type enumType = Enum.GetUnderlyingType(value.GetType());
            if (enumType == typeof(Int32))
            {
                return (((IConvertible)value).ToInt32(CultureInfo.InvariantCulture) == 0);
            }
            else if (enumType == typeof(Int16))
            {
                return (((IConvertible)value).ToInt16(CultureInfo.InvariantCulture) == 0);
            }
            else if (enumType == typeof(Int64))
            {
                return (((IConvertible)value).ToInt64(CultureInfo.InvariantCulture) == 0);
            }
            else if (enumType == typeof(Byte))
            {
                return (((IConvertible)value).ToByte(CultureInfo.InvariantCulture) == 0);
            }
            throw new NotImplementedException(String.Format("Unknown underlying value type {0}.", enumType.Name));
        }
        #endregion

        #region private (static) methods

        /// <summary>Represents the separator character for enumeration values that are represented by a bitwise OR (Flags).
        /// </summary>
        private static readonly char[] sm_OriginalFlagEnumSplitSeparator = new[] { ',' };

        /// <summary>Gets the values of a specific enumeration which produce a specific enumeration value, i.e. the union of the return values represents the <paramref name="enumValue"/>.
        /// </summary>
        /// <param name="enumValue">The enumeration value.</param>
        /// <returns>A enumeration of <see cref="Enum"/> objects that represents <paramref name="enumValue"/>, i.e. the union of the return values represents the <paramref name="enumValue"/>.</returns>
        private static IEnumerable<Enum> GetEnumComponents(Enum enumValue)
        {
            return enumValue
                    .ToString()
                    .Split(sm_OriginalFlagEnumSplitSeparator)
                    .Select((x => (Enum)Enum.Parse(enumValue.GetType(), x.Trim(), true)));
        }
        #endregion
    }
}