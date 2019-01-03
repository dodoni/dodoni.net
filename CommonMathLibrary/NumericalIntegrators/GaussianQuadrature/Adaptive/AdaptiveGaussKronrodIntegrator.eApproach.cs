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

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class AdaptiveGaussKronrodIntegrator
    {
        /// <summary>The integration rule used for a specific <see cref="AdaptiveGaussKronrodIntegrator"/> object.
        /// </summary>
        public enum Approach
        {
            /// <summary>Use a 7-point Gauss-Legendre together with 15-point Kronrod integration rule.
            /// </summary>
            [String("15-point Kronrod")]
            Kronrod15 = 15,

            /// <summary>Use a 10-point Gauss-Legendre togehter with 21-point Kronrod integration rule.
            /// </summary>
            [String("21-point Kronrod")]
            Kronrod21 = 21,

            /// <summary>Use a 15-point Gauss-Legendre togehter with 31-point Kronrod integration rule.
            /// </summary>
            [String("31-point Kronrod")]
            Kronrod31 = 31,

            /// <summary>Use a 20-point Gauss-Legendre togehter with 41-point Kronrod integration rule.
            /// </summary>
            [String("41-point Kronrod")]
            Kronrod41 = 41,

            /// <summary>Use a 25-point Gauss-Legendre togehter with 51-point Kronrod integration rule.
            /// </summary>
            [String("51-point Kronrod")]
            Kronrod51 = 51,

            /// <summary>Use a  30-point Gauss-Legendre together with 61-point Kronrod integration rule.
            /// </summary>
            [String("61-point Kronrod")]
            Kronrod61 = 61
        }
    }
}