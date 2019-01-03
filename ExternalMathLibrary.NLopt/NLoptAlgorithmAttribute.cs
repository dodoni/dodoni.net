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

using Dodoni.MathLibrary.Optimizer;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as <see cref="Attribute"/> class for the optimization algorithm of NLopt library 2.4.x, see http://ab-initio.mit.edu/wiki/index.php/NLopt.
    /// </summary>
    internal sealed class NLoptAlgorithmAttribute : Attribute
    {
        #region public (readonly) members

        /// <summary>The proceeding, i.e. a value indicating whether a local or global optimization approach is given.
        /// </summary>
        public readonly NLoptProceeding Proceeding;

        /// <summary>The gradient requirement, i.e. a value indicating whether the gradient will be taken into account.
        /// </summary>
        public readonly OrdinaryMultiDimOptimizer.GradientMethod GradientRequirement;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptAlgorithmAttribute"/> class.
        /// </summary>
        /// <param name="procedding">The proceeding, i.e. a value indicating whether a local or global optimization approach is given.</param>
        /// <param name="gradientRequirement">The gradient requirement, i.e. a value indicating whether the gradient will be taken into account.</param>
        public NLoptAlgorithmAttribute(NLoptProceeding procedding, OrdinaryMultiDimOptimizer.GradientMethod gradientRequirement)
        {
            Proceeding = procedding;
            GradientRequirement = gradientRequirement;

            IsRandomAlgorithm = false;
            BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required;
            NonlinearConstraintRequirement = NLoptNonlinearConstraintRequirement.None;
            SubsidiaryRequirement = NLoptSubsidiaryRequirement.None;
        }
        #endregion

        #region public properties

        /// <summary>Gets or sets a value indicating whether the optimization algorithm is randomly based.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this optimization algorithm is randomly based; otherwise, <c>false</c>.
        /// </value>
        public bool IsRandomAlgorithm
        {
            get;
            set;
        }

        /// <summary>Gets or sets the Box constraint requirement, i.e. a value indicating whether Box constraints are required.
        /// </summary>
        /// <value>The Box constraint requirement.</value>
        public NLoptBoundConstraintRequirement BoxConstraintRequirement
        {
            get;
            set;
        }

        /// <summary>Gets or sets the nonlinear constraint requirement, i.e. a value indicating whether nonlinear constraints are supported.
        /// </summary>
        /// <value>The nonlinear constraint requirement.</value>
        public NLoptNonlinearConstraintRequirement NonlinearConstraintRequirement
        {
            get;
            set;
        }

        /// <summary>Gets or sets a value indicating whether a subsidiary optimization algorithm is neede by 
        /// a specific NLopt optimization algorithm.
        /// </summary>
        /// <value>The subsidiary optimization requirement.</value>
        public NLoptSubsidiaryRequirement SubsidiaryRequirement
        {
            get;
            set;
        }
        #endregion

        #region public methods

        /// <summary>Gets the configuration of the NLopt algorithm.
        /// </summary>
        /// <returns>The <see cref="NLoptConfiguration"/> object which represents the configuration of the NLopt algorithm with respect to the attribute.</returns>
        public NLoptConfiguration GetConfiguration()
        {
            return new NLoptConfiguration(Proceeding, GradientRequirement, BoxConstraintRequirement, NonlinearConstraintRequirement, IsRandomAlgorithm, SubsidiaryRequirement);
        }
        #endregion
    }
}