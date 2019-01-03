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

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Represents a numerical integrator that implements a non-adaptive Gauss-Laguerre Quadrature integration rule
    /// which supports some caching functionality, i.e. the numerical integration of 
    /// <para>
    /// \int_0^\infty f(x) * x^\alpha * exp(-x) dx.
    /// </para>
    /// </summary>
    /// <remarks>The implementation is based on 
    /// <para>'Numerical recipes', §4.5, W. H. Press.</para>
    /// </remarks>
    public partial class GaussLaguerreConstAbscissaIntegrator : OneDimNumericalConstAbscissaIntegrator
    {
        #region private members

        /// <summary>The name of the numerical integrator.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>A value indicating whether the parameter \alpha is 0.0. 
        /// </summary>
        private bool m_AlphaIsZero;

        /// <summary>The weights of the Gauss-Laguerre Quadrature.
        /// </summary>
        private double[] m_Weights;

        /// <summary>The abscissas, i.e. evaluation points of the Gauss-Laguerre Quadrature.
        /// </summary>
        private double[] m_EvaluationPoints;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GaussLaguerreConstAbscissaIntegrator"/> class.
        /// </summary>
        /// <param name="alpha">The parameter \alpha of the Guass-Laguerre integrator.</param>
        /// <param name="order">The order of the Gauss-Laguerre integration.</param>
        public GaussLaguerreConstAbscissaIntegrator(double alpha, int order = 100)
            : base(OneDimNumericalIntegrator.BoundDescriptor.Create(OneDimNumericalIntegrator.BoundEvaluationType.Closed, 0.0), OneDimNumericalIntegrator.BoundDescriptor.Create(OneDimNumericalIntegrator.BoundEvaluationType.Unbounded, Double.PositiveInfinity))
        {
            Order = order;
            Alpha = alpha;
            m_AlphaIsZero = false;
            m_EvaluationPoints = GaussianQuadrature.Laguerre.GetValue(alpha, order, out m_Weights);

            m_Name = new IdentifierString(String.Format("Gauss-Laguerre const abscissa Integrator; alpha: {0}; order: {1}", alpha, order));
        }

        /// <summary>Initializes a new instance of the <see cref="GaussLaguerreConstAbscissaIntegrator"/> class with \alpha = 0.0.
        /// </summary>
        /// <param name="order">The order of the Gauss-Laguerre integration.</param>
        public GaussLaguerreConstAbscissaIntegrator(int order = 100)
            : base(OneDimNumericalIntegrator.BoundDescriptor.Create(OneDimNumericalIntegrator.BoundEvaluationType.Closed, 0.0), OneDimNumericalIntegrator.BoundDescriptor.Create(OneDimNumericalIntegrator.BoundEvaluationType.Unbounded, Double.PositiveInfinity))
        {
            Order = order;
            Alpha = 0.0;
            m_AlphaIsZero = true;
            m_EvaluationPoints = GaussianQuadrature.Laguerre.GetValue(order, out m_Weights);

            m_Name = new IdentifierString(String.Format("Gauss-Laguerre const abscissa Integrator; alpha: {0}; order: {1}", 0.0, order));
        }
        #endregion

        #region public properties

        /// <summary>Gets the order of the Gauss-Laguerre integration.
        /// </summary>
        /// <value>The order of the Gauss-Laguerre integration.</value>
        public int Order
        {
            get;
            private set;
        }

        /// <summary>Gets the parameter \alpha of the Gauss-Laguerre Quadrature.
        /// </summary>
        /// <value>The parameter \alpha of the Gauss-Laguerre Quadrature.</value>
        public double Alpha
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> object.</returns>
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
            infoOutputPackage.Add("Alpha", Alpha);
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