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
namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Serves as interface for a discrete (one-dimensional) probability distribution.
    /// </summary>
    public interface IDiscreteProbabilityDistribution : IProbabilityDistribution
    {
        /// <summary>Evaluate the density function at a specified point.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>A <see cref="System.Int32"/> which represents the value of the density function.</returns>
        int GetDensityValue(int x);

        /// <summary>Evaluate the cummulative distribution function at a specified point.
        /// </summary>
        /// <param name="x">The value where to evaluate.</param>
        /// <returns>A <see cref="System.Int32"/> which represents the value of the cummulative distribution function.</returns>
        int GetCdfValue(int x);

        /// <summary>Gets the inverse of the cummulative distribution function at a specified point.
        /// </summary>
        /// <param name="probability">The probability where to evaluate.</param>
        /// <returns>A <see cref="System.Int32"/> which represents the value of the inverse cummulative distribution function.</returns>
        new int GetInverseCdfValue(double probability);
    }
}