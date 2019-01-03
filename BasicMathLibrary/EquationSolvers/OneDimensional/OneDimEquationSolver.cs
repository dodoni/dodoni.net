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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.EquationSolvers
{
    /// <summary>Serves as factory for <see cref="IOneDimEquationSolverAlgorithm"/> objects that represents a 1-dimensional root finder algorithm for a specific function.
    /// </summary>
    public abstract partial class OneDimEquationSolver : IIdentifierNameable, IInfoOutputQueriable
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimEquationSolver"/> class.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail in its <see cref="Dodoni.BasicComponents.Containers.InfoOutputDetailLevel"/> representation.</param>
        protected OneDimEquationSolver(InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Full)
        {
            InfoOutputDetailLevel = infoOutputDetailLevel;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the 1-dimensional root finder.
        /// </summary>
        /// <value>The language independent name of the 1-dimensional root finder.</value>
        public IdentifierString Name
        {
            get { return GetName(); }
        }

        /// <summary>Gets the long name of the 1-dimensional root finder.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the 1-dimensional root finder.</value>
        public IdentifierString LongName
        {
            get { return GetLongName(); }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get;
            private set;
        }
        #endregion

        /// <summary>Gets a value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
        /// </summary>
        /// <value><c>true</c> if this instance is random algorithm; otherwise, <c>false</c>.</value>
        public bool IsRandomAlgorithm
        {
            get { return GetIsRandomAlgorithm(); }
        }

        /// <summary>Gets a factory for constraints of the algorithm represented by the current instance.
        /// </summary>
        /// <value>A factory for constraints of the algorithm represented by the current instance.</value>
        public IConstraintFactory Constraint
        {
            get { return GetConstraintFactory(); }
        }

        /// <summary>Gets a factory for <see cref="OneDimEquationSolver.IFunction"/> objects that encapsulate the objective function.
        /// </summary>
        /// <value>A factory for <see cref="OneDimEquationSolver.IFunction"/> objects that encapsulate the objective function.</value>
        public IFunctionFactory Function
        {
            get { return GetFunctionFactory(); }
        }
        #endregion

        #region public methods

        #region IInfoOutputQueriable Member

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public virtual void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Name", Name.String);
            infoOutputPackage.Add("Is random algorithm", IsRandomAlgorithm);

            Constraint.FillInfoOutput(infoOutput, categoryName + ".Constraint.Description");
            Function.FillInfoOutput(infoOutput, categoryName + ".ObjectiveFunction.Description");
        }

        /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
        public virtual bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel);
        }
        #endregion

        /// <summary>Creates a new <see cref="IOneDimEquationSolverAlgorithm"/> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimEquationSolverAlgorithm"/> object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support a unconstraint feasible region.</exception>
        public abstract IOneDimEquationSolverAlgorithm Create();

        /// <summary>Creates a new <see cref="IOneDimEquationSolverAlgorithm"/> object.
        /// </summary>
        /// <param name="constraint">A contraint for the root finder algorithm represented by the current instance, where the parameter has been created via property <see cref="OneDimEquationSolver.Constraint"/>; if 
        /// <c>null</c> an unconstraint feasible region is assumed, i.e. identical to <see cref="OneDimEquationSolver.Create()"/>.</param>
        /// <returns>A new <see cref="IOneDimEquationSolverAlgorithm"/> object.</returns>
        /// <remarks>If <paramref name="constraint"/> is <c>null</c> this method returns the same value as <see cref="OneDimEquationSolver.Create()"/>.</remarks>
        public abstract IOneDimEquationSolverAlgorithm Create(IConstraint constraint);

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name.String;
        }
        #endregion

        #region protected methods

        /// <summary>Gets descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="OneDimEquationSolver.IConstraintFactory"/> representation.
        /// </summary>
        /// <returns>Descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="OneDimEquationSolver.IConstraintFactory"/> representation.</returns>
        protected abstract IConstraintFactory GetConstraintFactory();

        /// <summary>Gets a factory for <see cref="OneDimEquationSolver.IFunction"/> objects that encapsulate the objective function.
        /// </summary>
        /// <returns>A factory for <see cref="OneDimEquationSolver.IFunction"/> objects that encapsulate the objective function.</returns>
        protected abstract IFunctionFactory GetFunctionFactory();

        /// <summary>Gets a value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
        /// </summary>
        /// <returns>A value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.</returns>
        protected abstract bool GetIsRandomAlgorithm();

        /// <summary>Gets the name of the 1-dimensional root finder.
        /// </summary>
        /// <returns>The name of the 1-dimensional root finder.</returns>
        protected abstract IdentifierString GetName();

        /// <summary>Gets the long name of the 1-dimensional root finder.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the 1-dimensional root finder.</returns>
        protected abstract IdentifierString GetLongName();
        #endregion
    }
}