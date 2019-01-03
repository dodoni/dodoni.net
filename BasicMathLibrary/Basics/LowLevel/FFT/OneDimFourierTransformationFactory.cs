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
using System.Text;
using System.Numerics;
using System.Collections.Generic;

using Dodoni.MathLibrary.Basics.LowLevel.Interdisciplinary;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Serves as wrapper for the factory of 1-dimensional (Fast) Fourier transformations.
    /// </summary>
    /// <remarks>This wrapper class is needed because the most implementations do not support a general Fractional Fourier Transformation
    /// and will be computed 'on top' of a ordinary (Fast) Fourier Transformation.</remarks>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class OneDimFourierTransformationFactory
    {
        #region nested classes

        /// <summary>Serves as factory for real (Fast) Fourier transformations.
        /// </summary>
        public class RealFactory
        {
            /// <summary>The factory of one-dimensional (real) Fourier Transformations.
            /// </summary>
            private IOneDimFourierTransformationFactory m_Factory;

            /// <summary>Initializes a new instance of the <see cref="RealFactory"/> class.
            /// </summary>
            /// <param name="fourierTransformationFactory">The (Fast) Fourier Transformation factory.</param>
            internal RealFactory(IOneDimFourierTransformationFactory fourierTransformationFactory)
            {
                m_Factory = fourierTransformationFactory;
            }

            /// <summary>Creates a specific one-dimensional real (Fast) Fourier transformation.
            /// </summary>
            /// <param name="length">The length, i.e. the number of real (input) coefficients.</param>
            /// <returns>A specific one-dimensional real Fourier transformation.</returns>
            public FFT.IOneDimensionalRealData Create(int length)
            {
                return m_Factory.CreateRealTransformation(length);
            }
        }
        #endregion

        #region public / private members

        /// <summary>The factory of real-valued (Fast) Fourier Transformations.
        /// </summary>
        public readonly RealFactory Real;

        /// <summary>The (Fast) Fourier Transformation factory 
        /// </summary>
        private IOneDimFourierTransformationFactory m_Factory;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimFourierTransformationFactory"/> class.
        /// </summary>
        /// <param name="fourierTransformationFactory">The (Fast) Fourier Transformation factory.</param>
        internal OneDimFourierTransformationFactory(IOneDimFourierTransformationFactory fourierTransformationFactory)
        {
            Real = new RealFactory(fourierTransformationFactory);
            m_Factory = fourierTransformationFactory;
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific one-dimensional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <returns>A specific one-dimensional Fourier transformation.</returns>
        public FFT.IOneDimensional Create(int length)
        {
            return m_Factory.Create(length);
        }

        /// <summary>Creates a specific one-dimensional Fractional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional Fourier transformation.</returns>
        public FFT.IOneDimensional Create(int length, double alpha)
        {
            if (m_Factory is IOneDimFractionalFourierTransformationFactory)
            {
                var fractionalFourierTransformationFactory = m_Factory as IOneDimFractionalFourierTransformationFactory;
                return fractionalFourierTransformationFactory.Create(length, alpha);
            }
            return new OneDimFractionalFourierTransformationRealAlpha(length, alpha, m_Factory);
        }

        /// <summary>Creates a specific one-dimensional Fractional Fourier transformation.
        /// </summary>
        /// <param name="length">The length, i.e. the number of complex coefficients.</param>
        /// <param name="alpha">The parameter of the fractional Fast Fourier transformation.</param>
        /// <returns>A specific one-dimensional Fourier transformation.</returns>
        public FFT.IOneDimensional Create(int length, Complex alpha)
        {
            if (m_Factory is IOneDimFractionalFourierTransformationFactory)
            {
                var fractionalFourierTransformationFactory = m_Factory as IOneDimFractionalFourierTransformationFactory;
                if (fractionalFourierTransformationFactory.FourierExponentialFactorRestriction.HasFlag(FFT.FourierExponentialFactorRestriction.Arbitrary) == true)
                {
                    return fractionalFourierTransformationFactory.Create(length, alpha);
                }
            }
            return new OneDimFractionalFourierTransformation(length, alpha, m_Factory);
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("1-dimensional (Fast) Fourier Transformation factory ({0})", m_Factory.ToString());
        }
        #endregion
    }
}