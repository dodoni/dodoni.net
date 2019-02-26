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
    /// <summary>Provides a managed .net implementation of vector operations with respect to (generic) mathematical functions.
    /// </summary>
    [Serializable]
    internal class BuildInVectorUnitBasics : IVectorUnitBasics
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BuildInVectorUnitBasics"/> class.
        /// </summary>
        internal BuildInVectorUnitBasics()
        {
        }
        #endregion

        #region IVectorUnitBasics Members

        #region Arithmetic functions

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] + b[j];
            }
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, Span<double> a, ReadOnlySpan<double> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] + b[j];
            }
        }

        /// <summary>Performs addition of vector <paramref name="a"/> and double precision number <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, ReadOnlySpan<double> a, double b, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] + b;
            }
        }

        /// <summary>Performs addition of vector <paramref name="a" /> and double precision number <paramref name="b" />, i.e. a_j = a_j + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input double precision number b.</param>
        public void Add(int n, Span<double> a, double b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] += b;
            }
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] + b[j];
            }
        }

        /// <summary>Performs element by element addition of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a + b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a + b.</param>
        /// <param name="b">The input vector b.</param>
        public void Add(int n, Span<Complex> a, ReadOnlySpan<Complex> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] + b[j];
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] - b[j];
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, Span<double> a, ReadOnlySpan<double> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] - b[j];
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] - b[j];
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a" /> and vector <paramref name="b" />, i.e. a = a - b [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a - b.</param>
        /// <param name="b">The input vector b.</param>
        public void Sub(int n, Span<Complex> a, ReadOnlySpan<Complex> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] - b[j];
            }
        }

        /// <summary>Performs element by element squaring of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector a[j]^2, j=0,...,<paramref name="n"/>-1.</param>
        public void Sqr(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] * a[j];
            }
        }

        /// <summary>Performs element by element squaring of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^2, j=0,...,<paramref name="n" />-1.</param>
        public void Sqr(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] *= a[j];
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] * b[j];
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, Span<double> a, ReadOnlySpan<double> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] *= b[j];
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = a[j] * b[j];
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a" /> and vector <paramref name="b" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]*b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Mul(int n, Span<Complex> a, ReadOnlySpan<Complex> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] * b[j];
            }
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \conj(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Conjugate(int n, ReadOnlySpan<Complex> a, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Conjugate(a[j]);
            }
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a" /> [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result ot the operation, i.e. \conj(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Conjugate(int n, Span<Complex> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Conjugate(a[j]);
            }
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Abs(a[j]);
            }
        }

        /// <summary>Computes absolute value of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the resulf ot the operation, i.e. |a[j]|, j=0,...,<paramref name="n" />-1.</param>
        public void Abs(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Abs(a[j]);
            }
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, ReadOnlySpan<Complex> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Abs(a[j]);
            }
        }

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
        public void LinearFraction(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = (scaleFactorA * a[j] + shiftA) / (scaleFactorB * b[j] + shiftB);
            }
        }
        #endregion

        #region Power and root functions

        /// <summary>Performs element by element inversion of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="y">The output vector.</param>
        public void Inv(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = 1.0 / a[j];
            }
        }

        /// <summary>Performs element by element inversion of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector; overwritten by the result of the operation.</param>
        public void Inv(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = 1.0 / a[j];
            }
        }

        /// <summary>Performs element by element square root calculation of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sqrt(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sqrt(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Sqrt(a[j]);
            }
        }

        /// <summary>Performs element by element square root calculation of the vector [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sqrt(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sqrt(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Sqrt(a[j]);
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, ReadOnlySpan<double> a, ReadOnlySpan<double> b, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Pow(a[j], b[j]);
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, Span<double> a, ReadOnlySpan<double> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Pow(a[j], b[j]);
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, ReadOnlySpan<Complex> a, ReadOnlySpan<Complex> b, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Pow(a[j], b[j]);
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b[j], j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The input vector b.</param>
        public void Pow(int n, Span<Complex> a, ReadOnlySpan<Complex> b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Pow(a[j], b[j]);
            }
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, ReadOnlySpan<double> a, double b, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Pow(a[j], b);
            }
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, Span<double> a, double b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Pow(a[j], b);
            }
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, ReadOnlySpan<Complex> a, Complex b, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Pow(a[j], b);
            }
        }

        /// <summary>Raises each element of a vector to the constant power [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. a[j]^b, j=0,...,<paramref name="n" />-1.</param>
        /// <param name="b">The constant power.</param>
        public void Pow(int n, Span<Complex> a, Complex b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Pow(a[j], b);
            }
        }
        #endregion

        #region Exponential and logarithmic functions

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Exp(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Exp(a[j]);
            }
        }

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Exp(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Exp(a[j]);
            }
        }

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Exp(int n, ReadOnlySpan<Complex> a, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Exp(a[j]);
            }
        }

        /// <summary>Computes an exponential of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \exp(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Exp(int n, Span<Complex> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Exp(a[j]);
            }
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Log(a[j]);
            }
        }

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Log(a[j]);
            }
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log(int n, ReadOnlySpan<Complex> a, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Log(a[j]);
            }
        }

        /// <summary>Computes natural logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log(int n, Span<Complex> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Log(a[j]);
            }
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log10(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Log10(a[j]);
            }
        }

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log10(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Log10(a[j]);
            }
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log10(int n, ReadOnlySpan<Complex> a, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Log10(a[j]);
            }
        }

        /// <summary>Computes denary logarithm of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \log_10(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Log10(int n, Span<Complex> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Log10(a[j]);
            }
        }
        #endregion

        #region Trigonometric functions

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Cos(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Cos(a[j]);
            }
        }

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Cos(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Cos(a[j]);
            }
        }

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Cos(int n, ReadOnlySpan<Complex> a, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Cos(a[j]);
            }
        }

        /// <summary>Computes cosine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \cos(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Cos(int n, Span<Complex> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Cos(a[j]);
            }
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sin(int n, ReadOnlySpan<double> a, Span<double> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Sin(a[j]);
            }
        }

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sin(int n, Span<double> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Sin(a[j]);
            }
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sin(int n, ReadOnlySpan<Complex> a, Span<Complex> y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Sin(a[j]);
            }
        }

        /// <summary>Computes sine of vector elements [in-place].
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a; overwritten by the result of the operation, i.e. \sin(a[j]), j=0,...,<paramref name="n" />-1.</param>
        public void Sin(int n, Span<Complex> a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Sin(a[j]);
            }
        }

        /// <summary>Computes sine and cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="z">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void SinCos(int n, ReadOnlySpan<double> a, Span<double> y, Span<double> z)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Sin(a[j]);
                z[j] = Math.Cos(a[j]);
            }
        }
        #endregion

        #endregion
    }
}