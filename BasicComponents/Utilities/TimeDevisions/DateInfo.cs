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

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Represents a date and a <see cref="System.String"/> object, for example the name of the holiday.
    /// </summary>
    public struct DateInfo : IFormattable, IConvertible, IComparable<DateTime>, IComparable<DateInfo>, IEquatable<DateTime>, IEquatable<DateInfo>
    {
        #region public static (readonly)

        /// <summary>Gets a <see cref="DateInfo"/> which represents 'not a date'.
        /// </summary>
        public static readonly DateInfo NaD = new DateInfo();

        /// <summary>Represents the smallest possible value of <see cref="DateInfo"/>. This field is read-only and corresponds to <see cref="System.DateTime.MinValue"/>.
        /// </summary>
        public static readonly DateInfo MinValue = new DateInfo(DateTime.MinValue);

        /// <summary>Represents the largest possible value of <see cref="DateInfo"/>. This field is read-only and corresponds to <see cref="System.DateTime.MaxValue"/>.
        /// </summary>
        public static readonly DateInfo MaxValue = new DateInfo(DateTime.MaxValue);
        #endregion

        #region public members

        /// <summary>The <see cref="System.DateTime"/> object.
        /// </summary>
        public readonly DateTime DateTime;

        /// <summary>The optional <see cref="System.String"/> object that describes this instance.
        /// </summary>
        public readonly string InfoString;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="DateInfo"/> struct.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <remarks>The time component of <paramref name="dateTime"/> will be ignored.</remarks>
        public DateInfo(DateTime dateTime)
        {
            DateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            InfoString = null;
        }

        /// <summary>Initializes a new instance of the <see cref="DateInfo"/> struct.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="infoString">The info string.</param>
        /// <remarks>The time component of <paramref name="dateTime"/> will be ignored.</remarks>
        public DateInfo(DateTime dateTime, string infoString)
        {
            DateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            InfoString = infoString;
        }

        /// <summary>Initializes a new instance of the <see cref="DateInfo"/> struct.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        public DateInfo(int year, int month, int day)
        {
            DateTime = new DateTime(year, month, day);
            InfoString = null;
        }

        /// <summary>Initializes a new instance of the <see cref="DateInfo"/> struct.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="infoString">The info string.</param>
        public DateInfo(int year, int month, int day, string infoString)
        {
            DateTime = new DateTime(year, month, day);
            InfoString = infoString;
        }
        #endregion

        #region public properties

        /// <summary>Gets the date component of the instance.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get { return DateTime.Date; }
        }

        /// <summary>Gets the day of the month represented by this instance.
        /// </summary>
        /// <value>The day.</value>
        public int Day
        {
            get { return DateTime.Day; }
        }

        /// <summary>Gets the day of the week represented by this instance.
        /// </summary>
        /// <value>The day of week.</value>
        public DayOfWeek DayOfWeek
        {
            get { return DateTime.DayOfWeek; }
        }

        /// <summary>Gets the day of the year represented by this instance.
        /// </summary>
        /// <value>The day of year.</value>
        public int DayOfYear
        {
            get { return DateTime.DayOfYear; }
        }

        /// <summary>Gets the month component of the date represented by this instance.
        /// </summary>
        /// <value>The month.</value>
        public int Month
        {
            get { return DateTime.Month; }
        }

        /// <summary>Gets the year component of the date represented by this instance.
        /// </summary>
        /// <value>The year.</value>
        public int Year
        {
            get { return DateTime.Year; }
        }
        #endregion

        #region public methods

        /// <summary>Converts the value of the current <see cref="DateInfo"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the current instance.
        /// </returns>
        public override string ToString()
        {
            if ((InfoString != null) && (InfoString.Length > 0))
            {
                return String.Format("{0} ({1})", DateTime.ToString("d"), InfoString);
            }
            return DateTime.ToString("d");
        }

        /// <summary>Converts the value of the current <see cref="DateInfo"/> object to its equivalent string representation.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The string representation of the current instance.</returns>
        public string ToString(string format)
        {
            if ((InfoString != null) && (InfoString.Length > 0))
            {
                return String.Format("{0} ({1})", DateTime.ToString(format), InfoString);
            }
            return DateTime.ToString(format);
        }

        #region IFormattable Member

        /// <summary>Converts the value of the current <see cref="DateInfo"/> object to its equivalent string representation.
        /// </summary>
        /// <param name="format">Some format convention for the string representation.</param>
        /// <param name="formatProvider">An object that implements the  <see cref="T:System.IFormatProvider"/> interface.</param>
        /// <returns>Returns the string representation of the current instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if ((InfoString != null) && (InfoString.Length > 0))
            {
                return String.Format("{0} ({1})", DateTime.ToString(format, formatProvider), InfoString);
            }
            return DateTime.ToString(format, formatProvider);
        }
        #endregion

        #region IConvertible Member

        /// <summary>Returns the <see cref="T:System.TypeCode"/> for this instance.
        /// </summary>
        /// <returns>The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        TypeCode IConvertible.GetTypeCode()
        {
            return DateTime.GetTypeCode();
        }

        /// <summary>Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A Boolean value equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 8-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A Unicode character equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.</returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return DateTime;
        }

        /// <summary>Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A double-precision floating-point number equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 16-bit signed integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 32-bit signed integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 64-bit signed integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 8-bit signed integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>A single-precision floating-point number equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of the current <see cref="DateInfo"/> object to its equivalent string representation.
        /// </summary>
        /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider"></see> 
        /// interface.</param>
        /// <returns>Returns the string representation of the current instance.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            if ((InfoString != null) && (InfoString.Length > 0))
            {
                return String.Format("{0} ({1})", DateTime.ToString(provider), InfoString);
            }
            return DateTime.ToString(provider);
        }

        /// <summary>Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, 
        /// using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">The <see cref="T:System.Type"/> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="conversionType"/> does not represents <see cref="System.DateTime"/> or <see cref="DateInfo"/>.</exception>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(DateInfo))
            {
                return this;
            }
            else if (conversionType == typeof(DateTime))
            {
                return this.DateTime;
            }
            else
            {
                throw new ArgumentException("This conversion is not supported.");
            }
        }

        /// <summary>Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 16-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 32-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        /// <summary>Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>An 64-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <exception cref="InvalidCastException">Will be thrown in any case.</exception>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }
        #endregion

        #region IComparable<DateTime> Member

        /// <summary>Compares the current object with another object of type <see cref="DateTime"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// <value>Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </value></returns>
        /// <remarks>The info string will not be taken into account.</remarks>
        public int CompareTo(DateTime other)
        {
            if ((this.DateTime.Day == other.Day) && (this.DateTime.Month == other.Month) && (this.DateTime.Year == other.Year))
            {
                return 0;
            }
            return this.DateTime.CompareTo(other);
        }
        #endregion

        #region IComparable<DateInfo> Member

        /// <summary>Compares this instance to a specified object of the same type and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// <value>Less than zero: This object is less than the <paramref name="other"/> parameter.
        /// Zero: This object is equal to <paramref name="other"/>.
        /// Greater than zero: This object is greater than <paramref name="other"/>.
        /// </value></returns>
        /// <remarks>The info string will not be taken into account.</remarks>
        public int CompareTo(DateInfo other)
        {
            return this.DateTime.CompareTo(other.DateTime);
        }
        #endregion

        #region IEquatable<DateTime> Member

        /// <summary>Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>The info string will not be taken into account.</remarks>
        public bool Equals(DateTime other)
        {
            return ((other.Day == this.DateTime.Day) && (other.Month == this.DateTime.Month) && (other.Year == this.DateTime.Year));
        }
        #endregion

        #region IEquatable<DateInfo> Member

        /// <summary>Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        /// <remarks>The info string will not be taken into account.</remarks>
        public bool Equals(DateInfo other)
        {
            return ((other.DateTime.Day == this.DateTime.Day) && (other.DateTime.Month == this.DateTime.Month) && (other.DateTime.Year == this.DateTime.Year));
        }
        #endregion

        #endregion
    }
}