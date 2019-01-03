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
    /// <summary>Represents the continuously compounded zero rate compounding, i.e. P(t,T) = exp( -(T-t) * r).
    /// </summary>
    public class ContinuouslyZeroRateCompounding : IZeroRateCompounding
    {
        #region private members

        /// <summary>The name of the compounding.
        /// </summary>
        private IdentifierString m_Name = new IdentifierString("Continuously Zero rate");
        #endregion

        #region protected internal constructors

        /// <summary>Initializes a new instance of the <see cref="ContinuouslyZeroRateCompounding"/> class.
        /// </summary>
        protected internal ContinuouslyZeroRateCompounding()
        {
        }
        #endregion

        #region public properties

        #region IZeroRateCompounding Member

        /// <summary>Gets the minimal time-to-maturity, i.e. a function call of <see cref="GetZeroRate(double, double)"/> with a <c>time-to-maturity</c> argument which is smaller
        /// than this number will throw an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <value>The minimal <c>time-to-maturity</c> argument.
        /// </value>
        public double MinimalTimeToMaturity
        {
            get { return MachineConsts.TinyEpsilon; }
        }
        #endregion

        #region IIdentifierNameable Member

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.
        /// </value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.
        /// </value>
        public IdentifierString Name
        {
            get { return m_Name; }
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

        #region IZeroRateCompounding Member

        /// <summary>Gets an equivalent zero rate representation of a specific zero-coupon bond.
        /// </summary>
        /// <param name="zeroCouponPrice">The value P(t,T) of a specific zero-coupon bond with time to maturity T-t.</param>
        /// <param name="timeToMaturity">The time to maturity of the zero-coupon bond.</param>
        /// <returns>An equivalent zero rate representation of <paramref name="zeroCouponPrice"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="timeToMaturity"/> is less than <see cref="IZeroRateCompounding.MinimalTimeToMaturity"/>.</exception>
        public double GetZeroRate(double zeroCouponPrice, double timeToMaturity)
        {
            if (zeroCouponPrice <= 0.0)
            {
                throw new ArgumentOutOfRangeException("zeroCouponPrice");
            }
            if (Math.Abs(timeToMaturity) < MachineConsts.TinyEpsilon)
            {
                throw new ArgumentOutOfRangeException("timeToMaturity", String.Format(ExceptionMessages.ArgumentOutOfRangeGreater, "Time-to-maturity", 0.0));
            }
            return -Math.Log(zeroCouponPrice) / timeToMaturity;
        }

        /// <summary>Gets a zero-coupon bond price with respect to a specific zero rate.
        /// </summary>
        /// <param name="zeroRate">The zero rate representation, compounded with respect to the current compounding rule.</param>
        /// <param name="timeToMaturity">The time to maturity of the zero-coupon bond.</param>
        /// <returns>
        /// A zero-coupon bond price P(t,T) which equivalent zero rate representation is equal to <paramref name="zeroRate"/>; the maturity <c>T</c> as well as the reference date <c>t</c>
        /// of the zero-coupon bond is undefined, but the time-to-maturity T-t is known.
        /// </returns>
        /// <remarks>This method is the inverse operation of <see cref="IZeroRateCompounding.GetZeroRate(double, double)"/>, i.e. applying the
        /// output of <see cref="IZeroRateCompounding.GetZeroRate(double, double)"/> to this method should return the original discount factor input.</remarks>
        /// <exception cref="ArgumentException">Thrown, if there is no zero-coupon bond price available with equivalent zero rate <paramref name="zeroRate"/>.</exception>
        public double GetZeroCouponBondPrice(double zeroRate, double timeToMaturity)
        {
            return Math.Exp(-timeToMaturity * zeroRate);
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return CompoundingResources.ZeroRateContinuously; }
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
            return "Continuously";
        }
        #endregion
    }
}