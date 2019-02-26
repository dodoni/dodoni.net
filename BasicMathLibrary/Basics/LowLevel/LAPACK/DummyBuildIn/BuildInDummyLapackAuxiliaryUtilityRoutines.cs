﻿/* MIT License
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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>A dummy implementation for LAPACK routines.
    /// </summary>
    /// <seealso cref="Dodoni.MathLibrary.Basics.LowLevel.LapackAuxiliaryUtilityRoutines.IMatrix" />
    internal class BuildInDummyLapackAuxiliaryUtilityRoutines : LapackAuxiliaryUtilityRoutines.IMatrix
    {
        public double dlangb(MatrixNormType matrixNormType, int n, int kl, int ku, ReadOnlySpan<double> a, Span<double> work)
        {
            throw new NotImplementedException();
        }

        public double dlange(MatrixNormType matrixNormType, int m, int n, ReadOnlySpan<double> a, Span<double> work)
        {
            throw new NotImplementedException();
        }

        public double dlansp(MatrixNormType matrixNormType, int n, ReadOnlySpan<double> ap, Span<double> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }

        public double zlangb(MatrixNormType matrixNormType, int n, int kl, int ku, ReadOnlySpan<Complex> a, Span<Complex> work)
        {
            throw new NotImplementedException();
        }

        public double zlange(MatrixNormType matrixNormType, int m, int n, ReadOnlySpan<Complex> a, Span<Complex> work)
        {
            throw new NotImplementedException();
        }

        public double zlansp(MatrixNormType matrixNormType, int n, ReadOnlySpan<Complex> ap, Span<Complex> work, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            throw new NotImplementedException();
        }
    }
}