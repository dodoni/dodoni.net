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
using System.Data;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Represents the 'Eigenvalues zeroing by iteration (EZI)' algorithm to create a valid correlation matrix and if desired a rank reduction.
    ///</summary>
    /// <remarks>Based on M. Morini, N. Webber 'An EZI method to reduce the rank of a correlation matrix', 2004.
    /// <para>This implementation returns pseudo-sqrt matrices which should be scaled with -1.0 and the columns should be re-order to get the same results as the reference. The correlation matrices are identical in both cases.</para></remarks>
    public partial class EziMatrixDecomposer : PseudoSqrtMatrixDecomposer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the algorithm.
        /// </summary>
        public static readonly EziMatrixDecomposerAbortCondition StandardAbortCondition;
        #endregion

        #region private members

        /// <summary>The name of the correlation matrix decomposer.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="EziMatrixDecomposer" /> class.
        /// </summary>
        static EziMatrixDecomposer()
        {
            StandardAbortCondition = EziMatrixDecomposerAbortCondition.Create();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EziMatrixDecomposer"/> class.
        /// </summary>
        /// <param name="maximalRank">The maximal rank of the resulting matrix.</param>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <remarks>The <see cref="EziMatrixDecomposer.StandardAbortCondition"/> is taken into account.</remarks>
        public EziMatrixDecomposer(int maximalRank, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
            : this(maximalRank, StandardAbortCondition, infoOutputDetailLevel)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EziMatrixDecomposer"/> class.
        /// </summary>
        /// <param name="maximalRank">The maximal rank of the resulting matrix.</param>
        /// <param name="abortCondition">The abort (stopping) condition for the EZI algorithm.</param>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        public EziMatrixDecomposer(int maximalRank, EziMatrixDecomposerAbortCondition abortCondition, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
        {
            MaximalRank = maximalRank;
            AbortCondition = abortCondition;
            m_Name = new IdentifierString(String.Format("EZI Decomposer; {0}", abortCondition.ToString()));
        }
        #endregion

        #region public properties

        /// <summary>Gets the maximal rank of the correlation matrices calculated by the current instance; if <c>null</c> negative eigenvalues are set to 0.0 only.
        /// </summary>
        /// <value>The maximal rank of the correlation matrices calculated by the current instance; if <c>null</c> negative eigenvalues are set to 0.0 only.</value>
        public int? MaximalRank
        {
            get;
            private set;
        }

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public EziMatrixDecomposerAbortCondition AbortCondition
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
        public override WorkspaceContainer CreateWorkspaceContainer(int dimension)
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
        public override DenseMatrix Create(DenseMatrix rawCorrelationMatrix, out State state, double[] outputEntries = null, WorkspaceContainer worksspaceContainer = null, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix)
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

            /* calculate an initial value for the EZI approach, i.e. apply the Eigenvalue zeroing (without normalization): */
            int initialRank;
            BLAS.Level1.dcopy(n * n, rawCorrelationMatrix.Data, ws.p);
            f1(n, ws.p, 0, ws, outputEntries, out initialRank);

            var detailDataTables = new List<DataTable>();
            if (InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High))
            {
                detailDataTables.Add(CreateDataTableWithEigenvalues("Eigenvalues.Adjusted[0]", initialRank, ws.eigenValues));
            }

            BLAS.Level3.dgemm(n, n, n, 1.0, outputEntries, outputEntries, 0.0, ws.p, transposeB: BLAS.MatrixTransposeState.Transpose);

            BLAS.Level1.dcopy(n * n, ws.p, ws.q);
            int minNumberOfEigenvaluesToSetZero = n - Math.Min(MaximalRank ?? n, initialRank);

            var a = 1.0;
            int rank = initialRank;
            for (int k = 1; k < AbortCondition.MaxIterations; k++)
            {
                f1(n, ws.q, minNumberOfEigenvaluesToSetZero, ws, outputEntries, out rank);
                if (InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.Full))
                {
                    detailDataTables.Add(CreateDataTableWithEigenvalues(String.Format("Eigenvalues.Adjusted[{0}", k), rank, ws.eigenValues));
                }

                BLAS.Level3.dgemm(n, n, n, 1.0, outputEntries, outputEntries, 0.0, ws.q, transposeB: BLAS.MatrixTransposeState.Transpose);

                /* normalize the correlation matrix, i.e. calculate <q> */
                for (int i = 0; i < n; i++)
                {
                    var t_i = 0.0;
                    for (int j = n - 1; j >= n - rank; j--)
                    {
                        t_i += outputEntries[i + j * n] * outputEntries[i + j * n];
                    }
                    BLAS.Level1.dscal(rank, 1.0 / Math.Sqrt(t_i), outputEntries, -n, i + (n - 1) * n); // [i+j*n] *= 1/Math.Sqrt(t_i) for j = n-1, ..., n-rank
                }
                BLAS.Level3.dgemm(n, n, n, 1.0, outputEntries, outputEntries, 0.0, ws.qNormalized, transposeB: BLAS.MatrixTransposeState.Transpose);

                if (AbortCondition.IsSatisfied(n, ws.p, ws.q, ws.qNormalized, ref a) == true)
                {
                    /* The eigenvalues are in ascending order. Thefore the first (and not last) 'rank' columns of the eigenvectors are not required. Therefore we swap the relevant part: */
                    BLAS.Level1.dswap(n * rank, outputEntries, 1, outputEntries, 1, n * (n - rank), 0);
                    state = State.Create(rank, new InfoOutputProperty[] { InfoOutputProperty.Create("Eigenvalues set to 0.0", n - rank) }, detailDataTables, iterationsNeeded: k, infoOutputDetailLevel: InfoOutputDetailLevel);
                    return new DenseMatrix(n, rank, outputEntries);
                }

                BLAS.Level1.dcopy(n * n, ws.q, ws.p); // set p := q
                for (int j = 0; j < n; j++)
                {
                    ws.q[j * n + j] = 1.0;
                }
            }

            /* The eigenvalues are in ascending order. Thefore the first (and not last) 'rank' columns of the eigenvectors are not required. Therefore we swap the relevant part: */
            BLAS.Level1.dswap(n * rank, outputEntries, 1, outputEntries, 1, n * (n - rank), 0);

            state = State.Create(rank, new InfoOutputProperty[] { InfoOutputProperty.Create("Number of EigenValues set to 0.0", n - rank) }, detailDataTables, AbortCondition.MaxIterations, infoOutputDetailLevel: InfoOutputDetailLevel);
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

        #region private methods

        /// <summary>Apply the Eigenvalue zeroing without normalization, i.e. f1 in §3.1.1. of the reference.
        /// </summary>
        /// <param name="n">The dimension of the (raw) correlation matrix.</param>
        /// <param name="rawCorrelationMatrix">The (raw) correlation matrix of dimension <paramref name="n"/> provided column-by-column; will be changed on exit.</param>
        /// <param name="minNumberOfEigenvaluesToSetZero"></param>
        /// <param name="ws">The workspace in its <see cref="Workspace"/> representation.</param>
        /// <param name="decomposedMatrix">The entries of the decomposed matrix B (output), where B * B^t is an approximation for the <paramref name="rawCorrelationMatrix"/>; provided column-by-column where the number of
        /// rows corresponds to the <paramref name="n"/> and the number of columns corresponds to <paramref name="rank"/>.</param>
        /// <param name="rank">The rank of the calculated <paramref name="decomposedMatrix"/> (output).</param>
        private void f1(int n, double[] rawCorrelationMatrix, int minNumberOfEigenvaluesToSetZero, Workspace ws, double[] decomposedMatrix, out int rank)
        {
            int m;
            LAPACK.EigenValues.Symmetric.driver_dsyevr(LapackEigenvalues.SymmetricGeneralJob.All, n, rawCorrelationMatrix, out m, ws.eigenValues, decomposedMatrix, ws.isuppz, ws.work, ws.iwork);

            rank = m;

            int i = 0;
            while ((i < m) && ((i < minNumberOfEigenvaluesToSetZero) || (ws.eigenValues[i] < 0.0)))
            {
                ws.eigenValues[i] = 0.0;
                i++;
                rank--;
            }

            VectorUnit.Basics.Sqrt(n, ws.eigenValues); // calculate sqrt of eigenvalues only once, i.e. the array 'eigenValues' contains the sqrt of the eigenvalues!
            for (i = 0; i < n; i++)
            {
                for (int j = n - 1; j >= n - rank; j--)
                {
                    decomposedMatrix[i + j * n] *= ws.eigenValues[j];
                }
            }

            /* The eigenvalues are in ascending order. Thefore the first (and not last) 'rank' columns of the eigenvectors are not required and should be set to 0.0. Moreover in 
             * the final result one has to swap the relevant columns - we will not do it in each iteration step. */
            BLAS.Level1.dscal(n * (n - rank), 0.0, decomposedMatrix);
        }
        #endregion
    }
}