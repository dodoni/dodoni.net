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

namespace Dodoni.MathLibrary
{
    /// <summary>Contains various constants concerning the machine precision, given in some <see cref="System.Double"/> representation.
    /// </summary>
    public static class MachineConsts
    {
        /// <summary>Positive value (= 1E-8); values which are less than this given value are shown to be 0.
        /// </summary>
        public const double Epsilon = 1E-8;

        /// <summary>Positive value (= 1E-11), which is less than <see cref="Epsilon"/> and can be used for 
        /// example as exit condition of algorithms.
        /// </summary>
        public const double TinyEpsilon = 1E-11;

        /// <summary>Positive value (= 1E-13), which is less than <see cref="Epsilon"/> and 
        /// <see cref="TinyEpsilon"/>.
        /// </summary>
        public const double SuperTinyEpsilon = 1E-13;

        /// <summary>Positive value (= 1E-16), which is almost equal to the real machine precision
        /// of a <see cref="System.Double"/>.
        /// </summary>
        public const double ExtremeTinyEpsilon = 1E-16;

        /// <summary>The smallest number such that 1.0 + <c>DBLEpsilon</c> != 1.0.
        /// </summary>
        public static readonly double DBL_Epsilon = Math.Pow(2, -53);

        /// <summary>Minus the logarithm of <see cref="DBL_Epsilon"/> which is the smallest number such that 1.0 + <c>DBLEpsilon</c> != 1.0.
        /// </summary>
        public static readonly double MinusLogOfDBL_Epsilon = -Math.Log(DBL_Epsilon);

        /// <summary>Positive value (= 1E+16) which represents a very large number which is also
        /// 1/<see cref="ExtremeTinyEpsilon"/>.
        /// </summary>
        public const double ExtremLargeValue = 1E+16;
    }
}