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

namespace Dodoni.Finance.StandardModels
{
    /// <summary>Serves as pricer of a European (plain vanilla) option.
    /// </summary>
    public interface IEuropeanOptionPricer : IOperable
    {
        /// <summary>Gets or sets the time to expiry, i.e. the span between valuation date and expiry date in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <value>The time to expiry.</value>
        double TimeToExpiry
        {
            get;
            set;
        }

        /// <summary>Gets or sets the forward at <see cref="TimeToExpiry"/>.
        /// </summary>
        /// <value>The forward at <see cref="TimeToExpiry"/>.</value>
        double Forward
        {
            get;
            set;
        }

        /// <summary>Reinitialize the current instance.
        /// </summary>
        /// <remarks>Call this method before calling <see cref="IEuropeanOptionPricer.GetNoneDiscountedValue(double)"/>, i.e. after model parameters  have been changed and before query option prices.</remarks>
        void ReInitialize();

        /// <summary>Gets the price of the option <c>at time of expiry</c>, i.e. not discounted.
        /// </summary>
        /// <param name="strike">The strike of the option.</param>
        /// <returns>The value of the option <c>at the time of expiry</c>, thus not discounted. To get the price just multiply the return value with the discount factor.</returns>
        double GetNoneDiscountedValue(double strike);
    }
}