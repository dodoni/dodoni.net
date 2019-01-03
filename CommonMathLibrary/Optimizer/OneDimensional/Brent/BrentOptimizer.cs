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
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Provides the one-dimensional Brent optimizer algorithm.
    /// </summary>
    /// <remarks>The implementation is based on Press, et al. (1992) "Numerical recipes in C", 2nd ed., p.402ff.</remarks>
    public partial class BrentOptimizer : OneDimOptimizer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the algorithm.
        /// </summary>
        public static readonly BrentOptimizerAbortCondition StandardAbortCondition;

        /// <summary>Represents a standard <see cref="IMinimumBracketing"/> approach.
        /// </summary>
        public static readonly IMinimumBracketing StandardMinimumBracketing;
        #endregion

        #region private members

        /// <summary>The name of the current instance in its <see cref="IdentifierString"/> representation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The factory for objective functions.
        /// </summary>
        private OneDimOptimizerFunctionFactory m_ObjectiveFunctionFactory;

        /// <summary>A factory for 'no constraint' constraints.
        /// </summary>
        private OneDimOptimizerConstraintFactory m_ConstraintDescriptor;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="BrentOptimizer" /> class.
        /// </summary>
        static BrentOptimizer()
        {
            StandardAbortCondition = BrentOptimizerAbortCondition.Create();
            StandardMinimumBracketing = new DownhillBracketing();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BrentOptimizer"/> class.
        /// </summary>
        /// <remarks>The <see cref="BrentOptimizer.StandardAbortCondition"/> and <see cref="BrentOptimizer.StandardMinimumBracketing"/> are taken into account.</remarks>
        public BrentOptimizer()
            : this(StandardMinimumBracketing, StandardAbortCondition)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BrentOptimizer"/> class.
        /// </summary>
        /// <param name="bracketingApproach">The Bracketing approach.</param>
        /// <param name="abortCondition">The abort (stopping) condition for the Brent optimizer.</param>
        public BrentOptimizer(IMinimumBracketing bracketingApproach, BrentOptimizerAbortCondition abortCondition)
        {
            BracketingApproach = bracketingApproach ?? throw new ArgumentNullException(nameof(bracketingApproach));
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));

            m_Name = new IdentifierString(String.Format("Brent; {0}", abortCondition.ToString()));
            m_ObjectiveFunctionFactory = new OneDimOptimizerFunctionFactory();
            m_ConstraintDescriptor = new OneDimOptimizerConstraintFactory(OneDimOptimizerConstraintFactory.ConstraintType.All);
        }
        #endregion

        #region public properties

        /// <summary>Gets the bracketing approach of the algorithm.
        /// </summary>
        /// <value>The bracketing approach.</value>
        public IMinimumBracketing BracketingApproach
        {
            get;
            private set;
        }

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public BrentOptimizerAbortCondition AbortCondition
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
            return new Algorithm(this, new OneDimOptimizerConstraint(m_ConstraintDescriptor, Interval.RealAxis));
        }

        /// <summary>Creates a new <see cref="IOneDimOptimizerAlgorithm" /> object.
        /// </summary>
        /// <param name="constraint">A contraint for the optimization algorithm represented by the current instance, where the parameter has been created via property <see cref="OneDimOptimizer.Constraint" />; if
        /// <c>null</c> an unconstraint feasible region is assumed, i.e. identical to <see cref="OneDimOptimizer.Create()" />.</param>
        /// <returns>A new <see cref="IOneDimOptimizerAlgorithm" /> object.</returns>
        /// <remarks>If <paramref name="constraint" /> is <c>null</c> this method returns the same value as <see cref="OneDimOptimizer.Create()" />.</remarks>
        public override IOneDimOptimizerAlgorithm Create(OneDimOptimizer.IConstraint constraint)
        {
            if (constraint == null)
            {
                return Create();
            }
            if (constraint is OneDimOptimizerConstraint)
            {
                return new Algorithm(this, (OneDimOptimizerConstraint)constraint);
            }
            throw new InvalidOperationException();
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
            return false;  // here we assume that the bracketing algorithm is deterministic
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