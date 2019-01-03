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
    /// <summary>Serves as interface for an european option that assumes a strike and where the underlying 
    /// is specified by a Black-like model, i.e. Black-Scholes, Bachelier (=Normal Black) etc.
    /// </summary>
    public interface IConstantVolatilityStandardEuropeanOption : IConstantVolatilityStandardOption
    {
        /// <summary>Gets or sets the strike of the option.
        /// </summary>
        /// <value>The strike.</value>
        double Strike
        {
            get;
            set;
        }

        /// <summary>Gets or sets the forward at <see cref="IConstantVolatilityStandardOption.TimeToExpiry"/>.
        /// </summary>
        /// <value>The forward at <see cref="IConstantVolatilityStandardOption.TimeToExpiry"/>.</value>
        double Forward
        {
            get;
            set;
        }

        /// <summary>Gets or sets the discount factor at <see cref="IConstantVolatilityStandardOption.TimeToExpiry"/>.
        /// </summary>
        /// <value>The discount factor at <see cref="IConstantVolatilityStandardOption.TimeToExpiry"/>.</value>
        double DiscountFactor
        {
            get;
            set;
        }

        /// <summary>Gets the price of the option <c>at time of expiry</c>, i.e. not discounted.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The value of the option <c>at the time of expiry</c>, thus not discounted. To get 
        /// the price just multiply the return value with the discount factor.</returns>
        double GetNoneDiscountedValue(double volatility);

        /// <summary>Gets the intrinsic value of the option.
        /// </summary>
        /// <returns>The intrisic value of the option.</returns>
        double GetIntrinsicValue();

        /// <summary>Gets the implied volatility for a specific non-discounted option price.
        /// </summary>
        /// <param name="noneDiscountedValue">The value of the option at the time of expiry, thus the price but <b>not</b> discounted to time 0.</param>
        /// <param name="impliedVolatility">The implied volatility (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedVolatility"/> contains valid data.</returns>
        /// <remarks>This method is the inverse function of <see cref="IConstantVolatilityStandardEuropeanOption.GetNoneDiscountedValue(double)"/>.</remarks>
        ImpliedCalculationResultState TryGetImpliedVolatilityOfNonDiscountedValue(double noneDiscountedValue, out double impliedVolatility);

        /// <summary>Gets the implied strike for a specific option price.
        /// </summary>
        /// <param name="optionValue">The value of the option.</param>
        /// <param name="volatility">The volatility.</param>
        /// <param name="impliedStrike">The implied strike (output).</param>
        /// <returns>A value indicating whether <paramref name="impliedStrike"/> contains valid data.</returns>
        ImpliedCalculationResultState TryGetImpliedStrike(double optionValue, double volatility, out double impliedStrike);

        /// <summary>Gets the forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward. 
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-delta of the option, i.e. the partial derivative of the option value formula with respect to the forward.</returns> 
        double GetForwardDelta(double volatility);

        /// <summary>Gets the forward-gamma of the option, i.e. the second partial derivative of the option value formula with respect to the forward.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The forward-gamma of the option, i.e. the second partial derivative of the option value 
        /// formula with respect to the forward.</returns>
        /// <remarks>The initial value of the underlying is equal to the forward times the discount factor.</remarks>
        double GetForwardGamma(double volatility);

        /// <summary>Gets the (forward-)vanna of the option, i.e. the partial derivative of the option value formual with respect 
        /// to the forward and with respect to the volatility, i.e. '\partial\sigma \partial F'.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The (forward-)vanna of the option, i.e. the partial derivative of the option value formual with respect 
        /// to the forward and with respect to the volatility, i.e. '\partial\sigma \partial F'.</returns>
        double GetForwardVanna(double volatility);

        /// <summary>Gets the strike-delta of the option, i.e. the partial derivative of the option value formula with 
        /// respect to the strike.
        /// </summary>
        /// <param name="volatility">The volatility.</param>
        /// <returns>The strike-delta of the option, i.e. the partial derivative of the option value formula with 
        /// respect to the strike.</returns>
        double GetStrikeDelta(double volatility);
    }
}