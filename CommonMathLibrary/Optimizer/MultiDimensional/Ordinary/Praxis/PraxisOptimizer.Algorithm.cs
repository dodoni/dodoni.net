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

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    public partial class PraxisOptimizer
    {
        /// <summary>Represents the implementation of the algorithm.
        /// </summary>
        private class Algorithm : IMultiDimOptimizerAlgorithm
        {
            #region private static (readonly) members

            /// <summary>A mega extreme large number used for the algorithm and set to the square of <see cref="MachineConsts.ExtremLargeValue"/>.
            /// </summary>
            private static readonly double sm_MegaExtremLargeNumber = MachineConsts.ExtremLargeValue * MachineConsts.ExtremLargeValue;

            /// <summary>The constant 'm2' used for the algorithm.
            /// </summary>
            private static readonly double sm_m2 = Math.Sqrt(MachineConsts.Epsilon);

            /// <summary>The constant 'm4' used for the algorithm.
            /// </summary>
            private static readonly double sm_m4 = Math.Sqrt(sm_m2);
            #endregion

            #region private members

            /// <summary>The <see cref="PraxisOptimizer"/> object that serves as factory of the current object.
            /// </summary>
            private PraxisOptimizer m_Optimizer;

            /// <summary>The objective function in its <see cref="OrdinaryMultiDimOptimizerFunction"/> representation.
            /// </summary>
            private OrdinaryMultiDimOptimizerFunction m_ObjectiveFunction;

            /// <summary>A value indicating wheter the problem is ill-conditioned, <c>true</c> is default.
            /// </summary>
            /// <remarks>This variable will be automatically set when the algorithm finds the problem to be ill-conditioned during iterations.</remarks>
            private bool m_IsIllConditioned = true;

            #region intermediate members

            /// <summary>The number of function evaluations need.
            /// </summary>
            private int m_EvaluationsNeeded = 0;

            /// <summary>The current point of the optimization procedure.
            /// </summary>
            private double[] m_CurrentPoint;

            /// <summary>The value of the given function evaluated at <see cref="m_CurrentPoint"/>.
            /// </summary>
            private double m_ValueOfCurrentPoint;

            /// <summary>The (columnwise given) matrix which contains the vectors which indicates the directions for the optimization procedure in the current optimization procedure step.
            /// </summary>
            /// <remarks>m_DicrectionMatrix[i,:] represents the i-th vector indicating the direction for the optimization procedure, i=0,..,n. For i = n a direction of P_N - P_0 will be stored.</remarks>
            private double[] m_DirectionMatrix;

            /// <summary>The adjusted step length parameter <c>ExpectedDistanceToSolution</c>.
            /// </summary>
            private double m_AdjustedExpectedDistanceToSolution;

            /// <summary>Contains the point which is used to evaluate the given function in a point on a specific line.
            /// </summary>
            /// <remarks>This member is used for performance reason only.</remarks>
            private double[] m_TempPointOnLine;

            /// <summary>Contains the number of calls of the one-dimensional search routine.
            /// </summary>
            private int m_OneDimensionalSearchCounter;

            /// <summary>The expected distance to the solution used for the termination condition of the algorithm.
            /// </summary>
            private double m_CurrentExpectedDistance;

            /// <summary>The minimum of the second derivatives (floored at some value > 0).
            /// </summary>
            private double m_MinOfSecondDerivatives;

            #region points used for the parabolic space curve

            /// <summary>The parameter qd0 used for the quadratic extrapolation.
            /// </summary>
            private double m_Extrapolation_qd0;

            /// <summary>The parameter qd1 used for the quadratic extrapolation.
            /// </summary>
            private double m_Extrapolation_qd1;

            /// <summary>The point Q0 used for the quadratic extrapolation.
            /// </summary>
            private double[] m_ExtrapolationPointQ0;

            /// <summary>The point Q1 used for the quadratic extrapolation.
            /// </summary>
            private double[] m_ExtrapolationPointQ1;
            #endregion

            #endregion

            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.
            /// </summary>
            /// <param name="optimizer">The <see cref="PraxisOptimizer"/> object that serves as factory of the current object.</param>
            /// <param name="dimension">The dimension of the feasible region.</param>
            internal Algorithm(PraxisOptimizer optimizer, int dimension)
            {
                m_Optimizer = optimizer;
                Dimension = dimension;

                m_DirectionMatrix = new double[dimension * dimension];
                m_TempPointOnLine = new double[dimension];
                m_ExtrapolationPointQ0 = new double[dimension];
                m_ExtrapolationPointQ1 = new double[dimension];
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
                    if (value is OrdinaryMultiDimOptimizerFunction)
                    {
                        var ordinaryObjectiveFunction = value as OrdinaryMultiDimOptimizerFunction;
                        if (ordinaryObjectiveFunction.Dimension != Dimension)
                        {
                            throw new InvalidOperationException(String.Format("Inconsistent dimension of feasible region and objective function."));
                        }
                        m_ObjectiveFunction = ordinaryObjectiveFunction;
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
            public PraxisOptimizerAbortCondition AbortCondition
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
                m_EvaluationsNeeded = 0;

                /* 1.) set some variables before starting the algorithm */
                int n = Dimension;

                m_AdjustedExpectedDistanceToSolution = m_Optimizer.ExpectedDistanceToSolution;
                double t2 = AbortCondition.Tolerance;

                if (m_AdjustedExpectedDistanceToSolution < 100 * AbortCondition.Tolerance)
                {
                    m_AdjustedExpectedDistanceToSolution = 100 * AbortCondition.Tolerance;
                }
                m_CurrentExpectedDistance = m_AdjustedExpectedDistanceToSolution;
                var expectedDistanceFactor = (m_IsIllConditioned == true) ? 0.1 : 0.01;
                var expectedDistanceLowerBound = 0.0;

                m_CurrentPoint = x;
                for (int i = 0; i < n; i++)
                {
                    m_ExtrapolationPointQ1[i] = m_CurrentPoint[i] = x[i];
                    m_ExtrapolationPointQ0[i] = 0.0;  // maybe this array contains some 'NaN' values from one earlier call of the optimizer

                    m_DirectionMatrix[i * n + i] = 1;
                }
                m_ValueOfCurrentPoint = m_ObjectiveFunction.GetValue(x);

                var estimatedSecondDerivatives = new double[n];  // estimations of the second derivatives of \lambda \mapsto f(x + \lambda \cdot v_j)
                m_MinOfSecondDerivatives = MachineConsts.ExtremeTinyEpsilon;  // represents the minimum of the estimated second derivatives

                double qf1 = m_ValueOfCurrentPoint;

                var y = new double[n]; // a temp object for a point in \R^n
                var z = new double[n]; // dito.

                double argMinOfDirectionFunction, fy;
                int convergenceCriterionCounter = m_OneDimensionalSearchCounter = 0;

                double lastFunctionValue = m_ValueOfCurrentPoint;

                /* 2.) start the main loop */
                for (int iteration = 1; iteration <= AbortCondition.MaxIterations; iteration++)
                {
                    lastFunctionValue = m_ValueOfCurrentPoint;  // used for the absolute/relative error (condition) 

                    var tempSecondDerivativeInFirstDirection = estimatedSecondDerivatives[0];
                    estimatedSecondDerivatives[0] = 0.0;

                    /* 3.) minimize along the first direction and return the minimum as well as an estimation for the 
                     * second derivative of \lamba \mapsto f(x+ \lambda * v), where v is the given direction */

                    argMinOfDirectionFunction = LinearQuadraticMinimization(0, 2, m_ValueOfCurrentPoint, 0.0, false, ref estimatedSecondDerivatives[0]);

                    // change the direction if necessary
                    if (argMinOfDirectionFunction <= 0.0)
                    {
                        BLAS.Level1.dscal(n, -1.0, m_DirectionMatrix);  // m_Direction[i,0] = - m_Direction[i,0], i.e. m_DirectionMatrix[i] -= m_DirectionMatrix[i] for i = 0,..., n-1
                    }
                    // reset the estimated second derivative if the new estimation changed to much
                    if ((tempSecondDerivativeInFirstDirection <= 0.9 * estimatedSecondDerivatives[0]) || (0.9 * tempSecondDerivativeInFirstDirection >= estimatedSecondDerivatives[0]))
                    {
                        BLAS.Level1.dscal(n, 0.0, estimatedSecondDerivatives); // set 'estimatedSecondDerivatives[j] = 0' for j = 0,..., n-1
                    }

                    /* 4.) consider the other directions */
                    for (int k = 1; k < n; k++)
                    {
                        /* store the current point before do further minimization or disturbance  */
                        fy = m_ValueOfCurrentPoint;
                        BLAS.Level1.dcopy(n, m_CurrentPoint, y);
                        m_IsIllConditioned = m_IsIllConditioned || (convergenceCriterionCounter > 0);

                        double functionDifference;
                        int kl;
                        do
                        {
                            kl = k;
                            functionDifference = 0.0;

                            /* disturb the given point if desired */
                            if (m_IsIllConditioned)
                            {
                                for (int i = 0; i < n; i++)
                                {
                                    z[i] = (0.1 * m_CurrentExpectedDistance + t2 * DoMath.Pow(10.0, convergenceCriterionCounter)) * (m_Optimizer.m_SingleRandomNumberStream.GetNextDouble() - 0.5);
                                    BLAS.Level1.daxpy(n, z[i], m_DirectionMatrix, m_CurrentPoint, 1, 1, n * i, 0);  // m_CurrentPoint[r] += z[i] * m_DirectionMatrix[i*n + r] for r=0,...,n-1
                                }
                                m_ValueOfCurrentPoint = m_ObjectiveFunction.GetValue(m_CurrentPoint);
                                m_EvaluationsNeeded++;
                            }

                            /* minimizing along non-conjugated directions */
                            for (int k2 = k; k2 < n; k2++)
                            {
                                double tempFunctionValue = m_ValueOfCurrentPoint;
                                argMinOfDirectionFunction = LinearQuadraticMinimization(k2, 2, m_ValueOfCurrentPoint, 0.0, false, ref estimatedSecondDerivatives[k2]);

                                double adjustedFunctionValue;
                                if (m_IsIllConditioned)
                                {
                                    double temp = argMinOfDirectionFunction + z[k2];
                                    adjustedFunctionValue = estimatedSecondDerivatives[k2] * temp * temp;
                                }
                                else
                                {
                                    adjustedFunctionValue = tempFunctionValue - m_ValueOfCurrentPoint;
                                }
                                if (functionDifference < adjustedFunctionValue)
                                {
                                    functionDifference = adjustedFunctionValue;
                                    kl = k2;
                                }
                            }
                            if ((m_IsIllConditioned == false) && (functionDifference < Math.Abs(100.0 * MachineConsts.Epsilon * m_ValueOfCurrentPoint)))
                            {
                                m_IsIllConditioned = true;
                            }
                        } while (!m_IsIllConditioned && (functionDifference < Math.Abs(100.0 * MachineConsts.Epsilon * m_ValueOfCurrentPoint)));

                        /* minimize along conjugate directions */
                        for (int k2 = 0; k2 <= k - 1; k2++)
                        {
                            LinearQuadraticMinimization(k2, 2, m_ValueOfCurrentPoint, 0.0, false, ref estimatedSecondDerivatives[k2]);
                        }

                        double tempFunctionValue1 = m_ValueOfCurrentPoint;
                        m_ValueOfCurrentPoint = fy;
                        expectedDistanceLowerBound = 0.0;
                        for (int i = 0; i < n; i++)
                        {
                            double yCoordinate = y[i];
                            double currentPointCoordinate = m_CurrentPoint[i];

                            expectedDistanceLowerBound += (currentPointCoordinate - yCoordinate) * (currentPointCoordinate - yCoordinate);
                            m_CurrentPoint[i] = yCoordinate;
                            y[i] = currentPointCoordinate - yCoordinate;
                        }
                        expectedDistanceLowerBound = Math.Sqrt(expectedDistanceLowerBound);

                        if (expectedDistanceLowerBound > MachineConsts.ExtremeTinyEpsilon)
                        {
                            for (int i = kl - 1; i >= k; i--)
                            {
                                for (int j = 0; j < n; j++)
                                {
                                    m_DirectionMatrix[j + (i + 1) * n] = m_DirectionMatrix[j + i * n];  // m_DirectionMatrix[j, i + 1] = m_DirectionMatrix[j, i];
                                }
                                estimatedSecondDerivatives[i + 1] = estimatedSecondDerivatives[i];
                            }
                            estimatedSecondDerivatives[k] = 0.0;
                            for (int i = 0; i < n; i++)
                            {
                                m_DirectionMatrix[i + k * n] = y[i] / expectedDistanceLowerBound;   // m_DirectionMatrix[i, k] = y[i] / expectedDistanceLowerBound;
                            }
                            expectedDistanceLowerBound = LinearQuadraticMinimization(k, 4, tempFunctionValue1, expectedDistanceLowerBound, true, ref estimatedSecondDerivatives[k]);
                            if (expectedDistanceLowerBound <= 0.0)
                            {
                                expectedDistanceLowerBound = -expectedDistanceLowerBound;
                                BLAS.Level1.dscal(n, -1.0, m_DirectionMatrix, 1, k * n); // set 'm_DirectionMatrix[i + k * n] = -m_DirectionMatrix[i + k * n]' for i=0,...n-1, thus m_DirectionMatrix[i, k] = -m_DirectionMatrix[i, k]
                            }
                        }
                        m_CurrentExpectedDistance *= expectedDistanceFactor;
                        if (m_CurrentExpectedDistance < expectedDistanceLowerBound)
                        {
                            m_CurrentExpectedDistance = expectedDistanceLowerBound;
                        }
                        t2 = sm_m2 * BLAS.Level1.dnrm2(n, m_CurrentPoint) + AbortCondition.Tolerance;

                        if (m_CurrentExpectedDistance > 0.5 * t2)
                        {
                            convergenceCriterionCounter = 0;
                        }
                        else
                        {
                            //  convergenceCriterionCounter++;  // it shows better results if we do not change the convergence-counter
                        }

                        if ((Double.IsInfinity(m_ValueOfCurrentPoint) == true) || (Double.IsNaN(m_ValueOfCurrentPoint) == true))
                        {
                            minimum = m_ValueOfCurrentPoint;
                            return State.Create(OptimizerErrorClassification.InvalidFunctionValue, minimum, m_EvaluationsNeeded, iteration);
                        }
                        if (AbortCondition.IsSatisfied(m_ValueOfCurrentPoint, lastFunctionValue, ref convergenceCriterionCounter) == true)
                        {
                            minimum = m_ValueOfCurrentPoint;
                            return State.Create(OptimizerErrorClassification.ProperResult, minimum, m_EvaluationsNeeded, iteration);
                        }
                        else if (m_EvaluationsNeeded >= AbortCondition.MaxEvaluations)
                        {
                            minimum = m_ValueOfCurrentPoint;
                            return State.Create(OptimizerErrorClassification.EvaluationLimitExceeded, minimum, m_EvaluationsNeeded, iteration);
                        }

                        /* try quadratic extrapolation in the case we are stuck in a curved valley */
                        QuadraticExtrapolation(ref qf1);

                        double largestSecondDerivative = 0.0;
                        for (int i = 0; i < n; i++)
                        {
                            estimatedSecondDerivatives[i] = 1.0 / Math.Sqrt(estimatedSecondDerivatives[i]);
                            if (largestSecondDerivative < estimatedSecondDerivatives[i])
                            {
                                largestSecondDerivative = estimatedSecondDerivatives[i];
                            }
                        }
                        /* create a new direction matrix with respect to the estimated second derivatives */
                        for (int j = 0; j < n; j++)
                        {
                            double temp = estimatedSecondDerivatives[j] / largestSecondDerivative;
                            BLAS.Level1.dscal(n, temp, m_DirectionMatrix, 1, j * n);  // m_DirectionMatrix[i + j * n] *= temp, i=0,...n-1
                        }

                        /* scale axis to reduce condition number */
                        if (m_Optimizer.ScalingFactor > 1.0)
                        {
                            double s = sm_MegaExtremLargeNumber;
                            for (int i = 0; i < n; i++)
                            {
                                z[i] = Math.Sqrt(BLAS.Level1.ddot(n, m_DirectionMatrix, m_DirectionMatrix, n, n, i, i));  // = sqrt( sum_j m_DirectionMatrix[i + j * n] * m_DirectionMatrix[i + j * n])
                                if (z[i] < sm_m4)
                                {
                                    z[i] = sm_m4;
                                }
                                if (s > z[i])
                                {
                                    s = z[i];
                                }
                            }
                            for (int i = 0; i < n; i++)
                            {
                                z[i] = z[i] / s;
                                if (z[i] > m_Optimizer.ScalingFactor)
                                {
                                    z[i] = m_Optimizer.ScalingFactor;
                                }
                            }
                        }
                        /* calculate a new set of orthogonal directions before repeating the main loop.
                         * In this implementation transpose the directionMatrix V first:*/
                        for (int i = 1; i < n; i++)
                        {
                            for (int j = 0; j <= i - 1; j++)
                            {
                                double temp = m_DirectionMatrix[i + j * n];
                                m_DirectionMatrix[i + j * n] = m_DirectionMatrix[j + i * n];
                                m_DirectionMatrix[j + i * n] = temp;
                            }
                        }

                        /* do a singular value decomposition */
                        if (m_Optimizer.ScalingFactor > 1.0)
                        {
                            for (int i = 0; i < n; i++)
                            {
                                double temp = z[i];
                                for (int j = 0; j < n; j++)
                                {
                                    m_DirectionMatrix[i + j * n] *= temp;  // m_DirectionMatrix[i, j] *= temp;
                                }
                            }
                            for (int i = 0; i < n; i++)
                            {
                                double temp = Math.Sqrt(BLAS.Level1.ddot(n, m_DirectionMatrix, m_DirectionMatrix, 1, 1, i * n, i * n));  // = \sqrt( sum_j  m_DirectionMatrix[j + i * n] * m_DirectionMatrix[j + i * n] )

                                estimatedSecondDerivatives[i] *= temp;
                                temp = 1.0 / temp;
                                for (int j = 0; j < n; j++)
                                {
                                    m_DirectionMatrix[j + i * n] *= temp;  //m_DirectionMatrix[j, i] *= temp;
                                }
                            }
                        }
                        for (int i = 0; i < n; i++)
                        {
                            if (largestSecondDerivative * estimatedSecondDerivatives[i] > MachineConsts.ExtremLargeValue)
                            {
                                estimatedSecondDerivatives[i] = MachineConsts.ExtremLargeValue;
                            }
                            else if (largestSecondDerivative * estimatedSecondDerivatives[i] < MachineConsts.ExtremeTinyEpsilon)
                            {
                                estimatedSecondDerivatives[i] = sm_MegaExtremLargeNumber;
                            }
                            else
                            {
                                estimatedSecondDerivatives[i] = 1.0 / (largestSecondDerivative * estimatedSecondDerivatives[i] * largestSecondDerivative * estimatedSecondDerivatives[i]);
                            }
                        }
                        Sort(estimatedSecondDerivatives);

                        m_MinOfSecondDerivatives = estimatedSecondDerivatives[n - 1];
                        if (m_MinOfSecondDerivatives < MachineConsts.ExtremeTinyEpsilon)
                        {
                            m_MinOfSecondDerivatives = MachineConsts.ExtremeTinyEpsilon;
                        }
                        m_IsIllConditioned = ((sm_m2 * estimatedSecondDerivatives[0]) > m_MinOfSecondDerivatives);
                    }
                }
                minimum = m_ValueOfCurrentPoint;
                return State.Create(OptimizerErrorClassification.IterationLimitExceeded, minimum, m_EvaluationsNeeded, AbortCondition.MaxEvaluations);
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

            #region private methods

            /// <summary>Does a minimization on a line ('linear') or along a curve (quadratic extrapolation).
            /// </summary>
            /// <param name="directionIndex">The null-based index that indicates the direction with respect to <see cref="m_DirectionMatrix"/>; otherwise (<c>-1</c>) the minimization will be take place along the curve q0, q1, q2.</param>
            /// <param name="numberOfIterations">The maximal number of times the interval may be halved to retry the calculation.</param>
            /// <param name="functionValue">The function value, i.e. f(x + \lambda \cdot v_j) where v_j is specified by <paramref name="directionIndex"/> and \lambda is specified by <paramref name="initialLambda"/>.</param>
            /// <param name="initialLambda">The initial lambda.</param>
            /// <param name="fk">Some flag.</param>
            /// <param name="estimatedSecondDerivative">Is either zero, or an approximation to the value of 1/2 times the second derivative of \lambda \mapsto f(x + \lambda \cdot v_j).</param>
            /// <returns>The argmin of the minimization problem \lambda \mapsto f(x+\lambda * v_j), where v = (v_i) is the specified direction matrix, i.e. the corresponding \lambda.</returns>
            /// <remarks>This method will change the current point as well as the value of the current point and used expected distance and the minimum of the estmated second derivatives.</remarks>
            private double LinearQuadraticMinimization(int directionIndex, int numberOfIterations, double functionValue, double initialLambda, bool fk, ref double estimatedSecondDerivative)
            {
                m_OneDimensionalSearchCounter++;

                double givenFunctionValue = functionValue;

                double f0, fMin;
                f0 = fMin = m_ValueOfCurrentPoint;

                double lambdaMin = 0.0;

                /* create a lower bound for the parameter lambda, i.e. in general we do not want to start in the point 0.0 itself */
                double s = BLAS.Level1.dnrm2(Dimension, m_CurrentPoint);
                bool dz = (estimatedSecondDerivative < MachineConsts.Epsilon);

                double t2;
                if (dz == true)
                {
                    t2 = sm_m4 * Math.Sqrt(Math.Abs(m_ValueOfCurrentPoint) / m_MinOfSecondDerivatives + s * m_CurrentExpectedDistance) + sm_m2 * m_CurrentExpectedDistance;
                }
                else
                {
                    t2 = sm_m4 * Math.Sqrt(Math.Abs(m_ValueOfCurrentPoint) / estimatedSecondDerivative + s * m_CurrentExpectedDistance) + sm_m2 * m_CurrentExpectedDistance;
                }
                s = s * sm_m4 + AbortCondition.Tolerance;
                if ((dz == true) && (t2 > s))
                {
                    t2 = s;
                }
                if (t2 < MachineConsts.ExtremeTinyEpsilon)
                {
                    t2 = MachineConsts.ExtremeTinyEpsilon;
                }
                if (t2 > 0.01 * m_AdjustedExpectedDistanceToSolution)
                {
                    t2 = 0.01 * m_AdjustedExpectedDistanceToSolution;
                }

                if ((fk == true) && (functionValue <= fMin))
                {
                    lambdaMin = initialLambda;
                    fMin = functionValue;
                }

                if ((fk == false) && (Math.Abs(initialLambda) < t2))
                {
                    initialLambda = ((initialLambda > 0.0) ? t2 : -t2);
                    functionValue = FunctionEvaluation(directionIndex, initialLambda);
                }
                if (functionValue < fMin)
                {
                    lambdaMin = initialLambda;
                    fMin = functionValue;
                }

                /* evaluate \lambda \mapsto f(x + \lambda * v_j) at another point and estimate the second derivative */
                double f2, lambda, derivative;
                if (dz == true)
                {
                    lambda = ((f0 < functionValue) ? -initialLambda : 2 * initialLambda);
                    f2 = FunctionEvaluation(directionIndex, lambda);
                    if (f2 <= fMin)
                    {
                        lambdaMin = lambda;
                        fMin = f2;
                    }
                    estimatedSecondDerivative = (lambda * (functionValue - f0) - initialLambda * (f2 - f0)) / (initialLambda * lambda * (initialLambda - lambda));
                }
                /* estimate the first derivative at 0 */
                derivative = (functionValue - f0) / initialLambda - initialLambda * estimatedSecondDerivative;

                /* predict the minimum */
                if (estimatedSecondDerivative <= MachineConsts.ExtremeTinyEpsilon)
                {
                    lambda = ((derivative < 0.0) ? m_AdjustedExpectedDistanceToSolution : -m_AdjustedExpectedDistanceToSolution);
                }
                else
                {
                    lambda = -0.5 * derivative / estimatedSecondDerivative;
                }
                if (Math.Abs(lambda) > m_AdjustedExpectedDistanceToSolution)
                {
                    lambda = ((lambda > 0.0) ? m_AdjustedExpectedDistanceToSolution : -m_AdjustedExpectedDistanceToSolution);
                }

                /* evaluate \lambda \mapsto f(x + \lambda \cdot v_j) at the predicted minimum and change the lambda if necessary... */
                f2 = FunctionEvaluation(directionIndex, lambda);

                int k = 0;
                while ((k < numberOfIterations) && (f2 > f0))
                {
                    k++;

                    if ((dz == false) && (f0 < functionValue) && (initialLambda * lambda > 0.0))
                    {
                        dz = true;
                        /* do the same as above - maybe it would be better to create a method for this but there are many variables involved -
                         * evaluate \lambda \mapsto f(x + \lambda * v_j) at another point and estimate the second derivative */
                        lambda = ((f0 < functionValue) ? -initialLambda : 2 * initialLambda);
                        f2 = FunctionEvaluation(directionIndex, lambda);
                        if (f2 <= fMin)
                        {
                            lambdaMin = lambda;
                            fMin = f2;
                        }
                        estimatedSecondDerivative = (lambda * (functionValue - f0) - initialLambda * (f2 - f0)) / (initialLambda * lambda * (initialLambda - lambda));

                        /* estimate the first derivative at 0 */
                        derivative = (functionValue - f0) / initialLambda - initialLambda * estimatedSecondDerivative;

                        /* predict the minimum */
                        if (estimatedSecondDerivative <= MachineConsts.ExtremeTinyEpsilon)
                        {
                            lambda = ((derivative < 0.0) ? m_AdjustedExpectedDistanceToSolution : -m_AdjustedExpectedDistanceToSolution);
                        }
                        else
                        {
                            lambda = -0.5 * derivative / estimatedSecondDerivative;
                        }
                        if (Math.Abs(lambda) > m_AdjustedExpectedDistanceToSolution)
                        {
                            lambda = ((lambda > 0.0) ? m_AdjustedExpectedDistanceToSolution : -m_AdjustedExpectedDistanceToSolution);
                        }
                    }
                    else
                    {
                        lambda *= 0.5;
                    }
                    f2 = FunctionEvaluation(directionIndex, lambda);
                }

                if (f2 > fMin)
                {
                    lambda = lambdaMin;
                }
                else
                {
                    fMin = f2;
                }

                /* Get a new estimate of the second derivative */
                if (Math.Abs(lambda * (lambda - initialLambda)) > MachineConsts.ExtremeTinyEpsilon)
                {
                    estimatedSecondDerivative = (lambda * (functionValue - f0) - initialLambda * (fMin - f0)) / (initialLambda * lambda * (initialLambda - lambda));
                }
                else
                {
                    if (k > 0)
                    {
                        estimatedSecondDerivative = 0.0;
                    }
                }
                if (estimatedSecondDerivative <= MachineConsts.ExtremeTinyEpsilon)
                {
                    estimatedSecondDerivative = MachineConsts.ExtremeTinyEpsilon;
                }

                if (givenFunctionValue < fMin)
                {
                    lambda = initialLambda;
                    m_ValueOfCurrentPoint = givenFunctionValue;
                }
                else
                {
                    m_ValueOfCurrentPoint = fMin;
                }

                /* update the current point x for the linear search */
                if (directionIndex >= 0)
                {
                    for (int i = 0; i < Dimension; i++)
                    {
                        m_CurrentPoint[i] += lambda * m_DirectionMatrix[i + Dimension * directionIndex];  //m_CurrentPoint[i] += lambda * m_DirectionMatrix[i, directionIndex];
                    }
                }
                return lambda;
            }

            /// <summary>Evaluate the function at a specific point with respect to some line or along some parabolic space curve.
            /// </summary>
            /// <param name="directionIndex">Index of the direction; <c>-1</c> indicates that the evaluation along some parabolic space curve will be take place.</param>
            /// <param name="lambda">The point to evaluate the function.</param>
            /// <returns>The value of the specified objective function at the specific point.</returns>
            private double FunctionEvaluation(int directionIndex, double lambda)
            {
                if (directionIndex != -1)
                {
                    for (int i = 0; i < Dimension; i++)
                    {
                        m_TempPointOnLine[i] = m_CurrentPoint[i] + lambda * m_DirectionMatrix[i + Dimension * directionIndex];
                    }
                }
                else /* search along parabolic space curve */
                {
                    double qa = lambda * (lambda - m_Extrapolation_qd1) / (m_Extrapolation_qd0 * (m_Extrapolation_qd0 + m_Extrapolation_qd1));
                    double qb = (lambda + m_Extrapolation_qd0) * (m_Extrapolation_qd1 - lambda) / (m_Extrapolation_qd0 * m_Extrapolation_qd1);
                    double qc = lambda * (lambda + m_Extrapolation_qd0) / (m_Extrapolation_qd1 * (m_Extrapolation_qd0 + m_Extrapolation_qd1));

                    for (int i = 0; i < Dimension; i++)
                    {
                        m_TempPointOnLine[i] = qa * m_ExtrapolationPointQ0[i] + qb * m_CurrentPoint[i] + qc * m_ExtrapolationPointQ1[i];
                    }
                }
                m_EvaluationsNeeded++;
                return m_ObjectiveFunction.GetValue(m_TempPointOnLine);
            }

            /// <summary>Does a quadratic extrapolation.
            /// </summary>
            /// <param name="qf1">The QF1.</param>
            private void QuadraticExtrapolation(ref double qf1)
            {
                double tempValue1, tempValue2;

                tempValue1 = m_ValueOfCurrentPoint;
                m_ValueOfCurrentPoint = qf1;
                qf1 = tempValue1;
                m_Extrapolation_qd1 = 0.0;

                /* swap the values of the current point and the point Q1 used for the extrapolation */
                for (int i = 0; i < Dimension; i++)
                {
                    tempValue1 = m_CurrentPoint[i];
                    tempValue2 = m_ExtrapolationPointQ1[i];

                    m_CurrentPoint[i] = tempValue2;
                    m_ExtrapolationPointQ1[i] = tempValue1;

                    m_Extrapolation_qd1 += (tempValue1 - tempValue2) * (tempValue1 - tempValue2);
                }

                m_Extrapolation_qd1 = Math.Sqrt(m_Extrapolation_qd1);
                tempValue2 = 0.0;
                double qa, qb, qc;
                if ((m_Extrapolation_qd0 > 0.0) && (m_Extrapolation_qd1 > 0.0) && (m_OneDimensionalSearchCounter >= 3 * Dimension * Dimension))
                {
                    tempValue1 = this.LinearQuadraticMinimization(-1, 2, qf1, m_Extrapolation_qd1, true, ref tempValue2);
                    qa = tempValue1 * (tempValue1 - m_Extrapolation_qd1) / (m_Extrapolation_qd0 * (m_Extrapolation_qd0 + m_Extrapolation_qd1));
                    qb = (tempValue1 + m_Extrapolation_qd0) * (m_Extrapolation_qd1 - tempValue1) / (m_Extrapolation_qd0 * m_Extrapolation_qd1);
                    qc = tempValue1 * (tempValue1 + m_Extrapolation_qd0) / (m_Extrapolation_qd1 * (m_Extrapolation_qd0 + m_Extrapolation_qd1));
                }
                else
                {
                    m_ValueOfCurrentPoint = qf1;
                    qa = qb = 0.0;
                    qc = 1.0;
                }
                m_Extrapolation_qd0 = m_Extrapolation_qd1;
                for (int i = 0; i < Dimension; i++)
                {
                    tempValue1 = m_ExtrapolationPointQ0[i];
                    m_ExtrapolationPointQ0[i] = m_CurrentPoint[i];
                    m_CurrentPoint[i] = qa * tempValue1 + qb * m_CurrentPoint[i] + qc * m_ExtrapolationPointQ1[i];
                }
            }

            /// <summary>Sorts the estimated second derivatives and the direction matrix in descending order.
            /// </summary>
            /// <param name="estimatedSecondDerivatives">The estimated second derivatives.</param>
            private void Sort(double[] estimatedSecondDerivatives)
            {
                for (int i = 0; i < Dimension - 1; i++)
                {
                    int k = 1;
                    double s = estimatedSecondDerivatives[i];
                    for (int j = i + 1; j < Dimension; j++)
                    {
                        if (estimatedSecondDerivatives[j] > s)
                        {
                            k = j;
                            s = estimatedSecondDerivatives[j];
                        }
                    }
                    if (k > i)
                    {
                        estimatedSecondDerivatives[k] = estimatedSecondDerivatives[i];
                        estimatedSecondDerivatives[i] = s;
                        for (int j = 0; j < Dimension; j++)
                        {
                            s = m_DirectionMatrix[j + i * Dimension];
                            m_DirectionMatrix[j + i * Dimension] = m_DirectionMatrix[j + Dimension * k];
                            m_DirectionMatrix[j + k * Dimension] = s;
                        }
                    }
                }
            }
            #endregion
        }
    }
}