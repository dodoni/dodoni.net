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
    /// <summary>Represents the interest compounding which is identical to <see cref="SimpleInterestCompounding"/> up to time to maturity 1Y times number of payments per year; afterwards identical to <see cref="PeriodicInterestCompounding"/>.
    /// </summary>
    public class SimpleThenPeriodicCompounding : IInterestRateCompounding
    {
        #region public (readonly) members

        /// <summary>The frequency of the compounding.
        /// </summary>
        public readonly IDateScheduleFrequency Frequency;

        /// <summary>The number of payments per year.
        /// </summary>
        public readonly double PaymentsPerYear;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="SimpleThenPeriodicCompounding" /> class.
        /// </summary>
        /// <param name="frequency">The frequency of the compounding to apply after time-to-maturity of one year.</param>
        internal SimpleThenPeriodicCompounding(IDateScheduleFrequency frequency)
        {
            if (frequency == null)
            {
                throw new ArgumentNullException("frequency");
            }
            Frequency = frequency;

            var frequencyTenor = frequency.GetFrequencyTenor();
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
            Name = new IdentifierString(String.Format("Simple;{0}", Frequency.Name.String));
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return String.Format(CompoundingResources.InterestSimpleThenPeriodic, PaymentsPerYear); }
        }

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
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
        /// <returns>The normalized interest amount, for example (1.0 + r)^t in the case of an annually compounded interest.</returns>
        public double GetInterestAmount(double interestRate, double interestPeriodLength)
        {
            if (interestPeriodLength * PaymentsPerYear <= 1.0)
            {
                return 1.0 + interestRate * interestPeriodLength;
            }
            else
            {
                return Math.Pow(1.0 + interestRate / PaymentsPerYear, interestPeriodLength * PaymentsPerYear);
            }
        }

        /// <summary>Gets a interest rate with respect to a specific (normalized) interest amount.
        /// </summary>
        /// <param name="interestAmount">The (normalized) interest amount.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate such that <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/> returns <paramref name="interestAmount"/>.</returns>
        /// <remarks>This method is the inverse operation of <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/>.</remarks>
        public double GetImpliedInterestRate(double interestAmount, double interestPeriodLength)
        {
            if (interestPeriodLength * PaymentsPerYear <= 1.0)
            {
                if (Math.Abs(interestPeriodLength) < MachineConsts.TinyEpsilon)
                {
                    throw new ArgumentException("interestPeriodLength");
                }
                return (interestAmount - 1.0) / interestPeriodLength;
            }
            else
            {
                if (interestAmount <= 0.0)
                {
                    throw new ArgumentException("normalizedInterest");
                }
                if (Math.Abs(interestPeriodLength) < MachineConsts.TinyEpsilon)
                {
                    throw new ArgumentException("interestAmount");
                }
                return PaymentsPerYear * (Math.Exp(Math.Log(interestAmount) / (PaymentsPerYear * interestPeriodLength)) - 1.0);
            }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.</returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name.String;
        }
        #endregion
    }
}