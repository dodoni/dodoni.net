﻿/* MIT License
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

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces.MissingValueReplenishments
{
    public partial class WeightedNearestGridPointMissingValueReplenishment
    {
        /// <summary>Serves as factory for missing value replenishment with respect to a interpolation along x-axis.
        /// </summary>
        public class AlongXAxis
        {
            #region public (readonly) members

            /// <summary>Apply a linear interpolation between the nearest grid points of a missing value to replenish.
            /// </summary>
            public readonly LabelMatrix.MissingValueReplenishment Linear;

            /// <summary>Apply a log-linear interpolation between the nearest grid points of a missing value to replenish.
            /// </summary>
            public readonly LabelMatrix.MissingValueReplenishment LogLinear;
            #endregion

            #region internal constuctors

            /// <summary>Initializes a new instance of the <see cref="AlongXAxis"/> class.
            /// </summary>
            internal AlongXAxis()
            {
                Linear = new NearestAlongXAxisReplenishment(GridPointCurve.Interpolator.Linear);
                LogLinear = new NearestAlongXAxisReplenishment(GridPointCurve.Interpolator.LogLinear);
            }
            #endregion
        }
    }
}