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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.EquationSolvers.OneDimensional
{
    /// <summary>Represents the Bisection root finding algorithm.
    /// </summary>
    /// <remarks>The implementation is based on 'Numerical Recipes in C: The art of scientific computing', Cambridge University Press.</remarks>
    public partial class BisectionRootFinder : OneDimEquationSolver
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the algorithm.
        /// </summary>
        public static readonly BisectionRootFinderAbortCondition StandardAbortCondition;

        /// <summary>Represents a standard <see cref="IRootBracketing"/> approach.
        /// </summary>
        public static readonly IRootBracketing StandardBracketing;
        #endregion

        #region private members

        /// <summary>The name of the current instance.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The constraint factory.
        /// </summary>
        private OneDimRootFinderConstraintFactory m_ConstraintFactory;

        /// <summary>The objective function factory.
        /// </summary>
        private OneDimRootFinderFunctionFactory m_FunctionFactory;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see BisectionRootFinder/> class.
        /// </summary>
        static BisectionRootFinder()
        {
            StandardAbortCondition = BisectionRootFinderAbortCondition.Create();
            StandardBracketing = new BisectionRootBracketing();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="BisectionRootFinder"/> class.
        /// </summary>
        /// <remarks>The <see cref="BisectionRootFinder.StandardAbortCondition"/> and <see cref="BisectionRootFinder.StandardBracketing"/> are taken into account.</remarks>
        public BisectionRootFinder()
            : this(StandardBracketing, StandardAbortCondition)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BisectionRootFinder"/> class.
        /// </summary>
        /// <param name="bracketingApproach">The Bracketing approach.</param>
        /// <param name="abortCondition">The abort (stopping) condition for the Brent optimizer.</param>
        public BisectionRootFinder(IRootBracketing bracketingApproach, BisectionRootFinderAbortCondition abortCondition)
        {
            Bracketing = bracketingApproach ?? throw new ArgumentNullException(nameof(bracketingApproach));
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));

            m_Name = new IdentifierString(String.Format("Bisection root finder; {0}", abortCondition.ToString()));

            m_ConstraintFactory = new OneDimRootFinderConstraintFactory(OneDimRootFinderConstraintFactory.ConstraintType.All);
            m_FunctionFactory = new OneDimRootFinderFunctionFactory(ObjectiveFunctionDerivativeRequirement.None);
        }
        #endregion

        #region public properties

        /// <summary>Gets the bracketing approach of the algorithm.
        /// </summary>
        /// <value>The bracketing approach.</value>
        public IRootBracketing Bracketing
        {
            get;
            private set;
        }

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public BisectionRootFinderAbortCondition AbortCondition
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IOneDimEquationSolverAlgorithm" /> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimEquationSolverAlgorithm" /> object.</returns>
        public override IOneDimEquationSolverAlgorithm Create()
        {
            return new Algorithm(this, new OneDimRootFinderConstraint(m_ConstraintFactory, Interval.RealAxis));
        }

        /// <summary>Creates a new <see cref="IOneDimEquationSolverAlgorithm" /> object.
        /// </summary>
        /// <param name="constraint">A contraint for the root finder algorithm represented by the current instance, where the parameter has been created via property <see cref="OneDimEquationSolver.Constraint" />; if
        /// <c>null</c> an unconstraint feasible region is assumed, i.e. identical to <see cref="OneDimEquationSolver.Create()" />.</param>
        /// <returns>A new <see cref="IOneDimEquationSolverAlgorithm" /> object.</returns>
        /// <remarks>If <paramref name="constraint" /> is <c>null</c> this method returns the same value as <see cref="OneDimEquationSolver.Create()" />.</remarks>
        public override IOneDimEquationSolverAlgorithm Create(OneDimEquationSolver.IConstraint constraint)
        {
            if (constraint == null)
            {
                return Create();
            }
            if (constraint is OneDimRootFinderConstraint)
            {
                return new Algorithm(this, (OneDimRootFinderConstraint)constraint);
            }
            throw new InvalidOperationException();
        }
        #endregion

        #region protected methods

        /// <summary>Gets descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="OneDimEquationSolver.IConstraintFactory" /> representation.
        /// </summary>
        /// <returns>Descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="OneDimEquationSolver.IConstraintFactory" /> representation.</returns>
        protected override OneDimEquationSolver.IConstraintFactory GetConstraintFactory()
        {
            return m_ConstraintFactory;
        }

        /// <summary>Gets a factory for <see cref="OneDimEquationSolver.IFunction" /> objects that encapsulate the objective function.
        /// </summary>
        /// <returns>A factory for <see cref="OneDimEquationSolver.IFunction" /> objects that encapsulate the objective function.</returns>
        protected override OneDimEquationSolver.IFunctionFactory GetFunctionFactory()
        {
            return m_FunctionFactory;
        }

        /// <summary>Gets a value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
        /// </summary>
        /// <returns>A value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.</returns>
        protected override bool GetIsRandomAlgorithm()
        {
            return false;
        }

        /// <summary>Gets the long name of the 1-dimensional root finder.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the 1-dimensional root finder.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_Name;
        }

        /// <summary>Gets the name of the 1-dimensional root finder.
        /// </summary>
        /// <returns>The name of the 1-dimensional root finder.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }
        #endregion
    }
}