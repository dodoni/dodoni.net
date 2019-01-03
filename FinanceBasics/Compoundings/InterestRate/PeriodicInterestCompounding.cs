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

using Dodoni.MathLibrary;
using Dodoni.BasicComponents;
using Dodoni.Finance.DateFactory;

namespace Dodoni.Finance.Compoundings
{
    /// <summary>Represents a periodic interest compounding, i.e. 'Notional' * ( 1.0 + r/n )^{t * n}, where n is the number of payments per year, for example (semi-)annually compounding etc.
    /// </summary>
    public class PeriodicInterestCompounding : IInterestRateCompounding
    {
        #region public (readonly) members

        /// <summary>The frequency of the compounding.
        /// </summary>
        public readonly IDateScheduleFrequency Frequency;

        /// <summary>The number of payments per year.
        /// </summary>
        public readonly double PaymentsPerYear;
        #endregion

        #region protected internal constructors

        /// <summary>Initializes a new instance of the <see cref="PeriodicInterestCompounding"/> class.
        /// </summary>
        /// <param name="frequency">The frequency of the compounding.</param>
        protected internal PeriodicInterestCompounding(IDateScheduleFrequency frequency)
        {
            if (frequency == null)
            {
                throw new ArgumentNullException("frequency");
            }
            Frequency = frequency;

            TenorTimeSpan frequencyTenor = frequency.GetFrequencyTenor();
            if (TenorTimeSpan.IsNull(frequencyTenor) == true)
            {
                throw new ArgumentException("frequency");
            }

            if (frequencyTenor.Years != 0)
            {
                PaymentsPerYear = 1.0 / frequencyTenor.Years;
            }
            if (frequencyTenor.Months != 0)
            {
                PaymentsPerYear += 12.0 / frequencyTenor.Months;
            }
            if (frequencyTenor.Days != 0)
            {
                PaymentsPerYear += 365.0 / frequencyTenor.Days;
            }
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Member

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.
        /// </value>
        public IdentifierString LongName
        {
            get { return Frequency.LongName; }
        }

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.
        /// </value>
        public IdentifierString Name
        {
            get { return Frequency.Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        public bool HasReadOnlyAnnotation
        {
            get { return true; }
        }
        #endregion

        #endregion

        #region public methods

        #region IInterestRateCompounding Members

        /// <summary>Gets the (normalized) interest amount.
        /// </summary>
        /// <param name="interestRate">The interest rate 'r'.</param>
        /// <param name="interestPeriodLength">The length of the interest period 't'.</param>
        /// <returns>The normalized interest amount, for example (1.0 + r)^t in the case of an annually compounded interest.
        /// </returns>
        public double GetInterestAmount(double interestRate, double interestPeriodLength)
        {
            return Math.Pow(1.0 + interestRate / PaymentsPerYear, interestPeriodLength * PaymentsPerYear);
        }

        /// <summary>Gets a interest rate with respect to a specific (normalized) interest amount.
        /// </summary>
        /// <param name="interestAmount">The (normalized) interest amount.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate such that <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/> returns <paramref name="interestAmount"/>.</returns>
        /// <remarks>This method is the inverse operation of <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/>.</remarks>
        public double GetImpliedInterestRate(double interestAmount, double interestPeriodLength)
        {
            if (interestAmount <= 0.0)
            {
                throw new ArgumentException("interestAmount");
            }
            if (Math.Abs(interestPeriodLength) < MachineConsts.TinyEpsilon)
            {
                throw new ArgumentException("interestPeriodLength");
            }
            return PaymentsPerYear * (Math.Exp(Math.Log(interestAmount) / (PaymentsPerYear * interestPeriodLength)) - 1.0);
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return String.Format(CompoundingResources.InterestPeriodic, PaymentsPerYear); }
        }

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
        /// </returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Frequency.Name.String;
        }
        #endregion
    }
}