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
    /// <summary>Provides vector operations with respect to generic mathematical functions.
    /// </summary>
    public interface IVectorUnitBasics
    {
        #region Arithmetic functions

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        void Add(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y);

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        void Add(int n, Span<double> a, ReadOnlySpan<double> b);

        /// <summary>Performs addition of vector <paramref name="a"/> and double precision number <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        /// <param name="y">The output vector a+b.</param>
        void Add(int n, ReadOnlySpan<double> a, double b, Span<double> y);

        /// <summary>Performs addition of vector <paramref name="a"/> and double precision number <paramref name="b"/>, i.e. a_j = a_j + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input double precision number b.</param>
        void Add(int n, Span<double> a, double b);

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        void Add(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y);

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        void Add(int n, Span<Complex> a, ReadOnlySpan<Complex> b);

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        void Sub(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y);

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        void Sub(int n, Span<double> a, ReadOnlySpan<double> b);

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        void Sub(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y);

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        void Sub(int n, Span<Complex> a, ReadOnlySpan<Complex> b);

        /// <summary>Performs element by element squaring of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector a[j]^2, j=0,...,<paramref name="n"/>-1.</param>
        void Sqr(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Performs element by element squaring of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^2, j=0,...,<paramref name="n"/>-1.</param>
        void Sqr(int n, Span<double> a);

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        void Mul(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y);

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="b">The input vector b.</param>
        void Mul(int n, Span<double> a, ReadOnlySpan<double> b);

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        void Mul(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y);

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="b">The input vector b.</param>
        void Mul(int n, Span<Complex> a, ReadOnlySpan<Complex> b);

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \conj(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Conjugate(int n, ReadOnlySpan<Complex> a, Span<Complex> y);

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result ot the operation, i.e. \conj(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Conjugate(int n, Span<Complex> a);

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        void Abs(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes absolute value of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the resulf ot the operation, i.e. |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        void Abs(int n, Span<double> a);

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        void Abs(int n, ReadOnlySpan<Complex> a, Span<double> y);

        /// <summary>Performs linear fraction transformation of vector <paramref name="a"/> and vector <paramref name="b"/> with scalar parameters.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y[j]= (<paramref name="scaleFactorA"/> * a[j] + <paramref name="shiftA"/>) / (<paramref name="scaleFactorB"/> * b[j] + <paramref name="shiftB"/>), j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="scaleFactorA">The scaling factor for vector a.</param>
        /// <param name="scaleFactorB">The scaling factor for vector b.</param>
        /// <param name="shiftA">Constant value for shifting addends of vector a.</param>
        /// <param name="shiftB">Constant value for shifting addends of vector b.</param>
        void LinearFraction(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0);
        #endregion

        #region Power and root functions

        /// <summary>Performs element by element inversion of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="y">The output vector.</param>
        void Inv(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Performs element by element inversion of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector; overwritten by the result of the operation.</param>
        void Inv(int n, Span<double> a);

        /// <summary>Performs element by element square root calculation of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sqrt(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Sqrt(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Performs element by element square root calculation of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sqrt(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Sqrt(int n, Span<double> a);

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        void Pow(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y);

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="b">The input vector b.</param>
        void Pow(int n, Span<double> a, ReadOnlySpan<double> b);

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        void Pow(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y);

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="b">The input vector b.</param>
        void Pow(int n, Span<Complex> a, ReadOnlySpan<Complex> b);

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        void Pow(int n, ReadOnlySpan<double> a, double b, Span<double> y);

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="b">The constant power.</param>
        void Pow(int n, Span<double> a, double b);

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        void Pow(int n, ReadOnlySpan<Complex> a, Complex b, Span<Complex> y);

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="b">The constant power.</param>
        void Pow(int n, Span<Complex> a, Complex b);
        #endregion

        #region Exponential and logarithmic functions

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Exp(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Exp(int n, Span<double> a);

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Exp(int n, ReadOnlySpan<Complex> a, Span<Complex> y);

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Exp(int n, Span<Complex> a);

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log(int n, Span<double> a);

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log(int n, ReadOnlySpan<Complex> a, Span<Complex> y);

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log(int n, Span<Complex> a);

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log10(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log10(int n, Span<double> a);

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log10(int n, ReadOnlySpan<Complex> a, Span<Complex> y);

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Log10(int n, Span<Complex> a);
        #endregion

        #region Trigonometric functions

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Cos(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Cos(int n, Span<double> a);

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Cos(int n, ReadOnlySpan<Complex> a, Span<Complex> y);

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Cos(int n, Span<Complex> a);

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Sin(int n, ReadOnlySpan<double> a, Span<double> y);

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Sin(int n, Span<double> a);

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Sin(int n, ReadOnlySpan<Complex> a, Span<Complex> y);

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void Sin(int n, Span<Complex> a);

        /// <summary>Computes sine and cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="z">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        void SinCos(int n, ReadOnlySpan<double> a, Span<double> y, Span<double> z);
        #endregion
    }
}