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
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>A implementation of the Levenberg-Marquardt optimizer.
    /// </summary>
    /// <remarks>The implementation is based on the pseudo code described in "A brief description of the Levenberg-Marquardt Algorithm implemented by levmar", Manolis I. A. Lourakis, Feb. 2005. Moreover
    /// the constraints are taken into account via the "Projected Levenberg-Marquardt method" in "Levenberg-Marquardt methods with strong local convergence properties for solving nonlinear equations with convex constraints", C. Kanzow, N. Yamashita, M. Fukushima, J. of computational and applied mathematics 173 (2005), p.321-343.</remarks>
    public partial class LevenbergMarquardtOptimizer : MultivariateOptimizer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the optimizer algorithm.
        /// </summary>
        public static readonly LevenbergMarquardtAbortCondition StandardAbortCondition;
        #endregion

        #region private members

        /// <summary>The name of the algorithm in its <see cref="IdentifierString"/> representation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The factory and descriptor for objective functions.
        /// </summary>
        private MultivariateFunctionFactory m_FunctionDescriptor;

        /// <summary>The factory and descriptor for constraints.
        /// </summary>
        private MultiDimOptimizerConstraintFactory m_ConstraintDescriptor;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="LevenbergMarquardtOptimizer" /> class.
        /// </summary>
        static LevenbergMarquardtOptimizer()
        {
            StandardAbortCondition = LevenbergMarquardtAbortCondition.Create();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LevenbergMarquardtOptimizer"/> class.
        /// </summary>
        /// <remarks>The <see cref="LevenbergMarquardtOptimizer.StandardAbortCondition"/> is taken into account.</remarks>
        public LevenbergMarquardtOptimizer()
            : this(StandardAbortCondition)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LevenbergMarquardtOptimizer"/> class.
        /// </summary>
        /// <param name="abortCondition">The abort (stopping) condition for the Simulated Annealing optimizer.</param>
        public LevenbergMarquardtOptimizer(LevenbergMarquardtAbortCondition abortCondition)
        {
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));

            m_Name = new IdentifierString("Levenberg-Marquardt");
            m_FunctionDescriptor = new MultivariateFunctionFactory();
            m_ConstraintDescriptor = new MultiDimOptimizerConstraintFactory(MultiDimOptimizerConstraintFactory.ConstraintType.Box | MultiDimOptimizerConstraintFactory.ConstraintType.LinearEquation | MultiDimOptimizerConstraintFactory.ConstraintType.LinearInEquation | MultiDimOptimizerConstraintFactory.ConstraintType.None);
        }
        #endregion

        #region public properties

        /// <summary>Gets a factory for <see cref="MultivariateFunctionFactory"/> objects that encapsulate the objective function.
        /// </summary>
        /// <value>A factory for <see cref="MultivariateFunctionFactory"/> objects that encapsulate the objective function.</value>
        public new MultivariateFunctionFactory Function
        {
            get { return m_FunctionDescriptor; }
        }

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public LevenbergMarquardtAbortCondition AbortCondition
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the unconstrainted feasible region.</param>
        /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support a unconstraint feasible region.</exception>
        public override IMultiDimOptimizerAlgorithm Create(int dimension)
        {
            return new Algorithm(this, dimension);
        }

        /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm"/> object.
        /// </summary>
        /// <param name="constraints">A collection of contraints for the optimization algorithm, where each constraint has been created via the <see cref="MultiDimOptimizer.Constraint"/> factory.</param>
        /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
        public override IMultiDimOptimizerAlgorithm Create(params IConstraint[] constraints)
        {
            if (constraints == null)
            {
                throw new ArgumentNullException(nameof(constraints));
            }
            if (constraints.Length == 0)
            {
                throw new ArgumentException(nameof(constraints));
            }

            /* check whether box constraints are specified only; simple calculation of projection map */
            if (constraints.Length == 1)
            {
                if (constraints[0] is MultiDimOptimizerConstraint)
                {
                    var con = (MultiDimOptimizerConstraint)constraints[0];
                    if (con.RegionRepresentation is MultiDimRegion.Interval)
                    {
                        return new Algorithm(this, con.RegionRepresentation as MultiDimRegion.Interval);
                    }
                }
            }
            return new Algorithm(this, constraints);
        }
        #endregion

        #region protected methods

        /// <summary>Gets descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="MultiDimOptimizer.IConstraintFactory"/> representation.
        /// </summary>
        /// <returns>Descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="MultiDimOptimizer.IConstraintFactory"/> representation.</returns>
        protected override IConstraintFactory GetConstraintFactory()
        {
            return m_ConstraintDescriptor;
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.
        /// </summary>
        /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.</returns>
        protected override MultiDimOptimizer.IFunctionFactory GetFunctionFactory()
        {
            return m_FunctionDescriptor;
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.
        /// </summary>
        /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.</returns>
        protected override MultivariateOptimizer.IFunctionFactory GetMultivariateFunctionDescriptor()
        {
            return m_FunctionDescriptor;
        }

        /// <summary>Gets a value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
        /// </summary>
        /// <returns>A value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.</returns>
        protected override bool GetIsRandomAlgorithm()
        {
            return false;
        }

        /// <summary>Gets the name of the multi-dimensional optimizer.
        /// </summary>
        /// <returns>The name of the multi-dimensional optimizer.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the multi-dimensional optimizer.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the multi-dimensional optimizer.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_Name;
        }
        #endregion
    }
}