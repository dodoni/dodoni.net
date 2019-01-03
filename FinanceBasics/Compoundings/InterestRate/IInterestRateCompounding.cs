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
using Dodoni.BasicComponents;

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for a interest compounding, for example 'Notional' * (1.0 + r)^t for an annually compounded interest etc.
    /// </summary>
    public interface IInterestRateCompounding : IIdentifierNameable, IAnnotatable
    {
        /// <summary>Gets the (normalized) interest amount.
        /// </summary>
        /// <param name="interestRate">The interest rate 'r'.</param>
        /// <param name="interestPeriodLength">The length of the interest period 't'.</param>
        /// <returns>The normalized interest amount, for example (1.0 + r)^t in the case of an annually compounded interest.</returns>
        double GetInterestAmount(double interestRate, double interestPeriodLength);

        /// <summary>Gets a interest rate with respect to a specific (normalized) interest amount.
        /// </summary>
        /// <param name="interestAmount">The (normalized) interest amount.</param>
        /// <param name="interestPeriodLength">The length of the interest period.</param>
        /// <returns>The interest rate such that <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/> returns <paramref name="interestAmount"/>.</returns>
        /// <remarks>This method is the inverse operation of <see cref="IInterestRateCompounding.GetInterestAmount(double, double)"/>.</remarks>
        double GetImpliedInterestRate(double interestAmount, double interestPeriodLength);
    }
}