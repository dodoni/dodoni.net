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

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Represents the Nelder-Mead-simplex search algorithm (also called 'downhill simplex method') for multi-dimensional optimization.
    /// </summary>
    /// <remarks>Based on W. H. Press, 'Numerical recipes', §10.4.</remarks>
    public partial class NelderMeadOptimizer : OrdinaryMultiDimOptimizer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the Nelder-Mead-Simplex search algorithm.
        /// </summary>
        public static readonly NelderMeadOptimizerAbortCondition StandardAbortCondition;

        /// <summary>Represents the standard transformation etc. for the support of specific constraints (the original algorithm does not support any constraints).
        /// </summary>
        public static readonly MultiDimOptimizerConstraintProvider StandardConstraintProvider;
        #endregion

        #region private members

        /// <summary>The name of the algorithm in its <see cref="IdentifierString"/> representation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The factory and descriptor for objective functions.
        /// </summary>
        private OrdinaryMultiDimOptimizerFunctionFactory m_FunctionDescriptor;

        /// <summary>The factory and descriptor for constraints.
        /// </summary>
        private MultiDimOptimizerConstraintFactory m_ConstraintDescriptor;

        /// <summary>A transformation etc. for the support of specific constraints (the original algorithm does not support any constraints).
        /// </summary>
        private MultiDimOptimizerConstraintProvider m_ConstraintProvider;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="NelderMeadOptimizer" /> class.
        /// </summary>
        static NelderMeadOptimizer()
        {
            StandardAbortCondition = NelderMeadOptimizerAbortCondition.Create();
            StandardConstraintProvider = MultiDimOptimizerConstraintProvider.BoxTransformation;
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="NelderMeadOptimizer"/> class.
        /// </summary>
        /// <param name="initialScaleFactor">A scaling factor which is used to create from a specific initial guess the N + 1 points (in \R^N) of the start simplex.</param>
        /// <remarks>The <see cref="NelderMeadOptimizer.StandardAbortCondition"/> and <see cref="NelderMeadOptimizer.StandardConstraintProvider"/> are taken into account.</remarks>
        public NelderMeadOptimizer(double initialScaleFactor = 1.0)
            : this(StandardAbortCondition, StandardConstraintProvider)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NelderMeadOptimizer"/> class.
        /// </summary>
        /// <param name="abortCondition">The abort (stopping) condition for the Nelder-Mead optimizer.</param>
        /// <param name="constraintProvider">The constraint provider, i.e. transformation etc. for the support of specific constraints (the original algorithm does not support any constraints).</param>
        /// <param name="initialScaleFactor">A scaling factor which is used to create from a specific initial guess the N + 1 points (in \R^N) of the start simplex.</param>
        public NelderMeadOptimizer(NelderMeadOptimizerAbortCondition abortCondition, MultiDimOptimizerConstraintProvider constraintProvider, double initialScaleFactor = 1.0)
        {
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));
            m_ConstraintProvider = constraintProvider ?? throw new ArgumentNullException(nameof(constraintProvider));

            m_Name = new IdentifierString("Nelder-Mead-Simplex search optimizer");
            m_FunctionDescriptor = new OrdinaryMultiDimOptimizerFunctionFactory();
            m_ConstraintDescriptor = new MultiDimOptimizerConstraintFactory(constraintProvider.SupportedConstraints);
            InitialScaleFactor = initialScaleFactor;
        }
        #endregion

        #region public properties

        /// <summary>Gets a scaling factor which is used to create from a specific initial guess the N + 1 points (in \R^N) of the start simplex.
        /// </summary>
        /// <remarks> P_i = P_0 + Q + c/sqrt(2) * e_{i-1}, j = 1,...,n where e_j is the unit vector with respect to the j-th coordinate,
        /// P_0 is the initial guess; Q = c/Sqrt(2) * (Sqrt(n+1) - 1)/n * (1,...,1) and c is the <see cref="InitialScaleFactor"/>.</remarks>
        public double InitialScaleFactor
        {
            get;
            private set;
        }

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public NelderMeadOptimizerAbortCondition AbortCondition
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
            return m_ConstraintProvider.Create(this, constraints);
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
        protected override OrdinaryMultiDimOptimizer.IFunctionFactory GetOrdinaryFunctionDescriptor()
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