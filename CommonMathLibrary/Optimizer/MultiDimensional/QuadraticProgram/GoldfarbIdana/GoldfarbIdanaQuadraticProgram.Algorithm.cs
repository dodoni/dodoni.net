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
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public partial class GoldfarbIdanaQuadraticProgram
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IMultiDimOptimizerAlgorithm
        {
            #region private members

            /// <summary>The <see cref="GoldfarbIdanaQuadraticProgram"/> object that serves as factory of the current object.
            /// </summary>
            private GoldfarbIdanaQuadraticProgram m_Optimizer;

            /// <summary>The objective function in its <see cref="QuadraticProgramFunction"/> representation.
            /// </summary>
            private QuadraticProgramFunction m_ObjectiveFunction;

            #region constraints

            /// <summary>The inequality constraint matrix C with C' * x >= c, where the number of rows corresponds to the dimension of the feasible region; perhaps <c>null</c>.
            /// </summary>
            private DenseMatrix m_ConstraintInequalityMatrix;

            /// <summary>The inequality constraint vector c with C' * x >= c, with k relevant entries, where k is the numer of columns of matrix C, i.e. the number of inequality constraints; perhaps <c>null</c>.
            /// </summary>
            private double[] m_ConstraintInequalityVector;

            /// <summary>The equality constraint matrix D with D' * x = d, where the number of rows corresponds to the dimension of the feasible region; perhaps <c>null</c>.
            /// </summary>
            private DenseMatrix m_ConstraintEqualityMatrix;

            /// <summary>The equality constraint vector d with D' * x = d, with k relevant entries, where k is the numer of columns of matrix D, i.e. the number of equality constraints; perhaps <c>null</c>.
            /// </summary>
            private double[] m_ConstraintEqualityVector;
            #endregion

            #region intermediate data

            /// <summary>A copy of matrix 'A' in the optimization 1/2 * x^t * A * x + b^t * x.
            /// </summary>
            private double[] m_CopyOfMatrixA;  // todo: store symmetric part only?

            /// <summary>A working copy of the arg min (argumentum maximi) 'x'.
            /// </summary>
            private double[] m_TempArgMin;

            /// <summary>The vector 'n^+' with respect to equation (3.20).
            /// </summary>
            private double[] m_nPlusVector;

            /// <summary>The vector 'd', 'd = J^t * n^+' with respect to equation (4.7).
            /// </summary>
            private double[] m_dVector;

            /// <summary>The vector 'z', 'z = J_2 * d_2' with respect to equation (4.8).
            /// </summary>
            private double[] m_zVector;

            /// <summary>The upper triangular matrix 'R' with respect to equation (4.2).
            /// </summary>
            private double[] m_MatrixR;

            /// <summary>The matrix 'J' with respect to equation (4.6).
            /// </summary>
            private double[][] m_MatrixJ;
            #endregion

            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="GoldfarbIdanaQuadraticProgram"/> object that serves as factory of the current object.</param>
            /// <param name="dimension">The dimension of the feasible region.</param>
            internal Algorithm(GoldfarbIdanaQuadraticProgram optimizer, int dimension)
            {
                m_Optimizer = optimizer ?? throw new ArgumentNullException(nameof(optimizer));
                if (dimension <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimension));
                }
                Dimension = dimension;

                m_dVector = new double[dimension];
                m_zVector = new double[dimension];
                m_MatrixR = new double[dimension * dimension];
                m_MatrixJ = new double[dimension][];

                m_nPlusVector = new double[dimension];
                m_TempArgMin = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    m_MatrixJ[j] = new double[dimension];
                }
                m_CopyOfMatrixA = new double[dimension * dimension];
            }

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="GoldfarbIdanaQuadraticProgram"/> object that serves as factory of the current object.</param>
            /// <param name="dimension">The dimension of the feasible region.</param>
            /// <param name="inequalityMatrix">The representation of the inequality constraint matrix C with C' * x >= c, provided column-by-column, where the number of rows corresponds to the dimension of the feasible region.</param>
            /// <param name="inequalityVector">The inequality constraint vector c with C' * x >= c, with k relevant entries, where k is the numer of columns of matrix C, i.e. the number of inequality constraints.</param>
            /// <param name="equalityMatrix">The equality constraint matrix D with D' * x = d, provided column-by-column, where the number of rows corresponds to the dimension of the feasible region.</param>            
            /// <param name="equalityVector">The equality constraint vector d with D' * x = d, with k relevant entries, where k is the numer of columns of matrix D, i.e. the number of equality constraints; perhaps <c>null</c>.</param>
            internal Algorithm(GoldfarbIdanaQuadraticProgram optimizer, int dimension, List<double> inequalityMatrix, List<double> inequalityVector, List<double> equalityMatrix, List<double> equalityVector)
                : this(optimizer, dimension)
            {
                if ((inequalityMatrix != null) && (inequalityMatrix.Count > 0))
                {
                    if ((inequalityMatrix.Count % dimension) != 0)
                    {
                        throw new ArgumentException(nameof(inequalityMatrix));
                    }
                    m_ConstraintInequalityMatrix = new DenseMatrix(dimension, inequalityMatrix.Count / dimension, inequalityMatrix.ToArray());
                    m_ConstraintInequalityVector = inequalityVector.ToArray();
                }
                if ((equalityMatrix != null) && (equalityMatrix.Count > 0))
                {
                    if ((equalityMatrix.Count % dimension) != 0)
                    {
                        throw new ArgumentException(nameof(equalityMatrix));
                    }
                    m_ConstraintEqualityMatrix = new DenseMatrix(dimension, equalityMatrix.Count / dimension, equalityMatrix.ToArray());
                    m_ConstraintEqualityVector = equalityVector.ToArray();
                }
            }
            #endregion

            #region public properties

            #region IMultiDimOptimizerAlgorithm Members

            /// <summary>Gets the factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type, i.e. with the same stopping condition etc.
            /// </summary>
            /// <value>The factory for further <see cref="IMultiDimOptimizerAlgorithm"/> objects of the same type.</value>
            public MultiDimOptimizer Factory
            {
                get { return m_Optimizer; }
            }

            /// <summary>Gets the dimension of the feasible region.
            /// </summary>
            /// <value>The dimension.</value>
            public int Dimension
            {
                get;
                private set;
            }

            /// <summary>Gets or sets the objective function in its <see cref="MultiDimOptimizer.IFunction"/> representation.
            /// </summary>
            /// <value>The objective function.</value>
            public MultiDimOptimizer.IFunction Function
            {
                get { return m_ObjectiveFunction; }
                set
                {
                    if (value is QuadraticProgramFunction)
                    {
                        m_ObjectiveFunction = (QuadraticProgramFunction)value;
                        if (m_ObjectiveFunction.A.RowCount != Dimension)
                        {
                            throw new InvalidOperationException(String.Format(ExceptionMessages.ArgumentHasWrongDimension, "Matrix 'A' in objective Function 1/2 * x^t * A * x + b^t * x"));
                        }
                    }
                    else
                    {
                        throw new InvalidCastException();
                    }
                }
            }
            #endregion

            #endregion

            #region private properties

            /// <summary>Gets the abort (stopping) condition of the algorithm.
            /// </summary>
            /// <value>The abort (stopping) condition of the algorithm.</value>
            public GoldfarbIdanaQuadraticProgramAbortCondition AbortCondition
            {
                get { return m_Optimizer.AbortCondition; }
            }
            #endregion

            #region public methods

            #region IMultiDimOptimizerAlgorithm Members

            /// <summary>Finds the minimum and argmin of <see cref="IMultiDimOptimizerAlgorithm.Function"/>.
            /// </summary>
            /// <param name="x">An array with at least <see cref="IMultiDimOptimizerAlgorithm.Dimension"/> elements which is perhaps taken into account as an initial guess of the algorithm; on exit this argument contains the argmin.</param>
            /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="x"/> which represents the argmin (output).</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="x"/> and <paramref name="minimum"/> contain valid data.</returns>
            public State FindMinimum(double[] x, out double minimum)
            {
                if (m_ObjectiveFunction == null)
                {
                    throw new InvalidOperationException("No objective function");
                }
                int n = Dimension;

                BLAS.Level1.dcopy(n * n, m_ObjectiveFunction.A.Data, m_CopyOfMatrixA);
                var b = m_ObjectiveFunction.b;

                int inequalityConstraintCount = m_ConstraintInequalityMatrix?.ColumnCount ?? 0;
                int equalityConstraintCount = m_ConstraintEqualityMatrix?.ColumnCount ?? 0;
                int constraintCount = equalityConstraintCount + inequalityConstraintCount;

                int evaluationsNeeded = 1;
                var traceOfA = m_ObjectiveFunction.A.GetTrace();

                /* 1.) Solve the unconstraint optimization problem, i.e. min_x 0.5 * x^t * A * x + b^t * x. 
                 * The solution is x_0 = -A^{-1} * b, i.e. A*x_0 = -b. 
                 * 
                 * It holds 
                 * 0.5 *x_0^t*A*x_0 + b^t * x_0 = 0.5 * [ b^t * (A^{-1})^t * b] - b^t *A^{-1} * b 
                 *   = -0.5 * [b^t * A^{-1} * b] = 0.5 * b^t * x_0. 
                 * */
                BLAS.Level1.dcopy(n, b, x);
                LAPACK.LinearEquations.Solver.driver_dposv(n, m_CopyOfMatrixA, x);

                BLAS.Level1.dscal(n, -1.0, x);
                minimum = 0.5 * BLAS.Level1.ddot(n, b, x);

                if (constraintCount == 0)
                {
                    return State.Create(OptimizerErrorClassification.ProperResult, minimum, iterationsNeeded: 1);
                }

                /* 2.) Compute the 'reduced' inverse Hessian operator 'H', which will not be store and compute directly. We use the same notation as in '§ 4. Numerically stable implementation' 
                 * of the reference. Here we set 'J', 'stepDirection "z"' and \trace(A), the other variables are 0.0. */

                var conditionEstimation = 0.0; /* conditionEstimation * trace(A) is an estimation for cond(A). */
                for (int j = 0; j < n; j++)
                {
                    // set m_MatrixJ[j] = e_j and solve L*x=e_j:
                    double[] rowOfJ = m_MatrixJ[j];
                    BLAS.Level1.dscal(n, 0.0, rowOfJ);
                    rowOfJ[j] = 1.0;

                    BLAS.Level2.dtrsv(n, m_CopyOfMatrixA, rowOfJ, BLAS.TriangularMatrixType.LowerTriangularMatrix, isUnitTriangular: false);
                    conditionEstimation += rowOfJ[j];
                }

                /* 3.) Add equality constraints to the active set: */
                BLAS.Level1.dscal(n * n, 0.0, m_MatrixR);  // set R[i,j] = 0

                var activeSet = new int[constraintCount];
                var rVector = new double[constraintCount];
                var uVector = new double[constraintCount];

                int constraintIndex = 0;
                double fullStepLength;
                double normOfR = 1.0; /* the norm of the matrix R */

                for (int i = 0; i < equalityConstraintCount; i++)
                {
                    BLAS.Level1.dcopy(n, m_ConstraintEqualityMatrix.Data, m_nPlusVector, 1, 1, i * n, 0);  // set 'n^+[j] = equalityConstraintMatrix[j, i]' for i=0,...,n-1
                    ComputeVectorD(n, m_MatrixJ, m_nPlusVector, m_dVector);
                    UpdateVectorZ(n, m_MatrixJ, m_dVector, constraintIndex, m_zVector);
                    UpdateVectorR(n, m_MatrixR, m_dVector, constraintIndex, rVector);

                    /* compute full step length, i.e. the minimum step in primal space s.t. the contraint becomes feasible */
                    fullStepLength = 0.0;
                    if (Math.Abs(BLAS.Level1.ddot(n, m_zVector, m_zVector)) > MachineConsts.Epsilon)
                    {
                        fullStepLength = -(BLAS.Level1.ddot(n, m_nPlusVector, x) - m_ConstraintEqualityVector[i]) / BLAS.Level1.ddot(n, m_zVector, m_nPlusVector);
                    }
                    BLAS.Level1.daxpy(n, fullStepLength, m_zVector, x); /* set 'x += fullStepLength * z': */

                    uVector[constraintIndex] = fullStepLength;
                    BLAS.Level1.daxpy(constraintIndex, -fullStepLength, rVector, uVector); /* set 'u[k] -= fullStepLength * r[k]' for k=0,..,constraintIndex-1. */

                    minimum += 0.5 * (fullStepLength * fullStepLength) * BLAS.Level1.ddot(n, m_zVector, m_nPlusVector);
                    activeSet[i] = -(i + 1);

                    if (AddConstraint(n, m_MatrixR, m_MatrixJ, m_dVector, ref constraintIndex, ref normOfR) == false)
                    {
                        throw new ArithmeticException("The Equality constraints are linear dependent.");
                    }
                }

                /* 4.) prepare for the main loop (inequality constraint adding) */
                var iai = new int[constraintCount];
                for (int j = 0; j < inequalityConstraintCount; j++)
                {
                    iai[j] = j;
                }

                double partialStepLength, stepLength;
                var iaexcl = new bool[constraintCount];
                var oldActiveSet = new int[constraintCount];

                int stepLengthIndex;
                int indexOfConstraintToAddToActiveSet; // this is the index of the constraint to be added to the active set

                var uVectorOld = new double[constraintCount];
                var s = new double[constraintCount];

                for (int iter = 1; iter <= AbortCondition.MaxIterations; iter++)
                {
                    evaluationsNeeded++;

                    /* step 1: choose a violated constraint */
                    for (int j = equalityConstraintCount; j < constraintIndex; j++)
                    {
                        int index = activeSet[j];
                        iai[index] = -1;
                    }

                    /* compute s[x] = ci^T * x + inequalityConstraintVector for all elements of K \ A */
                    double sumOfInfeasibilities = 0.0;
                    indexOfConstraintToAddToActiveSet = 0; /* the index of the chosen violated constraint */
                    for (int i = 0; i < inequalityConstraintCount; i++)
                    {
                        iaexcl[i] = true;
                        double sum = BLAS.Level1.ddot(n, m_ConstraintInequalityMatrix.Data, x, 1, 1, n * i, 0);  // = \sum_j=0^{n-1} inequalityConstraintMatrix[j, i] * argMin[j];
                        sum -= m_ConstraintInequalityVector[i];
                        s[i] = sum;
                        sumOfInfeasibilities += Math.Min(0.0, sum);
                    }

                    if (Math.Abs(sumOfInfeasibilities) <= 100.0 * inequalityConstraintCount * MachineConsts.Epsilon * traceOfA * conditionEstimation)
                    {
                        return State.Create(OptimizerErrorClassification.ProperResult, minimum, evaluationsNeeded, iter);  /* numerically there are not infeasibilities anymore */
                    }

                    /* save old values for u, activeSet [for 0 up to 'constraintIndex'] and 'argMin': */
                    BLAS.Level1.dcopy(n, x, m_TempArgMin);
                    for (int i = 0; i < constraintIndex; i++)
                    {
                        uVectorOld[i] = uVector[i];
                        oldActiveSet[i] = activeSet[i];
                    }

                    double value = 0.0;
                    bool checkFesability = true;
                    while (checkFesability)  /* Step 2: check for feasibility and determine a new S-pair */
                    {
                        for (int i = 0; i < inequalityConstraintCount; i++)
                        {
                            if ((s[i] < value) && (iai[i] != -1) && iaexcl[i])
                            {
                                value = s[i];
                                indexOfConstraintToAddToActiveSet = i;
                            }
                        }
                        if (value >= 0.0)
                        {
                            return State.Create(OptimizerErrorClassification.ProperResult, minimum, evaluationsNeeded, iter);
                        }

                        /* set n^+ = inequalityConstraintMatrix[:,indexOfConstraintToAddToActiveSet] */
                        BLAS.Level1.dcopy(n, m_ConstraintInequalityMatrix.Data, m_nPlusVector, 1, 1, n * indexOfConstraintToAddToActiveSet, 0);  // i.e. m_nPlusVector[:] = inequalityConstraintMatrix[:, indexOfConstraintToAddToActiveSet];

                        uVector[constraintIndex] = 0.0;  /* set u = [u 0]^T */
                        activeSet[constraintIndex] = indexOfConstraintToAddToActiveSet;   /* add ip to the active set A */

                        //  for each step direction
                        bool stepDirectionLeft = true;
                        while (stepDirectionLeft == true)
                        {
                            // 2.a)
                            /* compute z = H np: the step direction in the primal space (through J, see the paper) */
                            ComputeVectorD(n, m_MatrixJ, m_nPlusVector, m_dVector);
                            UpdateVectorZ(n, m_MatrixJ, m_dVector, constraintIndex, m_zVector);
                            UpdateVectorR(n, m_MatrixR, m_dVector, constraintIndex, rVector);   /* compute N* np (if q > 0): the negative of the step direction in the dual space */

                            /* Step 2b: compute step length */
                            stepLengthIndex = 0;
                            partialStepLength = Double.PositiveInfinity; /* = maximum step in dual space without violating dual feasibility */
                            /* find the index l s.t. it reaches the minimum of u+[argMin] / r */
                            for (int k = equalityConstraintCount; k < constraintIndex; k++)
                            {
                                if (rVector[k] > 0.0)
                                {
                                    if (uVector[k] / rVector[k] < partialStepLength)
                                    {
                                        partialStepLength = uVector[k] / rVector[k];
                                        stepLengthIndex = activeSet[k];
                                    }
                                }
                            }
                            /* Compute t2: full step length (minimum step in primal space such that the constraint ip becomes feasible */
                            if (Math.Abs(BLAS.Level1.ddot(n, m_zVector, m_zVector)) > MachineConsts.Epsilon)  // i.e. z != 0
                            {
                                fullStepLength = -s[indexOfConstraintToAddToActiveSet] / BLAS.Level1.ddot(n, m_zVector, m_nPlusVector);
                            }
                            else
                            {
                                fullStepLength = Double.PositiveInfinity;
                            }
                            stepLength = Math.Min(partialStepLength, fullStepLength);

                            /* Step 2c: determine new S-pair and take step: */

                            if (stepLength >= Double.PositiveInfinity) /* no step in primal or dual space */
                            {
                                return State.Create(OptimizerErrorClassification.RoundOffError, minimum, evaluationsNeeded, iter);
                            }
                            if (fullStepLength >= Double.PositiveInfinity)  /* step in dual space */
                            {
                                /* set u = u +  t * [-r 1] and drop constraint l from the active set A */
                                for (int k = 0; k < constraintIndex; k++)
                                {
                                    uVector[k] -= stepLength * rVector[k];
                                }
                                uVector[constraintIndex] += stepLength;
                                iai[stepLengthIndex] = stepLengthIndex;
                                DeleteConstraint(stepLengthIndex, n, m_MatrixR, m_MatrixJ, activeSet, uVector, equalityConstraintCount, ref constraintIndex);
                            }
                            else  /* step in primal and dual space */
                            {
                                BLAS.Level1.daxpy(n, stepLength, m_zVector, x); /* x = x + t * z */

                                /* update the solution value */
                                minimum += stepLength * BLAS.Level1.ddot(n, m_zVector, m_nPlusVector) * (0.5 * stepLength + uVector[constraintIndex]);

                                BLAS.Level1.daxpy(constraintIndex, -stepLength, rVector, uVector);  /* u = u + t * [-r 1] */
                                uVector[constraintIndex] += stepLength;

                                if (Math.Abs(stepLength - fullStepLength) < MachineConsts.Epsilon)
                                {
                                    /* full step has taken */
                                    /* add constraint ip to the active set*/
                                    if (AddConstraint(n, m_MatrixR, m_MatrixJ, m_dVector, ref constraintIndex, ref normOfR))
                                    {
                                        iai[indexOfConstraintToAddToActiveSet] = -1;
                                        checkFesability = false;
                                    }
                                    else
                                    {
                                        iaexcl[indexOfConstraintToAddToActiveSet] = false;
                                        DeleteConstraint(indexOfConstraintToAddToActiveSet, n, m_MatrixR, m_MatrixJ, activeSet, uVector, equalityConstraintCount, ref constraintIndex);
                                        for (int i = 0; i < inequalityConstraintCount; i++)
                                        {
                                            iai[i] = i;
                                        }
                                        for (int i = equalityConstraintCount; i < constraintIndex; i++)
                                        {
                                            activeSet[i] = oldActiveSet[i];
                                            uVector[i] = uVectorOld[i];
                                            iai[activeSet[i]] = -1;
                                        }
                                        BLAS.Level1.dcopy(n, m_TempArgMin, x);
                                    }
                                }
                                else
                                {
                                    /* a patial step has taken */
                                    /* drop constraint l */
                                    iai[stepLengthIndex] = stepLengthIndex;
                                    DeleteConstraint(stepLengthIndex, n, m_MatrixR, m_MatrixJ, activeSet, uVector, equalityConstraintCount, ref constraintIndex);

                                    /* update s[ip] = inequalityConstraintMatrix[:,indexOfConstraintToAddToActiveSet] * argMin + inequalityConstraintVector */
                                    s[indexOfConstraintToAddToActiveSet] = BLAS.Level1.ddot(n, m_ConstraintInequalityMatrix.Data, x, 1, 1, n * indexOfConstraintToAddToActiveSet, 0) - m_ConstraintInequalityVector[indexOfConstraintToAddToActiveSet];
                                }
                                stepDirectionLeft = false;
                            }
                        }
                    }
                }
                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, minimum, AbortCondition.MaxIterations, evaluationsNeeded);
            }
            #endregion

            #region IDisposable Members

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // nothing to do here
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return String.Format("{0}; dimension = {1}", Factory.Name.String, Dimension);
            }
            #endregion

            #region private (static) methods

            /// <summary>Computes the vector 'd = J^t * n^+' with respect to equation (4.7) in
            /// 'A numerically stable dual method for solving strictly convex quadratic programs'.
            /// D. Goldfarb, A. Idnani; Mathematical Programming 27 (1983) pp. 1-33.
            /// </summary>
            /// <param name="n">The dimension of 'J' as well as the lenght of vector 'd'.</param>
            /// <param name="matrixJ">The matrix 'J'.</param>
            /// <param name="nPlusVector">The vector 'n^+'.</param>
            /// <param name="d">The vector d (output).</param>
            private static void ComputeVectorD(int n, double[][] matrixJ, double[] nPlusVector, double[] d)
            {
                for (int j = 0; j < n; j++)
                {
                    var sum = 0.0;
                    for (int k = 0; k < n; k++)
                    {
                        sum += matrixJ[k][j] * nPlusVector[k];
                    }
                    d[j] = sum;
                }
            }

            /// <summary>Updates the vector 'z = J_2 * d_2', where J=(J_1, J_2) and d=(d_1, d_2)^t with respect to equation (4.8) in
            /// 'A numerically stable dual method for solving strictly convex quadratic programs'.
            /// D. Goldfarb, A. Idnani; Mathematical Programming 27 (1983) pp. 1-33.
            /// </summary>
            /// <param name="n">The dimension of 'J' as well as the length of vector 'z'.</param>
            /// <param name="matrixJ">The matrix 'J'.</param>
            /// <param name="d">The vector 'd'.</param>
            /// <param name="constraintIndex">The null-based index of the constraint.</param>
            /// <param name="zVector">The vector 'z' (output).</param>
            private static void UpdateVectorZ(int n, double[][] matrixJ, double[] d, int constraintIndex, double[] zVector)
            {
                for (int j = 0; j < n; j++)
                {
                    zVector[j] = BLAS.Level1.ddot(n - constraintIndex, matrixJ[j], d, 1, 1, constraintIndex, constraintIndex);  /*  = \sum_{k=constraintIndex}^n J[j][k] * d[k] */
                }
            }

            /// <summary>Updates the vector 'r', i.e. the solution of 'd_1 = R * r' with respect to equation (4.8) in
            /// 'A numerically stable dual method for solving strictly convex quadratic programs'.
            /// D. Goldfarb, A. Idnani; Mathematical Programming 27 (1983) pp. 1-33.
            /// </summary>
            /// <param name="n">The dimension of <paramref name="matrixR"/>.</param>
            /// <param name="matrixR">The upper triangular matrix 'R'.</param>
            /// <param name="d">The vector 'd'.</param>
            /// <param name="constraintIndex">The null-based index of the constraint.</param>
            /// <param name="rVector">The vector 'r' (output).</param>
            private static void UpdateVectorR(int n, double[] matrixR, double[] d, int constraintIndex, double[] rVector)
            {
                for (int i = constraintIndex - 1; i >= 0; i--)
                {
                    double sum = 0.0;
                    for (int k = i + 1; k < constraintIndex; k++)
                    {
                        sum += matrixR[i + n * k] * rVector[k];
                    }
                    rVector[i] = (d[i] - sum) / matrixR[i + n * i];
                }
            }

            /// <summary>Adds a constraint, i.e. updating 'J', 'R' as well as 'd' with respect to the remarks on page 12f. in
            /// 'A numerically stable dual method for solving strictly convex quadratic programs'.
            /// D. Goldfarb, A. Idnani; Mathematical Programming 27 (1983) pp. 1-33.
            /// </summary>
            /// <param name="n">The dimension of the <paramref name="matrixR"/>.</param>
            /// <param name="matrixR">The matrix R (will be changed).</param>
            /// <param name="matrixJ">The matrix J (will be changed).</param>
            /// <param name="dVector">The vector 'd' (will be changed).</param>
            /// <param name="constraintIndex">The null-based index of the constraint (will increment; output).</param>
            /// <param name="normOfMatrixR">The norm of matrix R.</param>
            /// <returns>A value indicating whether the constraint has been added successfully.</returns>
            private static bool AddConstraint(int n, double[] matrixR, double[][] matrixJ, double[] dVector, ref int constraintIndex, ref double normOfMatrixR)
            {
                /* we have to find the Givens rotation which will reduce the element d[j] to zero. If it is already zero we don't 
                 * have to do anything, except of decreasing j */

                for (int j = n - 1; j >= constraintIndex + 1; j--)
                {
                    /* The Givens rotation is done with the matrix 
                     *            |c   s|
                     * \hat{Q} =  |     |
                     *            |s  -c|.
                     * 
                     * If c is one, then element (j) of d is zero compared with element (j - 1). Hence we don't have to do anything. 
                     * If c is zero, then we just have to switch column (j) and column (j - 1) of J. 
                     * Since we only switch columns in J, we have to be careful how we update d depending on the sign of gs.
                     * Otherwise we have to apply the Givens rotation to these columns. The i - 1 element of d has to be updated to h.
                     */

                    double c = dVector[j - 1];
                    double s = dVector[j];

                    double h = DoMath.EuclidianDistance(c, s);
                    if (Math.Abs(h) >= MachineConsts.Epsilon)
                    {
                        dVector[j] = 0.0;
                        s /= h;
                        c /= h;
                        if (c < 0.0)
                        {
                            c = -c;
                            s = -s;
                            dVector[j - 1] = -h;
                        }
                        else
                        {
                            dVector[j - 1] = h;
                        }
                        double xny = s / (1.0 + c);
                        for (int k = 0; k < n; k++)
                        {
                            double[] tempArray = matrixJ[k];

                            double t1 = tempArray[j - 1];
                            double t2 = tempArray[j];
                            tempArray[j - 1] = t1 * c + t2 * s;
                            tempArray[j] = xny * (t1 + tempArray[j - 1]) - t2;
                        }
                    }
                }
                constraintIndex++;  /* update the number of constraints added*/

                /* To update R we have to put the 'constraintIndex' components of the d vector into column 'constraintIndex - 1' of R, i.e.  
                 *  set 'rMatrix[i + n * (constraintIndex - 1)] = d[i]' for i = 0,..,constraintIndex - 1:
                 */
                BLAS.Level1.dcopy(constraintIndex, dVector, matrixR, 1, 1, 0, n * (constraintIndex - 1));

                if (Math.Abs(dVector[constraintIndex - 1]) <= MachineConsts.Epsilon * normOfMatrixR)
                {
                    return false;  // the problem is degenerated
                }
                normOfMatrixR = Math.Max(normOfMatrixR, Math.Abs(dVector[constraintIndex - 1]));
                return true;
            }

            /// <summary>Deletes a specific (inequality) constraint.
            /// </summary>
            /// <param name="inequalityConstraintIndex">The null-based index of the (inequality) constraint to delete.</param>
            /// <param name="n">The dimension of <paramref name="matrixR"/>, <paramref name="matrixJ"/> etc.</param>
            /// <param name="matrixR">The matrix R (will be changed).</param>
            /// <param name="matrixJ">The matrix J (will be changed).</param>
            /// <param name="activeSet">The active set (will be changed).</param>
            /// <param name="u">The vector 'u' with respect to the dual problem.</param>
            /// <param name="equalityConstraintCount">The number of equality constraint in the active set.</param>
            /// <param name="activeSetCount">The number of elements in the active set (output: will be decrease).</param>
            private void DeleteConstraint(int inequalityConstraintIndex, int n, double[] matrixR, double[][] matrixJ, int[] activeSet, double[] u, int equalityConstraintCount, ref int activeSetCount)
            {
                int constraintIndexInActiveSet = -1;

                /* Find the index for the specific constraint to be removed... */
                for (int i = equalityConstraintCount; i < activeSetCount; i++)
                {
                    if (activeSet[i] == inequalityConstraintIndex)
                    {
                        constraintIndexInActiveSet = i;
                        break;
                    }
                }

                /* ... and remove the constraint from the active set and the duals: */
                for (int i = constraintIndexInActiveSet; i < activeSetCount - 1; i++)
                {
                    activeSet[i] = activeSet[i + 1];
                    u[i] = u[i + 1];
                    for (int j = 0; j < n; j++)
                    {
                        matrixR[j + n * i] = matrixR[j + (i + 1) * n];
                    }
                }

                activeSet[activeSetCount - 1] = activeSet[activeSetCount];
                u[activeSetCount - 1] = u[activeSetCount];
                activeSet[activeSetCount] = 0;
                u[activeSetCount] = 0.0;
                BLAS.Level1.dscal(activeSetCount, 0.0, matrixR, 1, n * (activeSetCount - 1));  // set (iq-1)-th column to 0.0

                activeSetCount--;   /* constraint has been fully removed */

                if (activeSetCount != 0)
                {
                    for (int j = constraintIndexInActiveSet; j < activeSetCount; j++)
                    {
                        double c = matrixR[j + n * j];
                        double s = matrixR[j + 1 + n * j];
                        double h = DoMath.EuclidianDistance(c, s);
                        if (Math.Abs(h) >= MachineConsts.Epsilon)
                        {
                            c /= h;
                            s /= h;
                            matrixR[j + 1 + n * j] = 0.0;
                            if (c < 0.0)
                            {
                                matrixR[j + n * j] = -h;
                                c = -c;
                                s = -s;
                            }
                            else
                            {
                                matrixR[j + n * j] = h;
                            }

                            double xny = s / (1.0 + c);
                            for (int k = j + 1; k < activeSetCount; k++)
                            {
                                double t1 = matrixR[j + n * k];
                                double t2 = matrixR[j + 1 + n * k];
                                matrixR[j + n * k] = t1 * c + t2 * s;
                                matrixR[j + 1 + n * k] = xny * (t1 + matrixR[j + n * k]) - t2;
                            }
                            for (int k = 0; k < n; k++)
                            {
                                double[] temp = matrixJ[k];

                                double t1 = temp[j];
                                double t2 = temp[j + 1];
                                temp[j] = t1 * c + t2 * s;
                                temp[j + 1] = xny * (matrixJ[k][j] + t1) - t2;
                            }
                        }
                    }
                }
            }
            #endregion
        }
    }
}