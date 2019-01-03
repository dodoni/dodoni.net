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
using System.Collections.Generic;

using NUnit.Framework;

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Surfaces
{
    /// <summary>Provides unit tests for GridPointSurface2d objects.
    /// </summary>
    public partial class GridPointSurface2dTests
    {
        /// <summary>Gets a collection of Interpolator etc. objects used as input for the test case objects where the <c>GetValue</c> method will be applied to grid points only.
        /// </summary>
        /// <value>The configuration input for the test case objects.</value>
        public static IEnumerable<TestCaseData> GetValue_AtGridPoint_TestCaseConfiguration
        {
            get
            {
                foreach (GridPointSurface2d.ConstructionOrder constructionOrder in Enum.GetValues(typeof(GridPointSurface2d.ConstructionOrder)))
                {
                    yield return new TestCaseData(GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, constructionOrder);
                    yield return new TestCaseData(GridPointCurve.Interpolator.LogLinear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, constructionOrder);
                    yield return new TestCaseData(GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, GridPointCurve.Interpolator.LogLinear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, constructionOrder);
                    yield return new TestCaseData(GridPointCurve.Interpolator.LogLinear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, GridPointCurve.Interpolator.LogLinear, GridPointCurve.Extrapolator.None.First, GridPointCurve.Extrapolator.None.Last, constructionOrder);
                    yield return new TestCaseData(GridPointCurve.Interpolator.LogLinear, GridPointCurve.Extrapolator.Constant.First, GridPointCurve.Extrapolator.None.Last, GridPointCurve.Interpolator.Linear, GridPointCurve.Extrapolator.Linear.SlopeOfFirstTwoGridPoints, GridPointCurve.Extrapolator.Constant.Last, constructionOrder);
                }
            }
        }
    }
}