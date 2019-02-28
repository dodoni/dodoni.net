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

using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary
{
    public partial class DiagonalMatrix
    {
        /// <summary>Multiplies a diagonal matrix with a dense matrix.
        /// </summary>
        /// <param name="diagonalMatrix">The diagonal matrix (left side).</param>
        /// <param name="denseMatrix">The dense matrix (right side).</param>
        /// <returns>The <see cref="DenseMatrix"/> object that represents the product of both specified matrices.</returns>
        public static DenseMatrix operator *(DiagonalMatrix diagonalMatrix, DenseMatrix denseMatrix)
        {
            MatrixSpecialFunction.TestMultiplicationInput(diagonalMatrix, denseMatrix);

            int rowCount = denseMatrix.RowCount;
            int columnCount = denseMatrix.ColumnCount;

            var data = new double[rowCount * columnCount];

            switch (denseMatrix.DataTransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:  // here, we use BLAS functions to speed up operation
                    BLAS.Level1.dcopy(rowCount * columnCount, denseMatrix.Data, data);

                    for (int i = 0; i < rowCount; i++)
                    {
                        BLAS.Level1.dscal(columnCount, diagonalMatrix[i, i], data.AsSpan().Slice(i), rowCount);  // c[i + RowCount * j] *= diagonal[i,i]
                    }
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    for (int i = 0; i < rowCount; i++)
                    {
                        double diagonalFactor = diagonalMatrix[i, i];
                        for (int j = 0; j < columnCount; j++)
                        {
                            data[i + rowCount * j] = diagonalFactor * denseMatrix[i, j];
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return new DenseMatrix(rowCount, columnCount, data);
        }

        /// <summary>Multiplies a diagonal matrix with a dense matrix.
        /// </summary>
        /// <param name="denseMatrix">The dense matrix (left side).</param>
        /// <param name="diagonalMatrix">The diagonal matrix (right side).</param>        
        /// <returns>The <see cref="DenseMatrix"/> object that represents the product of both specified matrices.</returns>
        public static DenseMatrix operator *(DenseMatrix denseMatrix, DiagonalMatrix diagonalMatrix)
        {
            MatrixSpecialFunction.TestMultiplicationInput(denseMatrix, diagonalMatrix);

            int rowCount = denseMatrix.RowCount;
            int columnCount = denseMatrix.ColumnCount;

            var data = new double[rowCount * columnCount];

            switch (denseMatrix.DataTransposeState)
            {
                case BLAS.MatrixTransposeState.NoTranspose:  // here, we use BLAS functions to speed up operation
                    BLAS.Level1.dcopy(rowCount * columnCount, denseMatrix.Data, data);

                    for (int j = 0; j < columnCount; j++)
                    {
                        BLAS.Level1.dscal(rowCount, diagonalMatrix[j, j], data.AsSpan().Slice(rowCount * j), 1);  // c[i + RowCount * j] *= diagonal[j,j]
                    }
                    break;

                case BLAS.MatrixTransposeState.Transpose:
                    for (int j = 0; j < columnCount; j++)
                    {
                        double diagonalFactor = diagonalMatrix[j, j];
                        for (int i = 0; i < rowCount; i++)
                        {
                            data[i + rowCount * j] = denseMatrix[i, j] * diagonalFactor;
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return new DenseMatrix(rowCount, columnCount, data);
        }
    }
}