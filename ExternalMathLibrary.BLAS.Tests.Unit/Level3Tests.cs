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

namespace Dodoni.MathLibrary.Basics.LowLevel.Native.BLAS
{
    /// <summary>Serves as unit test class for Level 3 BLAS methods with respect to <see cref="BlasNativeWrapper.Level3BLAS"/>.
    /// </summary>
    [TestFixture]
    public class Level3Tests : Level3BLASTests
    {
        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="Level3Tests"/> class.
        /// </summary>
        public Level3Tests()
        {
        }
        #endregion

        /// <summary>Gets the level 3 BLAS implementation.
        /// </summary>
        /// <returns>A <see cref="ILevel3BLAS"/> object that encapuslate the level 3 BLAS functions.
        /// </returns>
        protected override ILevel3BLAS GetLevel3BLAS()
        {
            return new BlasNativeWrapper().Level3;
        }
    }
}