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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public partial class LapackLinearEquations
    {
        /// <summary>Determines the matrix norm for a specific operation.
        /// </summary>
        public enum MatrixConditionNormType
        {
            /// <summary>Estimates the condition number of the specific matrix in 1-norm.
            /// </summary>
            One,

            /// <summary>Estimates the condition number of the specific matrix in infinity-norm.
            /// </summary>
            Infinity
        }

        /// <summary>Provides methods for the estimation of conditional numbers. For more informations see the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        public interface IMatrixConditionalNumbers
        {
            /// <summary>Estimates the reciprocal of the conditional number of a general matrix in the 1-norm or the infinity-norm.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="a">The LU-factored matrix A as returned by <c>?getrf</c>, at least <paramref name="n"/> elements.</param>
            /// <param name="normOfOriginalMatrix">The 1- or infinity-norm of original matrix.</param>
            /// <param name="work">A workspace array of length at least 4 * <paramref name="n"/>.</param>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <returns>The reciprocal of the conditional number , i.e. \norm{A} * \norm{A^{-1}}, where \norm{.} denotes the 1-norm or infinity-norm.</returns>
            double dgecon(int n, Span<double> a, double normOfOriginalMatrix, Span<double> work, MatrixConditionNormType matrixNormType = MatrixConditionNormType.Infinity);

            /// <summary>Estimates the reciprocal of the conditional number of a band matrix in the 1-norm or the infinity-norm.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="kl">The number of subdiagonals within the band of the input matrix.</param>
            /// <param name="ku">The number of superdiagonals within the band of the input matrix.</param>
            /// <param name="a">The LU-factored matrix A as returned by <c>?gbtrf</c>.</param>          
            /// <param name="normOfOriginalMatrix">The 1- or infinity-norm of original matrix.</param>
            /// <param name="work">A workspace array of length at least 3 * <paramref name="n"/>.</param>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <returns>The reciprocal of the conditional number , i.e. \norm{A} * \norm{A^{-1}}, where \norm{.} denotes the 1-norm or infinity-norm.</returns>
            double dgbcon(int n, int kl, int ku, Span<double> a, double normOfOriginalMatrix, Span<double> work, MatrixConditionNormType matrixNormType = MatrixConditionNormType.Infinity);

            /// <summary>Estimates the reciprocal of the conditional number of a tridiagonal matrix in the 1-norm or the infinity-norm using the factorization computed by <c>?gttrf</c>.
            /// </summary>
            /// <param name="n">The order of the matrix.</param>
            /// <param name="dl">The (<paramref name="n"/> - 1) subdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) multipliers that defines the matrix L from the LU factorization.</param>
            /// <param name="d">The <paramref name="n"/> diagonal elements of the input matrix; overwritten by the <paramref name="n"/> diagonal elements of the upper triangular matrix U from the LU factorization.</param>
            /// <param name="du">The (<paramref name="n"/> - 1) superdiagonal elements of the input matrix; overwritten by the (<paramref name="n"/> - 1) elements of the first superdiagonal of U.</param>
            /// <param name="du2">The (<paramref name="n"/> - 2) elements of the second superdiagonal of U (output).</param>
            /// <param name="iPivot">The <paramref name="n"/> pivot indices, i.e. row j was interchanged with row <paramref name="iPivot"/>[j].
            /// <paramref name="iPivot"/>[j] is always j or j+1; <paramref name="iPivot"/>[j] = j indicates a row interchange was not required (output).</param>
            /// <param name="normOfOriginalMatrix">The 1- or infinity-norm of original matrix.</param>
            /// <param name="work">A workspace array of length at least 3 * <paramref name="n"/>.</param>
            /// <param name="matrixNormType">The type of the matrix norm.</param>
            /// <returns>The reciprocal of the conditional number , i.e. \norm{A} * \norm{A^{-1}}, where \norm{.} denotes the 1-norm or infinity-norm.</returns>
            double dgtcon(int n, Span<double> dl, Span<double> d, Span<double> du, Span<double> du2, int[] iPivot, double normOfOriginalMatrix, Span<double> work, MatrixConditionNormType matrixNormType = MatrixConditionNormType.Infinity);

            // todo: add further LAPACK functions on demand
        }
    }
}