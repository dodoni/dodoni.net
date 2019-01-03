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
using System.Numerics;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides methods for the creation of matrices, i.e. for BLAS and Lapack.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class MatrixStorageConversion
    {
        /// <summary>Initializes a new instance of the <see cref="MatrixStorageConversion" /> class.
        /// </summary>
        internal MatrixStorageConversion()
        {
        }

        /// <summary>Creates a dense diagonal matrix (supplied column-by-column).
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="diagonalElements">The diagonal elements.</param>
        /// <returns>The dense diagonal matrix supplied column-by-column of dimension <paramref name="dimension"/> x <paramref name="dimension"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="diagonalElements"/> is <c>null</c>.</exception>
        public double[] CreateDenseDiagonalMatrix(int dimension, double[] diagonalElements)
        {
            if (diagonalElements == null)
            {
                throw new ArgumentNullException(nameof(diagonalElements));
            }
            var values = new double[dimension * dimension];  // [j*(n+1)] = [j+ j*n] = diag(j) for j = 0,...,n-1

            BLAS.Level1.dcopy(dimension, diagonalElements, values, 1, dimension + 1);
            return values;
        }

        /// <summary>Creates a dense diagonal matrix (supplied column-by-column).
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="diagonalElements">The diagonal elements.</param>
        /// <returns>The dense diagonal matrix supplied column-by-column of dimension <paramref name="dimension"/> x <paramref name="dimension"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="diagonalElements"/> is <c>null</c>.</exception>
        public Complex[] CreateDenseDiagonalMatrix(int dimension, Complex[] diagonalElements)
        {
            if (diagonalElements == null)
            {
                throw new ArgumentNullException(nameof(diagonalElements));
            }
            var values = new Complex[dimension * dimension];  // [j*(n+1)] = [j+ j*n] = diag(j) for j = 0,...,n-1

            BLAS.Level1.zcopy(dimension, diagonalElements, values, 1, dimension + 1);
            return values;
        }

        /// <summary>Creates a dense diagonal matrix (supplied column-by-column).
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="diagonalElements">The diagonal elements.</param>
        /// <param name="denseMatrixEntries">The dense diagonal matrix supplied column-by-column of dimension <paramref name="dimension"/> x <paramref name="dimension"/> (output); should contain at least <paramref name="dimension"/> x <paramref name="dimension"/> elements.</param>
        /// <returns>A reference of <paramref name="denseMatrixEntries"/> that contains the dense diagonal matrix supplied column-by-column of dimension <paramref name="dimension"/> x <paramref name="dimension"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="diagonalElements"/> of <paramref name="denseMatrixEntries"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the length of <paramref name="denseMatrixEntries"/> does not fit the requirements.</exception>
        /// <remarks>Use this method if one wants to re-use memory for performance reason.</remarks>
        public double[] CreateDenseDiagonalMatrix(int dimension, double[] diagonalElements, double[] denseMatrixEntries)
        {
            if (diagonalElements == null)
            {
                throw new ArgumentNullException(nameof(diagonalElements));
            }
            if (denseMatrixEntries == null)
            {
                throw new ArgumentNullException(nameof(denseMatrixEntries));
            }
            if (denseMatrixEntries.Length < dimension * dimension)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentHasWrongDimension, "denseMatrixEntries"), nameof(denseMatrixEntries));
            }
            /* 
             * denseMatrixEntries[j * (n+1)] = denseMatrixEntries[j+ j * n] = diag(j) for j = 0,...,n-1
             */
            BLAS.Level1.dcopy(dimension, diagonalElements, denseMatrixEntries, 1, dimension + 1);
            return denseMatrixEntries;
        }

        /// <summary>Creates a dense diagonal matrix (supplied column-by-column).
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="diagonalElements">The diagonal elements.</param>
        /// <param name="denseMatrixEntries">The dense diagonal matrix supplied column-by-column of dimension <paramref name="dimension"/> x <paramref name="dimension"/> (output); should contain at least <paramref name="dimension"/> x <paramref name="dimension"/> elements.</param>
        /// <returns>A reference of <paramref name="denseMatrixEntries"/> that contains the dense diagonal matrix supplied column-by-column of dimension <paramref name="dimension"/> x <paramref name="dimension"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="diagonalElements"/> of <paramref name="denseMatrixEntries"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the length of <paramref name="denseMatrixEntries"/> does not fit the requirements.</exception>
        /// <remarks>Use this method if one wants to re-use memory for performance reason.</remarks>
        public Complex[] CreateDenseDiagonalMatrix(int dimension, Complex[] diagonalElements, Complex[] denseMatrixEntries)
        {
            if (diagonalElements == null)
            {
                throw new ArgumentNullException(nameof(diagonalElements));
            }
            if (denseMatrixEntries == null)
            {
                throw new ArgumentNullException(nameof(denseMatrixEntries));
            }
            if (denseMatrixEntries.Length < dimension * dimension)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentHasWrongDimension, "denseMatrixEntries"), nameof(denseMatrixEntries));
            }
            /* 
             * denseMatrixEntries[j * (n+1)] = denseMatrixEntries[j+ j * n] = diag(j) for j = 0,...,n-1
             */
            BLAS.Level1.zcopy(dimension, diagonalElements, denseMatrixEntries, 1, dimension + 1);
            return denseMatrixEntries;
        }

        /// <summary>Creates a dense diagonal matrix (supplied column-by-column).
        /// </summary>
        /// <param name="diagonalElements">The diagonal elements.</param>
        /// <returns>The dense diagonal matrix supplied column-by-column of dimension n x n, where n is th enumber of <paramref name="diagonalElements"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="diagonalElements"/> is <c>null</c>.</exception>
        public double[] CreateDenseDiagonalMatrix(params double[] diagonalElements)
        {
            if (diagonalElements == null)
            {
                throw new ArgumentNullException(nameof(diagonalElements));
            }
            int n = diagonalElements.Length;
            double[] values = new double[n * n];  // [j*(n+1)] = [j+ j*n] = diag(j) for j = 0,...,n-1

            BLAS.Level1.dcopy(n, diagonalElements, values, 1, n + 1, 0, 0);
            return values;
        }

        /// <summary>Creates a dense diagonal matrix (supplied column-by-column).
        /// </summary>
        /// <param name="diagonalElements">The diagonal elements.</param>
        /// <returns>The dense diagonal matrix supplied column-by-column of dimension n x n, where n is th enumber of <paramref name="diagonalElements"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="diagonalElements"/> is <c>null</c>.</exception>
        public Complex[] CreateDenseDiagonalMatrix(params Complex[] diagonalElements)
        {
            if (diagonalElements == null)
            {
                throw new ArgumentNullException(nameof(diagonalElements));
            }
            int n = diagonalElements.Length;
            var values = new Complex[n * n];  // [j*(n+1)] = [j+ j*n] = diag(j) for j = 0,...,n-1

            BLAS.Level1.zcopy(n, diagonalElements, values, 1, n + 1, 0, 0);
            return values;
        }

        /// <summary>Converts a specific dense band matrix into general band matrix storage.
        /// </summary>
        /// <param name="denseBandMatrix">The dense band matrix, where the first null-based index corresponds to the row, the second to the column index.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonals of the matrix.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals of the matrix.</param>
        /// <param name="bandMatrix">The band matrix in the band matrix storage with at least
        /// (<paramref name="subDiagonalCount"/> + <paramref name="superDiagonalCount"/> + 1) * <paramref name="columnCount"/> elements (output).</param>
        /// <remarks>This method can be used for example for "ILevel2BLAS.dgbmv(int, int, int, int, double, double[], BLAS.MatrixTransposeState, double[], double, double[]).
        /// <para>The first <paramref name="superDiagonalCount"/> as well as the last <paramref name="subDiagonalCount"/> elements
        /// are undefined - these are values 'outside' the matrix and ignored in general.</para></remarks>
        public void CreateGeneralBandMatrix(double[,] denseBandMatrix, int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount, double[] bandMatrix)
        {
            int bandMatrixIndex = superDiagonalCount; // the first super diagonal elements are 'outside' the matrix and beliefe unchanged

            for (int j = 0; j < columnCount; j++)
            {
                for (int i = Math.Max(0, j - superDiagonalCount); i <= Math.Min(rowCount - 1, j + subDiagonalCount); i++)
                {
                    bandMatrix[bandMatrixIndex++] = denseBandMatrix[i, j];
                }
            }
        }

        /// <summary>Converts a specific dense band matrix into general band matrix storage.
        /// </summary>
        /// <param name="denseBandMatrix">The dense band matrix, where the first null-based index corresponds to the row, the second to the column index.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="subDiagonalCount">The number of sub-diagonals of the matrix.</param>
        /// <param name="superDiagonalCount">The number of super-diagonals of the matrix.</param>
        /// <param name="bandMatrix">The band matrix in the band matrix storage with at least
        /// (<paramref name="subDiagonalCount"/> + <paramref name="superDiagonalCount"/> + 1) * <paramref name="columnCount"/> elements (output).</param>
        /// <remarks>This method can be used for example for "ILevel2BLAS.dgbmv(int, int, int, int, double, double[], BLAS.MatrixTransposeState, double[], double, double[])".
        /// <para>The first <paramref name="superDiagonalCount"/> as well as the last <paramref name="subDiagonalCount"/> elements
        /// are undefined - these are values 'outside' the matrix and ignored in general.</para></remarks>
        public void CreateGeneralBandMatrix(double[][] denseBandMatrix, int rowCount, int columnCount, int subDiagonalCount, int superDiagonalCount, double[] bandMatrix)
        {
            int bandMatrixIndex = superDiagonalCount; // the first super diagonal elements are 'outside' the matrix and beliefe unchanged

            for (int j = 0; j < columnCount; j++)
            {
                for (int i = Math.Max(0, j - superDiagonalCount); i <= Math.Min(rowCount - 1, j + subDiagonalCount); i++)
                {
                    bandMatrix[bandMatrixIndex++] = denseBandMatrix[i][j];
                }
            }
        }

        /// <summary>Performs in-place transposition of a specific matrix.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="denseMatrix">The matrix provided column-by-column (column-major ordering).</param>
        public void InPlaceTranspose(int rowCount, int columnCount, double[] denseMatrix)
        {
            BLAS.Level3.aux_dgetrans(rowCount, columnCount, denseMatrix);
        }
    }
}