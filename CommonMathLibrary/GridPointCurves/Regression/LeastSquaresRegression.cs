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

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents a one-dimensional least squares regression.
    /// </summary>
    public partial class LeastSquaresRegression : GridPointCurve.Parametrization
    {
        #region private/public (readonly) members

        /// <summary>The order of the regression.
        /// </summary>
        public readonly int Order;

        /// <summary>The basis functions for the regression.
        /// </summary>
        public readonly ILeastSquaresRegressionBasisFunctions BasisFunctions;

        /// <summary>The absolute threshold for singular values, i.e. singular values less than the threshold are assumed to be <c>0.0</c>.
        /// </summary>
        public readonly double AbsoluteSingularValueThreshold;

        /// <summary>The relative threshold for singular values, i.e. singular values less than the product of the relative threshold and the greatest singular value are assumed to be <c>0.0</c>.
        /// </summary>
        public readonly double RelativeSingularValueThreshold;

        /// <summary>The name of the parametrization.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the parametrization.
        /// </summary>
        private IdentifierString m_LongName;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LeastSquaresRegression"/> class.
        /// </summary>
        /// <param name="order">The order of the regression.</param>
        /// <param name="basisFunctions">The basis functions to take into account for the regression.</param>
        /// <param name="absoluteSingularValueThreshold">The absolute threshold for singular values, i.e. singular values less than the threshold are assumed to be <c>0.0</c>.</param>
        /// <param name="relativeSingularValueThreshold">The relative threshold for singular values, i.e. singular values less than the product of the relative threshold and the greatest singular value are assumed to be <c>0.0</c>.</param>
        public LeastSquaresRegression(int order, ILeastSquaresRegressionBasisFunctions basisFunctions, double absoluteSingularValueThreshold = MachineConsts.Epsilon, double relativeSingularValueThreshold = MachineConsts.Epsilon)
            : base(CurveResource.AnnotationParametrizationLeastSquares, order + 1)
        {
            BasisFunctions = basisFunctions ?? throw new ArgumentNullException(nameof(basisFunctions));

            if (order < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(order));
            }
            Order = order;

            AbsoluteSingularValueThreshold = absoluteSingularValueThreshold;
            RelativeSingularValueThreshold = relativeSingularValueThreshold;

            m_Name = new IdentifierString(String.Format("LeastSquareRegression {0}", order));
            m_LongName = new IdentifierString(String.Format(CurveResource.LongNameParametrizationLeastSquares, order));
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="ICurveDataFitting"/> object that represents the implementation of the curve parametrization approach.
        /// </summary>
        /// <returns>A <see cref="ICurveDataFitting"/> object that represents the implementation of the curve parametrization approach.</returns>
        public override ICurveDataFitting Create()
        {
            return new Parametrization(this);
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the curve parametrization.
        /// </summary>
        /// <returns>The name of the curve parametrization.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the curve parametrization.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the curve parametrization.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_LongName;
        }
        #endregion
    }
}