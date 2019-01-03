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
namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class AdaptiveGaussKronrodIntegrator
    {
        /// <summary>Contains a lower and upper integration bound, as well as the deepness of the sub-integral, i.e. the deepness of the binary tree which is used for the adaptive integration method.
        /// </summary>
        private struct SubIntegral
        {
            /// <summary>The lower bound of the sub-integral.
            /// </summary>
            internal double LowerBound;

            /// <summary>The upper bound of the sub-integral.
            /// </summary>
            internal double UpperBound;

            /// <summary>The value of the sub-interval (via Kronrod).
            /// </summary>
            internal double Value;

            /// <summary>The value of the benchmark of the sub-interval (i.e. via Gauss-Legendre).
            /// </summary>
            internal double BenchmarkValue;

            /// <summary>The deepness in the binary tree which is used for the adaptive integration method.
            /// </summary>
            internal int Depth;

            /// <summary>Initializes a new instance of the <see cref="SubIntegral"/> struct.
            /// </summary>
            /// <param name="value">The value of the sub-interval (via Kronrod).</param>
            /// <param name="benchmarkValue">The value of the benchmark of the sub-interval (i.e. via Gauss-Legendre).</param>
            /// <param name="lowerBound">The lower bound.</param>
            /// <param name="upperBound">The upper bound.</param>
            /// <param name="depth">The depth.</param>
            internal SubIntegral(double value, double benchmarkValue, double lowerBound, double upperBound, int depth)
            {
                Value = value;
                BenchmarkValue = benchmarkValue;
                LowerBound = lowerBound;
                UpperBound = upperBound;
                Depth = depth;
            }
        }
    }
}