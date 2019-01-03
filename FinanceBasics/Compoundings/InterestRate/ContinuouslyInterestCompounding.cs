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

using Dodoni.MathLibrary;
using Dodoni.BasicComponents;

namespace Dodoni.Finance.Compoundings
{
    /// <summary>Represents continuously compounded interest, i.e. 'Notional' * exp( t * r).
    /// </summary>
    public class ContinuouslyInterestCompounding : IInterestRateCompounding
    {
        #region private members

        /// <summary>The name of the compounding.
        /// </summary>
        private static readonly IdentifierString sm_Name = new IdentifierString("Continuously");
        #endregion

        #region protected internal constructors

        /// <summary>Initializes a new instance of the <see cref="ContinuouslyInterestCompounding"/> class.
        /// </summary>
        protected internal ContinuouslyInterestCompounding()
        {
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
            get { return sm_Name; }
        }

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.
        /// </value>
        public IdentifierString Name
        {
            get { return sm_Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IAnnotatable.HasReadOnlyAnnotation
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
            return Math.Exp(interestRate * interestPeriodLength);
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
            return Math.Log(interestAmount) / interestPeriodLength;
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return CompoundingResources.InterestContinuously; }
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
            return sm_Name.String;
        }
        #endregion
    }
}