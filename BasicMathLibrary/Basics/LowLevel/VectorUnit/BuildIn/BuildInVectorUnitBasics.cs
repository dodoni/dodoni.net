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
        public void Add(int n, double[] a, double[] b, double[] y)
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
        public void Add(int n, double[] a, double[] b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] + b[j];
            }
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] + b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Add(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = a[startIndexA + j] + b[startIndexB + j];
            }
        }

        /// <summary>Performs addition of vector <paramref name="a"/> and double precision number <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input double precision number b.</param>
        /// <param name="y">The output vector a+b.</param>
        public void Add(int n, double[] a, double b, double[] y)
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
        public void Add(int n, double[] a, double b)
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
        public void Add(int n, Complex[] a, Complex[] b, Complex[] y)
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
        public void Add(int n, Complex[] a, Complex[] b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] + b[j];
            }
        }

        /// <summary>Performs element by element addition of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] + b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Add(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = a[startIndexA + j] + b[startIndexB + j];
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, double[] a, double[] b, double[] y)
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
        public void Sub(int n, double[] a, double[] b)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] - b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sub(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = a[startIndexA + j] - b[startIndexB + j];
            }
        }

        /// <summary>Performs element by element subtraction of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a-b.</param>
        public void Sub(int n, Complex[] a, Complex[] b, Complex[] y)
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
        public void Sub(int n, Complex[] a, Complex[] b)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] - b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sub(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = a[startIndexA + j] - b[startIndexB + j];
            }
        }

        /// <summary>Performs element by element squaring of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector a[j]^2, j=0,...,<paramref name="n"/>-1.</param>
        public void Sqr(int n, double[] a, double[] y)
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
        public void Sqr(int n, double[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] *= a[j];
            }
        }

        /// <summary>Performs element by element squaring of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^2 for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sqr(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = a[startIndexA + j] * a[startIndexA + j];
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, double[] a, double[] b, double[] y)
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
        public void Mul(int n, double[] a, double[] b)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] * b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Mul(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = a[startIndexA + j] * b[startIndexB + j];
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]*b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Mul(int n, Complex[] a, Complex[] b, Complex[] y)
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
        public void Mul(int n, Complex[] a, Complex[] b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = a[j] * b[j];
            }
        }

        /// <summary>Performs element by element multiplication of vector <paramref name="a"/> and vector <paramref name="b"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j] * b[<paramref name="startIndexY"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Mul(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = a[startIndexA + j] * b[startIndexB + j];
            }
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \conj(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Conjugate(int n, Complex[] a, Complex[] y)
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
        public void Conjugate(int n, Complex[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Conjugate(a[j]);
            }
        }

        /// <summary>Performs element by element conjugation of vector <paramref name="a"/>.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=\conj(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Conjugate(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Conjugate(a[startIndexA + j]);
            }
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, double[] a, double[] y)
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
        public void Abs(int n, double[] a)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=|a[<paramref name="startIndexA"/> + j]| for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Abs(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Abs(a[startIndexA + j]);
            }
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector |a[j]|, j=0,...,<paramref name="n"/>-1.</param>
        public void Abs(int n, Complex[] a, double[] y)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Complex.Abs(a[j]);
            }
        }

        /// <summary>Computes absolute value of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=|a[<paramref name="startIndexA"/> + j]| for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Abs(int n, Complex[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Abs(a[startIndexA + j]);
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
        public void LinearFraction(int n, double[] a, double[] b, double[] y, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = (scaleFactorA * a[j] + shiftA) / (scaleFactorB * b[j] + shiftB);
            }
        }

        /// <summary>Performs linear fraction transformation of vector <paramref name="a"/> and vector <paramref name="b"/> with scalar parameters.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := (<paramref name="scaleFactorA"/> * a[<paramref name="startIndexA"/> + j] + <paramref name="shiftA"/>) / (<paramref name="scaleFactorB"/> * b[<paramref name="startIndexB"/> + j] + <paramref name="shiftB"/>) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        /// <param name="scaleFactorA">The scaling factor for vector a.</param>
        /// <param name="scaleFactorB">The scaling factor for vector b.</param>
        /// <param name="shiftA">Constant value for shifting addends of vector a.</param>
        /// <param name="shiftB">Constant value for shifting addends of vector b.</param>
        public void LinearFraction(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY, double scaleFactorA, double scaleFactorB, double shiftA = 0.0, double shiftB = 0.0)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = (scaleFactorA * a[startIndexA + j] + shiftA) / (scaleFactorB * b[startIndexB + j] + shiftB);
            }
        }
        #endregion

        #region Power and root functions

        /// <summary>Performs element by element inversion of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector.</param>
        /// <param name="y">The output vector.</param>
        public void Inv(int n, double[] a, double[] y)
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
        public void Inv(int n, double[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = 1.0 / a[j];
            }
        }

        /// <summary>Performs element by element inversion of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=1.0 / a[<paramref name="startIndexA"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Inv(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = 1.0 / a[startIndexA + j];
            }
        }

        /// <summary>Performs element by element square root calculation of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sqrt(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sqrt(int n, double[] a, double[] y)
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
        public void Sqrt(int n, double[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Math.Sqrt(a[j]);
            }
        }

        /// <summary>Performs element by element square root calculation of the vector.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=\sqrt(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sqrt(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Sqrt(a[startIndexA + j]);
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, double[] a, double[] b, double[] y)
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
        public void Pow(int n, double[] a, double[] b)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b[<paramref name="startIndexB"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, double[] a, double[] b, double[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Pow(a[startIndexA + j], b[startIndexB + j]);
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector a[j]^b[j], j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, Complex[] a, Complex[] b, Complex[] y)
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
        public void Pow(int n, Complex[] a, Complex[] b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Pow(a[j], b[j]);
            }
        }

        /// <summary>Computes a to the power b for elements of two vectors.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The input vector b.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b[<paramref name="startIndexB"/> + j] for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexB">The null-based start index of <paramref name="b"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, Complex[] a, Complex[] b, Complex[] y, int startIndexA, int startIndexB, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Pow(a[startIndexA + j], b[startIndexB + j]);
            }
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, double[] a, double b, double[] y)
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
        public void Pow(int n, double[] a, double b)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, double[] a, double b, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Pow(a[startIndexA + j], b);
            }
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector a[j]^b, j=0,...,<paramref name="n"/>-1.</param>
        public void Pow(int n, Complex[] a, Complex b, Complex[] y)
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
        public void Pow(int n, Complex[] a, Complex b)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Pow(a[j], b);
            }
        }

        /// <summary>Raises each element of a vector to the constant power.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="b">The constant power.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] :=a[<paramref name="startIndexA"/> + j]^b for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Pow(int n, Complex[] a, Complex b, Complex[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Pow(a[startIndexA + j], b);
            }
        }
        #endregion

        #region Exponential and logarithmic functions

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Exp(int n, double[] a, double[] y)
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
        public void Exp(int n, double[] a)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \exp(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Exp(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Exp(a[startIndexA + j]);
            }
        }

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \exp(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Exp(int n, Complex[] a, Complex[] y)
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
        public void Exp(int n, Complex[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Exp(a[j]);
            }
        }

        /// <summary>Computes an exponential of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \exp(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Exp(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Exp(a[startIndexA + j]);
            }
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log(int n, double[] a, double[] y)
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
        public void Log(int n, double[] a)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Log(a[startIndexA + j]);
            }
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log(int n, Complex[] a, Complex[] y)
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
        public void Log(int n, Complex[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Log(a[j]);
            }
        }

        /// <summary>Computes natural logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Log(a[startIndexA + j]);
            }
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log10(int n, double[] a, double[] y)
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
        public void Log10(int n, double[] a)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log_10(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log10(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Log10(a[startIndexA + j]);
            }
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \log_10(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Log10(int n, Complex[] a, Complex[] y)
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
        public void Log10(int n, Complex[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Log10(a[j]);
            }
        }

        /// <summary>Computes denary logarithm of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \log_10(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Log10(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Log10(a[startIndexA + j]);
            }
        }
        #endregion

        #region Trigonometric functions

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Cos(int n, double[] a, double[] y)
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
        public void Cos(int n, double[] a)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \cos(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Cos(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Cos(a[startIndexA + j]);
            }
        }

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Cos(int n, Complex[] a, Complex[] y)
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
        public void Cos(int n, Complex[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Cos(a[j]);
            }
        }

        /// <summary>Computes cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \cos(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Cos(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Cos(a[startIndexA + j]);
            }
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sin(int n, double[] a, double[] y)
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
        public void Sin(int n, double[] a)
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
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \sin(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sin(int n, double[] a, double[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Sin(a[startIndexA + j]);
            }
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void Sin(int n, Complex[] a, Complex[] y)
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
        public void Sin(int n, Complex[] a)
        {
            for (int j = 0; j < n; j++)
            {
                a[j] = Complex.Sin(a[j]);
            }
        }

        /// <summary>Computes sine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \sin(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        public void Sin(int n, Complex[] a, Complex[] y, int startIndexA, int startIndexY)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Complex.Sin(a[startIndexA + j]);
            }
        }

        /// <summary>Computes sine and cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector \sin(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        /// <param name="z">The output vector \cos(a[j]), j=0,...,<paramref name="n"/>-1.</param>
        public void SinCos(int n, double[] a, double[] y, double[] z)
        {
            for (int j = 0; j < n; j++)
            {
                y[j] = Math.Sin(a[j]);
                z[j] = Math.Cos(a[j]);
            }
        }

        /// <summary>Computes sine and cosine of vector elements.
        /// </summary>
        /// <param name="n">The number of elements to be calculated.</param>
        /// <param name="a">The input vector a.</param>
        /// <param name="y">The output vector y with y[<paramref name="startIndexY"/> + j] := \sin(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="z">The output vector z with z[<paramref name="startIndexY"/> + j] := \cos(a[<paramref name="startIndexA"/> + j]) for j = 0,...,<paramref name="n"/>-1.</param>
        /// <param name="startIndexA">The null-based start index of <paramref name="a"/>.</param>
        /// <param name="startIndexY">The null-based start index of <paramref name="y"/>.</param>
        /// <param name="startIndexZ">The null-based start index of <paramref name="z"/>.</param>
        public void SinCos(int n, double[] a, double[] y, double[] z, int startIndexA, int startIndexY, int startIndexZ)
        {
            for (int j = 0; j < n; j++)
            {
                y[startIndexY + j] = Math.Sin(a[startIndexA + j]);
                z[startIndexZ + j] = Math.Cos(a[startIndexA + j]);
            }
        }
        #endregion

        #endregion
    }
}