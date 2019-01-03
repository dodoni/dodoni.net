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
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.Finance
{
    /// <summary>Represents two <see cref="TenorTimeSpan"/> objects which are interpreted as a spread, for example "10Y [US] vs. 5Y [EUR]" or "10Y [US] - 5Y [EUR]" par yields etc.
    /// </summary>
    /// <remarks>No white spaces are allowed in th delimiter 'vs.'.</remarks>
    public struct TenorTimeSpanSpread : IEquatable<TenorTimeSpanSpread>, IEquatable<TenorTimeSpan>
    {
        #region nested enumerations

        /// <summary>Represents the type of <see cref="System.String"/> representation of a specific <see cref="TenorTimeSpanSpread"/>.
        /// </summary>
        public enum StringRepresentationUsage
        {
            /// <summary>Using the delimiter 'vs.' between both <see cref="TenorTimeSpan"/> string representations.
            /// </summary>
            Versus,

            /// <summary>Using the delimiter '-' between both <see cref="TenorTimeSpan"/> string representations.
            /// </summary>
            Minus
        }
        #endregion

        #region private members

        /// <summary>The string representation usage, i.e. the delimiter used for the <see cref="System.String"/> representation (output); the 
        /// input (in constructor and tryParse methods) can contain one arbitrary delimiter 'vs.', '-'.
        /// </summary>
        private readonly StringRepresentationUsage m_StringRepresentationUsage;
        #endregion

        #region public/private (static) members

        /// <summary>The <see cref="TenorTimeSpanSpread"/> object which contains '0' years, '0' months and '0' days for <see cref="FirstTenor"/> and <see cref="SecondTenor"/>.
        /// </summary>
        public static readonly TenorTimeSpanSpread Null = new TenorTimeSpanSpread(TenorTimeSpan.Null, TenorTimeSpan.Null);

        /// <summary>The <see cref="IdentifierString"/> representation of the null-tenor-spread.
        /// </summary>
        private static IdentifierString sm_NullIdentifierStringRepresentation = new IdentifierString("<null>");
        #endregion

        #region public (readonly) members

        /// <summary>The first <see cref="TenorTimeSpan"/> object of the <see cref="TenorTimeSpanSpread"/>.
        /// </summary>
        public readonly TenorTimeSpan FirstTenor;

        /// <summary>An optional description of <see cref="FirstTenor"/>, for example the name of a currency; perhaps <see cref="String.Empty"/> or <c>null</c>.
        /// </summary>
        public readonly string FirstTenorDescription;

        /// <summary>The second <see cref="TenorTimeSpan"/> object of the <see cref="TenorTimeSpanSpread"/>; perhaps equal to <see cref="TenorTimeSpan.Null"/> which is a degenerated spread
        /// and represents a <see cref="TenorTimeSpan"/> object only.
        /// </summary>
        public readonly TenorTimeSpan SecondTenor;

        /// <summary>An optional description of <see cref="SecondTenor"/>, for example the name of a currency; perhaps <see cref="String.Empty"/> or <c>null</c>.
        /// </summary>
        public readonly string SecondTenorDescription;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="TenorTimeSpanSpread"/> struct.
        /// </summary>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <param name="description">The optional description of <paramref name="tenorTimeSpan"/>.</param>
        /// <param name="stringRepresentationUsage">The string representation usage, i.e. the delimiter used for the <see cref="TenorTimeSpanSpread.ToString()"/> method.</param>
        public TenorTimeSpanSpread(TenorTimeSpan tenorTimeSpan, string description = "", StringRepresentationUsage stringRepresentationUsage = StringRepresentationUsage.Versus)
        {
            FirstTenor = tenorTimeSpan;
            FirstTenorDescription = (description != null) ? description : String.Empty;
            SecondTenor = TenorTimeSpan.Null;
            SecondTenorDescription = String.Empty;
            m_StringRepresentationUsage = stringRepresentationUsage;
        }

        /// <summary>Initializes a new instance of the <see cref="TenorTimeSpanSpread"/> struct.
        /// </summary>
        /// <param name="firstTenor">The first tenor.</param>
        /// <param name="secondTenor">The second tenor.</param>
        /// <param name="firstTenorDescription">The optional description of the <paramref name="firstTenor"/>.</param>
        /// <param name="secondTenorDescription">The optional description of the <paramref name="secondTenor"/>.</param>
        /// <param name="stringRepresentationUsage">The string representation usage, i.e. the delimiter used for the <see cref="TenorTimeSpanSpread.ToString()"/> method.</param>
        public TenorTimeSpanSpread(TenorTimeSpan firstTenor, TenorTimeSpan secondTenor, string firstTenorDescription = "", string secondTenorDescription = "", StringRepresentationUsage stringRepresentationUsage = StringRepresentationUsage.Versus)
        {
            FirstTenor = firstTenor;
            SecondTenor = secondTenor;
            FirstTenorDescription = (firstTenorDescription != null) ? firstTenorDescription : String.Empty;
            SecondTenorDescription = (secondTenorDescription != null) ? secondTenorDescription : String.Empty;
            m_StringRepresentationUsage = stringRepresentationUsage;
        }

        /// <summary>Initializes a new instance of the <see cref="TenorTimeSpanSpread"/> struct.
        /// </summary>
        /// <param name="tenorTimeSpanSpread">The <see cref="System.String"/> representation of the tenor spread.</param>
        /// <exception cref="ArgumentException">Thrown, if <paramref name="tenorTimeSpanSpread"/> is invalid.</exception>
        public TenorTimeSpanSpread(string tenorTimeSpanSpread)
        {
            if (TryParse(tenorTimeSpanSpread, out FirstTenor, out SecondTenor, out FirstTenorDescription, out SecondTenorDescription, out m_StringRepresentationUsage) == false)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, tenorTimeSpanSpread), "tenorTimeSpanSpread");
            }
        }
        #endregion

        #region public properties

        /// <summary>Gets a value indicating whether this instance represents a <see cref="TenorTimeSpan"/>, i.e. it is a degenerated spread and <see cref="SecondTenor"/> is equal to <see cref="TenorTimeSpan.Null"/>.
        /// </summary>
        /// <value><c>true</c> if this instance is tenor time span; otherwise, <c>false</c>.
        /// </value>
        public bool IsTenorTimeSpan
        {
            get { return TenorTimeSpan.IsNull(SecondTenor); }
        }
        #endregion

        #region public methods

        #region IEquatable<TenorTimeSpanSpread> Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise <c>false</c>.</returns>
        public bool Equals(TenorTimeSpanSpread other)
        {
            return (FirstTenor.Equals(other.FirstTenor) && SecondTenor.Equals(other.SecondTenor));
        }
        #endregion

        #region IEquatable<TenorTimeSpan> Members

        /// <summary>Indicates whether the current object is equal to another object of type <see cref="TenorTimeSpan"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise <c>false</c>.</returns>
        public bool Equals(TenorTimeSpan other)
        {
            if (TenorTimeSpan.IsNull(SecondTenor) == false)
            {
                return false;
            }
            return FirstTenor.Equals(other);
        }
        #endregion

        /// <summary>Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return String.Format("{0} & {1}", FirstTenor.ToString(), SecondTenor.ToString()).GetHashCode();
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="stringRepresentationUsage">The string representation usage.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(StringRepresentationUsage stringRepresentationUsage)
        {
            if ((TenorTimeSpan.IsNull(FirstTenor) == true) && (TenorTimeSpan.IsNull(SecondTenor) == true) && (FirstTenorDescription == null || FirstTenorDescription.Length == 0) && (SecondTenorDescription == null || SecondTenorDescription.Length == 0))
            {
                return sm_NullIdentifierStringRepresentation.String;
            }

            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append(FirstTenor.ToString());
            if ((FirstTenorDescription != null) && (FirstTenorDescription.Length > 0))
            {
                if (strBuilder.Length > 0)
                {
                    strBuilder.Append(" ");
                }
                strBuilder.Append(String.Format(" [{0}]", FirstTenorDescription));
            }

            if ((TenorTimeSpan.IsNull(SecondTenor) == false) || (SecondTenorDescription != null && SecondTenorDescription.Length > 0))
            {
                switch (stringRepresentationUsage)
                {
                    case StringRepresentationUsage.Versus:
                        strBuilder.Append(String.Format(" vs. {0}", SecondTenor.ToString()));
                        break;

                    case StringRepresentationUsage.Minus:
                        strBuilder.Append(String.Format(" - {0}", SecondTenor.ToString()));
                        break;

                    default:
                        throw new NotImplementedException();
                }
                if ((SecondTenorDescription != null) && (SecondTenorDescription.Length > 0))
                {
                    strBuilder.Append(String.Format(" [{0}]", SecondTenorDescription));
                }
            }
            return strBuilder.ToString();
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToString(m_StringRepresentationUsage);
        }
        #endregion

        #region public static methods

        /// <summary>Sets the <see cref="System.String"/> representation of <see cref="TenorTimeSpanSpread.Null"/>.
        /// </summary>
        /// <param name="nullStringRepresentation">The <see cref="System.String"/> representation of <see cref="TenorTimeSpanSpread.Null"/>, i.e. used for <code>ToString</code> as well as for the Parse methods.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="nullStringRepresentation"/> is <c>null</c>.</exception>
        public static void SetNullStringRepresentation(string nullStringRepresentation)
        {
            if (nullStringRepresentation == null)
            {
                throw new ArgumentNullException("nullStringRepresentation", String.Format(ExceptionMessages.ArgumentNull, "String representation of TenorTimeSpanSpread.Null"));
            }
            sm_NullIdentifierStringRepresentation = new IdentifierString(nullStringRepresentation);
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.Finance.TenorTimeSpan"/> to <see cref="Dodoni.Finance.TenorTimeSpanSpread"/>.
        /// </summary>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator TenorTimeSpanSpread(TenorTimeSpan tenorTimeSpan)
        {
            return new TenorTimeSpanSpread(tenorTimeSpan);
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.Finance.TenorTimeSpanSpread"/> to <see cref="Dodoni.Finance.TenorTimeSpan"/>.
        /// </summary>
        /// <param name="tenorTimeSpanSpread">The tenor time span spread.</param>
        /// <returns>The result of the conversion.</returns>
        /// <exception cref="InvalidCastException">Thrown, if <paramref name="tenorTimeSpanSpread"/> does not represents a <see cref="TenorTimeSpan"/> object.</exception>
        public static explicit operator TenorTimeSpan(TenorTimeSpanSpread tenorTimeSpanSpread)
        {
            if (tenorTimeSpanSpread.IsTenorTimeSpan == false)
            {
                throw new InvalidCastException();
            }
            return tenorTimeSpanSpread.FirstTenor;
        }

        /// <summary>Converts a <see cref="System.String"/> into its <see cref="TenorTimeSpanSpread"/> representation.
        /// </summary>
        /// <param name="tenorTimeSpanSpread">The <see cref="System.String"/> representation of a <see cref="TenorTimeSpanSpread"/> object.</param>
        /// <returns>The <see cref="TenorTimeSpanSpread"/> object which <see cref="System.String"/> representation is equal to <paramref name="tenorTimeSpanSpread"/>.</returns>
        public static TenorTimeSpanSpread Parse(string tenorTimeSpanSpread)
        {
            return new TenorTimeSpanSpread(tenorTimeSpanSpread);
        }

        /// <summary>Converts a <see cref="System.String"/> into its <see cref="TenorTimeSpanSpread"/> representation.
        /// </summary>
        /// <param name="tenorSpreadString">The <see cref="System.String"/> representation of a <see cref="TenorTimeSpanSpread"/> object.</param>
        /// <param name="tenorTimeSpanSpread">The <see cref="TenorTimeSpanSpread"/> object (output).</param>
        /// <returns><c>true</c> if <paramref name="tenorTimeSpanSpread"/> contains valid data; <c>false</c> otherwise.</returns>
        public static bool TryParse(string tenorSpreadString, out TenorTimeSpanSpread tenorTimeSpanSpread)
        {
            TenorTimeSpan firstTenor, secondTenor;
            string firstTenorDescription, secondTenorDescription;
            StringRepresentationUsage stringRepresentationUsage;

            if (TryParse(tenorSpreadString, out firstTenor, out secondTenor, out firstTenorDescription, out secondTenorDescription, out stringRepresentationUsage) == false)
            {
                tenorTimeSpanSpread = TenorTimeSpanSpread.Null;
                return false;
            }
            tenorTimeSpanSpread = new TenorTimeSpanSpread(firstTenor, secondTenor, firstTenorDescription, secondTenorDescription, stringRepresentationUsage);
            return true;
        }

        /// <summary>Determines whether a specified <see cref="TenorTimeSpanSpread"/> object has a length of <c>0</c>, i.e. <see cref="TenorTimeSpanSpread.FirstTenor"/> = <see cref="TenorTimeSpanSpread.SecondTenor"/> = <see cref="TenorTimeSpan.Null"/>.
        /// </summary>
        /// <param name="tenorTimeSpanSpread">The <see cref="TenorTimeSpanSpread"/> object.</param>
        /// <returns><c>true</c> if both <see cref="TenorTimeSpan"/> objects of <paramref name="tenorTimeSpanSpread"/> are equal to <see cref="TenorTimeSpan.Null"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(TenorTimeSpanSpread tenorTimeSpanSpread)
        {
            return (TenorTimeSpan.IsNull(tenorTimeSpanSpread.FirstTenor) && (TenorTimeSpan.IsNull(tenorTimeSpanSpread.SecondTenor)));
        }

        /// <summary>Determines whether a specified <see cref="TenorTimeSpanSpread"/> object is a spread, i.e. <see cref="TenorTimeSpanSpread.FirstTenor"/> and <see cref="TenorTimeSpanSpread.SecondTenor"/> is <b>not</b> <see cref="TenorTimeSpan.Null"/>.
        /// </summary>
        /// <param name="tenorTimeSpanSpread">The <see cref="TenorTimeSpanSpread"/> object.</param>
        /// <returns><c>true</c> if both <see cref="TenorTimeSpan"/> objects of <paramref name="tenorTimeSpanSpread"/> are distinct from <see cref="TenorTimeSpan.Null"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSpread(TenorTimeSpanSpread tenorTimeSpanSpread)
        {
            return (TenorTimeSpan.IsNull(tenorTimeSpanSpread.FirstTenor) == false) && (TenorTimeSpan.IsNull(tenorTimeSpanSpread.SecondTenor) == false);
        }
        #endregion

        #region private (static) methods

        /// <summary>Converts a <see cref="System.String"/> object into two <see cref="TenorTimeSpan"/> objects and its descriptions.
        /// </summary>
        /// <param name="tenorSpreadString">The <see cref="System.String"/> representation of the tenor spread.</param>
        /// <param name="firstTenor">The first tenor (output).</param>
        /// <param name="secondTenor">The second tenor (output).</param>
        /// <param name="firstTenorDescription">The description of <paramref name="firstTenor"/>; perhaps <c>null</c> (output).</param>
        /// <param name="secondTenorDescription">The description of <paramref name="secondTenor"/>; perhaps <c>null</c> (output).</param>
        /// <param name="stringRepresentationUsage">The string representation usage, i.e. the delimiter used for the <see cref="TenorTimeSpanSpread.ToString()"/> method (output).</param>
        /// <returns>A value indicating whether <paramref name="firstTenor"/>, <paramref name="secondTenor"/>, <paramref name="firstTenorDescription"/> and <paramref name="secondTenorDescription"/> contains valid data.</returns>
        private static bool TryParse(string tenorSpreadString, out TenorTimeSpan firstTenor, out TenorTimeSpan secondTenor, out string firstTenorDescription, out string secondTenorDescription, out StringRepresentationUsage stringRepresentationUsage)
        {
            if (tenorSpreadString == null)
            {
                throw new ArgumentNullException("tenorSpreadString");
            }
            string tenorSpreadIDString = tenorSpreadString.ToIDString();

            firstTenor = secondTenor = TenorTimeSpan.Null;
            firstTenorDescription = secondTenorDescription = String.Empty;
            stringRepresentationUsage = StringRepresentationUsage.Minus;

            if (((tenorSpreadIDString.Length == 1) && (tenorSpreadIDString[0] == '0')) || (tenorSpreadIDString == sm_NullIdentifierStringRepresentation.IDString))
            {
                return true;  // special: '0' and "<null>" will be interpreted as 'null'
            }

            int delimiterIndex = tenorSpreadString.IndexOf('-');
            if (delimiterIndex >= 0)
            {
                if (TryParse(tenorSpreadString.Substring(0, delimiterIndex), out firstTenor, out firstTenorDescription) == false)
                {
                    return false;
                }
                return TryParse(tenorSpreadString.Substring(delimiterIndex + 1, tenorSpreadString.Length - delimiterIndex - 1), out secondTenor, out secondTenorDescription);
            }

            delimiterIndex = tenorSpreadString.IndexOf('v');  // first character of "vs."
            if (delimiterIndex < 0)
            {
                return TryParse(tenorSpreadString, out firstTenor, out firstTenorDescription);
            }
            else if (TryParse(tenorSpreadString.Substring(0, delimiterIndex), out firstTenor, out firstTenorDescription) == false)
            {
                return false;
            }

            if ((delimiterIndex + 2 < tenorSpreadString.Length) && (tenorSpreadString[delimiterIndex + 1] == 's') && (tenorSpreadString[delimiterIndex + 2] == '.'))
            {
                stringRepresentationUsage = StringRepresentationUsage.Versus;

                return TryParse(tenorSpreadString.Substring(delimiterIndex + 3, tenorSpreadString.Length - delimiterIndex - 3), out secondTenor, out secondTenorDescription);
            }
            return false;
        }

        /// <summary>Converts a <see cref="System.String"/> object into a <see cref="TenorTimeSpan"/> object and its optional description.
        /// </summary>
        /// <param name="tenorWithDescription">The tenor with [optional] description in its <see cref="System.String"/> representation.</param>
        /// <param name="tenor">The tenor (output).</param>
        /// <param name="tenorDescription">The description (output).</param>
        /// <returns>A value indicating whether <paramref name="tenor"/> and <paramref name="tenorDescription"/> contains valid data.</returns>
        private static bool TryParse(string tenorWithDescription, out TenorTimeSpan tenor, out string tenorDescription)
        {
            int descriptionStartIndex = tenorWithDescription.IndexOf('[');
            if (descriptionStartIndex < 0)
            {
                tenorDescription = String.Empty;
                return TenorTimeSpan.TryParse(tenorWithDescription, out tenor);
            }
            int descriptionEndIndex = tenorWithDescription.LastIndexOf(']');
            if (descriptionEndIndex < descriptionStartIndex)
            {
                tenorDescription = String.Empty;
                tenor = TenorTimeSpan.Null;
                return false;
            }
            string tenorString = tenorWithDescription.Substring(0, descriptionStartIndex);
            tenorDescription = tenorWithDescription.Substring(descriptionStartIndex + 1, descriptionEndIndex - descriptionStartIndex - 1);
            return TenorTimeSpan.TryParse(tenorString, out tenor);
        }
        #endregion
    }
}