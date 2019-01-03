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
using System.Linq;
using System.Text;
using System.Resources;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

namespace Dodoni.BasicComponents
{
    /// <summary>Represents a <see cref="System.String"/> object and a 'normalized' representation; each character which comes next to 
    /// <see cref="IdentifierString.IgnoringStartCharacter"/> will be ignored.
    /// </summary>
    [Serializable]
    public class IdentifierString : IComparable<String>, IComparable<IdentifierString>, IEquatable<IdentifierString>, IEquatable<string>
    {
        #region nested classes

        /// <summary>Represents a comparer where the identifier string is taken into account only.
        /// </summary>
        private class Compararer : IEqualityComparer<IdentifierString>
        {
            #region IEqualityComparer<IdentifierString> Members

            /// <summary>Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="IdentifierString"/> to compare.</param>
            /// <param name="y">The second object of type <see cref="IdentifierString"/> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public bool Equals(IdentifierString x, IdentifierString y)
            {
                return x.IDString == y.IDString;
            }

            /// <summary>Returns a hash code for a specific <see cref="IdentifierString"/> object.
            /// </summary>
            /// <param name="obj">The <see cref="IdentifierString"/> object.</param>
            /// <returns>A hash code for <paramref name="obj"/>, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public int GetHashCode(IdentifierString obj)
            {
                return obj.IDString.GetHashCode();
            }
            #endregion
        }
        #endregion

        #region public readonly static members

        /// <summary>A comparer implementation where the identifier string is taken into account only.
        /// </summary>
        public static readonly IEqualityComparer<IdentifierString> EqualityComparer = new Compararer();

        /// <summary>Represents the empty <see cref="IdentifierString"/>. This field is read-only.
        /// </summary>
        public static readonly IdentifierString Empty = new IdentifierString(String.Empty);

        /// <summary>The character which represents the start token for some comment, i.e. each character which comes next to this
        /// character will be ignored.
        /// </summary>
        public const char IgnoringStartCharacter = '@';
        #endregion

        #region public (readonly) members

        /// <summary>The (raw) <see cref="System.String"/> representation.
        /// </summary>
        public readonly string String;

        /// <summary>The <see cref="IdentifierString.String"/> object in its 'normalized' <see cref="System.String"/> representation.
        /// </summary>
        public readonly string IDString;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="IdentifierString"/> class.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="rawString"/> is <c>null</c>.</exception>
        public IdentifierString(string rawString)
        {
            String = rawString ?? throw new ArgumentNullException("rawString");
            IDString = GetIDString(rawString);
        }
        #endregion

        #region public methods

        #region IComparable<IdentifierString> Member

        /// <summary>Compares this instance to a specified <see cref="IdentifierString"/> object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">A <see cref="IdentifierString"/> object to compare to this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has this meanings:
        /// <para>Less than zero: This instance is less than <paramref name="other"/>.</para>
        /// <para>Zero: This instance is equal to <paramref name="other"/>.</para>
        /// <para>Greater than zero: This instance is greater than <paramref name="other"/>.</para>
        /// </returns>
        /// <remarks>The <see cref="IDString"/> is used for the compare operation.</remarks>
        public int CompareTo(IdentifierString other)
        {
            return this.IDString.CompareTo(other.IDString);
        }
        #endregion

        #region IComparable<string> Members

        /// <summary>Compares this instance to a specified <see cref="String"/> object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">A <see cref="String"/> object to compare to this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has this meanings:
        /// <para>Less than zero: This instance is less than <paramref name="other"/>.</para>
        /// <para>Zero: This instance is equal to <paramref name="other"/>.</para>
        /// <para>Greater than zero: This instance is greater than <paramref name="other"/>.</para>
        /// </returns>
        /// <remarks>The <see cref="IDString"/> is used for the compare operation.</remarks>
        public int CompareTo(string other)
        {
            return this.IDString.CompareTo(GetIDString(other));
        }
        #endregion

        #region IEquatable<IdentifierString> Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(IdentifierString other)
        {
            return IDString.Equals(other.IDString);
        }
        #endregion

        #region IEquatable<string> Members

        /// <summary>Indicates whether the current object is equal to another <see cref="System.String"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>The <see cref="IDString"/> is used for the compare operation.</remarks>
        public bool Equals(string other)
        {
            return IDString.Equals(GetIDString(other));
        }
        #endregion

        /// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>The <see cref="IDString"/> is used for the compare operation.</remarks>
        public override bool Equals(object obj)
        {
            if (obj is IdentifierString)
            {
                return (IDString == (((IdentifierString)obj).IDString));
            }
            else if (obj is string)
            {
                return (IDString == GetIDString((string)obj));
            }
            return false;
        }

