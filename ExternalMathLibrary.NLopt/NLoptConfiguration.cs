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
using Dodoni.MathLibrary.Optimizer;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as configuration of a specific NLopt optimization algorithm of NLopt library 2.4.x, see http://ab-initio.mit.edu/wiki/index.php/NLopt.
    /// </summary>
    public class NLoptConfiguration : IInfoOutputQueriable
    {
        #region public (readonly) members

        /// <summary>A value indicating whether Box constraints are required.
        /// </summary>
        public readonly NLoptBoundConstraintRequirement BoxConstraintRequirement;

        /// <summary>A value indicating whether nonlinear constraints are required.
        /// </summary>
        public readonly NLoptNonlinearConstraintRequirement NonlinearConstraintRequirement;

        /// <summary>A value indicating whether the algorithm is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
        /// </summary>
        public readonly bool IsRandomAlgorithm;

        /// <summary>A value indicating whether a subsidiary optimization algorithm is required by a specific NLopt optimization algorithm.
        /// </summary>
        public readonly NLoptSubsidiaryRequirement SubsidiaryRequirement;

        /// <summary>A value indicating whether a local or global optimization approach is specified.
        /// </summary>
        public readonly NLoptProceeding Proceeding;

        /// <summary>A value indicating whether the gradient will be taken into account.
        /// </summary>
        public readonly OrdinaryMultiDimOptimizer.GradientMethod GradientRequirement;
        #endregion

        #region nternal constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptConfiguration"/> class.
        /// </summary>
        /// <param name="procedding">A value indicating whether a local or global optimization approach is specified.</param>
        /// <param name="gradientRequirement">A value indicating whether the gradient will be taken into account.</param>
        /// <param name="boxConstraintRequirement">A value indicating whether Box constraints are required.</param>
        /// <param name="nonlinearConstraintRequirement">A value indicating whether nonlinear constraints are required.</param>
        /// <param name="isRandomAlgorithm">A value indicating whether the optimization algorithm is randomly based.</param>
        /// <param name="subsididaryRequirement">A value indicating whether a subsidiary optimization algorithm is required.</param>
        internal NLoptConfiguration(NLoptProceeding procedding, OrdinaryMultiDimOptimizer.GradientMethod gradientRequirement, NLoptBoundConstraintRequirement boxConstraintRequirement,
            NLoptNonlinearConstraintRequirement nonlinearConstraintRequirement, bool isRandomAlgorithm, NLoptSubsidiaryRequirement subsididaryRequirement)
        {
            Proceeding = procedding;
            GradientRequirement = gradientRequirement;

            IsRandomAlgorithm = isRandomAlgorithm;
            BoxConstraintRequirement = boxConstraintRequirement;
            NonlinearConstraintRequirement = nonlinearConstraintRequirement;
            SubsidiaryRequirement = subsididaryRequirement;
        }
        #endregion

        #region public properties

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

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return infoOutputDetailLevel == InfoOutputDetailLevel.Full;
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Gradient requirement", GradientRequirement);
            infoOutputPackage.Add("Proceeding", Proceeding);
            infoOutputPackage.Add("Is random algorithm", IsRandomAlgorithm);
            infoOutputPackage.Add("Box constraint requirement", BoxConstraintRequirement);
            infoOutputPackage.Add("Non-Linear constraint requirement", NonlinearConstraintRequirement);
            infoOutputPackage.Add("Subsidiary requirement", SubsidiaryRequirement);
        }
        #endregion

        #endregion

        #region public static methods

        /// <summary>Creates the specified <see cref="NLoptConfiguration"/>.
        /// </summary>
        /// <param name="nloptAlgorithm">The NLopt algorithm in its <see cref="NLoptAlgorithm"/> representation.</param>
        /// <returns>The configuration of the specified NLopt algorithm in its <see cref="NLoptConfiguration"/> representation.</returns>
        public static NLoptConfiguration Create(NLoptAlgorithm nloptAlgorithm)
        {
            var attribute = EnumAttribute.Create<NLoptAlgorithmAttribute>(nloptAlgorithm);
            return attribute.GetConfiguration();
        }
        #endregion
    }
}