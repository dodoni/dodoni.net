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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.EquationSolvers
{
    /// <summary>
    /// Serves as factory for generic <see cref="OneDimEquationSolver.IFunctionFactory" /> objects, i.e. simple encapsulated objective functions.
    /// </summary>
    public class OneDimRootFinderFunctionFactory : OneDimEquationSolver.IFunctionFactory
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimRootFinderFunctionFactory"/> class.
        /// </summary>
        /// <param name="derivativeRequirement">A value indicating whether the derivative of the objective function is required in the root finder algorithm.</param>
        public OneDimRootFinderFunctionFactory(OneDimEquationSolver.ObjectiveFunctionDerivativeRequirement derivativeRequirement = OneDimEquationSolver.ObjectiveFunctionDerivativeRequirement.None)
        {
            DerivativeRequirement = derivativeRequirement;
        }
        #endregion

        #region public properties

        #region IFunctionFactory Members

        /// <summary>Gets a value indicating whether the derivative of the objective function is required in the root finder algorithm.
        /// </summary>
        /// <value>The derivative requirement.</value>
        public OneDimEquationSolver.ObjectiveFunctionDerivativeRequirement DerivativeRequirement
        {
            get;
            private set;
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region IFunctionFactory Members

        /// <summary>Creates a specific <see cref="OneDimEquationSolver.IFunction" /> object.
        /// </summary>
        /// <param name="objectiveFunction">The objective function.</param>
        /// <returns>A specific <see cref="OneDimEquationSolver.IFunction" /> object with respect to the specified root finder algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="OneDimEquationSolver.IFunctionFactory.DerivativeRequirement"/> indicates that the derivative is required.</exception>
        public OneDimEquationSolver.IFunction Create(Func<double, double> objectiveFunction)
        {
            if (DerivativeRequirement == OneDimEquationSolver.ObjectiveFunctionDerivativeRequirement.Required)
            {
                throw new InvalidOperationException();
            }
            if (objectiveFunction == null)
            {
                throw new ArgumentNullException(nameof(objectiveFunction));
            }
            return new OneDimRootFinderFunction(this, objectiveFunction);
        }

        /// <summary>Creates a specific <see cref="OneDimEquationSolver.IFunction" /> object.
        /// </summary>
        /// <param name="objectiveFunction">The differentiable objective function.</param>
        /// <returns>A specific <see cref="OneDimEquationSolver.IFunction" /> object with respect to the specified optimization algorithm.</returns>
        public OneDimEquationSolver.IFunction Create(OneDimEquationSolver.DifferentiableObjectiveFunction objectiveFunction)
        {
            if (objectiveFunction == null)
            {
                throw new ArgumentNullException(nameof(objectiveFunction));
            }
            return new OneDimRootFinderDifferentiableFunction(this, objectiveFunction);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Derivative requirement", DerivativeRequirement);
        }
        #endregion

        #endregion
    }
}