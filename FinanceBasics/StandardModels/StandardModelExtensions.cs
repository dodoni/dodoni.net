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
using System.Collections.Generic;

namespace Dodoni.Finance.StandardModels
{
    /// <summary>Provides extension methods concerning standard option models, i.e. for Black, Bachelier etc.
    /// </summary>
    public static class StandardModelExtensions
    {
        #region nested classes

        private class Wrapper : IEuropeanOptionPricer
        {
            #region private members

            private IConstantVolatilityStandardEuropeanOption m_StandardOption;
            private double m_Volatility;
            #endregion


            internal Wrapper(IConstantVolatilityStandardEuropeanOption standardOption, double volatility)
            {
                m_StandardOption = standardOption;
                m_Volatility = volatility;
            }
            #region IEuropeanOptionPricer Members

            public double TimeToExpiry
            {
                get { return m_StandardOption.TimeToExpiry; }
                set { m_StandardOption.TimeToExpiry = value; }
            }

            public double Forward
            {
                get { return m_StandardOption.Forward; }
                set { m_StandardOption.Forward = value; }
            }

            public void ReInitialize()
            {

            }

            public double GetNoneDiscountedValue(double strike)
            {
                m_StandardOption.Strike = strike;
                return m_StandardOption.GetNoneDiscountedValue(m_Volatility);
            }

            #endregion

            #region IOperable Members

            public bool IsOperable
            {
                get { return true; }
            }
            #endregion
        }
        #endregion

        #region public (static) methods

        /// <summary>Returns a <see cref="IEuropeanOptionPricer"/> object that encapsulate the current instance.
        /// </summary>
        /// <param name="constantVolatilityStandardEuropeanOption">A specified European option based on a model with a constant volatility (i.e. Black, Bachelier etc.).</param>
        /// <param name="volatility">The volatility parameter.</param>
        /// <returns></returns>
        public static IEuropeanOptionPricer AsEuropeanOptionPricer(this IConstantVolatilityStandardEuropeanOption constantVolatilityStandardEuropeanOption, double volatility)
        {
            return new Wrapper(constantVolatilityStandardEuropeanOption, volatility);
        }
        #endregion
    }
}