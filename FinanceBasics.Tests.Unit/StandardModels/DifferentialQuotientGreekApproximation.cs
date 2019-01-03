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

namespace Dodoni.Finance.StandardModels
{
    /// <summary>Provides methods for the approximation of pricing derivatives (Greeks) applying a differential quotient to the pricing formula.
    /// </summary>
    public class DifferentialQuotientGreekApproximation
    {
        /// <summary>Gets the option value.
        /// </summary>
        /// <param name="strike">The strike.</param>
        /// <param name="forward">The forward.</param>
        /// <param name="timeToExpiry">The time-to-expiry.</param>
        /// <param name="discountFactor">The discount factor.</param>
        /// <param name="volatility">The volatility.</param>
        public delegate double GetOptionValue(double strike, double forward, double timeToExpiry, double discountFactor, double volatility);

        /// <summary>Gets the option forward-delta.
        /// </summary>
        /// <param name="optionParameters">The parameters of the option.</param>
        /// <param name="getOptionValue">A delegate that calculates the value of the option for specified parameters.</param>
        /// <returns>A approximation of the delta.</returns>
        public static double GetOptionForwardDelta(ConstantVolatilityStandardEuropeanOptionConfiguration optionParameters, GetOptionValue getOptionValue)
        {
            double h = 0.0000001;

            double value1 = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double value2 = getOptionValue(optionParameters.Strike, optionParameters.Forward + h, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            return (value2 - value1) / h;
        }

        /// <summary>Gets the option theta.
        /// </summary>
        /// <param name="optionParameters">The parameters of the option.</param>
        /// <param name="getOptionValue">A delegate that calculates the value of the option for specified parameters.</param>
        /// <returns>A approximation of the theta.</returns>
        public static double GetOptionTheta(ConstantVolatilityStandardEuropeanOptionConfiguration optionParameters, GetOptionValue getOptionValue)
        {
            double h = 0.0000001;

            double value1 = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double value2 = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry + h, optionParameters.DiscountFactor, optionParameters.Volatility);
            return (value2 - value1) / h;
        }

        /// <summary>Gets the option vega.
        /// </summary>
        /// <param name="optionParameters">The parameters of the option.</param>
        /// <param name="getOptionValue">A delegate that calculates the value of the option for specified parameters.</param>
        /// <returns>A approximation of the vega.</returns>
        public static double GetOptionVega(ConstantVolatilityStandardEuropeanOptionConfiguration optionParameters, GetOptionValue getOptionValue)
        {
            double h = 1E-8;

            double value1 = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility + h);
            double value2 = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);

            return (value1 - value2) / h;
        }

        /// <summary>Gets the option kappa, i.e. strike-delta.
        /// </summary>
        /// <param name="optionParameters">The parameters of the option.</param>
        /// <param name="getOptionValue">A delegate that calculates the value of the option for specified parameters.</param>
        /// <returns>A approximation of the kappa.</returns>
        public static double GetOptionKappa(ConstantVolatilityStandardEuropeanOptionConfiguration optionParameters, GetOptionValue getOptionValue)
        {
            double h = 0.0000001;

            double value1 = getOptionValue(optionParameters.Strike + h, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double value2 = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);

            return (value1 - value2) / h;
        }

        /// <summary>Gets the forward-gamma of the option, i.e. the second derivative with respect to the forward.
        /// </summary>
        /// <param name="optionParameters">The parameters of the option.</param>
        /// <param name="getOptionValue">A delegate that calculates the value of the option for specified parameters.</param>
        /// <returns>A approximation of the gamma.</returns>
        public static double GetOptionForwardGamma(ConstantVolatilityStandardEuropeanOptionConfiguration optionParameters, GetOptionValue getOptionValue)
        {
            double h = 0.00005;  // should be not too small, otherwise one get numerical problems

            double value = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double valueMinusH = getOptionValue(optionParameters.Strike, optionParameters.Forward - h, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double valueMinus2H = getOptionValue(optionParameters.Strike, optionParameters.Forward - 2.0 * h, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double valuePlusH = getOptionValue(optionParameters.Strike, optionParameters.Forward + h, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double valuePlus2H = getOptionValue(optionParameters.Strike, optionParameters.Forward + 2.0 * h, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);

            /* approximation of the second derivative: (Richardson's extrapolation)
             *   f''(x) \approx  [-f(x-2h) + 16 * f(x-h) - 30 * f(x) + 16 * f(x+h) - f(x+2h)] / (12 * h^2)
             */

            return (-valueMinus2H + 16.0 * valueMinusH - 30.0 * value + 16.0 * valuePlusH - valuePlus2H) / (12 * h * h);
        }

        /// <summary>Gets the Volga of the option, i.e. the second derivative with respect to the volatilty.
        /// </summary>
        /// <param name="optionParameters">The parameters of the option.</param>
        /// <param name="getOptionValue">A delegate that calculates the value of the option for specified parameters.</param>
        /// <returns>A approximation of the volga.</returns>
        public static double GetOptionVolga(ConstantVolatilityStandardEuropeanOptionConfiguration optionParameters, GetOptionValue getOptionValue)
        {
            double h = 0.00005;  // should be not too small, otherwise one get numerical problems

            double value = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double valueMinusH = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility - h);
            double valueMinus2H = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility - 2.0 * h);
            double valuePlusH = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility + h);
            double valuePlus2H = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility + 2.0 * h);

            /* approximation of the second derivative: (Richardson's extrapolation)
             *   f''(x) \approx  [-f(x-2h) + 16 * f(x-h) - 30 * f(x) + 16 * f(x+h) - f(x+2h)] / (12 * h^2)
             */
            return (-valueMinus2H + 16.0 * valueMinusH - 30.0 * value + 16.0 * valuePlusH - valuePlus2H) / (12 * h * h);
        }

        /// <summary>Gets the Forward-Vanna of the option, i.e. the derivative \partial^2/ {\partial \sigma \partial F} of the option value.
        /// </summary>
        /// <param name="optionParameters">The parameters of the option.</param>
        /// <param name="getOptionValue">A delegate that calculates the value of the option for specified parameters.</param>
        /// <returns>A approximation of the gamma.</returns>
        public static double GetOptionForwardVanna(ConstantVolatilityStandardEuropeanOptionConfiguration optionParameters, GetOptionValue getOptionValue)
        {
            double h = 0.00005;  // should be not too small, otherwise one get numerical problems
            double k = 0.00005;

            double value = getOptionValue(optionParameters.Strike, optionParameters.Forward, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility);
            double valuePlusHPlusK = getOptionValue(optionParameters.Strike, optionParameters.Forward + k, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility + h);
            double valuePlusHMinusK = getOptionValue(optionParameters.Strike, optionParameters.Forward - k, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility + h);
            double valueMinusHPlusK = getOptionValue(optionParameters.Strike, optionParameters.Forward + k, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility - h);
            double valueMinusHMinusK = getOptionValue(optionParameters.Strike, optionParameters.Forward - k, optionParameters.TimeToExpiry, optionParameters.DiscountFactor, optionParameters.Volatility - h);

            return (valuePlusHPlusK - valuePlusHMinusK - valueMinusHPlusK + valueMinusHMinusK) / (4.0 * h * k);
        }
    }
}