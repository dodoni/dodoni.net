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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Represents the constant curve extrapolation, i.e. the first/last gridpoint value or an individual value.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public partial class CurveExtrapolationConstant
    {
        #region public (readonly) members

        /// <summary>A constant curve extrapolation taking into account the last grid point.
        /// </summary>
        public readonly GridPointCurve.Extrapolator Last;

        /// <summary>A constant curve extrapolation taking into account the first grid point.
        /// </summary>
        public readonly GridPointCurve.Extrapolator First;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="CurveExtrapolationConstant"/> class.
        /// </summary>
        internal CurveExtrapolationConstant()
        {
            First = new FirstGridPoint();
            Last = new LastGridPoint();
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific constant curve extrapolation taken into account a specified value.
        /// </summary>
        /// <param name="buildingDirection">The building direction.</param>
        /// <param name="value">The value to take into account for the constant curve extrapolation.</param>
        /// <returns>A <see cref="GridPointCurve.Extrapolator"/> object that represents a constant curve extrapolation with respect to the desired direction and value.</returns>
        public GridPointCurve.Extrapolator Create(GridPointCurve.Extrapolator.BuildingDirection buildingDirection, double value)
        {
            return new Individual(buildingDirection, value);
        }
        #endregion
    }
}