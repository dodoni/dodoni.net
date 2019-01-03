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

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.ProbabilityTheory.Distributions;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Represents the Gauss-Copula in the case of dimension two.
    /// </summary>
    public class GaussCopula2d : ICopula2d
    {
        #region private members

        /// <summary>The correlation of the Gauss-Copula.
        /// </summary>
        private double m_Correlation;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GaussCopula2d"/> class.
        /// </summary>
        /// <remarks>The <see cref="Correlation"/> will be set to <see cref="System.Double.NaN"/>.</remarks>
        public GaussCopula2d()
        {
            m_Correlation = Double.NaN;
        }

        /// <summary>Initializes a new instance of the <see cref="GaussCopula2d"/> class.
        /// </summary>
        /// <param name="correlation">The correlation \rho \in ]0,1[.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="correlation"/> &lt;= 0 or <paramref name="correlation"/> >= 1.</exception>
        public GaussCopula2d(double correlation)
        {
            if ((correlation <= 0) || (correlation >= 1))
            {
                throw new ArgumentOutOfRangeException(nameof(correlation), String.Format(ExceptionMessages.ArgumentOutOfRange, "Correlation", "]0,1["));
            }
            m_Correlation = correlation;
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// 	<c>false</c> will be returned if the current instance represents some data, model, interpolation procedure,
        /// integration approach, optimization procedure etc. and no valid parameters are given.
        /// </remarks>
        public bool IsOperable
        {
            get { return !Double.IsNaN(m_Correlation); }
        }
        #endregion

        /// <summary>Gets the correlation.
        /// </summary>
        /// <value>The correlation.</value>
        public double Correlation
        {
            get { return m_Correlation; }
        }
        #endregion

        #region public methods

        #region ICopula2d Members

        /// <summary>Gets the value of the copula at some point.
        /// </summary>
        /// <param name="x">The argument x.</param>
        /// <param name="y">The argument y.</param>
        /// <returns>
        /// The value of the copula at (<paramref name="x"/>,<paramref name="y"/>).
        /// </returns>
        public double GetValue(double x, double y)
        {
            if ((x < MachineConsts.SuperTinyEpsilon) || (y < MachineConsts.SuperTinyEpsilon))
            {
                return 0.0;
            }
            if (1.0 - x < MachineConsts.SuperTinyEpsilon)
            {
                if (1.0 - y < MachineConsts.SuperTinyEpsilon)
                {
                    return x;
                }
                return y;
            }
            if (1.0 - y < MachineConsts.SuperTinyEpsilon)
            {
                return x;
            }
            return BivariateNormalDistribution.StandardCDFValue(StandardNormalDistribution.GetInverseCdfValue(x), StandardNormalDistribution.GetInverseCdfValue(y), m_Correlation);
        }
        #endregion

        /// <summary>Sets the <see cref="Correlation"/>.
        /// </summary>
        /// <param name="correlation">The correlation \rho \in ]0,1[.</param>
        /// <returns>A value indicating whether the correlation has been changed or the given correlation was not valid.</returns>
        public bool SetCorrelation(double correlation)
        {
            if ((correlation <= 0) || (correlation >= 1))
            {
                return false;
            }
            m_Correlation = correlation;
            return true;
        }
        #endregion
    }
}