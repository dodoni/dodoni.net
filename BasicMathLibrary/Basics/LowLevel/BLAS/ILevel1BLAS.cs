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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as interface for level 1 Basic Linear Algebra Subprograms, namely (C-)BLAS, i.e. <para>vector operations.</para> 
    /// </summary>
    /// <remarks>The function names are almost identically to the BLAS naming convention, see http://www.netlib.org/blas, here we
    /// restrict to double precision and complex numbers.
    /// See for example https://medium.com/@antao.almada/p-invoking-using-span-t-a398b86f95d3 for the use of Span{T} with respect to unmanaged libraries.</remarks>
    public interface ILevel1BLAS
    {
        #region double precision methods

        /// <summary>Compute the sum of the magnitudes of elements of some vector, i.e.
        /// <para>|x(0)|+ |x(incX)| + |x(2 * incX)| +... + |x(k * incX)|.</para>
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> </param>
        /// <param name="x">The vector 'x' with at least (1 + (<paramref name="n"/> - 1) * Abs( <paramref name="incX"/>) ) elements.</param>
        /// <param name="incX">The increment for indexing vector <paramref name="x"/>.</param>
        /// <returns>The sum of magnitudes of elements of the vector <paramref name="x"/>.</returns>
        double dasum(int n, ReadOnlySpan<double> x, int incX = 1);

        /// <summary>Perform a vector-vector operation defined as 
        /// <para>
        ///     y[k * <paramref name="incY"/>] += <paramref name="a"/> * x[k * <paramref name="incX"/>]
        /// </para>
        /// for k=0,..,<paramref name="n"/>-1. Thus some partial scalar times vector plus vector operation.
        /// </summary>
        /// <param name="n">The number of elements to add.</param>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
        /// <param name="y">The vector 'y' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements; contains the updated vector on exit.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void daxpy(int n, double a, ReadOnlySpan<double> x, Span<double> y, int incX = 1, int incY = 1);

        /// <summary>Copies a vector to another vector, i.e. 
        /// <para>
        ///   y[k * <paramref name="incY"/>] = x[k * <paramref name="incX"/>]
        /// </para> for k=0,..,<paramref name="n"/>-1.
        /// </summary>
        /// <param name="n">The number of elements to copy.</param>
        /// <param name="x">The vector 'x' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="y">The vector 'y' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements; contains the updated vector on exit.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void dcopy(int n, ReadOnlySpan<double> x, Span<double> y, int incX = 1, int incY = 1);

        /// <summary>Computes a vector-vector dot product, i.e. \sum_j x_{j * <paramref name="incX"/>} * y_{j * <paramref name="incY"/>).
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        /// <returns>The dot product of <paramref name="x"/> and <paramref name="y"/>, i.e. \sum_j x_{j * <paramref name="incX"/>} * y_{j * <paramref name="incY"/>).</returns>
        double ddot(int n, ReadOnlySpan<double> x, ReadOnlySpan<double> y, int incX = 1, int incY = 1);

        /// <summary>Computes the Euclidean norm of a vector, i.e. ||x||.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <returns>The euclidian norm of <paramref name="x"/>, i.e. \sqrt(x_0^2 + ... + x_n^2).</returns>
        double dnrm2(int n, ReadOnlySpan<double> x, int incX = 1);

        /// <summary>Performs rotation of points in the plane, i.e. x(i) = c * x(i) + s * y(i) and y(i) = c * y(i) - s * x(i).
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements; 'x(i) = c * x(i) + s * y(i)' after function evaluation.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements; 'y(i) = c * y(i) - s * x(i)' after function evaluation.</param>
        /// <param name="c">The scalar 'c'.</param>
        /// <param name="s">The scalar 's'.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void drot(int n, Span<double> x, Span<double> y, double c, double s, int incX = 1, int incY = 1);

        /// <summary>Computes the parameters for a Givens rotation; given the Cartesian coordinates (a, b) of a point p, these routines 
        /// return the parameters a, b, c, and s associated with the Givens rotation that zeros the y-coordinate of the point, i.e. 
        /// <para>(c s \\ -s c) * (a \\ b) = (r \\ 0).</para>
        /// </summary>
        /// <param name="a">Provides the x-coordinate of the point p; contains the parameter r associated with the Givens rotation after function evaluation.</param>
        /// <param name="b">Provides the y-coordinate of the point p; contains the parameter z associated with the Givens rotation after function evaluation.</param>
        /// <param name="c">Contains the parameter c associated with the Givens rotation (output).</param>
        /// <param name="s">Contains the parameter s associated with the Givens rotation (output).</param>
        void drotg(ref double a, ref double b, out double c, out double s);

        /// <summary>Performs rotation of points in the modified plane; x(i) = H*x(i) + H*y(i), y(i) = H*y(i) - H*x(i), 
        /// i.e. x[i] = h11 * x[i] + h12 * y[i], y[i] = h21 * x[i] + h22 * y[i] for each i, where H is a modified Givens transformation matrix 
        /// whose values are stored in the <paramref name="param"/>.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
        /// <param name="param">The elements of the param array are: param(0) contains a switch, flag. param(1-4) contain h11, h21, h12, and h22, respectively, 
        /// the components of the array H.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void drotm(int n, Span<double> x, Span<double> y, ReadOnlySpan<double> param, int incX = 1, int incY = 1);

        /// <summary>Construct the modified Givens transformation matrix H which zeros the second component of a 2-dimensional vector, i.e.
        /// given (x_1,y_1) compute matrix H such that <para>(x_1,0)^t = H * (x_1*\sqrt(d1), y_1 * \qrt(d2))^t.</para>
        /// </summary>
        /// <param name="d1">Provides the scaling factor for the x-coordinate of the input vector; the first diagonal element of the updated matrix on exit.</param>
        /// <param name="d2">Provides the scaling factor for the y-coordinate of the input vector; the second diagonal element of the updated matrix on exit.</param>
        /// <param name="x1">Provides the x-coordinate of the input vector; the x-coordinate of the rotated vector before scaling on exit.</param>
        /// <param name="y1">Provides the y-coordinate of the input vector.</param>
        /// <param name="param">The elements of the param array are: param(0) contains a switch, flag. param(1-4) contain h11, h21, h12, and h22, respectively, 
        /// the components of the array H.</param>
        void drotmg(ref double d1, ref double d2, ref double x1, double y1, Span<double> param);

        /// <summary>Computes the product of a vector by a scalar, i.e. x[j * <paramref name="incX"/>] = a * x[j * <paramref name="incX"/>], for j = 0,..., n-1.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x' with at least 1 + (<paramref name="n"/> -1) * <paramref name="incX"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        void dscal(int n, double a, Span<double> x, int incX = 1);

        /// <summary>Swaps a vector with another vector, i.e. y[j * <paramref name="incY"/>] = x[j * <paramref name="incX"/>] and 
        /// x[j * <paramref name="incX"/>] y[j * <paramref name="incY"/>]  for j = 0,...,n-1.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
        /// <param name="y">The vector 'y' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void dswap(int n, Span<double> x, Span<double> y, int incX = 1, int incY = 1);

        /// <summary>Finds the index of the element with maximum absolute value.
        /// </summary>
        /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
        /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <returns>The position of vector element <paramref name="x"/> that has the largest absolute value.</returns>
        int idamax(int n, ReadOnlySpan<double> x, int incX = 1);

        /// <summary>Finds the index of the element with smallest absolute value.
        /// </summary>
        /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
        /// <param name="x">The vector with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.</returns>
        /// <remarks>This method is not part of the BLAS standard.</remarks>
        int idamin(int n, ReadOnlySpan<double> x, int incX = 1);
        #endregion

        #region (double precision) complex methods

        /// <summary>Compute the sum of the magnitudes of elements of some vector, i.e.
        /// <para>|Re x(0)|+ |Im x(0)| + |Re x(incX)| + |Im x(incX)| + |Re x(2 * incX)| + |Im x(2 * incX)| + ...  + |Re x(k * incX)| + |x(k * incX)|.</para>
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> with at least
        /// <c>(1 + (<paramref name="n"/>-1)*abs(<paramref name="incX"/>))</c> elements.</param>
        /// <param name="x">The vector.</param>
        /// <param name="incX">The increment for indexing vector <paramref name="x"/>.</param>
        /// <returns>The sum of magnitudes of elements of the vector <paramref name="x"/>.</returns>
        double zasum(int n, ReadOnlySpan<Complex> x, int incX = 1);

        /// <summary>Perform a vector-vector operation defined as 
        /// <para>
        ///     y[k * <paramref name="incY"/>] += <paramref name="a"/> * x[k * <paramref name="incX"/>]
        /// </para>
        /// for k=0,..,<paramref name="n"/>-1. Thus some partial scalar times vector plus vector operation.
        /// </summary>
        /// <param name="n">The number of elements to add.</param>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
        /// <param name="y">The vector 'y' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements (output).</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void zaxpy(int n, Complex a, ReadOnlySpan<Complex> x, Span<Complex> y, int incX = 1, int incY = 1);

        /// <summary>Copies a vector to another vector, i.e. 
        /// <para>
        ///   y[k * <paramref name="incY"/>] = x[k * <paramref name="incX"/>]
        /// </para> for k=0,..,<paramref name="n"/>-1.
        /// </summary>
        /// <param name="n">The number of elements to copy.</param>
        /// <param name="x">The vector 'x' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="y">The vector 'y' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements; contains the updated vector on exit.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void zcopy(int n, ReadOnlySpan<Complex> x, Span<Complex> y, int incX = 1, int incY = 1);

        /// <summary>Computes a dot product of a conjugated vector with another vector, i.e. \sum conjugate(x) * y.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        /// <returns>The dot product of <paramref name="x"/> and <paramref name="y"/>, i.e. \sum conjugate(x) * y.</returns>
        Complex zdotc(int n, ReadOnlySpan<Complex> x, ReadOnlySpan<Complex> y, int incX = 1, int incY = 1);

        /// <summary>Computes a vector-vector product, i.e. \sum x * y.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        /// <returns>The vector-vector product of <paramref name="x"/> and <paramref name="y"/>, i.e. \sum x * y.</returns>
        Complex zdotu(int n, ReadOnlySpan<Complex> x, ReadOnlySpan<Complex> y, int incX = 1, int incY = 1);

        /// <summary>Performs rotation of points in the plane, i.e. x(i) = c * x(i) + s * y(i) and y(i) = c * y(i) - s * x(i).
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements; 'x(i) = c * x(i) + s * y(i)' after function evaluation.</param>
        /// <param name="y">The vector 'y' with at least <paramref name="n"/> elements; 'y(i) = c * y(i) - s * x(i)' after function evaluation.</param>
        /// <param name="c">The scalar 'c'.</param>
        /// <param name="s">The scalar 's'.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void zdrot(int n, Span<Complex> x, Span<Complex> y, double c, double s, int incX = 1, int incY = 1);

        /// <summary>Computes the product of a vector by a scalar, i.e. x[j * increment] = a * x[j * increment] for j = 0,..., n-1.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x' with at least 1 + (<paramref name="n"/> -1) * <paramref name="incX"/> elements; contains the updated vector after function call.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        void zdscal(int n, double a, Span<Complex> x, int incX = 1);

        /// <summary>Computes the Euclidean norm of a vector, i.e. ||x||.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="x">The vector 'x' with at least <paramref name="n"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <returns>The euclidian norm of <paramref name="x"/>, i.e. \sqrt(x_0^2 + ... + x_n^2).</returns>
        double znrm2(int n, ReadOnlySpan<Complex> x, int incX = 1);

        /// <summary>Computes the parameters for a Givens rotation; given the Cartesian coordinates (a, b) of a point p, these routines 
        /// return the parameters a, b, c, and s associated with the Givens rotation that zeros the y-coordinate of the point, i.e. 
        /// <para>(c s \\ -\conjugate(s) c) * (a \\ b) = (r \\ 0).</para>
        /// </summary>
        /// <param name="a">Provides the x-coordinate of the point p; contains the parameter r associated with the Givens rotation after function evaluation.</param>
        /// <param name="b">Provides the y-coordinate of the point p; contains the parameter z associated with the Givens rotation after function evaluation.</param>
        /// <param name="c">Contains the parameter c associated with the Givens rotation (output).</param>
        /// <param name="s">Contains the parameter s associated with the Givens rotation (output).</param>
        void zrotg(ref Complex a, ref Complex b, out double c, out Complex s);

        /// <summary>Computes the product of a vector by a scalar, i.e. x[j * increment] = a * x[j * increment], for j = 0,..., n-1.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/>.</param>
        /// <param name="a">The scalar factor 'a'.</param>
        /// <param name="x">The vector 'x' with at least 1 + (<paramref name="n"/> -1) * <paramref name="incX"/> elements; contains the updated vector after function call.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        void zscal(int n, Complex a, Span<Complex> x, int incX = 1);

        /// <summary>Swaps a vector with another vector, i.e. y[j * <paramref name="incY"/>] = x[j * <paramref name="incX"/>] and 
        /// x[j * <paramref name="incX"/>] y[j * <paramref name="incY"/>]  for j = 0,...,n-1.
        /// </summary>
        /// <param name="n">The number of elements of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="x">The vector 'x' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incX"/> elements.</param>
        /// <param name="y">The vector 'y' with at least 1 + ( <paramref name="n"/> - 1) * <paramref name="incY"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <param name="incY">The increment for <paramref name="y"/>.</param>
        void zswap(int n, Span<Complex> x, Span<Complex> y, int incX = 1, int incY = 1);

        /// <summary>Finds the index of the element with maximum absolute value, i.e. |Re(x(j + <paramref name="incX"/>))| + |Im(x( j + <paramref name="incX"/>))| with specified j.
        /// </summary>
        /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
        /// <param name="x">The vector with at least 1 + (<paramref name="n"/> -1 ) * <paramref name="incX"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <returns>The position of vector element <paramref name="x"/> that has the largest absolute value.</returns>
        int izamax(int n, ReadOnlySpan<Complex> x, int incX = 1);

        /// <summary>Finds the index of the element with smallest absolute value, i.e. |Re(x(j + <paramref name="incX"/>))| + |Im(x( j + <paramref name="incX"/>))| with specified j.
        /// </summary>
        /// <param name="n">The number of elements in vector <paramref name="x"/>.</param>
        /// <param name="x">The vector with at least 1 + (<paramref name="n"/> -1 ) * <paramref name="incX"/> elements.</param>
        /// <param name="incX">The increment for <paramref name="x"/>.</param>
        /// <returns>The position of vector element <paramref name="x"/> that has the smallest absolute value.</returns>
        /// <remarks>This method is not part of the BLAS standard.</remarks>
        int izamin(int n, ReadOnlySpan<Complex> x, int incX = 1);
        #endregion
    }
}