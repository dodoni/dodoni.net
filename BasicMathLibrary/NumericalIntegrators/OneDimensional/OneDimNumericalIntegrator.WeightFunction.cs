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

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class OneDimNumericalIntegrator
    {
        /// <summary>Represents the weight function w(x) in the numerical integration 
        ///  <para>
        ///   \int_a^b w(x) * f(x) dx.
        /// </para>
        /// </summary>
        public class WeightFunction
        {
            #region public static (readonly) members

            /// <summary>The weight function w(x) with w(x) = 1.0 for each x.
            /// </summary>
            public static readonly WeightFunction One = new WeightFunction();
            #endregion

            #region private members

            /// <summary>The delegate that represents the weight function.
            /// </summary>
            private Func<double, double> m_WeightFunctionDelegate;
            #endregion

            #region private/public constructors

            /// <summary>Initializes a new instance of the <see cref="WeightFunction"/> class.
            /// </summary>
            /// <remarks>The weight function w(x) is assumed to be 1.0.</remarks>
            private WeightFunction()
            {
                IsOne = true;
                m_WeightFunctionDelegate = x => 1.0;
            }

            /// <summary>Initializes a new instance of the <see cref="WeightFunction"/> class.
            /// </summary>
            /// <param name="weightFunction">The weight function.</param>
            public WeightFunction(Func<double, double> weightFunction)
            {
                IsOne = false;
                m_WeightFunctionDelegate = weightFunction;
            }
            #endregion

            #region static constructors

            /// <summary>Initializes the <see cref="WeightFunction"/> class.
            /// </summary>
            static WeightFunction()
            {
            }
            #endregion

            #region public properties

            /// <summary>Gets a value indicating whether the weight function w(x) satisfied w(x) = 1 for each x.
            /// </summary>
            /// <value><c>true</c> if the weight function is equal to one; otherwise, <c>false</c>.</value>
            public bool IsOne
            {
                get;
                private set;
            }
            #endregion

            #region public methods

            /// <summary>Gets the value of the weight function at a specified point.
            /// </summary>
            /// <param name="x">The argument of the weight function.</param>
            /// <returns>The value of the weight function w(<paramref name="x"/>).</returns>
            public double GetValue(double x)
            {
                return (IsOne == true) ? 1.0 : m_WeightFunctionDelegate(x);
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="WeightFunction"/> object.
            /// </summary>
            /// <param name="weightFunction">The weight function.</param>
            public static WeightFunction Create(Func<double, double> weightFunction)
            {
                return new WeightFunction(weightFunction);
            }
            #endregion
        }
    }
}
