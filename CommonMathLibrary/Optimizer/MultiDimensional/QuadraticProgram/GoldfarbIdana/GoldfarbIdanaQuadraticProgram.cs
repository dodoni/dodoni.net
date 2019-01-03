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
    /// <summary>Implements a quadratic program, i.e. the optimization problem 1/2 * x^t * A * x + b^t * x 
    /// under linear constraints C^t*x >= c, D^t*x  = d, with respect to the approch of D. Goldfarb and  A. Idnani. Based on
    /// <para>
    ///  'A numerically stable dual method for solving strictly convex quadratic programs', D. Goldfarb, A. Idnani, Mathematical Programming 27 (1983) pp. 1-33
    /// </para>
    /// and similar to the implementation given by Luca Di Gaspero http://www.dieg.uniud.it/digaspero/.
    /// </summary>    
    public partial class GoldfarbIdanaQuadraticProgram : QuadraticProgram
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the optimizer algorithm.
        /// </summary>
        public static readonly GoldfarbIdanaQuadraticProgramAbortCondition StandardAbortCondition;
        #endregion

        #region private members

        /// <summary>The name of the algorithm in its <see cref="IdentifierString"/> representation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The factory and descriptor for objective functions.
        /// </summary>
        private QuadraticProgramFunctionFactory m_FunctionDescriptor;

        /// <summary>The factory and descriptor for constraints.
        /// </summary>
        private MultiDimOptimizerConstraintFactory m_ConstraintDescriptor;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="GoldfarbIdanaQuadraticProgram" /> class.
        /// </summary>
        static GoldfarbIdanaQuadraticProgram()
        {
            StandardAbortCondition = GoldfarbIdanaQuadraticProgramAbortCondition.Create();
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GoldfarbIdanaQuadraticProgram"/> class.
        /// </summary>
        /// <remarks>The <see cref="GoldfarbIdanaQuadraticProgram.StandardAbortCondition"/> is taken into account.</remarks>
        public GoldfarbIdanaQuadraticProgram()
            : this(StandardAbortCondition)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GoldfarbIdanaQuadraticProgram"/> class.
        /// </summary>
        /// <param name="abortCondition">The abort (stopping) condition for the Simulated Annealing optimizer.</param>
        public GoldfarbIdanaQuadraticProgram(GoldfarbIdanaQuadraticProgramAbortCondition abortCondition)
        {
            AbortCondition = abortCondition ?? throw new ArgumentNullException(nameof(abortCondition));

            m_Name = new IdentifierString("Quadratic programming optimizer");
            m_FunctionDescriptor = new QuadraticProgramFunctionFactory();
            m_ConstraintDescriptor = new MultiDimOptimizerConstraintFactory(MultiDimOptimizerConstraintFactory.ConstraintType.Box | MultiDimOptimizerConstraintFactory.ConstraintType.LinearEquation | MultiDimOptimizerConstraintFactory.ConstraintType.LinearInEquation | MultiDimOptimizerConstraintFactory.ConstraintType.None);
        }
        #endregion

        #region public properties

        /// <summary>Gets a factory for <see cref="QuadraticProgramFunction"/> objects that encapsulate the objective function.
        /// </summary>
        /// <value>A factory for <see cref="QuadraticProgramFunction"/> objects that encapsulate the objective function.</value>
        public new QuadraticProgramFunctionFactory Function
        {
            get { return m_FunctionDescriptor; }
        }

        /// <summary>Gets the abort (stopping) condition of the algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the algorithm.</value>
        public GoldfarbIdanaQuadraticProgramAbortCondition AbortCondition
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
            List<double> inequalityMatrix = new List<double>(), equalityMatrix = new List<double>();  // entries are provided column-by-column, where each column has exakt d entries (where d: dimension of feasible region)
            List<double> inequalityVector = new List<double>(), equalityVector = new List<double>();

            int dimension = -1;

            foreach (var constraint in constraints)
            {
                if (constraint is MultiDimOptimizerConstraint)
                {
                    var con = (MultiDimOptimizerConstraint)constraint;

                    if (dimension == -1)
                    {
                        dimension = con.Dimension;
                    }
                    else if (dimension != con.Dimension)
                    {
                        throw new ArgumentException(nameof(constraints));
                    }
                    if (con.RegionRepresentation is MultiDimRegion.Interval)
                    {
                        ((MultiDimRegion.Interval)con.RegionRepresentation).GetRegionConstraints(inequalityMatrix, inequalityVector);
                    }
                    else if (con.RegionRepresentation is MultiDimRegion.LinearInequality)
                    {
                        ((MultiDimRegion.LinearInequality)con.RegionRepresentation).GetRegionConstraints(inequalityMatrix, inequalityVector);
                    }
                    else if (con.RegionRepresentation is MultiDimRegion.LinearEquality)
                    {
                        ((MultiDimRegion.LinearEquality)con.RegionRepresentation).GetRegionConstraints(equalityMatrix, equalityVector);
                    }
                    else
                    {
                        throw new ArgumentException(nameof(constraints));
                    }
                }
            }
            return new Algorithm(this, dimension, inequalityMatrix, inequalityVector, equalityMatrix, equalityVector);
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
        protected override QuadraticProgram.IFunctionFactory GetQuadraticFunctionDescriptor()
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