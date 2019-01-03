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
using Dodoni.MathLibrary.Optimizer;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Represents the Standard Angles Parameterization' (SAP) algorithm to create a valid correlation matrix and if desired a rank reduction.
    /// </summary>
    /// <remarks>Base on "Volatility and Correlation", R. Rebonato, Wiley 1999.</remarks>
    public partial class SapMatrixDecomposer : PseudoSqrtMatrixDecomposer
    {
        #region private members

        /// <summary>The name of the correlation matrix Decomposer.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The EZN approach used for the calculation of an initial value in the optimization approach.
        /// </summary>
        private EznMatrixDecomposer m_InitialDecomposer;

        /// <summary>The optimizer approach.
        /// </summary>
        private OrdinaryMultiDimOptimizer m_MultiDimOptimizer;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="SapMatrixDecomposer" /> class.
        /// </summary>
        /// <param name="multiDimOptmizer">The multi-dimensional </param>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        public SapMatrixDecomposer(OrdinaryMultiDimOptimizer multiDimOptmizer, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
            : base(infoOutputDetailLevel)
        {
            if (multiDimOptmizer == null)
            {
                throw new ArgumentNullException("multiDimOptmizer");
            }
            m_MultiDimOptimizer = multiDimOptmizer;
            m_InitialDecomposer = new EznMatrixDecomposer();
            m_Name = new IdentifierString("SAP Decomposer");
        }

        /// <summary>Initializes a new instance of the <see cref="SapMatrixDecomposer" /> class.
        /// </summary>
        /// <param name="maximalRank">The maximal rank of the resulting matrix.</param>
        /// <param name="multiDimOptmizer">The multi-dimensional </param>        
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        public SapMatrixDecomposer(int maximalRank, OrdinaryMultiDimOptimizer multiDimOptmizer, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
            : base(infoOutputDetailLevel)
        {
            if (multiDimOptmizer == null)
            {
                throw new ArgumentNullException("multiDimOptimizer");
            }
            m_MultiDimOptimizer = multiDimOptmizer;
            m_InitialDecomposer = new EznMatrixDecomposer(maximalRank);
            m_Name = new IdentifierString(String.Format("SAP Decomposer; max. rank: {0}", maximalRank));
        }
        #endregion

        #region public properties

        /// <summary>Gets the maximal rank of the correlation matrices calculated by the current instance; if <c>null</c> negative eigenvalues are set to 0.0 only.
        /// </summary>
        /// <value>The maximal rank of the correlation matrices calculated by the current instance; if <c>null</c> negative eigenvalues are set to 0.0 only.</value>
        public int? MaximalRank
        {
            get { return m_InitialDecomposer.MaximalRank; }
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object preferable for frequently operation of the same type. 
        /// </summary>
        /// <param name="dimension">The dimension of correlation matrices for which the workspace should be suited.</param>
        /// <returns>A specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object.</returns>
        public override PseudoSqrtMatrixDecomposer.WorkspaceContainer CreateWorkspaceContainer(int dimension)
        {
            return new Workspace(dimension, this);
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

            var ws = worksspaceContainer as Workspace;
            if ((ws == null) || (ws.Dimension < n))
            {
                ws = new Workspace(n, this);
            }

            /* calculate an initial value for the optimizer: 
             *   (i) Apply the EZN algorithm for the calculation of a matrix B_0 such that B_0 * B_0^t is near to the (raw) correlation matrix 
             *   (ii) calculate angle parameters \theta such that B_0 = B(\theta).
             *  */
            State initState;
            var initialAngleParameterMatrix = GetAngleParameter(m_InitialDecomposer.Create(rawCorrelationMatrix, out initState, outputEntries, ws.InitalDecomposerWorkspace, triangularMatrixType), ws.ArgMinData);

            /* prepare and apply optimization algorithm: */
            int rank = initState.Rank;
            if ((outputEntries == null) || (outputEntries.Length < n * n))
            {
                outputEntries = new double[n * rank];
            }

            var B = new DenseMatrix(n, rank, outputEntries, createDeepCopyOfArgument: false);
            var C = new DenseMatrix(n, n, ws.CorrelationMatrixData, createDeepCopyOfArgument: false);
            var optAlgorithm = m_MultiDimOptimizer.Create(n * (rank - 1));

            optAlgorithm.SetFunction(theta =>
            {
                GetParametricMatrix(theta, B);
                C.AddAssignment(B, B.T, beta: 0.0); // C = B * B^t

                VectorUnit.Basics.Sub(n * n, C.Data, rawCorrelationMatrix.Data, ws.TempDifferences);
                return BLAS.Level1.dnrm2sq(n * n, ws.TempDifferences);
            });
            double minimum;
            var optState = optAlgorithm.FindMinimum(ws.ArgMinData, out minimum);

            state = State.Create(rank, optState.IterationsNeeded, InfoOutputDetailLevel,
                Tuple.Create<string, IInfoOutputQueriable>("Initial.State", initState),
                Tuple.Create<string, IInfoOutputQueriable>("Final.Optimizer", optState),
                Tuple.Create<string, IInfoOutputQueriable>("Initial.Parameters", initialAngleParameterMatrix),
                InfoOutputDetailLevel.IsAtLeastAsComprehensiveAs(InfoOutputDetailLevel.High) ? Tuple.Create<string, IInfoOutputQueriable>("Final.Parameters", new DenseMatrix(n, rank, ws.ArgMinData, createDeepCopyOfArgument: false)) : null
                );

            GetParametricMatrix(ws.ArgMinData, B); // B should be already set to B(\theta^*), we just want to be sure that B is correct on exit
            return B;
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

        #region public static methods

        /// <summary>Gets the angle parameter \theta for a specific decomposed correlation matrix, i.e. determine \theta with B(\theta) = B, where B * B^t is some correlation matrix.
        /// </summary>
        /// <param name="decomposedMatrix">The decomposed matrix B, i.e. a n-by-r matrix where n corresponds to the dimension of the correlation matrix B * B^t and r corresponds to the rank of B.</param>
        /// <param name="theta">The n * (r - 1) angle parameter \theta (output).</param>
        /// <returns>The <see cref="DenseMatrix"/> representation of the angle parameter <paramref name="theta"/>.</returns>
        /// <remarks>The parameter \theta is specified in the following way:
        /// \theta_{1,1}, \theta_{1,2}, ..., \theta_{1,r-1},
        /// \theta_{2,1}, \theta_{2,2}, ..., \theta{2,r-1},
        /// ...
        /// \theta_{M,1}, \theta_{M,2}, ..., \theta_{M,r-1},
        /// where r is the rank and M is the dimension of the raw correlation matrix.
        /// </remarks>
        public static DenseMatrix GetAngleParameter(DenseMatrix decomposedMatrix, double[] theta)
        {
            int n = decomposedMatrix.RowCount;
            int rank = decomposedMatrix.ColumnCount;

            var thetaMatrix = new DenseMatrix(n, rank - 1, theta, createDeepCopyOfArgument: false);

            for (int i = 0; i < n; i++)
            {
                thetaMatrix[i, 0] = ACos(decomposedMatrix[i, 0]);

                var sinProduct = 1.0;
                for (int k = 1; k < rank - 1; k++)
                {
                    sinProduct *= Math.Sin(thetaMatrix[i, k - 1]);
                    thetaMatrix[i, k] = ACos(decomposedMatrix[i, k] / sinProduct);
                }
                /* the sign of the parameter \theta should be choose in way that for the input B(\theta) exactly \theta will be returned.
                 * Therefore one may change the sign of the last parameter \theta_{i,r-2} [corresponds to \theta_{i,n-1} in the notation of the reference,
                 * i.e. it should hold b_{i,n} = sin(\theta_{i,1}) ... \sin(\theta_{i,n-1}) and we check whether left/right side have the same sign].
                 */
                if (Math.Sign(decomposedMatrix[i, rank - 1]) * Math.Sign(sinProduct) == -1.0)  // the last expression is idential to: Math.Sign(sinProduct * Math.Sin(thetaMatrix[i, rank - 2])), because thetaMatrix[i, rank - 2] is between 0 and PI
                {
                    thetaMatrix[i, rank - 2] *= -1.0;
                }
            }
            return thetaMatrix;
        }

        /// <summary>Gets matrix B(\theta), where \theta represents a specific angle parameter set.
        /// </summary>
        /// <param name="theta">The parameter \theta, i.e. at least n * (r - 1) elements, where n corresponds to the dimension of the correlation matrix B * B^t, 
        /// i.e the number of columns of <paramref name="B"/> and r corresponds to the rank of <paramref name="B"/>, i.e. the number of rows of <paramref name="B"/>.</param>
        /// <param name="B">The matrix B to be filled in its <see cref="DenseMatrix"/> representation (output).</param>
        /// <remarks>The parameter \theta is specified in the following way:
        /// \theta_{1,1}, \theta_{1,2}, ..., \theta_{1,r-1},
        /// \theta_{2,1}, \theta_{2,2}, ..., \theta{2,r-1},
        /// ...
        /// \theta_{M,1}, \theta_{M,2}, ..., \theta_{M,r-1},
        /// where r is the rank and M is the dimension of the raw correlation matrix.
        /// </remarks>
        public static void GetParametricMatrix(double[] theta, DenseMatrix B)
        {
            int n = B.RowCount;
            int rank = B.ColumnCount;

            for (int i = 0; i < n; i++)
            {
                B[i, 0] = Math.Cos(theta[i]);

                var sinProduct = 1.0;

                for (int k = 1; k < rank - 1; k++)
                {
                    sinProduct *= Math.Sin(theta[i + (k - 1) * n]);
                    B[i, k] = sinProduct * Math.Cos(theta[i + k * n]);
                }
                B[i, rank - 1] = sinProduct * Math.Sin(theta[i + (rank - 2) * n]);
            }
        }
        #endregion

        #region private static methods

        /// <summary>Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="x">A number representing a cosine; apply a floor of -1.0 and a cap of 1.0.</param>
        /// <returns>An angle, θ, measured in radians; perhaps a floor or cap of <paramref name="x"/> is taken into account.</returns>
        ///<remarks>Because of rounding errors we apply a floor and cap to the argument.</remarks>
        private static double ACos(double x)
        {
            return Math.Acos(Math.Max(-1.0, Math.Min(x, 1.0)));
        }
        #endregion
    }
}