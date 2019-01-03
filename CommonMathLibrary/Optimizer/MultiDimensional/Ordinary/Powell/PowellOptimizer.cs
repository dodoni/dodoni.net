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
using Dodoni.MathLibrary.Optimizer.OneDimensional;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Represent Powell's multi-dimensional optimization method.
    /// </summary>
    /// <remarks>Based on W. H. Press, 'Numerical recipes', §10.5.</remarks>
    public partial class PowellOptimizer : OrdinaryMultiDimOptimizer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of Powell's algorithm.
        /// </summary>
        public static readonly PowellOptimizerAbortCondition StandardAbortCondition;

        /// <summary>Represents the standard line search optimizer.
        /// </summary>
        public static readonly OneDimOptimizer StandardLineSearchOptimizer;

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

        /// <summary>Initializes the <see cref="PowellOptimizer" /> class.
        /// </summary>
        static PowellOptimizer()
        {
            StandardAbortCondition = PowellOptimizerAbortCondition.Create();
            StandardLineSearchOptimizer = new BrentOptimizer();
            StandardConstraintProvider = MultiDimOptimizerConstraintProvider.BoxTransformation;
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="PowellOptimizer"/> class.
        /// </summary>
        /// <remarks>The <see cref="PowellOptimizer.StandardAbortCondition"/>, <see cref="PowellOptimizer.StandardLineSearchOptimizer"/> and <see cref="PowellOptimizer.StandardConstraintProvider"/> are taken into account.</remarks>
        public PowellOptimizer()
            : this(StandardAbortCondition, StandardLineSearchOptimizer, StandardConstraintProvider)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PowellOptimizer"/> class.
        /// </summary>
        /// <param name="abortCondition">The abort (stopping) condition for the Simulated Annealing optimizer.</param>
        /// <param name="lineSearchOptimizer">The line search optimizer to take into account.</param>
        /// <param name="constraintProvider">The constraint provider, i.e. transformation etc. for the support of specific constraints (the original algorithm does not support any constraints).</param>
        public PowellOptimizer(PowellOptimizerAbortCondition abortCondition, OneDimOptimizer lineSearchOptimizer, MultiDimOptimizerConstraintProvider constraintProvider)
        {
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));
            LineSearchOptimizer = lineSearchOptimizer ?? throw new ArgumentNullException(nameof(lineSearchOptimizer));
            m_ConstraintProvider = constraintProvider ?? throw new ArgumentNullException(nameof(constraintProvider));

            m_Name = new IdentifierString("Powell optimizer");
            m_FunctionDescriptor = new OrdinaryMultiDimOptimizerFunctionFactory();
            m_ConstraintDescriptor = new MultiDimOptimizerConstraintFactory(constraintProvider.SupportedConstraints);
        }
        #endregion

        #region public properties

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public PowellOptimizerAbortCondition AbortCondition
        {
            get;
            private set;
        }

        /// <summary>Gets the line search optimizer applied by Powell's approach.
        /// </summary>
        /// <value>The line search optimizer applied by Powell's approach.</value>
        public OneDimOptimizer LineSearchOptimizer
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