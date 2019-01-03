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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.GridPointCurves
{
    /// <summary>Provides a interface for cubic spline curve interpolation.
    /// </summary>
    public partial class CurveInterpolationCubicSpline
    {
        /// <summary>Serves as factory for <see cref="ICubicSplineBoundaryCondition"/> objects.
        /// </summary>
        public abstract class BoundaryCondition : IIdentifierNameable, IAnnotatable
        {
            #region private members

            /// <summary>A short description of the boundary condition.
            /// </summary>
            private string m_Annotation;
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="BoundaryCondition"/> class.
            /// </summary>
            protected BoundaryCondition()
            {
                m_Annotation = String.Empty;
            }

            /// <summary>Initializes a new instance of the <see cref="BoundaryCondition"/> class.
            /// </summary>
            /// <param name="annotation">The annotation, i.e. short description, of the boundary condition.</param>
            protected BoundaryCondition(string annotation)
            {
                m_Annotation = (annotation != null)? annotation: String.Empty;
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the boundary condition.
            /// </summary>
            /// <value>The language independent name of the boundary condition.</value>
            public IdentifierString Name
            {
                get { return GetName(); }
            }

            /// <summary>Gets the long name of the boundary condition.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the boundary condition.</value>
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

            #endregion

            #region public methods

            #region IAnnotatable Members

            /// <summary>Sets the annotation of the current instance.
            /// </summary>
            /// <param name="annotation">The annotation.</param>
            /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.</returns>
            public bool TrySetAnnotation(string annotation)
            {
                m_Annotation = (annotation != null) ? annotation : String.Empty;
                return true;
            }
            #endregion

            /// <summary>Creates a <see cref="ICubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.
            /// </summary>
            /// <returns>A <see cref="ICubicSplineBoundaryCondition"/> object that represents the implementation of the boundary condition.</returns>
            public abstract ICubicSplineBoundaryCondition Create();

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
            public override string ToString()
            {
                return Name.String;
            }
            #endregion

            #region protected methods

            /// <summary>Gets the name of the boundary condition.
            /// </summary>
            /// <returns>The name of the boundary condition.</returns>
            protected abstract IdentifierString GetName();

            /// <summary>Gets the long name of the boundary condition.
            /// </summary>
            /// <returns>The (perhaps) language dependent long name of the boundary condition.</returns>
            protected abstract IdentifierString GetLongName();
            #endregion
        }
    }
}