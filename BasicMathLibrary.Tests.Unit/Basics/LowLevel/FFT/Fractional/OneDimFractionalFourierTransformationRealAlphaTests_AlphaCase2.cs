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
using NUnit.Framework;

using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel.Interdisciplinary
{
    /// <summary>Serves as unit test class for one-dimensional Fourier Transformations, i.e. a specified <see cref="OneDimFractionalFourierTransformationRealAlpha"/> object
    /// with respect to a specified parameter \alpha and a Benchmark object of type <see cref="BuildInNaiveOneDimFourierTransformationFactory"/>.
    /// </summary>
    [TestFixture]
    public class OneDimFractionalFourierTransformationRealAlphaTests_AlphaCase2 : OneDimFourierTransformationTests
    {
        #region private members

        /// <summary>The specified parameter \alpha.
        /// </summary>
        private double m_Alpha = -2.651;

        /// <summary>The 1-dimensional (ordinary) Fourier factory.
        /// </summary>
        private IOneDimFourierTransformationFactory m_Factory = new BuildInNaiveOneDimFourierTransformationFactory();
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimFractionalFourierTransformationRealAlphaTests_AlphaCase2"/> class.
        /// </summary>
        public OneDimFractionalFourierTransformationRealAlphaTests_AlphaCase2()
        {
        }
        #endregion

        #region protected/internal methods

        /// <summary>Creates the test object, i.e. the <see cref="FFT.IOneDimensional"/> object under test.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <returns>The <see cref="FFT.IOneDimensional"/> object to test.
        /// </returns>
        protected override FFT.IOneDimensional CreateTestObject(int length)
        {
            return new OneDimFractionalFourierTransformationRealAlpha(length, m_Alpha, m_Factory);
        }

        /// <summary>Creates the benchmark object.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <returns>The benchmark object, i.e. a simple managed code build-In implementation.
        /// </returns>
        internal override BuildInNaiveOneDimFourierTransformation CreateBenchmarkObject(int length)
        {
            return new BuildInNaiveOneDimFourierTransformation(length, m_Alpha);
        }
        #endregion
    }
}