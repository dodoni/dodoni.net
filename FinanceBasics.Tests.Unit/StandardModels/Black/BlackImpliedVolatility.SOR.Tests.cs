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

namespace Dodoni.Finance.StandardModels.Black
{
    /// <summary>Serves as class for unit tests with respect to <see cref="ImpliedBlackVolatilityApproach.SOR"/>.
    /// </summary>
    public class BlackImpliedVolatility_SOR_Tests : BlackImpliedVolatilityTests
    {
        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account (output).</param>
        /// <param name="testLevel">A delegate that returns for a specific test scenario a specific <see cref="BlackImpliedVolatilityTests.ToleranceLevel" /> object. The second argument is the option value. Perhaps <c>null</c>.</param>
        /// <returns>The object under test.</returns>
        protected override BlackEuropeanCall.IImpliedVolatilityApproach GetCallOptionImpliedVolatilityApproach(out double tolerance, out Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel)
        {
            tolerance = 1E-4;
            testLevel = null;
            return ImpliedBlackVolatilityApproach.SOR.Create();
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <param name="testLevel">A delegate that returns for a specific test scenario a specific <see cref="BlackImpliedVolatilityTests.ToleranceLevel" /> object. The second argument is the option value. Perhaps <c>null</c>.</param>
        /// <returns>The object under test.</returns>
        protected override BlackEuropeanPut.IImpliedVolatilityApproach GetPutOptionImpliedVolatilityApproach(out double tolerance, out Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel)
        {
            tolerance = 1E-4;
            testLevel = null;
            return ImpliedBlackVolatilityApproach.SOR.Create();
        }

        /// <summary>Gets the object under test.
        /// </summary>
        /// <param name="tolerance">The tolerance to take into account.</param>
        /// <param name="testLevel">A delegate that returns for a specific test scenario a specific <see cref="BlackImpliedVolatilityTests.ToleranceLevel" /> object. The second argument is the option value. Perhaps <c>null</c>.</param>
        /// <returns>The object under test.</returns>
        protected override BlackEuropeanStraddle.IImpliedVolatilityApproach GetStraddleOptionImpliedVolatilityApproach(out double tolerance, out Func<ConstantVolatilityStandardEuropeanOptionConfiguration, double, ToleranceLevel> testLevel)
        {
            tolerance = 1E-4;
            testLevel = null;
            return ImpliedBlackVolatilityApproach.SOR.Create();
        }
    }
}