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
using Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Implements the PRAXIS algorithm (principal axis method) to find a minimum of a real-valued function.
    /// </summary>
    /// <remarks>The implementation is based on the following references:
    /// <para>Powell: 'An efficient method for finding the minimum of a function in several variables without
    /// calculating derivatives, computer journal 7 (1964), p.155-162.</para>
    /// <para>Richard P. Brent: 'Algorithms for minimization without derivatives' (1973), §7.</para>
    /// <para>Karl Gegenfurtner: 'PRAXIS: Brent's algorithm for function minimization', 
    /// Behavior Research Methods, Instruments and Computers 1992, 24 (4), p.560-564. Also a (first?) C implementation (1987).</para>
    /// <para>Ali Uenlue, Michael D. Kickmeier: 'EPM: A strategy for principal axis minimization', 2004.</para>
    /// </remarks>
    public partial class PraxisOptimizer : OrdinaryMultiDimOptimizer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the optimizer algorithm.
        /// </summary>
        public static readonly PraxisOptimizerAbortCondition StandardAbortCondition;

        /// <summary>Represents the standard transformation etc. for the support of specific constraints (the original algorithm does not support any constraints).
        /// </summary>
        public static readonly MultiDimOptimizerConstraintProvider StandardConstraintProvider;
        #endregion

        #region private members

        /// <summary>The (single) random number stream.
        /// </summary>
        private SingleRandomNumberStream m_SingleRandomNumberStream;

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

        /// <summary>Initializes the <see cref="PraxisOptimizer" /> class.
        /// </summary>
        static PraxisOptimizer()
        {
            StandardAbortCondition = PraxisOptimizerAbortCondition.Create();
            StandardConstraintProvider = MultiDimOptimizerConstraintProvider.BoxTransformation;
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="PraxisOptimizer"/> class.
        /// </summary>
        /// <param name="randomNumberStream">The random number stream.</param>
        /// <param name="scalingFactor">A scaling parameter. If the scales for the different parameters are very different this value should be/ set to a value of about 10.0.</param>
        /// <param name="expectedDistanceToSolution">A step length parameter which should be set equal to the expected distance from the solution.</param>
        /// <remarks>The <see cref="PraxisOptimizer.StandardAbortCondition"/> and <see cref="PraxisOptimizer.StandardConstraintProvider"/> are taken into account.</remarks>
        public PraxisOptimizer(IRandomNumberStream randomNumberStream, double scalingFactor = 1.0, double expectedDistanceToSolution = 1.0)
            : this(randomNumberStream, StandardAbortCondition, StandardConstraintProvider, scalingFactor, expectedDistanceToSolution)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PraxisOptimizer"/> class.
        /// </summary>
        /// <param name="randomNumberStream">The random number stream.</param>
        /// <param name="abortCondition">The abort (stopping) condition for the Simulated Annealing optimizer.</param>
        /// <param name="constraintProvider">The constraint provider, i.e. transformation etc. for the support of specific constraints (the original algorithm does not support any constraints).</param>
        /// <param name="scalingFactor">A scaling parameter. If the scales for the different parameters are very different this value should be/ set to a value of about 10.0.</param>
        /// <param name="expectedDistanceToSolution">A step length parameter which should be set equal to the expected distance from the solution.</param>
        public PraxisOptimizer(IRandomNumberStream randomNumberStream, PraxisOptimizerAbortCondition abortCondition, MultiDimOptimizerConstraintProvider constraintProvider, double scalingFactor = 1.0, double expectedDistanceToSolution = 1.0)
        {
            if (randomNumberStream == null)
            {
                throw new ArgumentNullException(nameof(randomNumberStream));
            }
            m_SingleRandomNumberStream = new SingleRandomNumberStream(randomNumberStream, 250);
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));
            m_ConstraintProvider = constraintProvider ?? throw new ArgumentNullException(nameof(constraintProvider));

            ScalingFactor = scalingFactor;
            ExpectedDistanceToSolution = expectedDistanceToSolution;
            m_Name = new IdentifierString("PRAXIS optimizer");
            m_FunctionDescriptor = new OrdinaryMultiDimOptimizerFunctionFactory();
            m_ConstraintDescriptor = new MultiDimOptimizerConstraintFactory(constraintProvider.SupportedConstraints);
        }
        #endregion

        #region public properties

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public PraxisOptimizerAbortCondition AbortCondition
        {
            get;
            private set;
        }

        /// <summary>Gets a steplength parameter which should be set equal to the expected distance from the solution, 1.0 is default.
        /// </summary>
        /// <value>A steplength parameter which should be set equal to the expected distance from the solution, 1.0 is default.</value>
        /// <remarks>Exceptionally small or large values of lead to slower convergence on the first few iterations.</remarks>
        public double ExpectedDistanceToSolution
        {
            get;
            private set;
        }

        /// <summary>Gets a scaling parameter, 1.0 is default and indicates no scaling.
        /// </summary>
        /// <value>A scaling parameter, 1.0 is default and indicates no scaling.</value>
        /// <remarks>If the scales for the different parameters are very different this value should be/ set to a value of about 10.0.</remarks>
        public double ScalingFactor
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
            return true;
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