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
using System.Collections.Generic;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Serves as interface for a non-adaptive numerical integrator which supports some caching functionality, i.e.
    /// <para>
    /// \int_a^b w(x) * f(x) dx \approx \sum_{j=0}^N w(x_j) * f(x_j, j),
    /// </para>
    /// where f is the function to integrate and w is some strictly positive weight function. The abscissas (x_j) will not change.
    /// </summary>
    /// <remarks>If several numerical integrations are needed for a similar set of parameters one may store some pre-calculated values in
    /// one (or more) array(s) and enter these values into the function value, thus we write f(x,k) instead of f(x), where x = x_k for some
    /// at the calculation start already known array {x_0,x_1,...x_n} of abscissas.
    /// <para>For one application see for example 'Accelerating the Calibration of Stochastic Volatility Models', Fiodar Kilin, Frankfurt School of Finance and Management, No. 6, May 2007.</para>
    /// </remarks>
    public interface IOneDimNumericalConstAbscissaIntegratorAlgorithm : IOneDimNumericalIntegratorAlgorithm
    {
        /// <summary>Gets or sets the function to integrate.
        /// </summary>
        /// <value>The function to integrate.</value>
        new OneDimNumericalConstAbscissaIntegrator.FunctionToIntegrate FunctionToIntegrate
        {
            get;
            set;
        }

        /// <summary>Gets the factory for further <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> objects of the same type and with the same configuration.
        /// </summary>
        /// <value>The factory for further <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> objects of the same type and with the same configuration.</value>
        new OneDimNumericalConstAbscissaIntegrator Factory
        {
            get;
        }

        /// <summary>The abscissas, i.e. evaluation points of the numerical integration approach.
        /// </summary>
        /// <value>The abscissas, i.e. evaluation points of the numerical integration approach.</value>
        IEnumerable<double> Abscissas
        {
            get;
        }
    }
}