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

namespace Dodoni.Finance
{
    /// <summary>Represents two <see cref="TenorTimeSpan"/> instances, one corresponds to some expiry
    /// and one reflects to the tenor (maturity) used for example for swap/swaption volatilities.
    /// </summary>
    public struct ExpiryTenorTimeSpan : IEquatable<ExpiryTenorTimeSpan>
    {
        /// <summary>The expiry.
        /// </summary>
        public readonly TenorTimeSpan ExpiryTimeSpan;

        /// <summary>The tenor (maturity).
        /// </summary>
        public readonly TenorTimeSpan TenorTimeSpan;

        /// <summary>Initializes a new instance of the <see cref="ExpiryTenorTimeSpan"/> struct.
        /// </summary>
        /// <param name="expiryTimeSpan">The expiry time span of some swaption.</param>
        /// <param name="tenorTimeSpan">The tenor (maturity) time span of some swap/swaption.</param>
        public ExpiryTenorTimeSpan(TenorTimeSpan expiryTimeSpan, TenorTimeSpan tenorTimeSpan)
        {
            ExpiryTimeSpan = expiryTimeSpan;
            TenorTimeSpan = tenorTimeSpan;
        }

        #region IEquatable<ExpiryTenorTimeSpan> Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(ExpiryTenorTimeSpan other)
        {
            return ExpiryTimeSpan.Equals(other.ExpiryTimeSpan) && TenorTimeSpan.Equals(other.TenorTimeSpan);
        }
        #endregion
    }
}