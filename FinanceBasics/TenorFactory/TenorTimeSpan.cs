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
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.Finance
{
    /// <summary>Represents some time span, especially the number of years, months and days.
    /// </summary>
    public struct TenorTimeSpan : IComparable<TenorTimeSpan>, IComparer<TenorTimeSpan>, IEquatable<TenorTimeSpan>
    {
        #region nested enumerations

        /// <summary>Rounding rules for <see cref="GetTimeSpanInBetween(DateTime,DateTime,RoundingRule)"/>.
        /// </summary>
        public enum RoundingRule
        {
            /// <summary>The output is the counterpart of <see cref="TenorTimeSpanExtensions.AddTenorTimeSpan(DateTime, TenorTimeSpan)"/>, i.e. applying
            /// the result of <see cref="GetTimeSpanInBetween(DateTime,DateTime,RoundingRule)"/> to the start date should return the end date.
            /// </summary>
            Exact,

            /// <summary>Skip the <c>number of days</c> component. If the absolute number of days is greater or equal than 16 the <c>month</c> component will be increase (decreased) by one.
            /// </summary>
            NearestMonth,

            /// <summary>Calculate the number of weeks that represents roughly the time span, i.e. Math.Abs(numberOfDays) mod 7 \leq 3 set the number of weeks to (numberOfDays / 7); otherwise (numberOfDays / 7) + sgn(numberOfDays) * 7.
            /// </summary>
            NearestWeek
        }
        #endregion

        #region private (static) members

        /// <summary>The pattern string for the tenor string conversion, i.e. "5Y6M3W5D".
        /// </summary>
        private const string sm_TenorPatternString = @"^-?([0123456789]+[YMWD]{1,1})+$";

        /// <summary>The regular expression for the tenor string conversion.
        /// </summary>
        private static readonly Regex sm_TenorRegularExpression = new Regex(sm_TenorPatternString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>The sub-pattern string  for the tenor string conversion, for example "...5Y...".
        /// </summary>
        private const string sm_TenorSubPatternString = @"[0123456789]+[YMWD]{1,1}";

        /// <summary>The regular expression for the sub-patern for the tenor string conversion.
        /// </summary>
        private static readonly Regex sm_TenorSubRegularExpression = new Regex(sm_TenorSubPatternString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>The <see cref="IdentifierString"/> representation of the null-tenor.
        /// </summary>
        private static IdentifierString sm_NullTenorIdentifierStringRepresentation = new IdentifierString("<null>");
        #endregion

        #region public static members

        /// <summary>The 'overnight' tenor.
        /// </summary>
        public static readonly TenorTimeSpan OvernightTenor = new TenorTimeSpan(TenorType.Overnight);

        /// <summary>The string which is used if the tenor is given by some string and it reflects the 'overnight' case.
        /// </summary>
        public static readonly string OvernightTenorStringRepresentation = TenorType.Overnight.ToFormatString();

        /// <summary>The 'tomorrow-against-next-day' tenor.
        /// </summary>
        public static readonly TenorTimeSpan TomorrowNextTenor = new TenorTimeSpan(TenorType.TomorrowNext);

        /// <summary>The string which is used if the tenor is given by some string and it reflects the 'tomorrow next' case.
        /// </summary>
        public static readonly string TomorrowNextTenorStringRepresentation = TenorType.TomorrowNext.ToFormatString();

        /// <summary>The <see cref="TenorTimeSpan"/> object which represents '0' years, '0' months and '0' days.
        /// </summary>
        public static readonly TenorTimeSpan Null = new TenorTimeSpan(0, 0, 0);
        #endregion

        #region public (readonly) members

        /// <summary>The number of years.
        /// </summary>
        public readonly int Years;

        /// <summary>The number of months.
        /// </summary>
        public readonly int Months;

        /// <summary>The number of days.
        /// </summary>
        public readonly int Days;

        /// <summary>The type of the tenor.
        /// </summary>
        public readonly TenorType TenorType;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="TenorTimeSpan"/> struct.
        /// </summary>
        /// <param name="years">The years.</param>
        /// <param name="months">The months.</param>
        /// <param name="days">The days.</param>
        /// <remarks><see cref="TenorType"/> is set to <see cref="Dodoni.Finance.TenorType.RegularTenor"/>.</remarks>
        /// <exception cref="ArgumentException">Thrown, if some argument is positiv and another is negative.</exception>
        public TenorTimeSpan(int years, int months, int days)
        {
            if (((years >= 0) && (months >= 0) && (days >= 0)) || ((years <= 0) && (months <= 0) && (days <= 0)))
            {
                // normalize the number of months and years:
                Years = years + months / 12;  // the sign '+' is correct in any case
                Months = months % 12;

                Days = days;
            }
            else
            {
                throw new ArgumentException();
            }
            TenorType = TenorType.RegularTenor;
        }

        /// <summary>Initializes a new instance of the <see cref="TenorTimeSpan"/> struct.
        /// </summary>
        /// <param name="tenorString">The tenor string.</param>
        /// <exception cref="ArgumentException">Thrown, if the format of <paramref name="tenorString"/> is incorrect.</exception>
        public TenorTimeSpan(string tenorString)
        {
            if (TryParse(tenorString, out Years, out Months, out Days, out TenorType) == false)
            {
                throw new ArgumentException("tenorString", "Can not convert the tenor string to a number of years, months and days.");
            }

            // normalize the number of months and years:
            Years += Months / 12;  // the sign '+' is correct in any case
            Months = Months % 12;
        }
        #endregion

        #region private constructors

        /// <summary>Initializes a new instance of the <see cref="TenorTimeSpan"/> struct.
        /// </summary>
        /// <param name="tenorType">The tenor type.</param>
        /// <remarks>
        /// <para>
        ///        This constructor will be used from the static constructor only.
        /// </para>
        /// The number of years and months will be set to <c>0</c>, the number of days is set to 1 ('ON') or 2 ('TN').</remarks>
        /// <exception cref="ArgumentException">Thrown if <paramref name="tenorType"/> is not equal to <see cref="Dodoni.Finance.TenorType.TomorrowNext"/> or <see cref="Dodoni.Finance.TenorType.Overnight"/>
        /// </exception>
        private TenorTimeSpan(TenorType tenorType)
        {
            Years = 0;
            Months = 0;
            if (tenorType == TenorType.TomorrowNext)
            {
                Days = 2;
            }
            else if (tenorType == TenorType.Overnight)
            {
                Days = 1;
            }
            else
            {
                throw new ArgumentException("tenorType");
            }
            TenorType = tenorType;
        }
        #endregion

        #region public properties

        /// <summary>Gets a value indicating whether the current <see cref="TenorTimeSpan"/> object represents
        /// a positive time span.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is positive; otherwise, <c>false</c>.
        /// </value>
        public bool IsPositive
        {
            get { return ((Years >= 0) && (Months >= 0) && (Days >= 0)); }
        }
        #endregion

        #region public methods

        #region IComparable<TenorTimeSpan> Member

        /// <summary>Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="value">A <see cref="TenorTimeSpan"/> instance to compare with the instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the comparands. The return value has these
        /// meanings:ValueMeaning Less than zero. This instance is less than <paramref name="value"/>.
        /// Zero. This instance is equal to <paramref name="value"/>. Greater than zero. This instance is greather than <paramref name="value"/>.
        /// </returns>
        public int CompareTo(TenorTimeSpan value)
        {
            /* compute the number of days. If ON -2, if TN -1; else 0 days are added: */
            return 360 * (Years - value.Years) + 30 * (Months - value.Months) + Days - value.Days + (int)TenorType - (int)value.TenorType;
        }

        /// <summary>Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="value">A <see cref="TenorTimeSpan"/> instance to compare with the instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the comparands. The return value has these
        /// meanings:ValueMeaning Less than zero. This instance is less than <paramref name="value"/>.
        /// Zero. This instance is equal to <paramref name="value"/>. Greater than zero. This instance is greather than <paramref name="value"/>.
        /// </returns>
        int IComparable<TenorTimeSpan>.CompareTo(TenorTimeSpan value)
        {
            /* compute the number of days. If ON -2, if TN -1; else 0 days are added: */
            return 360 * (Years - value.Years) + 30 * (Months - value.Months) + Days - value.Days + (int)TenorType - (int)value.TenorType;
        }
        #endregion

        #region IComparer<TenorTimeSpan> Member

        /// <summary>Compares two <see cref="TenorTimeSpan"/> instances.
        /// </summary>
        /// <param name="x">A <see cref="TenorTimeSpan"/> instance to compare.</param>
        /// <param name="y">A <see cref="TenorTimeSpan"/> instance to compare.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the comparands. The return value has these
        /// meanings: ValueMeaning less than zero. <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero. This <paramref name="x"/> is equal to <paramref name="y"/>. Greater than zero. This <paramref name="x"/> is greather than <paramref name="y"/>.
        /// </returns>
        public int Compare(TenorTimeSpan x, TenorTimeSpan y)
        {
            return 360 * (x.Years - y.Years) + 30 * (x.Months - y.Months) + x.Days - y.Days + (int)x.TenorType - (int)y.TenorType;   // = x.ToRawDays() - y.ToRawDays() + x.Type - y.Type;
        }
        #endregion

        #region IEquatable<TenorTimeSpan> Members

        /// <summary>Indicates whether the current object is equal to an other object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise <c>false</c>.</returns>
        public bool Equals(TenorTimeSpan other)
        {
            return (Years == other.Years) && (Months == other.Months) && (Days == other.Days) && (TenorType == other.TenorType);
        }
        #endregion

        /// <summary>Convertes the time span to the number of days, where one year is assumed to have 360 days
        /// and each month is assumed to have 30 days.
        /// </summary>
        /// <returns>The number of days represented by the current instance.</returns>
        public int ToRawDays()
        {
            return 360 * Years + 30 * Months + Days;
        }

        /// <summary>Converts the time span to a (raw) year fraction, i.e. <see cref="Years"/> + <see cref="Months"/> / 12.0 + <see cref="Days"/> / 365.0.
        /// </summary>
        /// <returns>The year fraction represented by the current instance.</returns>
        public double ToRawYearFraction()
        {
            return Years + Months / 12.0 + Days / 365.0;
        }

        /// <summary>Gets the (absolute) number of months that represents the current instance.
        /// </summary>
        /// <returns>The (absolute) number of months that represens the current instance.</returns>
        /// <remarks>>This method ignores <see cref="Days"/>.</remarks>
        public int GetNumberOfMonths()
        {
            return Years * 12 + Months;
        }

        /// <summary>Converts the value of the current <see cref="TenorTimeSpan"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A <see cref="System.String"/> object that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (TenorType == TenorType.Overnight)
            {
                return OvernightTenorStringRepresentation;
            }
            else if (TenorType == TenorType.TomorrowNext)
            {
                return TomorrowNextTenorStringRepresentation;
            }
            else
            {
                string returnString = (IsPositive == true) ? String.Empty : "- ";

                if (Years != 0)
                {
                    returnString += String.Format("{0}Y ", Math.Abs(Years));
                }
                if (Months != 0)
                {
                    returnString += String.Format("{0}M ", Math.Abs(Months));
                }
                if (Days != 0)
                {
                    int days = Math.Abs(Days);

                    if (days >= 7)
                    {
                        returnString += String.Format("{0}W ", days / 7);

                        days = days - 7 * (days / 7);
                    }
                    if (days != 0)
                    {
                        returnString += String.Format("{0}D", days);
                    }
                }
                if (returnString.Length == 0)
                {
                    return sm_NullTenorIdentifierStringRepresentation.String;
                }
                return returnString.Trim();
            }
        }

        /// <summary>Determines whether a <see cref="System.DateTime"/> is inside some time period where a specified start date and
        /// deferred days are given.
        /// </summary>
        /// <remarks>Determines wheter <paramref name="startDate"/> &lt; <paramref name="testDate"/> &lt; <paramref name="startDate"/> plus this plus <paramref name="deferredDays"/>;
        /// if the time span represented by the current object is positive, in a reverse order otherwise.</remarks>
        /// <param name="startDate">The start date.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="deferredDays">The number of deferred days applyied to the end date of the time period only.</param>
        /// <returns>
        /// 	<c>true</c> if <paramref name="testDate"/> is inside the specific time period; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInsideTimeSpan(DateTime startDate, DateTime testDate, int deferredDays = 0)
        {
            DateTime endDate = startDate.AddYears(Years);
            endDate = endDate.AddMonths(Months);
            endDate = endDate.AddDays(Days);

            if (startDate <= endDate)
            {
                return ((startDate <= testDate) && (testDate <= endDate.AddDays(deferredDays)));
            }
            return ((startDate >= testDate) && (testDate >= endDate.AddDays(-deferredDays)));
        }
        #endregion

        #region public (static) methods

        /// <summary>Sets the <see cref="System.String"/> representation of <see cref="TenorTimeSpan.Null"/>.
        /// </summary>
        /// <param name="nullStringRepresentation">The <see cref="System.String"/> representation of <see cref="TenorTimeSpan.Null"/>, i.e. used for <code>ToString</code> as well as for the Parse methods.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="nullStringRepresentation"/> is <c>null</c>.</exception>
        public static void SetNullStringRepresentation(string nullStringRepresentation)
        {
            if (nullStringRepresentation == null)
            {
                throw new ArgumentNullException("nullStringRepresentation", String.Format(ExceptionMessages.ArgumentNull, "String representation of TenorTimeSpan.Null"));
            }
            sm_NullTenorIdentifierStringRepresentation = new IdentifierString(nullStringRepresentation);
        }

        /// <summary>Converts a <see cref="System.String"/> into its <see cref="TenorTimeSpan"/> representation.
        /// </summary>
        /// <param name="tenorString">The tenor string.</param>
        /// <returns>The <see cref="TenorTimeSpan"/> object which <see cref="System.String"/> representation is equal to <paramref name="tenorString"/>.</returns>
        public static TenorTimeSpan Parse(string tenorString)
        {
            return new TenorTimeSpan(tenorString);
        }

        /// <summary>Converts a string given in a tenor format, i.e. "6m", "1y6m" but also "TN", "ON" etc. into its <see cref="TenorTimeSpan"/> representation.
        /// </summary>
        /// <param name="tenorString">The tenor string.</param>
        /// <param name="tenorTimeSpan">The tenor time span (output).</param>
        /// <returns><c>true</c> if the output <paramref name="tenorTimeSpan"/> is valid; <c>false</c> otherwise.</returns>
        public static bool TryParse(string tenorString, out TenorTimeSpan tenorTimeSpan)
        {
            int years, months, days;
            TenorType tenorType;

            if (TryParse(tenorString, out years, out months, out days, out tenorType) == true)
            {
                if (tenorType == TenorType.RegularTenor)
                {
                    // normalize the number of months and years:
                    years += months / 12;  // the sign '+' is correct in any case
                    months = months % 12;

                    tenorTimeSpan = new TenorTimeSpan(years, months, days);
                }
                else if (tenorType == TenorType.Overnight)
                {
                    tenorTimeSpan = OvernightTenor;
                }
                else
                {
                    tenorTimeSpan = TomorrowNextTenor;
                }
                return true;
            }
            tenorTimeSpan = TenorTimeSpan.Null;
            return false;
        }

        /// <summary>Creates a new instance of the <see cref="TenorTimeSpan"/> struct.
        /// </summary>
        /// <param name="tenorString">The tenor string.</param>
        /// <returns>The <see cref="TenorTimeSpan"/> object which <see cref="System.String"/> representation is equal to <paramref name="tenorString"/>.</returns>
        /// <remarks>This method is identical to <see cref="TenorTimeSpan.Parse(string)"/>.</remarks>
        public static TenorTimeSpan Create(string tenorString)
        {
            return Parse(tenorString);
        }

        /// <summary>Creates a new instance of the <see cref="TenorTimeSpan"/> struct.
        /// </summary>
        /// <param name="years">The years.</param>
        /// <param name="months">The months.</param>
        /// <param name="days">The days.</param>
        /// <returns>The specified <see cref="TenorTimeSpan"/> object.</returns>
        /// <remarks><see cref="TenorType"/> is set to <see cref="Dodoni.Finance.TenorType.RegularTenor"/>.</remarks>
        /// <exception cref="ArgumentException">Thrown, if some argument is positiv and another is negative.</exception>
        public static TenorTimeSpan Create(int years, int months, int days)
        {
            return new TenorTimeSpan(years, months, days);
        }

        /// <summary>Determines whether the specified tenor time span has a length of <c>0</c>, is the null tenor, i.e. <see cref="TenorTimeSpan.Years"/>=<see cref="TenorTimeSpan.Months"/>=<see cref="TenorTimeSpan.Days"/>=0 and <see cref="TenorTimeSpan.TenorType"/>=<see cref="Dodoni.Finance.TenorType.RegularTenor"/>.
        /// </summary>
        /// <param name="tenorTimeSpan">The tenor time span.</param>
        /// <returns><c>true</c> if the specified tenor time span has a lenght of <c>0</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(TenorTimeSpan tenorTimeSpan)
        {
            return ((tenorTimeSpan.Years == 0) && (tenorTimeSpan.Months == 0) && (tenorTimeSpan.Days == 0) && (tenorTimeSpan.TenorType == TenorType.RegularTenor));
        }

        /// <summary>Gets a <see cref="TenorTimeSpan"/> representation of a specific time span.
        /// </summary>
        /// <param name="startDate">The start date of the time span.</param>
        /// <param name="endDate">The end date of the time span.</param>
        /// <param name="roundingRule">A rounding rule to take into account. Often one is interest in a year and month representation only.</param>
        /// <returns>A <see cref="TenorTimeSpan"/> representation of the time span between <paramref name="startDate"/> and <paramref name="endDate"/> where
        /// years and months are taken into account only.</returns>
        public static TenorTimeSpan GetTimeSpanInBetween(DateTime startDate, DateTime endDate, TenorTimeSpan.RoundingRule roundingRule = RoundingRule.Exact)
        {
            int sign = (endDate >= startDate) ? 1 : -1;

            /* 1.) compute the number of years, i.e. adjust the start date: */
            int numberOfYears = endDate.Year - startDate.Year;

            var tempStartDate = startDate.AddYears(numberOfYears);

            if ((sign == 1) && (tempStartDate > endDate))
            {
                numberOfYears = numberOfYears - 1;
            }
            else if ((sign == -1) && (tempStartDate < endDate))
            {
                numberOfYears = numberOfYears + 1;
            }
            startDate = startDate.AddYears(numberOfYears);

            /* 2.) compute the number of months, i.e. adjust the start date: */
            int numberOfMonths = endDate.Month - startDate.Month;

            if (sign * startDate.Month > sign * endDate.Month)
            {
                numberOfMonths += sign * 12;
            }

            tempStartDate = startDate.AddMonths(numberOfMonths);
            if ((sign == 1) && (tempStartDate > endDate))
            {
                numberOfMonths = Math.Max(0, numberOfMonths - 1);
            }
            else if ((sign == -1) && (tempStartDate < endDate))
            {
                numberOfMonths = Math.Min(12, numberOfMonths + 1);
            }
            startDate = startDate.AddMonths(numberOfMonths);


            /* 3.) compute the number of days and apply the 'rounding rule': */
            int numberOfDays = endDate.Subtract(startDate).Days;

            switch (roundingRule)
            {
                case RoundingRule.NearestMonth:
                    if (Math.Abs(numberOfDays) > 15)
                    {
                        return new TenorTimeSpan(numberOfYears, numberOfMonths + sign, 0);
                    }
                    return new TenorTimeSpan(numberOfYears, numberOfMonths, 0);

                case RoundingRule.Exact:
                    return new TenorTimeSpan(numberOfYears, numberOfMonths, numberOfDays);

                case RoundingRule.NearestWeek:
                    int r = Math.Abs(numberOfDays) % 7;

                    if (r <= 3)
                    {
                        return new TenorTimeSpan(numberOfYears, numberOfMonths, 7 * (numberOfDays / 7));
                    }
                    else
                    {
                        return new TenorTimeSpan(numberOfYears, numberOfMonths, 7 * (numberOfDays / 7) + sign * 7);
                    }
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region internal (static) methods

        /// <summary>Converts a string given in a tenor format, i.e. "6m", "1y6m" etc. to a number of
        /// years, months and days.
        /// </summary>
        /// <param name="tenorString">The tenor string.</param>
        /// <param name="years">The number of years (output).</param>
        /// <param name="months">The number of months (output).</param>
        /// <param name="days">The number of days (output).</param>
        /// <param name="tenorType">The type of the tenor (output).</param>
        /// <returns>Returns an instance of <see cref="TenorTimeSpan"/> with the number of years,
        /// months and days according to <paramref name="tenorString"/> or null if
        /// the given string could not be converted.</returns>
        /// <remarks>Also overnight or tomorrow next will be converted from a string.</remarks>
        /// <exception cref=" ArgumentNullException">Thrown, if <paramref name="tenorString"/> is <c>null</c>.</exception>
        internal static bool TryParse(string tenorString, out int years, out int months, out int days, out TenorType tenorType)
        {
            years = months = days = 0;
            tenorType = TenorType.RegularTenor;

            if (tenorString == null)
            {
                throw new ArgumentNullException("tenorString");
            }
            tenorString = tenorString.Replace(" ", String.Empty);

            if (((tenorString.Length == 1) && (tenorString[0] == '0')) || (tenorString.ToIDString() == sm_NullTenorIdentifierStringRepresentation.IDString)) // special: '0' and "<null>" will be interpreted as 'null'
            {
                years = months = days = 0;
                tenorType = TenorType.RegularTenor;
                return true;
            }

            if (sm_TenorRegularExpression.IsMatch(tenorString))
            {
                int sign = 1;
                MatchCollection matchCollection;

                if (tenorString[0] == '-')
                {
                    sign = -1;
                    matchCollection = sm_TenorSubRegularExpression.Matches(tenorString, 1);
                }
                else
                {
                    matchCollection = sm_TenorSubRegularExpression.Matches(tenorString);
                }

                foreach (Match match in matchCollection)
                {
                    /* mach is of the form '5y', '7m' etc., i.e. the last character is the unitiy:*/
                    char unityAsChar = match.Value[match.Length - 1];
                    int number = Int32.Parse(match.Value.Remove(match.Length - 1));

                    switch (unityAsChar)
                    {
                        case 'Y':
                        case 'y':
                            years += sign * number;
                            break;
                        case 'M':
                        case 'm':
                            months += sign * number;
                            break;
                        case 'W':
                        case 'w':
                            days += sign * 7 * number;
                            break;
                        case 'D':
                        case 'd':
                            days += sign * number;
                            break;
                    }
                }
                return true;
            }
            else   /* maybe it is overnight or tomorrow next */
            {
                days = 1;

                tenorString = tenorString.ToUpper();
                if (tenorString == OvernightTenorStringRepresentation)
                {
                    tenorType = TenorType.Overnight;
                    return true;
                }
                else if (tenorString == TomorrowNextTenorStringRepresentation)
                {
                    tenorType = TenorType.TomorrowNext;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}