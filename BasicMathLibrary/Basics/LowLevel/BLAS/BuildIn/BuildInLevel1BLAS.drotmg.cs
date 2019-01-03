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
        /// <summary>Construct the modified Givens transformation matrix H which zeros the second component of a 2-dimensional vector, i.e.
        /// given (x_1,y_1) compute matrix H such that <para>(x_1,0)^t = H * (x_1*\sqrt(d1), y_1 * \qrt(d2))^t.</para>
        /// </summary>
        /// <param name="d1">Provides the scaling factor for the x-coordinate of the input vector; the first diagonal element of the updated matrix on exit.</param>
        /// <param name="d2">Provides the scaling factor for the y-coordinate of the input vector; the second diagonal element of the updated matrix on exit.</param>
        /// <param name="x1">Provides the x-coordinate of the input vector; the x-coordinate of the rotated vector before scaling on exit.</param>
        /// <param name="y1">Provides the y-coordinate of the input vector.</param>
        /// <param name="param">The elements of the param array are: param(0) contains a switch, flag. param(1-4) contain h11, h21, h12, and h22, respectively,
        /// the components of the array H.</param>
        public void drotmg(ref double d1, ref double d2, ref double x1, double y1, double[] param)
        {
            /*
             * CONSTRUCT THE MODIFIED GIVENS TRANSFORMATION MATRIX H WHICH ZEROS
             *     THE SECOND COMPONENT OF THE 2-VECTOR  (DSQRT(DD1)*DX1,DSQRT(DD2)*DY2)**T.
             *     WITH DPARAM(1)=DFLAG, H HAS ONE OF THE FOLLOWING FORMS..
             *
             *     DFLAG=-1.D0     DFLAG=0.D0        DFLAG=1.D0     DFLAG=-2.D0
             *
             *       (DH11  DH12)    (1.D0  DH12)    (DH11  1.D0)    (1.D0  0.D0)
             *     H=(          )    (          )    (          )    (          )
             *       (DH21  DH22),   (DH21  1.D0),   (-1.D0 DH22),   (0.D0  1.D0).
             *     LOCATIONS 2-4 OF DPARAM CONTAIN DH11, DH21, DH12, AND DH22
             *     RESPECTIVELY. (VALUES OF 1.D0, -1.D0, OR 0.D0 IMPLIED BY THE
             *     VALUE OF DPARAM(1) ARE NOT STORED IN DPARAM.)
             *
             *     THE VALUES OF GAMSQ AND RGAMSQ SET IN THE DATA STATEMENT MAY BE
             *     INEXACT.  THIS IS OK AS THEY ARE ONLY USED FOR TESTING THE SIZE
             *     OF DD1 AND DD2.  ALL ACTUAL SCALING OF DATA IS DONE USING GAM.
             */

            double gamsq = 16777216;
            double rgamsq = 5.9604645e-8;
            double gam = 4096;

            double dh11, dh12, dh21, dh22;
            dh11 = dh12 = dh21 = dh22 = 0.0;  // necessary for the compiler to avoid "use of unassigned variable" 

            double dflag = -1; // necessary for the compiler to avoid "use of unassigned variable" 
            if (d1 < 0)
            {
                dflag = -1;
                dh11 = dh12 = dh21 = dh22 = 0.0;
                d1 = d2 = x1 = 0;
            }
            else
            {
                double p2 = d2 * y1;
                if (p2 == 0)
                {
                    dflag = -2;
                    param[0] = dflag;
                    return;
                }
                // regular case
                double p1 = d1 * x1;
                double q2 = p2 * y1;
                double q1 = p1 * x1;

                double du;
                if (Math.Abs(q1) > Math.Abs(q2))
                {
                    dh21 = -y1 / x1;
                    dh12 = p2 / p1;
                    du = 1.0 - dh12 * dh21;

                    if (du > 0.0)
                    {
                        dflag = 0;
                        d1 = d1 / du;
                        d2 = d2 / du;
                        x1 = x1 * du;
                    }
                }
                else
                {
                    if (q2 < 0)
                    {
                        dflag = -1;
                        dh11 = dh12 = dh21 = dh22 = 0;
                        d1 = d2 = x1 = 0;
                    }
                    else
                    {
                        dflag = 1;
                        dh11 = p1 / p2;
                        dh22 = x1 / y1;
                        du = 1 + dh11 * dh22;
                        double temp = d2 / du;
                        d2 = d1 / du;
                        d1 = temp;
                        x1 = y1 * du;
                    }
                }


                // scale-check
                if (d1 != 0.0)
                {
                    while ((d1 <= rgamsq) || (d1 >= gamsq))
                    {
                        if (dflag == 0)
                        {
                            dh11 = dh22 = 1;
                            dflag = -1;
                        }
                        else
                        {
                            dh21 = -1;
                            dh12 = 1;
                            dflag = -1;
                        }

                        if (d1 <= rgamsq)
                        {
                            d1 *= gam * gam;
                            x1 /= gam;
                            dh11 /= gam;
                            dh12 /= gam;
                        }
                        else
                        {
                            d1 /= gam * gam;
                            x1 *= gam;
                            dh11 *= gam;
                            dh12 *= gam;
                        }
                    }
                }

                if (d2 != 0)
                {
                    while ((Math.Abs(d2) <= rgamsq) || (Math.Abs(d2) >= gamsq))
                    {
                        if (dflag == 0)
                        {
                            dh11 = dh22 = 1;
                            dflag = -1;
                        }
                        else
                        {
                            dh21 = -1;
                            dh12 = 1;
                            dflag = -1;
                        }
                        if (Math.Abs(d2) <= rgamsq)
                        {
                            d2 = d2 * gam * gam;
                            dh21 /= gam;
                            dh22 /= gam;
                        }
                        else
                        {
                            d2 /= gam * gam;
                            dh21 *= gam;
                            dh22 *= gam;
                        }
                    }
                }
            }

            if (dflag < 0)
            {
                param[1] = dh11;
                param[2] = dh21;
                param[3] = dh12;
                param[4] = dh22;
            }
            else if (dflag == 0)
            {
                param[2] = dh21;
                param[3] = dh12;
            }
            else
            {
                param[1] = dh11;
                param[4] = dh22;
            }
            param[0] = dflag;
        }
    }
}