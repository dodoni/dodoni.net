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
namespace Dodoni.Finance.StandardModels
{
    /// <summary>Serves as interface for a specific option with a single underlying which is given with respect
    /// to a Black-like model, i.e. Black-Scholes model, Bachelier (=Normal Black) model etc., thus assume 
    /// a constant volatility, constant interest rate etc.
    /// </summary>
    public interface IConstantVolatilityStandardOption
    {
        /// <summary>Gets the type of the option.
        /// </summary>
        /// <value>The type of the option.</value>
        OptionType OptionType
        {
            get;
        }

        /// <summary>Gets or sets the time to expiry, i.e. time span between valuation date and expiry date 
        /// in its <see cref="System.Double"/> representation.
        /// </summary>
        /// <value>The time to expiry.</value>
        double TimeToExpiry
        {
            get;
            set;
        }

        /// <summary>Gets the price of the option.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The value of the option.</returns>
        double GetValue(double volatility);

        /// <summary>Gets the implied volatility for a specific option price.
        /// </summary>
        /// <param name="optionValue">The value of the option.</param>
        /// <param name="impliedVolatility">The implied volatility (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedVolatility"/> contains valid data.</returns>
        /// <remarks>This method is the inverse function of <see cref="IConstantVolatilityStandardOption.GetValue(double)"/>.</remarks>
        ImpliedCalculationResultState TryGetImpliedVolatility(double optionValue, out double impliedVolatility);

        /// <summary>Gets the vega of the option, i.e. the partial derivative of the option value formula with respect to the volatility.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The vega (also called lambda) of the option, i.e. the partial derivative of the option value formula with respect to the volatility.</returns>
        double GetVega(double volatility);

        /// <summary>Gets the volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The volga of the option, i.e. the second partial derivative of the option value formula with respect to the volatility.</returns>
        double GetVolga(double volatility);

        /// <summary>Gets the theta of the option, i.e. the partial derivative of the option value formula with respect to the time to maturity.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The theta of the option, i.e. the partial derivative of the option value formula with respect to the time to expiry.</returns>
        /// <remarks>Attention: The discount factor is assumed to be a constant and not time dependent, i.e. <b>not</b> of the form P(0,t)=exp(-r * t) etc.
        /// Therefore the discount factor will not be taken into account for the partial derivative with respect to the time to maturity.</remarks>
        double GetTheta(double volatility);
    }
}