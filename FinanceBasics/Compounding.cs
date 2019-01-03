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

using Dodoni.Finance.Compoundings;

namespace Dodoni.Finance
{
    /// <summary>Serves as factory for compounding conventions.
    /// </summary>
    public static class Compounding
    {
        #region public static (readonly) members

        /// <summary>Zero rate compounding conventions, i.e. for zero-coupon bond prices P(t,T).
        /// </summary>
        public static ZeroRateCompounding ZeroRate = new ZeroRateCompounding();

        /// <summary>Compounding conventions for interest rates.
        /// </summary>
        public static InterestRateCompounding InterestRate = new InterestRateCompounding();
        #endregion

        #region public (static) methods

        /// <summary>Gets a zero rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="zeroRate">The zero rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="zeroRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="timeToMaturity">The time to maturity of the underlying zero-coupon bond.</param>
        /// <returns>The zero rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        /// <remarks>Internally first the zero-coupon bond price will be computed with respect to <paramref name="sourceCompounding"/> afterwards
        /// the zero rate will be computed with respect to <paramref name="destinationCompounding"/>. Therefore it may cause some overhead. In
        /// time critical situations one may use a type safed overload of this method instead.</remarks>
        public static double GetConvertedZeroRate(double zeroRate, IZeroRateCompounding sourceCompounding, IZeroRateCompounding destinationCompounding, double timeToMaturity = 1.0)
        {
            if (sourceCompounding == null)
            {
                throw new ArgumentNullException("sourceCompounding");
            }
            double zeroCouponPrice = sourceCompounding.GetZeroCouponBondPrice(zeroRate, timeToMaturity);

            if (destinationCompounding == null)
            {
                throw new ArgumentNullException("destinationCompounding");
            }
            return destinationCompounding.GetZeroRate(zeroCouponPrice, timeToMaturity);
        }

        /// <summary>Gets a zero rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="zeroRate">The zero rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="zeroRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="timeToMaturity">The time to maturity of the underlying zero-coupon bond.</param>
        /// <returns>The zero rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedZeroRate(double zeroRate, ContinuouslyZeroRateCompounding sourceCompounding, PeriodicZeroRateCompounding destinationCompounding, double timeToMaturity = 1.0)
        {
            if (sourceCompounding == null)
            {
                throw new ArgumentNullException("sourceCompounding");
            }
            if (destinationCompounding == null)
            {
                throw new ArgumentNullException("destinationCompounding");
            }
            return (Math.Exp(zeroRate / destinationCompounding.PaymentsPerYear) - 1.0) * destinationCompounding.PaymentsPerYear;
        }

        /// <summary>Gets a zero rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="zeroRate">The zero rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="zeroRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="timeToMaturity">The time to maturity of the underlying zero-coupon bond.</param>
        /// <returns>The zero rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedZeroRate(double zeroRate, PeriodicZeroRateCompounding sourceCompounding, ContinuouslyZeroRateCompounding destinationCompounding, double timeToMaturity = 1.0)
        {
            if (sourceCompounding == null)
            {
                throw new ArgumentNullException("sourceCompounding");
            }
            if (destinationCompounding == null)
            {
                throw new ArgumentNullException("destinationCompounding");
            }
            return sourceCompounding.PaymentsPerYear * Math.Log(1.0 + zeroRate / sourceCompounding.PaymentsPerYear);
        }

        /// <summary>Gets a interest rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="interestRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        /// <remarks>Internally first the normalized interest will be computed with respect to <paramref name="sourceCompounding"/>. Afterwards
        /// the interest rate will be computed with respect to <paramref name="destinationCompounding"/>. Therefore it may cause some overhead. In
        /// time critical situations one may use a type safed overload of this method instead.</remarks>
        public static double GetConvertedInterestRate(double interestRate, IInterestRateCompounding sourceCompounding, IInterestRateCompounding destinationCompounding, double interestPeriodLength)
        {
            if (sourceCompounding == null)
            {
                throw new ArgumentNullException("sourceCompounding");
            }
            double normalizedInterest = sourceCompounding.GetInterestAmount(interestRate, interestPeriodLength);

            if (destinationCompounding == null)
            {
                throw new ArgumentNullException("destinationCompounding");
            }
            return destinationCompounding.GetImpliedInterestRate(normalizedInterest, interestPeriodLength);
        }

        /// <summary>Gets a interest rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="interestRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedInterestRate(double interestRate, ContinuouslyInterestCompounding sourceCompounding, PeriodicInterestCompounding destinationCompounding, double interestPeriodLength)
        {
            if (destinationCompounding == null)
            {
                throw new ArgumentNullException("destinationCompounding");
            }
            return (Math.Exp(interestRate / destinationCompounding.PaymentsPerYear) - 1.0) * destinationCompounding.PaymentsPerYear;
        }

        /// <summary>Gets a interest rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="interestRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedInterestRate(double interestRate, ContinuouslyInterestCompounding sourceCompounding, SimpleInterestCompounding destinationCompounding, double interestPeriodLength)
        {
            return (Math.Exp(interestRate * interestPeriodLength) - 1.0) / interestPeriodLength;
        }

        /// <summary>Gets a interest rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="interestRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedInterestRate(double interestRate, PeriodicInterestCompounding sourceCompounding, ContinuouslyInterestCompounding destinationCompounding, double interestPeriodLength)
        {
            if (sourceCompounding == null)
            {
                throw new ArgumentNullException("sourceCompounding");
            }
            return sourceCompounding.PaymentsPerYear * Math.Log(1.0 + interestRate / sourceCompounding.PaymentsPerYear);
        }

        /// <summary>Gets a interest rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="interestRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedInterestRate(double interestRate, PeriodicInterestCompounding sourceCompounding, SimpleInterestCompounding destinationCompounding, double interestPeriodLength)
        {
            if (sourceCompounding == null)
            {
                throw new ArgumentNullException("sourceCompounding");
            }
            return (Math.Pow(1.0 + interestRate / sourceCompounding.PaymentsPerYear, interestPeriodLength * sourceCompounding.PaymentsPerYear) - 1.0) / interestPeriodLength;
        }

        /// <summary>Gets a interest rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="interestRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedInterestRate(double interestRate, SimpleInterestCompounding sourceCompounding, ContinuouslyInterestCompounding destinationCompounding, double interestPeriodLength)
        {
            return Math.Log(1.0 + interestRate * interestPeriodLength) / interestPeriodLength;
        }

        /// <summary>Gets a interest rate with respect to a specific compounding convention.
        /// </summary>
        /// <param name="interestRate">The interest rate.</param>
        /// <param name="sourceCompounding">The compounding convention of the input <paramref name="interestRate"/>.</param>
        /// <param name="destinationCompounding">The compounding convention to convert to.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate in its <paramref name="destinationCompounding"/> compounding convention.</returns>
        public static double GetConvertedInterestRate(double interestRate, SimpleInterestCompounding sourceCompounding, PeriodicInterestCompounding destinationCompounding, double interestPeriodLength)
        {
            if (destinationCompounding == null)
            {
                throw new ArgumentNullException("destinationCompounding");
            }
            return (Math.Exp(Math.Log(1.0 + interestRate * interestPeriodLength) / (interestPeriodLength * destinationCompounding.PaymentsPerYear)) - 1.0) * destinationCompounding.PaymentsPerYear;
        }
        #endregion
    }
}