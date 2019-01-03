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
using System.Security;
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as wrapper for optimization algorithms based on the NLopt library 2.4.x, see
    /// <para>http://ab-initio.mit.edu/wiki/index.php/NLopt.</para>
    /// NLopt combines several free/open-source nonlinear optimization libraries by various authors.  See the COPYING, COPYRIGHT, and README
    /// files in the subdirectories for the original copyright and licensing information of these packages.
    /// 
    /// The compiled NLopt library, i.e. the combined work of all of the included optimization routines, is licensed under the conjunction of
    /// all of these licensing terms.  Currently, the most restrictive terms are for the code in the "luksan" directory, which is licensed under
    /// the GNU Lesser General Public License (GNU LGPL), version 2.1 or later (see luksan/COPYRIGHT).
    /// 
    /// That means that the compiled NLopt library is governed by the terms of the LGPL.
    /// 
    /// Other portions of NLopt, including any modifications to the abovementioned packages, are licensed under the standard "MIT License:"
    /// 
    /// Copyright (c) 2007-2011 Massachusetts Institute of Technology
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the
    /// "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish,
    /// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
    /// the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    /// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    /// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    /// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </summary>           
    public class NLoptPtr : IDisposable
    {
        #region public (const) members

        /// <summary>The name of the external dll.
        /// </summary>
        public const string dllName = "libNLopt.dll";

        /// <summary>The calling convention of the functions in the NLopt library.
        /// </summary>
        public const CallingConvention callingConvention = CallingConvention.Cdecl;
        #endregion

        #region private function import

        [DllImport(dllName, EntryPoint = "nlopt_create", CallingConvention = callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr nlopt_create(NLoptAlgorithm algorithm, int n);

        [DllImport(dllName, EntryPoint = "nlopt_destroy", CallingConvention = callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void nlopt_destroy(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_set_min_objective", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_min_objective(IntPtr opt, [MarshalAs(UnmanagedType.FunctionPtr)] NLoptObjectiveFunction f, IntPtr data);

        [DllImport(dllName, EntryPoint = "nlopt_optimize", CallingConvention = callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern NLoptResultCode nlopt_optimize(IntPtr opt, double[] argMin, out double minimum);

        [DllImport(dllName, EntryPoint = "nlopt_set_local_optimizer", CallingConvention = callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern NLoptResultCode nlopt_set_local_optimizer(IntPtr opt, IntPtr localOpt);

        [DllImport(dllName, EntryPoint = "nlopt_set_initial_step", CallingConvention = callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern NLoptResultCode nlopt_set_initial_step(IntPtr opt, double[] dx);

        [DllImport(dllName, EntryPoint = "nlopt_set_initial_step1", CallingConvention = callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern NLoptResultCode nlopt_set_initial_step1(IntPtr opt, double dx);

        [DllImport(dllName, EntryPoint = "nlopt_get_dimension", CallingConvention = callingConvention)]
        private static extern int nlopt_get_dimension(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_algorithm_name", CallingConvention = callingConvention), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr nlopt_AlgorithmName(NLoptAlgorithm algorithm);

        #region stopping criteria

        [DllImport(dllName, EntryPoint = "nlopt_set_ftol_rel", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_ftol_rel(IntPtr opt, double tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_get_ftol_rel", CallingConvention = callingConvention)]
        private static extern double nlopt_get_ftol_rel(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_set_ftol_abs", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_ftol_abs(IntPtr opt, double tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_get_ftol_abs", CallingConvention = callingConvention)]
        private static extern double nlopt_get_ftol_abs(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_set_xtol_rel", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_xtol_rel(IntPtr opt, double tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_get_xtol_rel", CallingConvention = callingConvention)]
        private static extern double nlopt_get_xtol_rel(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_set_xtol_abs", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_xtol_abs(IntPtr opt, double[] tolerances);

        [DllImport(dllName, EntryPoint = "nlopt_get_xtol_abs", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_get_xtol_abs(IntPtr opt, double[] tolerances);

        [DllImport(dllName, EntryPoint = "nlopt_set_xtol_abs1", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_xtol_abs(IntPtr opt, double tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_set_maxeval", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_maxeval(IntPtr opt, int maxeval);

        [DllImport(dllName, EntryPoint = "nlopt_get_maxeval", CallingConvention = callingConvention)]
        private static extern int nlopt_get_maxeval(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_set_maxtime", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_maxtime(IntPtr opt, int maxtime);

        [DllImport(dllName, EntryPoint = "nlopt_get_maxtime", CallingConvention = callingConvention)]
        private static extern int nlopt_get_maxtime(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_force_stop", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_force_stop(IntPtr opt);
        #endregion

        #region constraints

        [DllImport(dllName, EntryPoint = "nlopt_set_lower_bounds1", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_lower_bounds(IntPtr opt, double lowerBound);

        [DllImport(dllName, EntryPoint = "nlopt_set_lower_bounds", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_lower_bounds(IntPtr opt, double[] lowerBounds);

        [DllImport(dllName, EntryPoint = "nlopt_get_lower_bounds", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_get_lower_bounds(IntPtr opt, double[] lowerBound);

        [DllImport(dllName, EntryPoint = "nlopt_set_upper_bounds1", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_upper_bounds(IntPtr opt, double upperBound);

        [DllImport(dllName, EntryPoint = "nlopt_set_upper_bounds", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_set_upper_bounds(IntPtr opt, double[] upperBounds);

        [DllImport(dllName, EntryPoint = "nlopt_get_upper_bounds", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_get_upper_bounds(IntPtr opt, double[] upperBound);

        [DllImport(dllName, EntryPoint = "nlopt_add_inequality_constraint", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_add_inequality(IntPtr opt, [MarshalAs(UnmanagedType.FunctionPtr)]  NLoptInequalityConstraintFunction constraintFunction, IntPtr data, double tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_add_inequality_mconstraint", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_add_inequality_mconstraint(IntPtr opt, int m, [MarshalAs(UnmanagedType.FunctionPtr)] NLoptVectorInequalityConstraintFunction constraintFunction, IntPtr data, double[] tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_remove_inequality_constraints", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_remove_inequality_constraints(IntPtr opt);

        [DllImport(dllName, EntryPoint = "nlopt_add_equality_constraint", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_add_equality(IntPtr opt, [MarshalAs(UnmanagedType.FunctionPtr)] NLoptEqualityConstraintFunction equalityConstraintFunction, IntPtr data, double tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_add_equality_mconstraint", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_add_equality_mconstraint(IntPtr opt, int m, [MarshalAs(UnmanagedType.FunctionPtr)] NLoptVectorEqualityConstraintFunction constraintFunction, IntPtr data, double[] tolerance);

        [DllImport(dllName, EntryPoint = "nlopt_remove_equality_constraints", CallingConvention = callingConvention)]
        private static extern NLoptResultCode nlopt_remove_equality_constraints(IntPtr opt);
        #endregion

        #endregion

        #region private members

        /// <summary>The NLopt algorithm in its <see cref="NLoptAlgorithm"/> representation.
        /// </summary>
        private NLoptAlgorithm m_NLoptAlgorithm;

        /// <summary>The pointer with respect to the internal NLopt algorithm struct 'nlopt_opt'.
        /// </summary>
        private IntPtr m_NLopPtr = IntPtr.Zero;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptPtr"/> class.
        /// </summary>
        /// <param name="algorithm">The NLopt algorithm in its <see cref="NLoptAlgorithm"/> representation.</param>
        /// <param name="dimension">The dimension of the feasible region.</param>
        public NLoptPtr(NLoptAlgorithm algorithm, int dimension)
        {
            m_NLoptAlgorithm = algorithm;

            m_NLopPtr = nlopt_create(m_NLoptAlgorithm, dimension);
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new Exception(String.Format(ExceptionMessages.ObjectIsNotOperable, "NLopt[Ptr]"));
            }
        }
        #endregion

        #region destructor

        /// <summary>Finalizes an instance of the <see cref="NLoptPtr" /> class.
        /// </summary>
        ~NLoptPtr()
        {
            if (m_NLopPtr != IntPtr.Zero)
            {
                nlopt_destroy(m_NLopPtr);
                m_NLopPtr = IntPtr.Zero;
            }
        }
        #endregion

        #region public properties

        #region stopping conditions

        /// <summary>Gets the maximum number of function evaluations taken into account as stopping condition.
        /// </summary>
        /// <value>The maximum function evaluations.</value>
        public int MaxEvaluations
        {
            get
            {
                if (m_NLopPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("NLoptPtr");
                }
                int maxEvaluations = nlopt_get_maxeval(m_NLopPtr);
                return (maxEvaluations <= 0) ? Int32.MaxValue : maxEvaluations;
            }
        }

        /// <summary>Gets the maximum evaluation time (in seconds) taken into account as stopping condition.
        /// </summary>
        /// <value>The maximum evaluation time (in seconds).</value>
        public int MaxEvaluationTime
        {
            get
            {
                if (m_NLopPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("NLoptPtr");
                }
                int maxEvaluationTime = nlopt_get_maxtime(m_NLopPtr);
                return (maxEvaluationTime <= 0) ? Int32.MaxValue : maxEvaluationTime;
            }
        }

        /// <summary>Gets the absolute function tolerance taken into account as stopping condition.
        /// </summary>
        /// <value>The absolute function value tolerance.</value>
        public double AbsoluteFTolerance
        {
            get
            {
                if (m_NLopPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("NLoptPtr");
                }
                double fAbsTolerance = nlopt_get_ftol_abs(m_NLopPtr);
                return (fAbsTolerance <= 0.0) ? 0.0 : fAbsTolerance;
            }
        }

        /// <summary>Gets the relative function value tolerance taken into account as stopping condition.
        /// </summary>
        /// <value>The relative function value tolerance.</value>
        public double RelativeFTolerance
        {
            get
            {
                if (m_NLopPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("NLoptPtr");
                }
                var fRelTolerance = nlopt_get_ftol_rel(m_NLopPtr);
                return (fRelTolerance <= 0.0) ? 0.0 : fRelTolerance;
            }
        }

        /// <summary>Gets the absolute argument tolerance taken into account as stopping condition.
        /// </summary>
        /// <value>The absolute argument tolerance.</value>
        public double[] AbsoluteXTolerance
        {
            get
            {
                if (m_NLopPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("NLoptPtr");
                }
                var absTolerance = new double[Dimension];
                if (nlopt_get_xtol_abs(m_NLopPtr, absTolerance) == NLoptResultCode.Success)
                {
                    return absTolerance;
                }
                return null;
            }
        }

        /// <summary>Gets the relative argument tolerance taken into account as stopping condition.
        /// </summary>
        /// <value>The relative argument tolerance.</value>
        public double RelativeXTolerance
        {
            get
            {
                if (m_NLopPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("NLoptPtr");
                }
                var relTolerance = nlopt_get_xtol_rel(m_NLopPtr);
                return (relTolerance <= 0) ? 0.0 : relTolerance;
            }
        }
        #endregion

        /// <summary>Gets the dimension of the feasible region.
        /// </summary>
        /// <value>The dimension of the feasible region.</value>
        public int Dimension
        {
            get
            {
                if (m_NLopPtr == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("NLoptPtr");
                }
                return nlopt_get_dimension(m_NLopPtr);
            }
        }
        #endregion

        #region public/internal methods

        #region IDisposable Member

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (m_NLopPtr != IntPtr.Zero)
            {
                nlopt_destroy(m_NLopPtr);
                m_NLopPtr = IntPtr.Zero;
            }
        }
        #endregion

        #region stopping conditions

        /// <summary>Sets the maximum numbers of function evaluations to take into account as stopping condition.
        /// </summary>
        /// <param name="maxNumberOfEvaluations">The maximum number of iterations.</param>
        /// <returns>A value indicating whether <see cref="MaxEvaluations"/> has been changed.</returns>
        public NLoptResultCode TrySetMaxEvaluations(int maxNumberOfEvaluations)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_maxeval(m_NLopPtr, maxNumberOfEvaluations);
        }

        /// <summary>Sets the maximum evaluation time (in seconds) to take into account as stopping condition.
        /// </summary>
        /// <param name="maxEvaluationTime">The max evaluation time.</param>
        /// <returns>A value indicating whether <see cref="MaxEvaluationTime"/> has been changed.</returns>
        public NLoptResultCode TrySetMaxEvaluationTime(int maxEvaluationTime)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_maxtime(m_NLopPtr, maxEvaluationTime);
        }

        /// <summary>Sets the absolute function value tolerance (ftol) to take into account as stopping condition.
        /// </summary>
        /// <param name="absoluteTolerance">The absolute tolerance.</param>
        /// <returns>A value indicating whether <see cref="AbsoluteFTolerance"/> has been changed.</returns>
        public NLoptResultCode TrySetAbsoluteFValueTolerance(double absoluteTolerance)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_ftol_abs(m_NLopPtr, absoluteTolerance);
        }

        /// <summary>Sets the relative function value tolerance (ftol) to take into account as stopping condition.
        /// </summary>
        /// <param name="relativeTolerance">The relative tolerance.</param>
        /// <returns>A value indicating whether <see cref="RelativeFTolerance"/> has been changed.</returns>
        public NLoptResultCode TrySetRelativeFValueTolerance(double relativeTolerance)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_ftol_rel(m_NLopPtr, relativeTolerance);
        }

        /// <summary>Sets the absolute argument tolerance (xtol) to take into account as stopping condition.
        /// </summary>
        /// <param name="absoluteTolerance">The absolute tolerance.</param>
        /// <returns>A value indicating whether <see cref="AbsoluteXTolerance"/> has been changed.</returns>
        public NLoptResultCode TrySetAbsoluteXTolerance(double absoluteTolerance)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_xtol_abs(m_NLopPtr, absoluteTolerance);
        }

        /// <summary>Sets the relative argument tolerance (xtol) to take into account as stopping condition.
        /// </summary>
        /// <param name="relativeTolerance">The relative tolerance.</param>
        /// <returns>A value indicating whether <see cref="RelativeXTolerance"/> has been changed.</returns>
        public NLoptResultCode TrySetRelativeXTolerance(double relativeTolerance)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_xtol_rel(m_NLopPtr, relativeTolerance);
        }

        /// <summary>Attempt to force the calculation.
        /// </summary>
        /// <returns>A value indicating whether the calculation could be stopped.</returns>
        public NLoptResultCode TryCancellationPending()
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_force_stop(m_NLopPtr);
        }
        #endregion

        #region constraint functions

        /// <summary>Sets the lower bounds of the Bound constraint.
        /// </summary>
        /// <param name="lowerBounds">The lower bounds.</param>
        /// <returns>A value indicating whether the lower bounds have been changed.</returns>
        public NLoptResultCode SetLowerBounds(double[] lowerBounds)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_lower_bounds(m_NLopPtr, lowerBounds);
        }

        /// <summary>Sets the upper bounds of the Bound constraint.
        /// </summary>
        /// <param name="upperBounds">The upper bounds.</param>
        /// <returns>A value indicating whether the upper bounds have been changed.</returns>
        public NLoptResultCode SetUpperBounds(double[] upperBounds)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_upper_bounds(m_NLopPtr, upperBounds);
        }

        /// <summary>Adds a specific inequality constraint.
        /// </summary>
        /// <param name="inequalityConstraintFunction">The inequality constraint function, i.e. a argument x will be accepted iff c(x) &lt; <paramref name="tolerance"/>.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>A value indicating whether the inequality constraint has been added.</returns>
        public NLoptResultCode AddInequalityConstraint(NLoptInequalityConstraintFunction inequalityConstraintFunction, double tolerance)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_add_inequality(m_NLopPtr, inequalityConstraintFunction, IntPtr.Zero, tolerance);
        }

        /// <summary>Adds a specific inequality constraint.
        /// </summary>
        /// <param name="inequalityConstraintFunction">The inequality constraint function, i.e. a argument x will be accepted iff c(x) &lt; <paramref name="tolerance"/> component-wise.</param>
        /// <param name="constraintFunctionDimension"> The dimensionality of the constraint function.</param>
        /// <param name="tolerance">An array of at least <paramref name="constraintFunctionDimension"/> elements that represents the tolerance in each constraint dimension; or <c>null</c> for zero tolerances.</param>
        /// <returns>A value indicating whether the inequality constraint has been added.</returns>
        public NLoptResultCode AddInequalityConstraint(NLoptVectorInequalityConstraintFunction inequalityConstraintFunction, int constraintFunctionDimension, double[] tolerance)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_add_inequality_mconstraint(m_NLopPtr, constraintFunctionDimension, inequalityConstraintFunction, IntPtr.Zero, tolerance);
        }

        /// <summary>Removes all inequality constraints.
        /// </summary>
        /// <returns>A value indicating whether the inequality constraints has been removed.</returns>
        public NLoptResultCode RemoveInequalityConstraints()
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_remove_inequality_constraints(m_NLopPtr);
        }

        /// <summary>Adds a specific inequality constraint.
        /// </summary>
        /// <param name="equalityConstraintFunction">The equality constraint function, i.e. a argument x will be accepted iff |c(x)| &lt; <paramref name="tolerance"/>.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>A value indicating whether the equality constraint has been added.</returns>
        public NLoptResultCode AddEqualityConstraint(NLoptEqualityConstraintFunction equalityConstraintFunction, double tolerance)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_add_equality(m_NLopPtr, equalityConstraintFunction, IntPtr.Zero, tolerance);
        }

        /// <summary>Removes all equality constraints.
        /// </summary>
        /// <returns>A value indicating whether the equality constraints has been removed.</returns>
        public NLoptResultCode RemoveEqualityConstraints()
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_remove_equality_constraints(m_NLopPtr);
        }
        #endregion

        /// <summary>Sets the real-valued function to minimize.
        /// </summary>
        /// <param name="function">The objective function with respect to the NLopt function pointer representation.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="function"/> is <c>null</c>.</exception>
        public void SetFunction(NLoptObjectiveFunction function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            nlopt_set_min_objective(m_NLopPtr, function, IntPtr.Zero);
        }

        /// <summary>Sets the real-valued function to minimize.
        /// </summary>
        /// <param name="function">The objective function with respect to the NLopt function pointer representation.</param>
        /// <param name="data">The 'data' argument of the objective function in its <see cref="GCHandle"/> representation.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="function"/> is <c>null</c>.</exception>
        public void SetFunction(NLoptObjectiveFunction function, GCHandle data)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            nlopt_set_min_objective(m_NLopPtr, function, GCHandle.ToIntPtr(data));
        }

        /// <summary>Finds the minimum, i.e. starts the optimization.
        /// </summary>
        /// <param name="argMin">The input is the initial guess, the output is the argMin.</param>
        /// <param name="minimum">The minimum (output).</param>
        /// <returns>A value indicating the stopping condition etc. used the optimizer to get the result.</returns>
        public NLoptResultCode FindMinimum(double[] argMin, out double minimum)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_optimize(m_NLopPtr, argMin, out minimum);
        }

        /// <summary>Sets a specific subsidiary (local) optimizer.
        /// </summary>
        /// <param name="nloptLocalOptimizer">The local optimizer.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="nloptLocalOptimizer"/> is <c>null</c>.</exception>
        public NLoptResultCode SetLocalOptimizer(NLoptPtr nloptLocalOptimizer)
        {
            if (nloptLocalOptimizer == null)
            {
                throw new ArgumentNullException(nameof(nloptLocalOptimizer));
            }
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_local_optimizer((IntPtr)m_NLopPtr, (IntPtr)nloptLocalOptimizer.m_NLopPtr);
        }

        /// <summary>Sets the initial size of the algorithm, i.e. the size to perturb x when the optimization starts.
        /// </summary>
        /// <param name="initialStepSize">The initial size of the step.</param>
        /// <returns>A value indicating whether the initial step size has been changed.</returns>
        public NLoptResultCode SetInitialStepSize(double initialStepSize)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_initial_step1(m_NLopPtr, initialStepSize);
        }

        /// <summary>Sets the initial size of the algorithm, i.e. the size to perturb x when the optimization starts.
        /// </summary>
        /// <param name="initialStepSize">The initial size of the step (for each coordinate, i.e. a array of length at least <see cref="Dimension"/>).</param>
        /// <returns>A value indicating whether the initial step size has been changed.</returns>
        public NLoptResultCode SetInitialStepSize(double[] initialStepSize)
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            return nlopt_set_initial_step(m_NLopPtr, initialStepSize);
        }

        /// <summary>Removes the constraints.
        /// </summary>
        public void RemoveConstraints()
        {
            if (m_NLopPtr == IntPtr.Zero)
            {
                throw new ObjectDisposedException("NLoptPtr");
            }
            nlopt_remove_inequality_constraints(m_NLopPtr);
            nlopt_remove_equality_constraints(m_NLopPtr);
            nlopt_set_upper_bounds(m_NLopPtr, Double.PositiveInfinity);
            nlopt_set_lower_bounds(m_NLopPtr, Double.NegativeInfinity);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return GetName(m_NLoptAlgorithm);
        }
        #endregion

        #region public static methods

        /// <summary>Gets the name of a specific NLopt algorithm.
        /// </summary>
        /// <param name="nloptalgorithm">The NLopt algorithm in its <see cref="NLoptAlgorithm"/> representation.</param>
        /// <returns>The name of the specific NLopt algorithm in its <see cref="System.String"/> representation.</returns>
        public static string GetName(NLoptAlgorithm nloptalgorithm)
        {
            return Marshal.PtrToStringAnsi(nlopt_AlgorithmName(nloptalgorithm));
        }
        #endregion
    }
}