        /// <summary>Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <remarks>The <see cref="IDString"/> is used for the compare operation.</remarks>
        public override int GetHashCode()
        {
            return IDString.GetHashCode();
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return String;
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="IdentifierString"/>.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        /// <returns>A <see cref="IdentifierString"/> object that represents <paramref name="rawString"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="rawString"/> is <c>null</c>.</exception>
        public static IdentifierString Create(string rawString)
        {
            return new IdentifierString(rawString);
        }

        /// <summary>Creates a new <see cref="IdentifierString"/>.
        /// </summary>
        /// <param name="resourceManager">The resource manager with respect to the resource which contains the <see cref="System.String"/> representations.</param>
        /// <param name="resourcePropertyName">The property name with respect to a given <paramref name="resourceManager"/> which contains some language dependend <see cref="System.String"/> representation.</param>
        /// <exception cref="ArgumentException">Thrown if no resource file is found or the property name is not valid for the given resource file.</exception>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="resourceManager"/> is <c>null</c>.</exception>
        /// <remarks>Use this method if a <see cref="IdentifierString"/> should be language depended, i.e. if a resource file is available.</remarks>
        public static IdentifierString Create(ResourceManager resourceManager, string resourcePropertyName)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }
            try
            {
                string rawString = resourceManager.GetString(resourcePropertyName, Thread.CurrentThread.CurrentUICulture);
                if (rawString == null)
                {
                    throw new ArgumentException(String.Format("Resource file name {0} or resource property name {1} invalid.", resourceManager.BaseName, resourcePropertyName));
                }
                return new IdentifierString(rawString);
            }
            catch (MissingManifestResourceException e)
            {
                throw new ArgumentException(String.Format("Resource file name {0} or resource property name {1} invalid.", resourceManager.BaseName, resourcePropertyName), e);
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException(String.Format("Resource file name {0} or resource property name {1} invalid.", resourceManager.BaseName, resourcePropertyName), e);
            }
        }

        /// <summary>Creates a new <see cref="IdentifierString"/>.
        /// </summary>
        /// <param name="fullResourceName">The resource name (no language dependend suffix) and the corresponding namespace.</param>
        /// <param name="resourcePropertyName">The property name with respect to a given resource which contains some language dependend <see cref="System.String"/> representation.</param>
        /// <param name="assembly">The assembly to take into account.</param>
        /// <exception cref="ArgumentException">Thrown if no resource file is found or the property name is not valid for the given resource file.</exception>
        /// <remarks>Use this method if a <see cref="IdentifierString"/> should be language depended, i.e. if a resource file is available.</remarks>
        public static IdentifierString Create(string fullResourceName, string resourcePropertyName, Assembly assembly)
        {
            var resourceManager = new ResourceManager(fullResourceName, assembly);

            return Create(resourceManager, resourcePropertyName);
        }

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.BasicComponents.IdentifierString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="identifierString">The identifier string.</param>
        /// <returns>The result of the conversion, i.e. the 'raw' <see cref="System.String"/> representation.</returns>
        public static explicit operator string(IdentifierString identifierString)
        {
            return identifierString?.String;
        }
        #endregion

        #region internal static methods

        /// <summary>Gets the ID string, i.e. a 'normalized' <see cref="String"/> representation.
        /// </summary>
        /// <param name="rawString">The (raw) string.</param>
        /// <param name="ignorCommentCharacter">A value indicating whether all character which comes next to <see cref="IdentifierString.IgnoringStartCharacter"/> will be ignored.</param>
        /// <returns>A 'normalized' representation of <paramref name="rawString"/>, especially without white spaces.
        /// </returns>
        internal static string GetIDString(string rawString, bool ignorCommentCharacter = true)
        {
            var rawStringSpan = MemoryExtensions.AsSpan(rawString);

            if (ignorCommentCharacter == true)
            {
                int splitIndex = rawString.IndexOf(IgnoringStartCharacter);
                if (splitIndex >= 0)
                {
                    rawStringSpan = rawStringSpan.Slice(0, splitIndex);  // 'rawString = rawString.Remove(splitIndex)'
                }
            }
            int k = 0;
            var identifierAsArray = new char[rawStringSpan.Length];

            for (int i = 0; i < rawStringSpan.Length; i++)
            {
                char c = rawStringSpan[i];
                if (Char.IsWhiteSpace(c) == false)
                {
                    identifierAsArray[k] = Char.ToLower(c);
                    k++;
                }
            }
            return new string(identifierAsArray, startIndex: 0, length: k);
        }
        #endregion     
    }
}