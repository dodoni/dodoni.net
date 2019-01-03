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
    /// <summary>Serves as factory for <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> objects that allowes a numerical integration of a real-valued function in one variable 
    /// where the abscissas (x_j) will not change, i.e. with some specific caching technique.
    /// <para>For one application see for example 'Accelerating the Calibration of Stochastic Volatility Models', Fiodar Kilin, Frankfurt School of Finance and Management, No. 6, May 2007.</para>
    /// </summary>
    public abstract class OneDimNumericalConstAbscissaIntegrator : IIdentifierNameable, IInfoOutputQueriable
    {
        #region delegate declarations/nested classes

        /// <summary>Represents a function returning a double-precision floating-point number that has one double-precision floating-point 
        /// number argument which is a element of a specific array and a second argument indicates the null-based index of the first argument.
        /// </summary>
        /// <param name="x_k">The value x_k where to evaluate the function, i.e. the k-th element of the <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm.Abscissas"/>.</param>
        /// <param name="k">The null-based index <c>k</c> of the point <paramref name="x_k"/>, i.e. of the <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm.Abscissas"/>.</param>
        /// <returns>The value of the function at <paramref name="x_k"/>.</returns>
        public delegate double FunctionToIntegrate(double x_k, int k);

        /// <summary>Represents an adapter class for <see cref="OneDimNumericalConstAbscissaIntegrator"/> objects.
        /// </summary>
        private class OneDimNumericalIntegratorWrapper : OneDimNumericalIntegrator
        {
            #region private members

            /// <summary>The cacheable integrator instance to wrap.
            /// </summary>
            private OneDimNumericalConstAbscissaIntegrator m_OneDimNumericalConstAbscissaIntegrator;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="OneDimNumericalIntegratorWrapper"/> class.
            /// </summary>
            /// <param name="oneDimNumericalConstAbscissaIntegrator">The one-dimensional numerical integrator.</param>
            internal OneDimNumericalIntegratorWrapper(OneDimNumericalConstAbscissaIntegrator oneDimNumericalConstAbscissaIntegrator)
                : base(oneDimNumericalConstAbscissaIntegrator.LowerBoundDescriptor, oneDimNumericalConstAbscissaIntegrator.UpperBoundDescriptor)
            {
                m_OneDimNumericalConstAbscissaIntegrator = oneDimNumericalConstAbscissaIntegrator;
            }
            #endregion

            #region public methods

            /// <summary>Creates a new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.
            /// </summary>
            /// <returns>A new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.</returns>
            public override IOneDimNumericalIntegratorAlgorithm Create()
            {
                return m_OneDimNumericalConstAbscissaIntegrator.Create();
            }

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public override void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                m_OneDimNumericalConstAbscissaIntegrator.FillInfoOutput(infoOutput, categoryName);
            }

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
            /// </returns>
            public override bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return m_OneDimNumericalConstAbscissaIntegrator.TrySetInfoOutputDetailLevel(infoOutputDetailLevel);
            }
            #endregion

            #region protected methods

            /// <summary>Gets the name of the numerical integrator.
            /// </summary>
            /// <returns>The name of the numerical integrator.</returns>
            protected override IdentifierString GetName()
            {
                return m_OneDimNumericalConstAbscissaIntegrator.GetName();
            }

            /// <summary>Gets the long name of the numerical integrator.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the numerical integrator.</returns>
            protected override IdentifierString GetLongName()
            {
                return m_OneDimNumericalConstAbscissaIntegrator.GetLongName();
            }
            #endregion
        }
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimNumericalConstAbscissaIntegrator"/> class.
        /// </summary>
        /// <param name="lowerBoundDescriptor">A description of the lower integration bound.</param>
        /// <param name="upperBoundDescriptor">A description of the upper integration bound.</param>
        protected OneDimNumericalConstAbscissaIntegrator(OneDimNumericalIntegrator.BoundDescriptor lowerBoundDescriptor, OneDimNumericalIntegrator.BoundDescriptor upperBoundDescriptor)
        {
            if (lowerBoundDescriptor == null)
            {
                throw new ArgumentNullException(nameof(lowerBoundDescriptor));
            }
            LowerBoundDescriptor = lowerBoundDescriptor;

            if (upperBoundDescriptor == null)
            {
                throw new ArgumentNullException(nameof(upperBoundDescriptor));
            }
            UpperBoundDescriptor = upperBoundDescriptor;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the numerical integrator.
        /// </summary>
        /// <value>The language independent name of the numerical integrator.</value>
        public IdentifierString Name
        {
            get { return GetName(); }
        }

        /// <summary>Gets the long name of the numerical integrator.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the numerical integrator.</value>
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

        /// <summary>Gets the descriptor of the lower bound of the numerical integrator.
        /// </summary>
        /// <value>The descriptor of the lower bound.</value>
        public OneDimNumericalIntegrator.BoundDescriptor LowerBoundDescriptor
        {
            get;
            private set;
        }

        /// <summary>Gets the descriptor of the upper bound of the numerical integrator.
        /// </summary>
        /// <value>The descriptor of the upper bound.</value>
        public OneDimNumericalIntegrator.BoundDescriptor UpperBoundDescriptor
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        #region IInfoOutputQueriable Member

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public abstract void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General");

        /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public virtual bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            InfoOutputDetailLevel = infoOutputDetailLevel;
            return true;
        }
        #endregion

        /// <summary>Creates a new <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimNumericalConstAbscissaIntegratorAlgorithm"/> object.</returns>
        public abstract IOneDimNumericalConstAbscissaIntegratorAlgorithm Create();

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name.String;
        }
        #endregion

        #region public static methods

        /// <summary>Performs an implicit conversion from <see cref="OneDimNumericalConstAbscissaIntegrator"/> to <see cref="OneDimNumericalIntegrator"/>.
        /// </summary>
        /// <param name="cacheableOneDimNumericalIntegrator">The cacheable one-dimensional numerical integrator.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OneDimNumericalIntegrator(OneDimNumericalConstAbscissaIntegrator cacheableOneDimNumericalIntegrator)
        {
            if (cacheableOneDimNumericalIntegrator == null)
            {
                return null;
            }
            return new OneDimNumericalIntegratorWrapper(cacheableOneDimNumericalIntegrator);
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the numerical integrator.
        /// </summary>
        /// <returns>The name of the numerical integrator.</returns>
        protected abstract IdentifierString GetName();

        /// <summary>Gets the long name of the numerical integrator.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the numerical integrator.</returns>
        protected abstract IdentifierString GetLongName();
        #endregion
    }
}