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

namespace Dodoni.MathLibrary.Basics.LowLevel.Native.FFTW
{
    /// <summary>Serves as unit test class for <see cref="FftwOneDimFourierTransformation"/>.
    /// </summary>
    [TestFixture]
    public class FftwOneDimFourierTransformationTests : OneDimFourierTransformationTests
    {
        /// <summary>Initializes a new instance of the <see cref="FftwOneDimFourierTransformationTests"/> class.
        /// </summary>
        public FftwOneDimFourierTransformationTests()
        {
        }

        /// <summary>Creates the test object, i.e. the <see cref="FFT.IOneDimensional"/> object under test.
        /// </summary>
        /// <param name="length">The number of Fourier coefficients.</param>
        /// <returns>The <see cref="FFT.IOneDimensional"/> object to test.</returns>
        protected override FFT.IOneDimensional CreateTestObject(int length)
        {
            return new FftwOneDimFourierTransformation(length);
        }
    }
}