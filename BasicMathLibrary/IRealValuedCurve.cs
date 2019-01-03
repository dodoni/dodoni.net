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

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as interface for some real valued curve, for example some polynomial or
    /// some grid points together with a interpolation/extrapolation/parametrization etc.
    /// </summary>
    /// <remarks>The domain of definition is assumed to be a (bounded or unbounded) interval.</remarks>
    public interface IRealValuedCurve : IOperable
    {
        #region properties

        /// <summary>Gets the lower bound of the domain of definition.
        /// </summary>
        /// <value>The lower bound of the domain of definition, perhaps <see cref="System.Double.NegativeInfinity"/> 
        /// or <see cref="System.Double.NaN"/>.</value>
        double LowerBound { get; }

        /// <summary>Gets the upper bound of the domain of definition.
        /// </summary>
        /// <value>The upper bound of the domain of definition, perhaps <see cref="System.Double.PositiveInfinity"/>
        /// or <see cref="System.Double.NaN"/>.</value>
        double UpperBound { get; }
        #endregion

        #region methods

        /// <summary>Gets the value at a specific argument.
        /// </summary>
        /// <param name="pointToEvaluate">The point to evaluate.</param>
        /// <returns>The value of the curve at <paramref name="pointToEvaluate"/>.</returns>
        /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
        double GetValue(double pointToEvaluate);

        /// <summary>Gets the value of the integral \int_a^b f(x) dx.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The value of \int_a^b f(x) dx.</returns>
        /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.
        /// Moreover it is assumed that the <paramref name="lowerBound"/> is less or equal than the <paramref name="upperBound"/>.</remarks>
        double GetIntegral(double lowerBound, double upperBound);
        #endregion
    }
}