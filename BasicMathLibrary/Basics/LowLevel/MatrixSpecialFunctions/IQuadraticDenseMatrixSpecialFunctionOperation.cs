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
namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Represents a specific scalar function with respect to a specified <see cref="DenseMatrix"/> object.
    /// </summary>
    /// <param name="t">The scalar parameter.</param>
    /// <param name="resultMatrixEntries">Contains on exit the column-by-column provided matrix entries of the result; at least dim(A)^2 elements, where A is the specified dense (quadratic) matrix.</param>
    /// <param name="work">Serves as workspace array for the calculation with a suitable minimum length; if <c>null</c> no workspace array will be used.</param>
    /// <returns>The result of the calculation, where the <see cref="DenseMatrix.Data"/> property is a shallow copy of <paramref name="resultMatrixEntries"/>.</returns>
    public delegate DenseMatrix DenseMatrixRealScalarFunction(double t, double[] resultMatrixEntries, double[] work = null);

    /// <summary>Serves as interface for calculation of a specific matrix operation with respect to a quadratic dense matrix, for example exp(A), ln(A), sin(A) etc.
    /// </summary>
    public interface IQuadraticDenseMatrixSpecialFunctionOperation
    {
        /// <summary>Gets the value of the specified matrix operation.
        /// </summary>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <returns>The value of the specified operation in its <see cref="DenseMatrix"/> representation.</returns>
        DenseMatrix GetValue(DenseMatrix a);

        /// <summary>Gets the value of the specified matrix operation.
        /// </summary>
        /// <value>The value of the specified matrix operation.</value>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <returns>The value of the specified operation in its <see cref="DenseMatrix"/> representation.</returns>
        DenseMatrix this[DenseMatrix a]
        {
            get;
        }

        /// <summary>Gets a optimal workspace array length for the calculation of the specified matrix operation for a specified dimension.
        /// </summary>
        /// <param name="dimension">The dimension of the (quadratic) matrix.</param>
        /// <returns>The optimal workspace array length.</returns>
        int GetWorkspaceLength(int dimension);

        /// <summary>Gets the value of the specified matrix operation.
        /// </summary>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <param name="resultMatrixEntries">Contains on exit the column-by-column provided matrix entries of the result; at least (dimension of <paramref name="a"/>)^2 elements.</param>
        /// <param name="work">Serves as workspace array for the calculation with minimum length specified by <see cref="GetWorkspaceLength(int)"/>; if <c>null</c> no workspace array will be used.</param>
        /// <returns>The value of the specified matrix operation in its <see cref="DenseMatrix"/> representation, where the <see cref="DenseMatrix.Data"/> property is a shallow copy of <paramref name="resultMatrixEntries"/>.</returns>
        /// <remarks>This method can be used to avoid memory allocation for the calculation of several matrix operations of the same type.</remarks>
        DenseMatrix GetValue(DenseMatrix a, double[] resultMatrixEntries, double[] work = null);

        /// <summary>Creates a <see cref="DenseMatrixRealScalarFunction"/> object that encapsulate the specified matrix operation f(t * <paramref name="a"/>) for real parameter t.
        /// </summary>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <returns>A <see cref="DenseMatrixRealScalarFunction"/> object that encapsulate the specified matrix operation f(t * <paramref name="a"/>) for real parameter t.</returns>
        DenseMatrixRealScalarFunction Create(DenseMatrix a);
    }
}