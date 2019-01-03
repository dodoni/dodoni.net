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
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Represents a numerical integrator that uses a adaptive Gauss-Kronrod integration rule.
    /// </summary>
    /// <remarks>
    /// See 
    /// <para>'Handbook of computational methods for integration', P. K. Kythe, M. R. Schäferkotter.</para>
    /// The weights and abscissas are taken from the Hal project, i.e. see http://tharkad.pa.msu/~asteiner/hal/html/gsl__inte__qag__b_8h-source.html.edu. 
    /// Weights and abscissas for the 7-point Gauss-Legendre and 15-point Kronrod rule you will also find on http://en.wikipedia.org/wiki/Gaussian_quadrature.
    /// </remarks>
    public partial class AdaptiveGaussKronrodIntegrator : OneDimNumericalIntegrator
    {
        #region public static (readonly) members

        /// <summary>Represents a standard exit condition.
        /// </summary>
        public static readonly StoppingCriteria StandardExitCondition = new StoppingCriteria(10, absoluteTolerance: MachineConsts.Epsilon, relativeTolerance: MachineConsts.Epsilon);
        #endregion

        #region private members

        /// <summary>The name of the numerical integrator.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="AdaptiveGaussKronrodIntegrator"/> class.
        /// </summary>
        /// <param name="integrationRule">The integration rule.</param>
        /// <param name="exitCondition">The exit condition.</param>
        public AdaptiveGaussKronrodIntegrator(Approach integrationRule, StoppingCriteria exitCondition)
            : base(BoundDescriptor.Closed, BoundDescriptor.Closed)
        {
            if (exitCondition == null)
            {
                throw new ArgumentNullException("exitCondition");
            }
            ExitCondition = exitCondition;
            IntegrationRule = integrationRule;
            m_Name = new IdentifierString(String.Format("Adaptive Gauss-Kronrod {0} Integrator", integrationRule.ToFormatString()));
        }

        /// <summary>Initializes a new instance of the <see cref="AdaptiveGaussKronrodIntegrator"/> class.
        /// </summary>
        /// <param name="integrationRule">The integration rule.</param>
        /// <remarks>The <see cref="StandardExitCondition"/> is taken into account.</remarks>
        public AdaptiveGaussKronrodIntegrator(Approach integrationRule)
            : this(integrationRule, StandardExitCondition)
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets the exit condition.
        /// </summary>
        /// <value>The exit condition.</value>
        public StoppingCriteria ExitCondition
        {
            get;
            private set;
        }

        /// <summary>Gets or sets the integration rule.
        /// </summary>
        /// <value>The integration rule of the adaptive Gauss-Kronrod method.</value>
        public Approach IntegrationRule
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.</returns>
        public override IOneDimNumericalIntegratorAlgorithm Create()
        {
            return new Algorithm(this);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public override void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Dimension", 1);
            infoOutputPackage.Add("Weight function is 1.0", true);

            LowerBoundDescriptor.FillInfoOutput(infoOutput, categoryName + ".LowerBoundDescriptor");
            UpperBoundDescriptor.FillInfoOutput(infoOutput, categoryName + ".UpperBoundDescriptor");
            ExitCondition.FillInfoOutput(infoOutput, categoryName + ".ExitCondition");
        }

        /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public override bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }
        #endregion

        #region protected methods

        /// <summary>Gets the long name of the numerical integrator.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the numerical integrator.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_Name;
        }

        /// <summary>Gets the name of the numerical integrator.
        /// </summary>
        /// <returns>The name of the numerical integrator.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }
        #endregion
    }
}