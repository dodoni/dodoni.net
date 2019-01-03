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
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as managed code implementation of BLAS level 1 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.</remarks>
    internal partial class BuildInLevel1BLAS
    {
        /// <summary>Computes the parameters for a Givens rotation; given the Cartesian coordinates (a, b) of a point p, these routines
        /// return the parameters a, b, c, and s associated with the Givens rotation that zeros the y-coordinate of the point.
        /// </summary>
        /// <param name="a">Provides the x-coordinate of the point p; contains the parameter r associated with the Givens rotation after function evaluation.</param>
        /// <param name="b">Provides the y-coordinate of the point p; contains the parameter z associated with the Givens rotation after function evaluation.</param>
        /// <param name="c">Contains the parameter c associated with the Givens rotation (output).</param>
        /// <param name="s">Contains the parameter s associated with the Givens rotation (output).</param>
        public void drotg(ref double a, ref double b, out double c, out double s)
        {
            double roe = (Math.Abs(b) < Math.Abs(a)) ? a : b;
            double scale = Math.Abs(a) + Math.Abs(b);

            double r;
            if (scale == 0.0)
            {
                c = 1.0;
                s = 0.0;
                r = 0.0;
            }
            else
            {
                r = scale * Math.Sqrt((a / scale) * (a / scale) + (b / scale) * (b / scale));
                r = Math.Sign(roe) * r;
                c = a / r;
                s = b / r;
            }

            double z;
            if ((0.0 < Math.Abs(c)) && (Math.Abs(c) <= s))
            {
                z = 1.0 / c;
            }
            else
            {
                z = s;
            }

            a = r;
            b = z;
        }
    }
}