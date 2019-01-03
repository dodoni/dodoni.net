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
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as abstract basis for multi-dimensional optimization problems with respect to real-valued objective functions. 
    /// </summary>
    public abstract partial class OrdinaryMultiDimOptimizer : MultiDimOptimizer
    {
        #region private nested classes (for explicit cast to Quadratic Program)

        /// <summary>A wrapper that encapsulates a <see cref="OrdinaryMultiDimOptimizer"/> in a <see cref="QuadraticProgram"/> representation.
        /// </summary>
        private class QuadraticProgramRepresentation : QuadraticProgram
        {
            #region private members

            /// <summary>The encapsulated <see cref="OrdinaryMultiDimOptimizer"/> object.
            /// </summary>
            private OrdinaryMultiDimOptimizer m_OrdinaryMultiDimOptimizer;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="QuadraticProgramRepresentation"/> class.
            /// </summary>
            /// <param name="ordinaryMultiDimOptimizer">The ordinary multi-dimensional optimizer.</param>
            internal QuadraticProgramRepresentation(OrdinaryMultiDimOptimizer ordinaryMultiDimOptimizer)
            {
                m_OrdinaryMultiDimOptimizer = ordinaryMultiDimOptimizer;
            }
            #endregion

            #region public/protected methods

            /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm"/> object.
            /// </summary>
            /// <param name="dimension">The dimension of the unconstrainted feasible region.</param>
            /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
            /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support a unconstraint feasible region.</exception>
            public override IMultiDimOptimizerAlgorithm Create(int dimension)
            {
                return m_OrdinaryMultiDimOptimizer.Create(dimension);
            }

            /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm"/> object.
            /// </summary>
            /// <param name="constraints">A collection of contraints for the optimization algorithm represented by the current instance, where each constraint has been created via a specific function of property <see cref="MultiDimOptimizer.Constraint"/>.</param>
            /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
            public override IMultiDimOptimizerAlgorithm Create(params IConstraint[] constraints)
            {
                return m_OrdinaryMultiDimOptimizer.Create(constraints);
            }

            /// <summary>Gets descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="MultiDimOptimizer.IConstraintFactory"/> representation.
            /// </summary>
            /// <returns>Descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="MultiDimOptimizer.IConstraintFactory"/> representation.</returns>
            protected override IConstraintFactory GetConstraintFactory()
            {
                return m_OrdinaryMultiDimOptimizer.GetConstraintFactory();
            }

            /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.
            /// </summary>
            /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.</returns>
            protected override MultiDimOptimizer.IFunctionFactory GetFunctionFactory()
            {
                return m_OrdinaryMultiDimOptimizer.GetFunctionFactory();
            }

            /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.
            /// </summary>
            /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.</returns>
            protected override QuadraticProgram.IFunctionFactory GetQuadraticFunctionDescriptor()
            {
                var ordinaryFunctionDescriptor = m_OrdinaryMultiDimOptimizer.GetOrdinaryFunctionDescriptor();
                return new QuadraticFunctionDescriptor(ordinaryFunctionDescriptor);
            }

            /// <summary>Gets a value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
            /// </summary>
            /// <returns>A value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.</returns>
            protected override bool GetIsRandomAlgorithm()
            {
                return m_OrdinaryMultiDimOptimizer.GetIsRandomAlgorithm();
            }

            /// <summary>Gets the name of the multi-dimensional optimizer.
            /// </summary>
            /// <returns>The name of the multi-dimensional optimizer.</returns>
            protected override IdentifierString GetName()
            {
                return m_OrdinaryMultiDimOptimizer.GetName();
            }

            /// <summary>Gets the long name of the multi-dimensional optimizer.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the multi-dimensional optimizer.</returns>
            protected override IdentifierString GetLongName()
            {
                return m_OrdinaryMultiDimOptimizer.GetLongName();
            }
            #endregion
        }

        /// <summary>This class is needed in <see cref="QuadraticProgramRepresentation"/> for the explicit cast into <see cref="QuadraticProgram"/> representation.
        /// </summary>
        private class QuadraticFunctionDescriptor : QuadraticProgram.IFunctionFactory, MultiDimOptimizer.IFunctionFactory
        {
            #region private members

            /// <summary>The encapsulated <see cref="OrdinaryMultiDimOptimizer.IFunctionFactory"/> object.
            /// </summary>
            private OrdinaryMultiDimOptimizer.IFunctionFactory m_OrdinaryFunctionDescriptor;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="QuadraticFunctionDescriptor"/> class.
            /// </summary>
            /// <param name="ordinaryFunctionDescriptor">The ordinary function descriptor.</param>
            internal QuadraticFunctionDescriptor(OrdinaryMultiDimOptimizer.IFunctionFactory ordinaryFunctionDescriptor)
            {
                m_OrdinaryFunctionDescriptor = ordinaryFunctionDescriptor;
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return m_OrdinaryFunctionDescriptor.InfoOutputDetailLevel; }
            }

            /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return m_OrdinaryFunctionDescriptor.TrySetInfoOutputDetailLevel(infoOutputDetailLevel);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
            {
                m_OrdinaryFunctionDescriptor.FillInfoOutput(infoOutput, categoryName);
            }
            #endregion

            #region IFunctionFactory Members

            /// <summary>Creates a specific <see cref="MultiDimOptimizer.IFunction"/> object that represents the objective function  1/2 * x^t * A * x + b^t * x.
            /// </summary>
            /// <param name="A">The quadratic program matrix A.</param>
            /// <param name="b">The quadratic program vector b.</param>
            /// <returns>The specific <see cref="MultiDimOptimizer.IFunction"/> object that represents the objective function  1/2 * x^t * A * x + b^t * x.</returns>
            public IFunction Create(DenseMatrix A, double[] b)
            {
                return m_OrdinaryFunctionDescriptor.Create(A.RowCount, x => 0.5 * DenseMatrix.GetBilinearForm(A, x) + BLAS.Level1.ddot(A.RowCount, x, b));
            }
            #endregion
        }
        #endregion

        /// <summary>Initializes a new instance of the <see cref="OrdinaryMultiDimOptimizer"/> class.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail in its <see cref="Dodoni.BasicComponents.Containers.InfoOutputDetailLevel"/> representation.</param>
        protected OrdinaryMultiDimOptimizer(InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
            : base(infoOutputDetailLevel)
        {
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the objective function.
        /// </summary>
        /// <value>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the objective function.</value>
        public new IFunctionFactory Function
        {
            get { return GetOrdinaryFunctionDescriptor(); }
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.
        /// </summary>
        /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.</returns>
        protected abstract OrdinaryMultiDimOptimizer.IFunctionFactory GetOrdinaryFunctionDescriptor();

        /// <summary>Performs an explicit conversion from <see cref="Dodoni.MathLibrary.Optimizer.MultiDimensional.OrdinaryMultiDimOptimizer"/> to <see cref="Dodoni.MathLibrary.Optimizer.MultiDimensional.QuadraticProgram"/>.
        /// </summary>
        /// <param name="ordinaryMultiDimOptimizer">The ordinary multi-dimensional optimizer.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator QuadraticProgram(OrdinaryMultiDimOptimizer ordinaryMultiDimOptimizer)
        {
            return new QuadraticProgramRepresentation(ordinaryMultiDimOptimizer);
        }
    }
}