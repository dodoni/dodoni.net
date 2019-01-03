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
namespace Dodoni.Finance.StandardModels.Bachelier
{
    /// <summary>Serves as class for unit tests with respect to <see cref="ImpliedBachelierVolatilityApproach.RationalApproximation"/>.
    /// </summary>
    public class BachelierImpliedVolatility_Rational_Tests : BachelierImpliedVolatilityTests
    {
        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <returns>The object under test.</returns>
        protected override BachelierEuropeanCall.IImpliedVolatilityApproach GetCallOptionImpliedVolatilityApproach(out double tolerance)
        {
            tolerance = 1E-8;
            return ImpliedBachelierVolatilityApproach.RationalApproximation.Create();
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <returns>The object under test.</returns>
        protected override BachelierEuropeanPut.IImpliedVolatilityApproach GetPutOptionImpliedVolatilityApproach(out double tolerance)
        {
            tolerance = 1E-8;
            return ImpliedBachelierVolatilityApproach.RationalApproximation.Create();
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <returns>The object under test.</returns>
        protected override BachelierEuropeanStraddle.IImpliedVolatilityApproach GetStraddleOptionImpliedVolatilityApproach(out double tolerance)
        {
            tolerance = 1E-8;
            return ImpliedBachelierVolatilityApproach.RationalApproximation.Create();
        }
    }
}