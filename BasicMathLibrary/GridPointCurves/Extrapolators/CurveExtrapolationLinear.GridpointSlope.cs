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
    public partial class CurveExtrapolationLinear
    {
        /// <summary>Serves as linear curve extrapolation where the slope is specified by the slope of the first/last two grid points.
        /// </summary>
        private class GridpointSlope : GridPointCurve.Extrapolator
        {
            #region nested classes

            /// <summary>Serves as implementation of the extrapolation approach.
            /// </summary>
            private class Extrapolator : ICurveExtrapolator
            {
                #region private members

                /// <summary>The curve interpolator.
                /// </summary>
                private ICurveDataFitting m_CurveInterpolator;

                /// <summary>The slope of the linear extrapolation.
                /// </summary>
                private double m_Slope;

                /// <summary>The reference point, which is equal to the lower or upper bound of the curve.
                /// </summary>
                private double m_ReferencePoint;

                /// <summary>The reference value, i.e. the value at the <see cref="m_ReferencePoint"/>.
                /// </summary>
                private double m_ReferenceValue;

                /// <summary>A value indicating whether to extrapolate to +\infinity or -\infinity.
                /// </summary>
                private BuildingDirection m_BuildingDirection;
                #endregion

                #region public constructors

                /// <summary>Initializes a new instance of the <see cref="Extrapolator"/> class.
                /// </summary>
                /// <param name="curveInterpolator">The interpolation approach of the curve.</param>
                /// <param name="buildingDirection">The building direction of the curve extrapolation.</param>
                /// <param name="extrapolatorFactory">The <see cref="GridPointCurve.Extrapolator"/> object that serves as factory for the current object.</param>
                public Extrapolator(ICurveDataFitting curveInterpolator, BuildingDirection buildingDirection, GridPointCurve.Extrapolator extrapolatorFactory)
                {
                    m_CurveInterpolator = curveInterpolator ?? throw new ArgumentNullException(nameof(curveInterpolator));
                    m_BuildingDirection = buildingDirection;
                    m_Slope = Double.NaN;
                    m_ReferencePoint = Double.NaN;
                    m_ReferenceValue = Double.NaN;
                    Factory = extrapolatorFactory;
                }
                #endregion

                #region public properties

                #region ICurveExtrapolator Members

                /// <summary>Gets the factory of <see cref="ICurveExtrapolator" /> objects of the same type and configuration.
                /// </summary>
                /// <value>The factory of <see cref="ICurveExtrapolator" /> objects of the same type and configuration.</value>
                public GridPointCurve.Extrapolator Factory
                {
                    get;
                    private set;
                }
                #endregion

                #region IOperable Members

                /// <summary>Gets a value indicating whether this instance is operable.
                /// </summary>
                /// <value><c>true</c> if this instance is operable; otherwise, <c>false</c>.</value>
                public bool IsOperable
                {
                    get { return (Double.IsNaN(m_Slope) == false) && (Double.IsNaN(m_ReferencePoint) == false) && (Double.IsNaN(m_ReferenceValue) == false); }
                }
                #endregion

                #region IRealValuedCurve Members

                /// <summary>Gets the lower bound of the domain of definition.
                /// </summary>
                /// <value>The lower bound of the domain of definition, perhaps <see cref="System.Double.NegativeInfinity"/> or <see cref="System.Double.NaN"/>.</value>
                public double LowerBound
                {
                    get { return (m_BuildingDirection == BuildingDirection.FromFirstGridPoint) ? Double.NegativeInfinity : m_ReferencePoint; }
                }

                /// <summary>Gets the upper bound of the domain of definition.
                /// </summary>
                /// <value>The upper bound of the domain of definition, perhaps <see cref="System.Double.PositiveInfinity"/> or <see cref="System.Double.NaN"/>.</value>
                public double UpperBound
                {
                    get { return (m_BuildingDirection == BuildingDirection.FromLastGridPoint) ? Double.PositiveInfinity : m_ReferencePoint; }
                }
                #endregion

                #region IInfoOutputQueriable Members

                /// <summary>Gets the info-output level of detail.
                /// </summary>
                /// <value>The info-output level of detail.</value>
                public InfoOutputDetailLevel InfoOutputDetailLevel
                {
                    get { return InfoOutputDetailLevel.Full; }
                }
                #endregion

                #endregion

                #region public methods

                #region ICurveExtrapolator Members

                /// <summary>Updates the current curve extrapolator.
                /// </summary>
                /// <remarks>This method should be called if grid points have been changed, added, removed etc. and before evaluating the grid point curve at a specified point.
                /// <para>If no problem occurred, the flag <see cref="IOperable.IsOperable"/> will be set to <c>true</c>.</para>
                /// </remarks>
                public void Update()
                {
                    if (m_CurveInterpolator.GridPointCount >= 2)
                    {
                        int gridIndex = (m_BuildingDirection == BuildingDirection.FromFirstGridPoint) ? 0 : m_CurveInterpolator.GridPointCount - 2;

                        double x1 = m_CurveInterpolator.GridPointArguments[gridIndex];
                        double x2 = m_CurveInterpolator.GridPointArguments[gridIndex + 1];
                        double y1 = m_CurveInterpolator.GetValue(x1);
                        double y2 = m_CurveInterpolator.GetValue(x2);

                        if (x2 - x1 > MachineConsts.Epsilon)
                        {
                            m_Slope = (y2 - y1) / (x2 - x1);
                        }
                        else
                        {
                            throw new ArithmeticException("Linear extrapolation failed.");
                        }
                        m_ReferencePoint = (m_BuildingDirection == BuildingDirection.FromFirstGridPoint) ? m_CurveInterpolator.LowerBound : m_CurveInterpolator.UpperBound;
                        m_ReferenceValue = m_CurveInterpolator.GetValue(m_ReferencePoint);
                    }
                    else
                    {
                        m_Slope = Double.NaN;  // IsOperable flag will be set to 'false'
                    }
                }
                #endregion

                #region IRealValuedCurve Members

                /// <summary>Gets the value at a specific argument.
                /// </summary>
                /// <param name="pointToEvaluate">The point to evaluate.</param>
                /// <returns>The value of the curve at <paramref name="pointToEvaluate"/>.</returns>
                /// <remarks>The argument must be an element of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
                public double GetValue(double pointToEvaluate)
                {
                    return m_Slope * (pointToEvaluate - m_ReferencePoint) + m_ReferenceValue;
                }

                /// <summary>Gets the value of the integral \int_a^b f(x) dx.
                /// </summary>
                /// <param name="lowerBound">The lower bound.</param>
                /// <param name="upperBound">The upper bound.</param>
                /// <returns>The value of \int_a^b f(x) dx.</returns>
                /// <remarks>The arguments must be elements of the domain of definition, represented by <see cref="IRealValuedCurve.LowerBound"/> and <see cref="IRealValuedCurve.UpperBound"/>.</remarks>
                public double GetIntegral(double lowerBound, double upperBound)
                {
                    /* integration of 't \mapsto a * (t - t1) + x1' : */
                    return m_Slope * (0.5 * (upperBound * upperBound - lowerBound * lowerBound) - m_ReferencePoint * (upperBound - lowerBound)) + m_ReferenceValue * (upperBound - lowerBound);
                }
                #endregion

                #region IInfoOutputQueriable Members

                /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
                /// </summary>
                /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
                /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
                public void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
                {
                    var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                    infoOutputPackage.Add("Name", Factory.Name.String);
                    infoOutputPackage.Add("Long Name", Factory.LongName.String);
                    infoOutputPackage.Add("Building direction", m_BuildingDirection);

                    infoOutputPackage.Add("Reference Point", m_ReferencePoint);
                    infoOutputPackage.Add("Reference Value", m_ReferenceValue);
                }

                /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
                /// </summary>
                /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
                /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.</returns>
                public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
                {
                    return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
                }
                #endregion

                #endregion
            }
            #endregion

            #region private members

            /// <summary>The name of the curve extrapolation.
            /// </summary>
            private IdentifierString m_Name;

            /// <summary>The long name of the curve extrapolation.
            /// </summary>
            private IdentifierString m_LongName;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="GridpointSlope"/> class.
            /// </summary>
            /// <param name="buildingDirection">The building direction.</param>
            public GridpointSlope(BuildingDirection buildingDirection)
                : base(buildingDirection)
            {
                switch (buildingDirection)
                {
                    case BuildingDirection.FromFirstGridPoint:
                        m_Name = new IdentifierString("Linear:First two Gridpoint Slope");
                        m_LongName = new IdentifierString(CurveResource.LongNameExtrapolationLinearFirstTwoSlope);
                        TrySetAnnotation(CurveResource.AnnotationExtrapolationLinearFirstTwoSlope);
                        break;

                    case BuildingDirection.FromLastGridPoint:
                        m_Name = new IdentifierString("Linear:Last two Gridpoint Slope");
                        m_LongName = new IdentifierString(CurveResource.LongNameExtrapolationLinearLastTwoSlope);
                        TrySetAnnotation(CurveResource.AnnotationExtrapolationLinearLastTwoSlope);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            #endregion

            #region public methods

            /// <summary>Creates a <see cref="ICurveExtrapolator"/> object that represents the implementation of the extrapolation approach.
            /// </summary>
            /// <param name="curveInterpolator">The interpolation approach of the curve.</param>
            /// <returns>A <see cref="ICurveExtrapolator"/> object that represents the implementation of the extrapolation approach.</returns>
            public override ICurveExtrapolator Create(ICurveDataFitting curveInterpolator)
            {
                return new Extrapolator(curveInterpolator, ExtrapolationBuildingDirection, this);
            }

            /// <summary>Gets the level of grid point dependency, i.e. the number of grid points required for the curve extrapolation.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points.</param>
            /// <returns>The level of grid point dependency, i.e. the number of grid points required for the curve extrapolation.</returns>
            public override int GetLevelOfGridPointDependency(int gridPointCount)
            {
                return 2;  // the first/last two grid points are required for extrapolation only (i.e. for slope calculation)
            }
            #endregion

            #region protected methods

            /// <summary>Gets the name of the curve extrapolator.
            /// </summary>
            /// <returns>The name of the curve extrapolator.</returns>
            protected override IdentifierString GetName()
            {
                return m_Name;
            }

            /// <summary>Gets the long name of the curve extrapolator.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the curve extrapolator.</returns>
            protected override IdentifierString GetLongName()
            {
                return m_LongName;
            }
            #endregion
        }
    }
}