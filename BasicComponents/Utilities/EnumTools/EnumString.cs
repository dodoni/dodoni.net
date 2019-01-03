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
using System.Resources;
using System.Threading;
using System.Collections.Generic;

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Represents an item of a specific enumeration and some <see cref="System.String"/> representation.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>    
    public class EnumString<TEnum> : IComparable<EnumString<TEnum>>, IComparable<string>, IComparable<IdentifierString>, IEquatable<IdentifierString>, IEquatable<string>
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        #region public (readonly) members

        /// <summary>The value.
        /// </summary>
        public readonly TEnum Value;

        /// <summary>The <see cref="IdentifierString"/> representation of <see cref="Value"/>.
        /// </summary>
        public readonly IdentifierString StringRepresentation;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="EnumString&lt;TEnum&gt;"/> class.
        /// </summary>
        /// <param name="value">The element of a specific enumeration.</param>
        /// <param name="stringRepresentation">A <see cref="System.String"/> representation of the <paramref name="value"/>.</param>
        internal EnumString(TEnum value, string stringRepresentation)
        {
            Value = value;
            StringRepresentation = new IdentifierString(stringRepresentation);
        }

        /// <summary>Initializes a new instance of the <see cref="EnumString&lt;TEnum&gt;"/> class.
        /// </summary>
        /// <param name="value">The element of a specific enumeration.</param>
        /// <param name="stringRepresentation">A <see cref="IdentifierString"/> representation of the <paramref name="value"/>.</param>
        internal EnumString(TEnum value, IdentifierString stringRepresentation)
        {
            Value = value;
            StringRepresentation = stringRepresentation;
        }
        #endregion

        #region public methods

        #region IComparable<EnumString<TEnum>> Members

        /// <summary>Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(EnumString<TEnum> other)
        {
            return Value.CompareTo(other.Value);
        }
        #endregion

        #region IComparable<string> Members

        /// <summary>Compares the current object with another object of type <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </returns>
        /// <remarks>The <see cref="IdentifierString"/> representation of <paramref name="other"/> is used for the comparisson,
        /// i.e. ignoring white spaces etc.</remarks>
        public int CompareTo(string other)
        {
            return StringRepresentation.IDString.CompareTo(IdentifierString.GetIDString(other));
        }
        #endregion

        #region IComparable<IdentifierString> Members

        /// <summary>Compares the current object with another object of type <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </returns>
        /// <remarks>The <see cref="IdentifierString"/> representation of <paramref name="other"/> is used for the comparisson,
        /// i.e. ignoring white spaces etc.</remarks>
        public int CompareTo(IdentifierString other)
        {
            return StringRepresentation.IDString.CompareTo(other.IDString);
        }
        #endregion

        #region IEquatable<IdentifierString> Members

        /// <summary>Indicates whether the current object is equal to another object of type <see cref="IdentifierString"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(IdentifierString other)
        {
            if (other == null)
            {
                return false;
            }
            return (other.IDString == StringRepresentation.IDString);
        }
        #endregion

        #region IEquatable<string> Members

        /// <summary>Indicates whether the current object is equal to another object of type <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(string other)
        {
            if (other == null)
            {
                return false;
            }
            return (StringRepresentation.IDString == IdentifierString.GetIDString(other));
        }
        #endregion

        /// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is EnumString<TEnum> other)
            {
                return other.Value.Equals(Value);
            }
            return false;
        }

        /// <summary>Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return (string)StringRepresentation;
        }
        #endregion

        #region public static methods

        /// <summary>Retrieves a collection of the values of the constants in a specified enumeration in its <see cref="EnumString&lt;TEnum&gt;"/> representation.
        /// </summary>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation.</param>        
        /// <returns>A collection of <see cref="EnumString&lt;TEnum&gt;"/> objects that contains the items of the enumeration </returns>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="TEnum"/> does not represents an enumeration.</exception>
        public static IEnumerable<EnumString<TEnum>> GetValues(EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            Type enumerationType = typeof(TEnum);
            if (enumerationType.IsEnum == false)
            {
                throw new ArgumentException(String.Format("The type {0} does not represents an enumeration.", enumerationType.ToString()));
            }
            foreach (TEnum value in Enum.GetValues(enumerationType))
            {
                Enum enumValueAsEnum = (Enum)((object)value); // one can not convert it directly!

                yield return new EnumString<TEnum>(value, EnumExtensions.ToFormatString(enumValueAsEnum, enumStringRepresentationUsage));
            }
        }

        /// <summary>Converts the string representation of an enumeration item to its enumeration item equivalent.</summary>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the
        /// enumeration represented by <typeparamref name="TEnum"/>.</param>
        /// <returns>The element of the enumeration with respect to <paramref name="stringRepresentation"/>.</returns>
        /// <remarks>White spaces etc. will be ignored.</remarks>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represent an enumeration.</exception>
        /// <exception cref="FormatException">Thrown, if there is no enumeration item with string representation <paramref name="stringRepresentation"/>.</exception>        
        public static TEnum Parse(string stringRepresentation, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            if (TryParse(stringRepresentation, out TEnum value, enumStringRepresentationUsage) == true)
            {
                return value;
            }
            throw new FormatException(String.Format(ExceptionMessages.ArgumentIsInvalid, stringRepresentation));
        }

        /// <summary>Converts the string representation of an enumeration item to its enumeration item equivalent.</summary>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="value">The element of the enumeration with respect to <paramref name="stringRepresentation"/> (output).</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the
        /// enumeration represented by <typeparamref name="TEnum"/>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <remarks>White spaces etc. will be ignored.</remarks>
        /// <exception cref="ArgumentException">Thrown, if <typeparamref name="TEnum"/> does not represent an enumeration.</exception>
        public static bool TryParse(string stringRepresentation, out TEnum value, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "TEnum: " + typeof(TEnum).ToString()));
            }
            if ((stringRepresentation == null) || (stringRepresentation.Length == 0))
            {
                value = default;
                return false;
            }
            string stringRepresentationID = IdentifierString.GetIDString(stringRepresentation, false);

            if (enumStringRepresentationUsage == EnumStringRepresentationUsage.ToStringMethod)
            {
                return Enum.TryParse<TEnum>(stringRepresentationID.Replace(EnumString.FlagsEnumSeparatorChar, EnumString.dotNetFlagsEnumSeparatorChar), true, out value);
            }

            ResourceManager resourceManager = null;
            Type enumerationType = typeof(TEnum);
            if (enumStringRepresentationUsage == EnumStringRepresentationUsage.LanguageStringAttribute)
            {
                if (Attribute.GetCustomAttribute(enumerationType, typeof(LanguageResourceAttribute)) is LanguageResourceAttribute languageResourceFile)
                {
                    resourceManager = new ResourceManager(languageResourceFile.FullResourceName, enumerationType.Assembly);
                }
            }

            /* if the enumeration contains the 'Flags' attribute, we assume that the input is given with respect to some
             * comma separated (see 'FlagsEnumSeparatorChar')  list and the return value will be the bitwise OR relation of the corresponding elements. */
            if (enumerationType.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)  // no [Flags] attribute available
            {
                foreach (TEnum enumValue in Enum.GetValues(enumerationType)) // linear is no problem because the number of elements is in general very small
                {
                    if (GetFormatString(enumValue, resourceManager) == stringRepresentationID)
                    {
                        value = enumValue;
                        return true;
                    }
                } // it is not a Flag, but perhaps a bitwise OR combination (which is unusual for a non-flag enumeration)
            }
            /* split the strings into a list of strings, where the 'FlagsEnumSeparatorChar' is take into
             * account, transform each of the sub-strings to some ID-representation, compare to the elements
             * of the given list and build a new string, which will be used for the Parse-Method of System.Enum.
             * Here, we can not apply '|=' to the matched elements, that's the reason to do it this way:
             */
            StringBuilder bString = new StringBuilder();

            foreach (string subString in stringRepresentationID.Split(EnumString.FlagsEnumSeparatorChar))
            {
                foreach (TEnum enumValue in Enum.GetValues(enumerationType))  // linear is no problem because the number of elements is in general very small
                {
                    if (GetFormatString(enumValue, resourceManager) == subString)
                    {
                        if (bString.Length > 0)
                        {
                            bString.Append(EnumString.dotNetFlagsEnumSeparatorChar);
                        }
                        bString.Append(enumValue.ToString());  // transform to the internal string representation
                    }
                }
            }
            return Enum.TryParse<TEnum>(bString.ToString(), true, out value);
        }

        /// <summary>Performs an implicit conversion from <see cref="Dodoni.BasicComponents.Utilities.EnumString&lt;TEnum&gt;"/> to <typeparamref name="TEnum"/>.
        /// </summary>
        /// <param name="enumString">The <see cref="EnumString&lt;TEnum&gt;"/> object to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TEnum(EnumString<TEnum> enumString)
        {
            return enumString.Value;
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.BasicComponents.Utilities.EnumString&lt;TEnum&gt;"/> to <see cref="Dodoni.BasicComponents.Utilities.EnumString"/>.
        /// </summary>
        /// <param name="enumString">The <see cref="EnumString&lt;TEnum&gt;"/> object to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator EnumString(EnumString<TEnum> enumString)
        {
            return new EnumString(((Enum)(object)enumString.Value), enumString.StringRepresentation);
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.BasicComponents.Utilities.EnumString&lt;TEnum&gt;"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="enumString">The <see cref="EnumString&lt;TEnum&gt;"/> object to convert.</param>
        /// <returns>The result of the conversion, i.e. the raw string representation component of <paramref name="enumString"/>.</returns>
        public static explicit operator string(EnumString<TEnum> enumString)
        {
            return enumString.StringRepresentation.String;
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.BasicComponents.Utilities.EnumString&lt;TEnum&gt;"/> to <see cref="Dodoni.BasicComponents.IdentifierString"/>.
        /// </summary>
        /// <param name="enumString">The <see cref="EnumString&lt;TEnum&gt;"/> object to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator IdentifierString(EnumString<TEnum> enumString)
        {
            return enumString.StringRepresentation;
        }
        #endregion

        #region private methods

        /// <summary>Gets the <see cref="System.String"/> representation of a specific enumeration item.
        /// </summary>
        /// <param name="value">The element of a specific enumeration.</param>
        /// <param name="resourceManager">The resource manager, perhaps <c>null</c> if no resource manager is available for <paramref name="value"/>.</param>
        /// <returns>The <see cref="System.String"/> representation of <paramref name="value"/> taken into account the <paramref name="resourceManager"/>,
        /// i.e. <see cref="LanguageStringAttribute"/> if != <c>null</c>; otherwise <see cref="StringAttribute"/> is used if available; 
        /// otherwise the return value of the <c>ToString()</c> method of <paramref name="value"/> will be returned.</returns>
        private static string GetFormatString(TEnum value, ResourceManager resourceManager)
        {
            if (resourceManager != null) // try language dependent string representation
            {
                LanguageStringAttribute languageStringAttribute = EnumAttribute.Create<LanguageStringAttribute>(value);
                if (languageStringAttribute != null)
                {
                    return IdentifierString.GetIDString(resourceManager.GetString(languageStringAttribute.ResourcePropertyName, Thread.CurrentThread.CurrentUICulture), false);
                }
            }
            StringAttribute stringAttribute = EnumAttribute.Create<StringAttribute>(value);
            if (stringAttribute != null)
            {
                return IdentifierString.GetIDString(stringAttribute.StringRepresentation, false);
            }
            return IdentifierString.GetIDString(value.ToString(), false);
        }
        #endregion
    }

    /// <summary>Represents an item of a specific enumeration and some <see cref="System.String"/> representation.
    /// </summary>
    public class EnumString : IComparable<EnumString>, IComparable<string>, IComparable<IdentifierString>, IEquatable<IdentifierString>, IEquatable<string>
    {
        #region internal (const) members

        /// <summary>The separator character of the .NET functions used for the string representation of a bit field; It seems not to be documented whether this is language dependend.
        /// </summary>
        internal const char dotNetFlagsEnumSeparatorChar = ',';
        #endregion

        #region public static members

        /// <summary>Represents the separator character for the string representation of a bit field, i.e. if the enumeration contains the <see cref="System.FlagsAttribute"/>.
        /// </summary>
        public static char FlagsEnumSeparatorChar = ',';
        #endregion

        #region public (readonly) members

        /// <summary>The value.
        /// </summary>
        public readonly Enum Value;

        /// <summary>The <see cref="IdentifierString"/> representation of <see cref="Value"/>.
        /// </summary>
        public readonly IdentifierString StringRepresentation;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="EnumString"/> class.
        /// </summary>
        /// <param name="value">The element of a specific enumeration.</param>
        /// <param name="stringRepresentation">A <see cref="System.String"/> representation of the <paramref name="value"/>.</param>
        internal EnumString(Enum value, string stringRepresentation)
        {
            Value = value;
            StringRepresentation = new IdentifierString(stringRepresentation);
        }

        /// <summary>Initializes a new instance of the <see cref="EnumString"/> class.
        /// </summary>
        /// <param name="value">The element of a specific enumeration.</param>
        /// <param name="stringRepresentation">A <see cref="IdentifierString"/> representation of the <paramref name="value"/>.</param>
        internal EnumString(Enum value, IdentifierString stringRepresentation)
        {
            Value = value;
            StringRepresentation = stringRepresentation;
        }
        #endregion

        #region public methods

        #region IComparable<EnumString> Members

        /// <summary>Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(EnumString other)
        {
            return Value.CompareTo(other.Value);
        }
        #endregion

        #region IComparable<string> Members

        /// <summary>Compares the current object with another object of type <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </returns>
        /// <remarks>The <see cref="IdentifierString"/> representation of <paramref name="other"/> is used for the comparisson,
        /// i.e. ignoring white spaces etc.</remarks>
        public int CompareTo(string other)
        {
            return StringRepresentation.IDString.CompareTo(IdentifierString.GetIDString(other));
        }
        #endregion

        #region IComparable<IdentifierString> Members

        /// <summary>Compares the current object with another object of type <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </returns>
        /// <remarks>The <see cref="IdentifierString"/> representation of <paramref name="other"/> is used for the comparisson,
        /// i.e. ignoring white spaces etc.</remarks>
        public int CompareTo(IdentifierString other)
        {
            return StringRepresentation.IDString.CompareTo(other.IDString);
        }
        #endregion

        #region IEquatable<IdentifierString> Members

        /// <summary>Indicates whether the current object is equal to another object of type <see cref="IdentifierString"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(IdentifierString other)
        {
            if (other == null)
            {
                return false;
            }
            return (other.IDString == StringRepresentation.IDString);
        }
        #endregion

        #region IEquatable<string> Members

        /// <summary>Indicates whether the current object is equal to another object of type <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(string other)
        {
            if (other == null)
            {
                return false;
            }
            return (StringRepresentation.IDString == IdentifierString.GetIDString(other));
        }
        #endregion

        /// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is EnumString other)
            {
                return other.Value.Equals(Value);
            }
            return false;
        }

        /// <summary>Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return (string)StringRepresentation;
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="EnumString"/>.
        /// </summary>
        /// <param name="value">The item of a specific enumeration.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of <paramref name="value"/>.</param>
        /// <returns>A <see cref="EnumString"/> instance that contains <paramref name="value"/> as well as some (perhaps language depending) string representation.</returns>
        public static EnumString Create(Enum value, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            return new EnumString(value, EnumExtensions.ToFormatString(value, enumStringRepresentationUsage));
        }

        /// <summary>Retrieves a collection of the values of the constants in a specified enumeration in its <see cref="EnumString"/> representation.
        /// </summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation.</param>        
        /// <returns>A collection of <see cref="EnumString"/> objects that contains the items of the enumeration </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="enumType"/> does not represents an enumeration.</exception>
        public static IEnumerable<EnumString> GetValues(Type enumType, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            if (enumType.IsEnum == false)
            {
                throw new ArgumentException(String.Format("The type {0} does not represents an enumeration.", enumType.ToString()));
            }
            foreach (Enum value in Enum.GetValues(enumType))
            {
                yield return new EnumString(value, EnumExtensions.ToFormatString(value, enumStringRepresentationUsage));
            }
        }

        /// <summary>Creates a new <see cref="EnumString&lt;TEnum&gt;"/>.
        /// </summary>
        /// <param name="value">The item of a specific enumeration.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of <paramref name="value"/>.</param>
        /// <returns>A <see cref="EnumString&lt;TEnum&gt;"/> instance that contains <paramref name="value"/> as well as some (perhaps language depending) string representation.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> does not represents the item of an enumeration.</exception>
        public static EnumString<TEnum> Create<TEnum>(TEnum value, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            Type enumerationType = typeof(TEnum);
            if (enumerationType.IsEnum == false)
            {
                throw new ArgumentException(String.Format("The type {0} does not represents an enumeration.", enumerationType.ToString()));
            }
            if (enumStringRepresentationUsage == EnumStringRepresentationUsage.ToStringMethod)
            {
                return new EnumString<TEnum>(value, value.ToString());
            }
            Enum enumValueAsEnum = (Enum)((object)value); // we can not convert it in a different way
            return new EnumString<TEnum>(value, EnumExtensions.ToFormatString(enumValueAsEnum, enumStringRepresentationUsage));
        }

        /// <summary>Converts the string representation of an enumeration item to its enumeration item equivalent.</summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the
        /// enumeration represented by <paramref name="enumType"/>.</param>
        /// <returns>The item of the enumeration corresponds to <paramref name="stringRepresentation"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="enumType"/> does not represent an enumeration.</exception>
        /// <exception cref="FormatException">Thrown, if there is no enumeration item with string representation <paramref name="stringRepresentation"/>.</exception>
        public static Enum Parse(Type enumType, string stringRepresentation, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            if (TryParse(enumType, stringRepresentation, out Enum value, enumStringRepresentationUsage) == true)
            {
                return value;
            }
            throw new FormatException(String.Format(ExceptionMessages.ArgumentIsInvalid, stringRepresentation));
        }

        /// <summary>Converts the string representation of an enumeration item to its enumeration item equivalent.</summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="value">The element of the enumeration with respect to <paramref name="stringRepresentation"/> (output).</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the
        /// enumeration represented by <paramref name="enumType"/>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="enumType"/> does not represent an enumeration.</exception>
        public static bool TryParse(Type enumType, string stringRepresentation, out Enum value, EnumStringRepresentationUsage enumStringRepresentationUsage = EnumStringRepresentationUsage.LanguageStringAttribute)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (enumType.IsEnum == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "EnumType: " + enumType.ToString()), "EnumType");
            }
            if ((stringRepresentation == null) || (stringRepresentation.Length == 0))
            {
                value = null;
                return false;
            }
            string stringRepresentationID = IdentifierString.GetIDString(stringRepresentation, false);

            if (enumStringRepresentationUsage == EnumStringRepresentationUsage.ToStringMethod)
            {
                try  // there is not 'Enum.TryParse' method!
                {
                    value = (Enum)Enum.Parse(enumType, stringRepresentationID.Replace(EnumString.FlagsEnumSeparatorChar, EnumString.dotNetFlagsEnumSeparatorChar), true);
                    return true;
                }
                catch
                {
                    value = null;
                    return false;
                }
            }

            ResourceManager resourceManager = null;
            if (enumStringRepresentationUsage == EnumStringRepresentationUsage.LanguageStringAttribute)
            {
                if (Attribute.GetCustomAttribute(enumType, typeof(LanguageResourceAttribute)) is LanguageResourceAttribute languageResourceFile)
                {
                    resourceManager = new ResourceManager(languageResourceFile.FullResourceName, enumType.Assembly);
                }
            }

            /* if the enumeration contains the 'Flags' attribute, we assume that the input is given with respect to some
             * comma separated (see 'FlagsEnumSeparatorChar')  list and the return value will be the bitwise OR relation of the corresponding elements. */
            if (enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)  // no [Flags] attribute available
            {
                foreach (Enum enumValue in Enum.GetValues(enumType))
                {
                    if (GetFormatString(enumValue, resourceManager) == stringRepresentationID)
                    {
                        value = enumValue;
                        return true;
                    }
                } // it is not a Flag, but perhaps a bitwise OR combination (which is unusual for a non-flag enumeration)
            }
            /* split the strings into a list of strings, where the 'FlagsEnumSeparatorChar' is take into
             * account, transform each of the sub-strings to some ID-representation, compare to the elements
             * of the given list and build a new string, which will be used for the Parse-Method of System.Enum.
             * Here, we can not apply '|=' to the matched elements, that's the reason to do it this way:
             */
            StringBuilder bString = new StringBuilder();

            foreach (string subString in stringRepresentationID.Split(FlagsEnumSeparatorChar))
            {
                foreach (Enum enumValue in Enum.GetValues(enumType))  // linear is no problem because the number of elements is in general very small
                {
                    if (GetFormatString(enumValue, resourceManager).ToIDString() == subString)
                    {
                        if (bString.Length > 0)
                        {
                            bString.Append(EnumString.dotNetFlagsEnumSeparatorChar);
                        }
                        bString.Append(enumValue.ToString());  // transform to the internal string representation
                    }
                }
            }
            // no tryParse method available in Enum for a non-generic parameter, i.e. use the Enum.Parse method and some try-catch block
            try
            {
                value = (Enum)Enum.Parse(enumType, bString.ToString(), true);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        /// <summary>Performs an implicit conversion from <see cref="Dodoni.BasicComponents.Utilities.EnumString"/> to <see cref="System.Enum"/>.
        /// </summary>
        /// <param name="enumString">The <see cref="EnumString"/> object to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Enum(EnumString enumString)
        {
            return enumString.Value;
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.BasicComponents.Utilities.EnumString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="enumString">The <see cref="EnumString"/> object to convert.</param>
        /// <returns>The result of the conversion, i.e. the raw string representation component of <paramref name="enumString"/>.</returns>
        public static explicit operator string(EnumString enumString)
        {
            return enumString.StringRepresentation.String;
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.BasicComponents.Utilities.EnumString"/> to <see cref="Dodoni.BasicComponents.IdentifierString"/>.
        /// </summary>
        /// <param name="enumString">The <see cref="EnumString"/> object to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator IdentifierString(EnumString enumString)
        {
            return enumString.StringRepresentation;
        }
        #endregion

        #region private static methods

        /// <summary>Gets the <see cref="System.String"/> representation of a specific enumeration item.
        /// </summary>
        /// <param name="value">The element of a specific enumeration.</param>
        /// <param name="resourceManager">The resource manager, perhaps <c>null</c> if no resource manager is available for <paramref name="value"/>.</param>
        /// <returns>The <see cref="System.String"/> representation of <paramref name="value"/> taken into account the <paramref name="resourceManager"/>,
        /// i.e. <see cref="LanguageStringAttribute"/> if != <c>null</c>; otherwise <see cref="StringAttribute"/> is used if available; 
        /// otherwise the return value of the <c>ToString()</c> method of <paramref name="value"/> will be returned.</returns>
        private static string GetFormatString(Enum value, ResourceManager resourceManager)
        {
            if (resourceManager != null) // try language dependent string representation
            {
                LanguageStringAttribute languageStringAttribute = EnumAttribute.Create<LanguageStringAttribute>(value);
                if (languageStringAttribute != null)
                {
                    return IdentifierString.GetIDString(resourceManager.GetString(languageStringAttribute.ResourcePropertyName, Thread.CurrentThread.CurrentUICulture), false);
                }
            }
            StringAttribute stringAttribute = EnumAttribute.Create<StringAttribute>(value);
            if (stringAttribute != null)
            {
                return IdentifierString.GetIDString(stringAttribute.StringRepresentation, false);
            }
            return IdentifierString.GetIDString(value.ToString(), false);
        }
        #endregion
    }
}