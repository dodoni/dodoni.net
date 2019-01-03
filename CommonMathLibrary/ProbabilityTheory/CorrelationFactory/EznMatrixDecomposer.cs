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

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Represents some eigenvalue zeroing (Eigenvalue zeroing with normalization - EZN) to create a valid correlation matrix and if desired a rank reduction.
    /// </summary>
    /// <remarks>Based on R. Rebonato, P. Jäckel, 'The most general methodology to create a valid correlation matrix for risk management and option pricing purposes', Quantitative Research Center of the NaWest Group.
    /// <para>This implementation returns pseudo-sqrt matrices which should be scaled with -1.0 and the columns should be re-order to get the same results as the reference. The correlation matrices are identical in both cases.</para></remarks>
    public partial class EznMatrixDecomposer : PseudoSqrtMatrixDecomposer
    {
        #region private members

        /// <summary>The name of the correlation matrix Decomposer.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EznMatrixDecomposer" /> class.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        public EznMatrixDecomposer(InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
            : base(infoOutputDetailLevel)
        {
            MaximalRank = null;
            m_Name = new IdentifierString("EZN Decomposer");
        }

        /// <summary>Initializes a new instance of the <see cref="EznMatrixDecomposer" /> class.
        /// </summary>
        /// <param name="maximalRank">The maximal rank of the resulting matrix.</param>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        public EznMatrixDecomposer(int maximalRank, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
            : base(infoOutputDetailLevel)
        {
            MaximalRank = maximalRank;
            m_Name = new IdentifierString(String.Format("EZN Decomposer; max. rank: {0}", maximalRank));
        }
        #endregion

        #region public propertie

        /// <summary>Gets the maximal rank of the correlation matrices calculated by the current instance; if <c>null</c> negative eigenvalues are set to 0.0 only.
        /// </summary>
        /// <value>The maximal rank of the correlation matrices calculated by the current instance; if <c>null</c> negative eigenvalues are set to 0.0 only.</value>
        public int? MaximalRank
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object preferable for frequently operation of the same type. 
        /// </summary>
        /// <param name="dimension">The dimension of correlation matrices for which the workspace should be suited.</param>
        /// <returns>A specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object.</returns>
        public override PseudoSqrtMatrixDecomposer.WorkspaceContainer CreateWorkspaceContainer(int dimension)
        {
            return new Workspace(dimension);
        }

        /// <summary>Creates a 'n x r' matrix B such that B * B' is a correlation matrix and 'near' to the specified symmetric, normalized matrix of dimension n. A rank reduction will apply if r is strict less than n.
        /// </summary>
        /// <param name="rawCorrelationMatrix">The symmetric, normalized matrix where to find the 'nearest' correlation matrix.</param>
        /// <param name="state">The state of the operation in its <see cref="PseudoSqrtMatrixDecomposer.State"/> representation (output).</param>
        /// <param name="triangularMatrixType">A value indicating which part of <paramref name="rawCorrelationMatrix"/> to take into account.</param>
        /// <param name="outputEntries">This argument will be used to store the matrix entries of the resulting matrix B, i.e. the return value array points to this array if != <c>null</c>; otherwise a memory allocation will be done.</param>
        /// <param name="worksspaceContainer">A specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object to reduce memory allocation; ignored if <c>null</c>.</param>
        /// <returns>A <see cref="DenseMatrix"/> object that represents a matrix B such that B * B' is the 'nearest' correlation matrix with respect to <paramref name="rawCorrelationMatrix"/>.</returns>
        /// <remarks>In general the return object does <b>not</b> represents the pseudo-root of <paramref name="rawCorrelationMatrix"/>, i.e. output of the Cholesky decomposition.
        /// <para>The parameters <paramref name="outputEntries"/>, <paramref name="worksspaceContainer"/> allows to avoid memory allocation and to re-use arrays if the calculation of correlation matrices will be done often.</para></remarks>
        public override DenseMatrix Create(DenseMatrix rawCorrelationMatrix, out State state, double[] outputEntries = null, PseudoSqrtMatrixDecomposer.WorkspaceContainer worksspaceContainer = null, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
        {
            if (rawCorrelationMatrix.IsQuadratic == false)
            {
                throw new ArgumentException("rawCorrelationMatrix");
            }
            int n = rawCorrelationMatrix.RowCount;

            if ((outputEntries == null) || (outputEntries.Length < n * n))
            {
                outputEntries = new double[n * n];
            }
            var ws = worksspaceContainer as Workspace;
            if ((ws == null) || (ws.Dimension < n))
            {
                ws = new Workspace(n);
            }

            int m;
            BLAS.Level1.dcopy(n * n, rawCorrelationMatrix.Data, ws.data);
            LAPACK.EigenValues.Symmetric.driver_dsyevr(LapackEigenvalues.SymmetricGeneralJob.All, n, ws.data, out m, ws.eigenValues, outputEntries, ws.isuppz, ws.work, ws.iwork);
            var originalEigenValueDataTable = InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High) ? CreateDataTableWithEigenvalues("Eigenvalues.Original", m, ws.eigenValues) : null;

            int rank = n;
            int minNumberOfEigenvaluesToSetZero = n - Math.Min(MaximalRank ?? n, n);
            int i = 0;
            while ((i < minNumberOfEigenvaluesToSetZero) || (ws.eigenValues[i] < 0.0))
            {
                ws.eigenValues[i] = 0.0;
                i++;
                rank--;
            }
            var adjustedEigenValueDataTable = InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High) ? CreateDataTableWithEigenvalues("Eigenvalues.Adjusted", m, ws.eigenValues) : null;

            VectorUnit.Basics.Sqrt(n, ws.eigenValues); // calculate sqrt of eigenvalues only once, i.e. the array 'eigenValues' contains the sqrt of the eigenvalues!
            for (i = 0; i < n; i++)
            {
                var t_i = 0.0;
                for (int j = n - 1; j >= n - rank; j--)
                {
                    t_i += outputEntries[i + j * n] * outputEntries[i + j * n] * ws.eigenValues[j] * ws.eigenValues[j];
                    outputEntries[i + j * n] *= ws.eigenValues[j];
                }
                BLAS.Level1.dscal(rank, 1.0 / Math.Sqrt(t_i), outputEntries, -n, i + (n - 1) * n); // [i+j*n] *= 1/Math.Sqrt(tempValue) for j = n-1, ..., n-rank
            }

            /* The eigenvalues are in ascending order. Thefore the first (and not last) 'rank' columns of the eigenvectors are not required. Therefore we swap the relevant part  */
            BLAS.Level1.dscal(n * (n - rank), 0.0, outputEntries);
            BLAS.Level1.dswap(n * rank, outputEntries, 1, outputEntries, 1, n * (n - rank), 0);

            state = State.Create(rank, detailProperties: new[] { InfoOutputProperty.Create("Eigenvalues set to 0.0", n - rank) }, detailDataTables: new[] { originalEigenValueDataTable, adjustedEigenValueDataTable }, iterationsNeeded: 1, infoOutputDetailLevel: InfoOutputDetailLevel);
            return new DenseMatrix(n, rank, outputEntries);
        }

        /// <summary>Creates a 'n x r' matrix B such that B * B' is a correlation matrix and 'near' to the specified symmetric, normalized matrix of dimension n. A rank reduction will apply if r is strict less than n.
        /// </summary>
        /// <param name="rawCorrelationMatrix">The symmetric, normalized matrix where to find the 'nearest' correlation matrix.</param>
        /// <param name="rank">The rank of the matrix B, which should corresponds to the number of columns (output).</param>
        /// <param name="outputEntries">This argument will be used to store the matrix entries of the resulting matrix B, i.e. the return value array points to this array if != <c>null</c>; otherwise a memory allocation will be done.</param>
        /// <returns>A <see cref="DenseMatrix" /> object that represents a matrix B such that B * B' is the 'nearest' correlation matrix with respect to <paramref name="rawCorrelationMatrix" />.</returns>
        /// <remarks>
        /// In general the return object does <b>not</b> represents the pseudo-root of <paramref name="rawCorrelationMatrix" />, i.e. output of the Cholesky decomposition.
        /// <para>The parameter <paramref name="outputEntries" /> allowes to avoid memory allocation and to re-use arrays if the calculation of correlation matrices will be done often.</para>
        /// </remarks>
        public DenseMatrix Create(SymmetricMatrix rawCorrelationMatrix, out int rank, double[] outputEntries = null)
        {
            int n = rawCorrelationMatrix.RowCount;

            if ((outputEntries == null) || (outputEntries.Length < n * n))
            {
                outputEntries = new double[n * n];
            }
            var work = new double[3 * n];
            var eigenValues = new double[n];
            var data = new double[n * (n + 1) / 2];
            BLAS.Level1.dcopy(n * (n + 1) / 2, rawCorrelationMatrix.Data, data);

            LAPACK.EigenValues.Symmetric.driver_dspev(LapackEigenvalues.SymmetricGeneralJob.All, n, data, eigenValues, outputEntries, work);

            int minNumberOfEigenvaluesToSetZero = n - Math.Min(MaximalRank ?? n, n);
            int i = 0;
            rank = n;
            while ((i < minNumberOfEigenvaluesToSetZero) || (eigenValues[i] < 0.0))
            {
                eigenValues[i] = 0.0;
                i++;
                rank--;
            }
            VectorUnit.Basics.Sqrt(n, eigenValues); // calculate sqrt of eigenvalues only once

            for (i = 0; i < n; i++)
            {
                var t_i = 0.0;
                for (int j = n - 1; j >= n - rank; j--)
                {
                    t_i += outputEntries[i + j * n] * outputEntries[i + j * n] * eigenValues[j] * eigenValues[j];
                    outputEntries[i + j * n] *= eigenValues[j];
                }
                BLAS.Level1.dscal(rank, 1.0 / Math.Sqrt(t_i), outputEntries, -n, i + (n - 1) * n); // [i+j*n] *= 1/Math.Sqrt(tempValue) for j = n-1, ..., n-rank
            }

            /* The eigenvalues are in ascending order. Thefore the first (and not last) 'rank' columns of the eigenvectors are not required. Therefore we swap the relevant part  */
            BLAS.Level1.dscal(n * (n - rank), 0.0, outputEntries);  // set all not relevant elements of the array to 0.0
            BLAS.Level1.dswap(n * rank, outputEntries, 1, outputEntries, 1, n * (n - rank), 0);

            return new DenseMatrix(n, rank, outputEntries);
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the correlation matrix decomposer.
        /// </summary>
        /// <returns>The name of the correlation matrix decomposer.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the correlation matrix decomposer.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the correlation matrix decomposer.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_Name;
        }
        #endregion
    }
}