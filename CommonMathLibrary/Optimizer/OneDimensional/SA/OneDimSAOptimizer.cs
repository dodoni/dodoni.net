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
using Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine;

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Implements a one-dimensional optimization via Simulated Annealing (bounded interval constraints are supported only).
    /// </summary>
    /// <remarks>Based on 
    /// <para>'Global optimization of statistical functions with simulated annealing', W. L. Goffe, G. D. Ferrier, J. Rogers, Journal of Econometrics 60 (1994), p.65-99. </para>
    /// The convergence tests are simular to them given in <para>'Use of a simulated annealing algorithm to fit compartmental models with an application to fractal pharmacokinetics', 
    /// R. E. Marsh, T. A. Riauka, S. A. McQuarrie, 2007. See also the Fortran implementation 'simann.f'.</para>
    /// </remarks>
    public partial class OneDimSAOptimizer : OneDimOptimizer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the Simulated Annealing algorithm.
        /// </summary>
        public static readonly OneDimSAOptimizerAbortCondition StandardAbortCondition;

        /// <summary>Represents the standard configuration of the Simulated Annealing algorithm.
        /// </summary>
        public static readonly OneDimSAOptimizerConfiguration StandardConfiguration;
        #endregion

        #region private members

        /// <summary>The name of the current instance in its <see cref="IdentifierString"/> representation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The factory for objective functions.
        /// </summary>
        private OneDimOptimizerFunctionFactory m_ObjectiveFunctionFactory;

        /// <summary>The descriptor and factory for constraints.
        /// </summary>
        private OneDimOptimizerConstraintFactory m_ConstraintDescriptor;

        /// <summary>The (single) random number stream.
        /// </summary>
        private SingleRandomNumberStream m_SingleRandomNumberStream;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="OneDimSAOptimizer" /> class.
        /// </summary>
        static OneDimSAOptimizer()
        {
            StandardAbortCondition = OneDimSAOptimizerAbortCondition.Create();
            StandardConfiguration = OneDimSAOptimizerConfiguration.Create();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimSAOptimizer"/> class.
        /// </summary>
        /// <param name="randomNumberStream">The random number stream.</param>
        /// <remarks>The <see cref="OneDimSAOptimizer.StandardAbortCondition"/> and <see cref="OneDimSAOptimizer.StandardConfiguration"/> are taken into account.</remarks>
        public OneDimSAOptimizer(IRandomNumberStream randomNumberStream)
            : this(randomNumberStream, StandardConfiguration, StandardAbortCondition)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="OneDimSAOptimizer"/> class.
        /// </summary>
        /// <param name="randomNumberStream">The random number stream.</param>
        /// <param name="configuration">The configuration of the Simulated Annealing optimizer.</param>
        /// <param name="abortCondition">The abort (stopping) condition for the Simulated Annealing optimizer.</param>
        public OneDimSAOptimizer(IRandomNumberStream randomNumberStream, OneDimSAOptimizerConfiguration configuration, OneDimSAOptimizerAbortCondition abortCondition)
        {
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (randomNumberStream == null)
            {
                throw new ArgumentNullException(nameof(randomNumberStream));
            }
            m_SingleRandomNumberStream = new SingleRandomNumberStream(randomNumberStream, 250);

            m_Name = new IdentifierString(String.Format("1-dim Simulated Annealing; {0}", abortCondition.ToString()));
            m_ObjectiveFunctionFactory = new OneDimOptimizerFunctionFactory();
            m_ConstraintDescriptor = new OneDimOptimizerConstraintFactory(OneDimOptimizerConstraintFactory.ConstraintType.BoundedInterval);
        }
        #endregion

        #region public properties

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public OneDimSAOptimizerAbortCondition AbortCondition
        {
            get;
            private set;
        }

        /// <summary>Gets the configuration of the Simulated Annealing algorithm in its <see cref="OneDimSAOptimizerConfiguration"/> representation.
        /// </summary>
        /// <value>The configuration of the Simulated Annealing algorithm in its <see cref="OneDimSAOptimizerConfiguration"/> representation.</value>
        public OneDimSAOptimizerConfiguration Configuration
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IOneDimOptimizerAlgorithm" /> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimOptimizerAlgorithm" /> object.</returns>
        public override IOneDimOptimizerAlgorithm Create()
        {
            throw new InvalidOperationException();  // this algorithm does not support unbounded constraints
        }

        /// <summary>Creates a new <see cref="IOneDimOptimizerAlgorithm" /> object.
        /// </summary>
        /// <param name="constraint">A contraint for the optimization algorithm represented by the current instance, where the parameter has been created via property <see cref="OneDimOptimizer.Constraint" />; if
        /// <c>null</c> an unconstraint feasible region is assumed, i.e. identical to <see cref="OneDimOptimizer.Create()" />.</param>
        /// <returns>A new <see cref="IOneDimOptimizerAlgorithm" /> object.</returns>
        /// <remarks>If <paramref name="constraint" /> is <c>null</c> this method returns the same value as <see cref="OneDimOptimizer.Create()" />.</remarks>
        public override IOneDimOptimizerAlgorithm Create(OneDimOptimizer.IConstraint constraint)
        {
            if (constraint is OneDimOptimizerConstraint)
            {
                var oneDimConstraint = (OneDimOptimizerConstraint)constraint;
                if ((Double.IsNegativeInfinity(oneDimConstraint.IntervalRepresentation.Infimum) == false) && (Double.IsPositiveInfinity(oneDimConstraint.IntervalRepresentation.Supremum) == false))
                {
                    return new Algorithm(this, (OneDimOptimizerConstraint)constraint);
                }
            }
            throw new InvalidOperationException(nameof(constraint));
        }

        /// <summary>Gets descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="OneDimOptimizer.IConstraintFactory" /> representation.
        /// </summary>
        /// <returns>Descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="OneDimOptimizer.IConstraintFactory" /> representation.</returns>
        protected override OneDimOptimizer.IConstraintFactory GetConstraintFactory()
        {
            return m_ConstraintDescriptor;
        }

        /// <summary>Gets a factory for <see cref="OneDimOptimizer.IFunction" /> objects that encapsulate the objective function.
        /// </summary>
        /// <returns>A factory for <see cref="OneDimOptimizer.IFunction" /> objects that encapsulate the objective function.</returns>
        protected override OneDimOptimizer.IFunctionFactory GetFunctionFactory()
        {
            return m_ObjectiveFunctionFactory;
        }

        /// <summary>Gets a value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
        /// </summary>
        /// <returns>A value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.</returns>
        protected override bool GetIsRandomAlgorithm()
        {
            return true;
        }

        /// <summary>Gets the name of the 1-dimensional optimizer.
        /// </summary>
        /// <returns>The name of the 1-dimensional optimizer.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the 1-dimensional optimizer.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the 1-dimensional optimizer.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_Name;
        }
        #endregion
    }
}