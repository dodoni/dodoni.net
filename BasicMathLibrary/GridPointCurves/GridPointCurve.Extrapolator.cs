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

namespace Dodoni.MathLibrary.GridPointCurves
{
    public static partial class GridPointCurve
    {
        /// <summary>Serves as abstract basis class for curve extrapolation, i.e. as factory for <see cref="ICurveExtrapolator"/> objects. Moverover it provides some basic curve extrapolation approaches.
        /// </summary>
        public abstract class Extrapolator : IIdentifierNameable, IAnnotatable
        {
            #region nested enumerations

            /// <summary>Represents the building direction of a specified curve extrapolator.
            /// </summary>
            public enum BuildingDirection
            {
                /// <summary>The extrapolation takes place from the first grid point (more precisely from <see cref="IRealValuedCurve.LowerBound"/> of the curve interpolator) to -infinity.
                /// </summary>
                FromFirstGridPoint,

                /// <summary>The extrapolation take place from the last grid point (more precisely from <see cref="IRealValuedCurve.UpperBound"/> of the curve interpolator)  to +infinity.
                /// </summary>
                FromLastGridPoint
            }
            #endregion

            #region static (readonly) members

            /// <summary>Provides constant curve extrapolation approaches.
            /// </summary>
            public static readonly CurveExtrapolationConstant Constant = new CurveExtrapolationConstant();

            /// <summary>Provides linear curve extrapolation approaches.
            /// </summary>
            public static readonly CurveExtrapolationLinear Linear = new CurveExtrapolationLinear();

            /// <summary>The 'none' curve extrapolation, i.e. an exception will be thrown if one tries to access to a value outside the range of the grid points.
            /// </summary>
            public static readonly CurveExtrapolationNone None = new CurveExtrapolationNone();
            #endregion

            #region private members

            /// <summary>A short description of the extrapolator.
            /// </summary>
            private string m_Annotation;

            /// <summary>A value indicating whether the extrapolation has to be applied to the left or right side, i.e. starting point is the first or last grid point.
            /// </summary>
            /// <remarks>The extrapolation 'none' supports both directions.</remarks>
            private BuildingDirection m_BuildingDirection;
            #endregion

            #region static constructor

            /// <summary>Initializes the <see cref="Extrapolator"/> class.
            /// </summary>
            static Extrapolator()
            {
            }
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="Extrapolator"/> class.
            /// </summary>
            /// <param name="buildingDirection">A value indicating whether the extrapolation has to be applied to the left or right side, i.e. starting point is the first or last grid point.</param>
            /// <remarks>The extrapolation 'none' supports both directions.</remarks>            
            protected Extrapolator(BuildingDirection buildingDirection)
            {
                m_BuildingDirection = buildingDirection;
                m_Annotation = String.Empty;
            }

            /// <summary>Initializes a new instance of the <see cref="Extrapolator"/> class.
            /// </summary>
            /// <param name="buildingDirection">A value indicating whether the extrapolation has to be applied to the left or right side, i.e. starting point is the first or last grid point.</param>
            /// <param name="annotation">The annotation, i.e. short description, of the curve extrapolator.</param>
            /// <remarks>The extrapolation 'none' supports both directions.</remarks>            
            protected Extrapolator(BuildingDirection buildingDirection, string annotation)
            {
                m_BuildingDirection = buildingDirection;
                m_Annotation = annotation ?? String.Empty;
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the curve extrapolator.
            /// </summary>
            /// <value>The language independent name of the curve extrapolator.</value>
            public IdentifierString Name
            {
                get { return GetName(); }
            }

            /// <summary>Gets the long name of the curve extrapolator.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the curve extrapolator.</value>
            public IdentifierString LongName
            {
                get { return GetLongName(); }
            }
            #endregion

            #region IAnnotatable Members

            /// <summary>Gets a value indicating whether the annotation is read-only.
            /// </summary>
            /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
            public bool HasReadOnlyAnnotation
            {
                get { return false; }
            }

            /// <summary>Gets the annotation of the current instance.
            /// </summary>
            /// <value>The annotation of the current instance.</value>
            public string Annotation
            {
                get { return m_Annotation; }
            }
            #endregion

            /// <summary>Gets the building direction, i.e. a value indicating whether the extrapolation has to be applied to the left or right side, i.e. starting point is the first or last grid point.
            /// </summary>
            /// <value>The building direction of the curve extrapolator.</value>
            /// <remarks>The extrapolation 'none' supports both directions.</remarks>
            public BuildingDirection ExtrapolationBuildingDirection
            {
                get { return m_BuildingDirection; }
            }
            #endregion

            #region public methods

            #region IAnnotatable Members

            /// <summary>Sets the annotation of the current instance.
            /// </summary>
            /// <param name="annotation">The annotation.</param>
            /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.</returns>
            public bool TrySetAnnotation(string annotation)
            {
                m_Annotation = annotation ?? String.Empty;
                return true;
            }
            #endregion

            /// <summary>Creates a <see cref="ICurveExtrapolator"/> object that represents the implementation of the extrapolation approach.
            /// </summary>
            /// <param name="curveInterpolator">The interpolation approach of the curve.</param>
            /// <returns>A <see cref="ICurveExtrapolator"/> object that represents the implementation of the extrapolation approach.</returns>
            public abstract ICurveExtrapolator Create(ICurveDataFitting curveInterpolator);

            /// <summary>Gets the level of grid point dependency, i.e. the number of grid points required for the curve extrapolation.
            /// </summary>
            /// <param name="gridPointCount">The number of grid points.</param>
            /// <returns>The level of grid point dependency, i.e. the number of grid points required for the curve extrapolation.</returns>
            /// <remarks>For example the constant curve extrapolation requires the first/last grid point, a linear curve extrapolation requires the derivative of the first/last grid point or the slope
            /// of the first/last two grid points etc.</remarks>
            public abstract int GetLevelOfGridPointDependency(int gridPointCount);

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return Name.String;
            }
            #endregion

            #region protected methods

            /// <summary>Gets the name of the curve extrapolator.
            /// </summary>
            /// <returns>The name of the curve extrapolator.</returns>
            protected abstract IdentifierString GetName();

            /// <summary>Gets the long name of the curve extrapolator.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the curve extrapolator.</returns>
            protected abstract IdentifierString GetLongName();
            #endregion
        }
    }
}