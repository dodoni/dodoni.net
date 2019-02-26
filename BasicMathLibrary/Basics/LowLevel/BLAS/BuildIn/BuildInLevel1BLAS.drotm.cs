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

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as managed code implementation of BLAS level 1 operations.
    /// </summary>
    /// <remarks>Some of the methods are straightforward ports of the Fortran implementation (http://www.netlib.org/blas). It is recommended to use wrapper of a native code implementation.</remarks>
    internal partial class BuildInLevel1BLAS
    {
        /// <summary>Performs rotation of points in the modified plane; x(i) = H*x(i) + H*y(i), y(i) = H*y(i) - H*x(i),
        /// i.e. x[i] = h11 * x[i] + h12 * y[i], y[i] = h21 * x[i] + h22 * y[i] for each i, where H is a modified Givens transformation matrix
        /// whose values are stored in the <paramref name="param" />.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x" /> and <paramref name="y" />.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n" /> elements.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n" /> elements.</param>
        /// <param name="param">The elements of the param array are: param(0) contains a switch, flag. param(1-4) contain h11, h21, h12, and h22, respectively,
        /// the components of the array H.</param>
        /// <param name="incX">The increment for <paramref name="x" />.</param>
        /// <param name="incY">The increment for <paramref name="y" />.</param>
        public void drotm(int n, Span<double> x, Span<double> y, ReadOnlySpan<double> param, int incX = 1, int incY = 1)
        {
            /*  
             * Purpose 
             * APPLY THE MODIFIED GIVENS TRANSFORMATION, H, TO THE 2 BY N MATRIX (X**T) , WHERE **T INDICATES TRANSPOSE. THE ELEMENTS OF X ARE IN (Y**T) 
             * X(LX+I*INCX), I = 0 TO N-1, WHERE LX = 1 IF INCX .GE. 0, ELSE LX = (-INCX)*N, AND SIMILARLY FOR SY USING LY AND INCY. 
             * WITH PARAM(1)=FLAG, H HAS ONE OF THE FOLLOWING FORMS. 
             *     FLAG=-1.D0     FLAG=0.D0        FLAG=1.D0     FLAG=-2.D0 
             *     (DH11  DH12)    (1.D0  DH12)    (DH11  1.D0)    (1.D0  0.D0) 
             *   H=(          )    (          )    (          )    (          ) 
             *     (DH21  DH22),   (DH21  1.D0),   (-1.D0 DH22),   (0.D0  1.D0). 
             *       
             *  PARAM(0)=FLAG, PARAM(1)=DH11, PARAM(2)=DH21, PARAM(3)=DH12, PARAM(4)=DH22 
           */
            double dh11 = param[1];
            double dh12 = param[3];
            double dh21 = param[2];
            double dh22 = param[4];

            double flag = param[0];
            if ((n > 0) && (flag + 2.0 != 0.0))
            {
                if ((incX == incY) && (incX > 0))
                {
                    int nsteps = n * incX;
                    if (flag < 0.0)
                    {
                        for (int i = 0; incX < 0 ? i > nsteps : i < nsteps; i += incX)
                        {
                            double w = x[i];
                            double z = y[i];
                            x[i] = w * dh11 + z * dh12;
                            y[i] = w * dh21 + z * dh22;
                        }
                    }
                    else if (flag == 0)
                    {
                        for (int i = 0; incX < 0 ? i > nsteps : i < nsteps; i += incX)
                        {
                            double w = x[i];
                            double z = y[i];
                            x[i] = w + z * dh12;
                            y[i] = w * dh21 + z;
                        }
                    }
                    else
                    {
                        for (int i = 0; incX < 0 ? i > nsteps : i < nsteps; i += incX)
                        {
                            double w = x[i];
                            double z = y[i];
                            x[i] = w * dh11 + z;
                            y[i] = -w + dh22 * z;
                        }
                    }
                }
                else
                {
                    int ix = (incX >= 0) ? 0 : (-n + 1) * incX;
                    int iy = (incY >= 0) ? 0 : (-n + 1) * incY;

                    if (flag < 0.0)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            double w = x[ix];
                            double z = y[iy];
                            x[ix] = w * dh11 + z * dh12;
                            y[iy] = w * dh21 + z * dh22;
                            ix += incX;
                            iy += incY;
                        }
                    }
                    else if (flag == 0)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            double w = x[ix];
                            double z = y[iy];
                            x[ix] = w + z * dh12;
                            y[iy] = w * dh21 + z;
                            ix += incX;
                            iy += incY;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < n; i++)
                        {
                            double w = x[ix];
                            double z = y[iy];
                            x[ix] = w * dh11 + z;
                            y[iy] = -w + dh22 * z;
                            ix += incX;
                            iy += incY;
                        }
                    }
                }
            }
        }
    }
}