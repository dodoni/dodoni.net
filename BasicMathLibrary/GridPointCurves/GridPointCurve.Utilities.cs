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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.GridPointCurves
{
    public static partial class GridPointCurve
    {
        /// <summary>Provides several utility methods concerning grid point curve objects.
        /// </summary>
        internal static class Utilities
        {
            /// <summary>Gets the null-based index of the nearest label which less than the specified value and not the last label.
            /// </summary>
            /// <param name="value">The value which should be compared to the <see cref="System.Double"/> representation of the labels.</param>
            /// <param name="doubleLabels">The <see cref="System.Double"/> representation of the labels in ascending order.</param>
            /// <param name="labelCount">The number of <paramref name="doubleLabels"/>.</param>
            /// <returns>The null-based index of the nearest label which is smaller than <paramref name="value"/>. If <paramref name="value"/> is identical to the 
            /// last label the number of labels -2 will returned.</returns>
            /// <exception cref="ArgumentException">Thrown, if <paramref name="value"/> is smaller than the first element of <paramref name="doubleLabels"/> or greater than 
            /// the last (i.e. (<paramref name="labelCount"/> -1)-th) value of <paramref name="doubleLabels"/>.</exception>
            public static int GetNonLastNearestIndex(double value, double[] doubleLabels, int labelCount)
            {
                if (value < doubleLabels[0])
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, value, doubleLabels[0]), nameof(value));
                }
                int nonLastLeftGridPointIndex = Array.BinarySearch<double>(doubleLabels, 0, labelCount, value);
                if (nonLastLeftGridPointIndex < 0)
                {
                    nonLastLeftGridPointIndex = (~nonLastLeftGridPointIndex) - 1;
                    if (nonLastLeftGridPointIndex >= labelCount)  // value is greater than each element of the array
                    {
                        throw new ArgumentException(String.Format(ExceptionMessages.ArgumentOutOfRangeLessEqual, value, doubleLabels[labelCount - 1]), nameof(value));
                    }
                }
                else if (nonLastLeftGridPointIndex == labelCount - 1)
                {
                    return labelCount - 2;
                }
                return nonLastLeftGridPointIndex;
            }
        }
    }
}