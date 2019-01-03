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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Serves as interface for integration algorithms of a real-valued function in one real argument, i.e. represents an approximation of
    /// <para>
    /// \int_a^b w(x) * f(x) dx,
    /// </para>
    /// where f:I \to \R, I: Interval with a = inf I, b = sup I, is the function to integrate and w(x) is a specific strictly positive weight function.
    /// </summary>
    public interface IOneDimNumericalIntegratorAlgorithm : IOperable, IInfoOutputQueriable
    {
        /// <summary>Gets or sets the function to integrate.
        /// </summary>
        /// <value>The function to integrate.</value>
        Func<double, double> FunctionToIntegrate
        {
            get;
            set;
        }

        /// <summary>Gets the weight function w(x) of the numerical integration 
        ///  <para>
        /// \int_a^b w(x) * f(x) dx,
        /// </para>
        /// where f:I \to \R, I: Interval with a = inf I, b = sup I, is the function to integrate and w(x) is a specific strictly positive weight function.
        /// </summary>
        /// <value>The weight function.</value>
        OneDimNumericalIntegrator.WeightFunction WeightFunction
        {
            get;
        }

        /// <summary>Gets the lower integration bound.
        /// </summary>
        /// <value>The lower integration bound.</value>
        double LowerBound
        {
            get;
        }

        /// <summary>Gets the upper integration bound.
        /// </summary>
        /// <value>The upper integration bound.</value>
        double UpperBound
        {
            get;
        }

        /// <summary>Gets the factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.
        /// </summary>
        /// <value>The factory for further <see cref="IOneDimNumericalIntegratorAlgorithm"/> objects of the same type and with the same configuration.</value>
        OneDimNumericalIntegrator Factory
        {
            get;
        }

        /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
        /// </summary>
        /// <returns>An approximation of the specific integral.</returns>
        double GetValue();

        /// <summary>Gets the value of the integral \int_a^b w(x) * f(x) dx.
        /// </summary>
        /// <param name="value">An approximation of the specific integral.</param>
        /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="value"/> contains valid data.</returns>
        OneDimNumericalIntegrator.State GetValue(out double value);

        /// <summary>Sets the lower integration bound.
        /// </summary>
        /// <param name="lowerBound">The lower integration bound.</param>
        /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound"/> has been set to <paramref name="lowerBound"/>.</returns>
        bool TrySetLowerBound(double lowerBound);

        /// <summary>Sets the upper integration bound.
        /// </summary>
        /// <param name="upperBound">The upper integration bound.</param>
        /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound"/> has been set to <paramref name="upperBound"/>.</returns>
        bool TrySetUpperBound(double upperBound);

        /// <summary>Sets the lower and upper integration bounds.
        /// </summary>
        /// <param name="lowerBound">The lower integration bound.</param>
        /// <param name="upperBound">The upper integration bound.</param>
        /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound"/> and <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound"/> have been changed.</returns>
        bool TrySetBounds(double lowerBound, double upperBound);
    }
}
