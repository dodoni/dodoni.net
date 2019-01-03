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
namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Serves as factory for the linear curve extrapolation approach.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public partial class CurveExtrapolationLinear
    {
        #region public (readonly) members

        /// <summary>The slope is specified by the derivative of the last grid point.
        /// </summary>
        public readonly GridPointCurve.Extrapolator LastDerivative;

        /// <summary>The slope is specified by the derivative of the first grid point.
        /// </summary>
        public readonly GridPointCurve.Extrapolator FirstDerivative;

        /// <summary>The slope is specified by the slope of the first two grid points.
        /// </summary>
        public readonly GridPointCurve.Extrapolator SlopeOfFirstTwoGridPoints;

        /// <summary>The slope is specified by the slope of the last two grid points.
        /// </summary>
        public readonly GridPointCurve.Extrapolator SlopeOfLastTwoGridPoints;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="CurveExtrapolationLinear"/> class.
        /// </summary>
        internal CurveExtrapolationLinear()
        {
            FirstDerivative = new Differentiable(GridPointCurve.Extrapolator.BuildingDirection.FromFirstGridPoint);
            LastDerivative = new Differentiable(GridPointCurve.Extrapolator.BuildingDirection.FromLastGridPoint);

            SlopeOfFirstTwoGridPoints = new GridpointSlope(GridPointCurve.Extrapolator.BuildingDirection.FromFirstGridPoint);
            SlopeOfLastTwoGridPoints = new GridpointSlope(GridPointCurve.Extrapolator.BuildingDirection.FromLastGridPoint);
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific linear curve extrapolation taken into account a specified slope.
        /// </summary>
        /// <param name="buildingDirection">The building direction.</param>
        /// <param name="slope">The slope of the linear curve extrapolation.</param>
        /// <returns>A <see cref="GridPointCurve.Extrapolator"/> object that represents a linear curve extrapolation with respect to the desired direction and slope.</returns>
        public GridPointCurve.Extrapolator Create(GridPointCurve.Extrapolator.BuildingDirection buildingDirection, double slope)
        {
            return new Individual(buildingDirection, slope);
        }
        #endregion
    }
}