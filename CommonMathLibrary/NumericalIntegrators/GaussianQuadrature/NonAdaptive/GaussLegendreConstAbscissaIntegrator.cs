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
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Represents a numerical integrator that implements a non-adaptive Gauss-Legendre Quadrature integration rule
    /// which supports some caching functionality, i.e. the numerical integration of 
    /// <para>
    /// \int_a^b  f(x) dx.
    /// </para>
    /// </summary>
    /// <remarks>Based on 
    /// <para>W. H. Press, 'Numerical recipes', §4.5.</para>
    /// </remarks>
    public partial class GaussLegendreConstAbscissaIntegrator : OneDimNumericalConstAbscissaIntegrator
    {
        #region private members

        /// <summary>The name of the numerical integrator.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GaussLegendreConstAbscissaIntegrator"/> class.
        /// </summary>
        /// <param name="order">The order of the Gauss-Legendre integration.</param>
        public GaussLegendreConstAbscissaIntegrator(int order = 50)
            : base(OneDimNumericalIntegrator.BoundDescriptor.Closed, OneDimNumericalIntegrator.BoundDescriptor.Closed)
        {
            Order = order;
            m_Name = new IdentifierString("Gauss-Legendere const abscissa Integrator");
        }
        #endregion

        #region public properties

        /// <summary>Gets the order of the Gauss-Legendre integration.
        /// </summary>
        /// <value>The order of the Gauss-Legendere integration.</value>
        public int Order
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.</returns>
        public override IOneDimNumericalConstAbscissaIntegratorAlgorithm Create()
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
            infoOutputPackage.Add("Weight function is 1.0", false);
            infoOutputPackage.Add("Order", Order);

            LowerBoundDescriptor.FillInfoOutput(infoOutput, categoryName + ".LowerBoundDescriptor");
            UpperBoundDescriptor.FillInfoOutput(infoOutput, categoryName + ".UpperBoundDescriptor");
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