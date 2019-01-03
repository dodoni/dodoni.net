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

using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics.LowLevel.BuildIn
{
    /// <summary>Serves as (primitive) managed code implementation of BLAS level 3 operations (fall-back solution).
    /// </summary>
    /// <remarks>This implementation is based on the C code of the BLAS implementation, see http://www.netlib.org/clapack/cblas. </remarks>
    internal partial class BuildInLevel3BLAS
    {
        /// <summary>Computes a matrix-matrix product where one input matrix is symmetric, i.e. C := \alpha*A*B + \beta*C or C := \alpha*B*A +\beta*C.
        /// </summary>
        /// <param name="m">The number of rows of the matrix C.</param>
        /// <param name="n">The number of columns of the matrix C.</param>
        /// <param name="alpha">The scalar \alpha.</param>
        /// <param name="a">The symmetric matrix A supplied column-by-column of dimension (<paramref name="lda"/>, ka), where ka is <paramref name="m"/> if to calculate C := \alpha * A*B + \beta*C; otherwise <paramref name="n"/>.</param>
        /// <param name="b">The matrix B supplied column-by-column of dimension (<paramref name="ldb"/>,<paramref name="n"/>).</param>
        /// <param name="beta">The scalar \beta.</param>
        /// <param name="c">The matrix C supplied column-by-column of dimension (<paramref name="ldc"/>,<paramref name="n"/>); input/output.</param>
        /// <param name="lda">The leading dimension of <paramref name="a"/>, must be at least max(1,<paramref name="m"/>) if <paramref name="side"/>=left; max(1,n) otherwise.</param>
        /// <param name="ldb">The leading dimension of <paramref name="b"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="ldc">The leading dimension of <paramref name="c"/>, must be at least max(1,<paramref name="m"/>).</param>
        /// <param name="side">A value indicating whether to calculate C := \alpha * A*B + \beta*C or C := \alpha * B*A +\beta*C.</param>
        /// <param name="triangularMatrixType">A value whether matrix A is in its upper or lower triangular representation.</param>
        public void dsymm(int m, int n, double alpha, double[] a, double[] b, double beta, double[] c, int lda, int ldb, int ldc, BLAS.Side side = BLAS.Side.Left, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.UpperTriangularMatrix)
        {
            if (m == 0 || n == 0 || ((alpha == 0.0) && (beta == 1.0)))
            {
                return; // nothing to do
            }

            if (side == BLAS.Side.Left)  // C = \alpha *A *B +\beta*C
            {
                if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            double temp = alpha * b[i + j * lda];

                            double temp2 = 0.0;
                            for (int k = 0; k < i; k++)
                            {
                                c[k + j * ldc] += temp * a[k + i * lda];
                                temp2 += b[k + j * ldb] * a[k + i * lda];
                            }
                            c[i + j * ldc] = beta * c[i + j * ldc] + temp * a[i + i * lda] + alpha * temp2;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int i = m - 1; i >= 0; i--)
                        {
                            double temp = alpha * b[i + j * ldb];
                            double temp2 = 0.0;
                            for (int k = i + 1; k < m; k++)
                            {
                                c[k + j * ldc] += temp * a[k + i * lda];
                                temp2 += b[k + j * ldb] * a[k + i * lda];
                            }
                            c[i + j * ldc] = beta * c[i + j * ldc] + temp * a[i + i * lda] + alpha * temp2;
                        }
                    }
                }
            }
            else if (side == BLAS.Side.Right)  // C = \alpha*B*A + \beta *C
            {
                for (int j = 0; j < n; j++)
                {
                    double temp = alpha * a[j + j * lda];
                    for (int i = 0; i < m; i++)
                    {
                        c[i + j * ldc] = beta * c[i + j * ldc] + temp * b[i + j * ldb];
                    }

                    for (int k = 0; k < j; k++)
                    {
                        if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                        {
                            temp = alpha * a[k + j * lda];
                        }
                        else
                        {
                            temp = alpha * a[j + k * lda];
                        }
                        for (int i = 0; i < m; i++)
                        {
                            c[i + j * ldc] += temp * b[i + k * ldb];
                        }
                    }
                    for (int k = j + 1; k < n; k++)
                    {
                        if (triangularMatrixType == BLAS.TriangularMatrixType.UpperTriangularMatrix)
                        {
                            temp = alpha * a[j + k * lda];
                        }
                        else
                        {
                            temp = alpha * a[k + j * lda];
                        }
                        for (int i = 0; i < m; i++)
                        {
                            c[i + j * ldc] += temp * b[i + k * ldb];
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException(side.ToString());
            }
        }
    }
}