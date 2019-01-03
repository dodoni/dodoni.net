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

using Dodoni.BasicComponents;

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for the calculation of a specific zero-coupon zero rate representation and vice a versa.
    /// </summary>
    public interface IZeroRateCompounding : IIdentifierNameable, IAnnotatable
    {
        /// <summary>Gets the minimal time-to-maturity, i.e. a function call of <see cref="GetZeroRate(double, double)"/> with a <c>time-to-maturity</c> argument which is smaller 
        /// than this number will throw an <see cref="ArgumentOutOfRangeException"/>. 
        /// </summary>
        /// <value>The minimal <c>time-to-maturity</c> argument.</value>
        double MinimalTimeToMaturity
        {
            get;
        }

        /// <summary>Gets an equivalent zero rate representation of a specific zero-coupon bond.
        /// </summary>
        /// <param name="zeroCouponPrice">The value P(t,T) of a specific zero-coupon bond with time to maturity T-t.</param>
        /// <param name="timeToMaturity">The time to maturity of the zero-coupon bond.</param>
        /// <returns>An equivalent zero rate representation of <paramref name="zeroCouponPrice"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown, if <paramref name="timeToMaturity"/> is less than <see cref="IZeroRateCompounding.MinimalTimeToMaturity"/>.</exception>
        double GetZeroRate(double zeroCouponPrice, double timeToMaturity);        

        /// <summary>Gets a zero-coupon bond price with respect to a specific zero rate.
        /// </summary>
        /// <param name="zeroRate">The zero rate representation, compounded with respect to the current compounding rule.</param>
        /// <param name="timeToMaturity">The time to maturity of the zero-coupon bond.</param>
        /// <returns>A zero-coupon bond price P(t,T) which equivalent zero rate representation is equal to <paramref name="zeroRate"/>; the maturity <c>T</c> as well as the reference date <c>t</c>
        /// of the zero-coupon bond is undefined, but the time-to-maturity T-t is known.</returns>
        /// <remarks>This method is the inverse operation of <see cref="IZeroRateCompounding.GetZeroRate(double, double)"/>, i.e. applying the
        /// output of <see cref="IZeroRateCompounding.GetZeroRate(double, double)"/> to this method should return the original discount factor input.</remarks>
        /// <exception cref="ArgumentException">Thrown, if there is no zero-coupon bond price available with equivalent zero rate <paramref name="zeroRate"/>.</exception>
        double GetZeroCouponBondPrice(double zeroRate, double timeToMaturity);
    }
}