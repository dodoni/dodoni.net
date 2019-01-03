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
using System.Collections.ObjectModel;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;

namespace Dodoni.MathLibrary.Surfaces.MissingValueReplenishments
{
    /// <summary>Represents the implementation of the replenish rule 'None replenish'.
    /// </summary>
    internal class MatrixReplenishRuleNone : LabelMatrix.MissingValueReplenishment
    {
        #region nested classes

        /// <summary>Serves as implementation of the missing value replenishment.
        /// </summary>
        private class Replenishment : IMissingValueReplenishment
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Replenishment"/> class.
            /// </summary>
            internal Replenishment()
            {
            }
            #endregion

            #region IMissingValueReplenishment Members

            /// <summary>Fill missing values of a specified matrix with respect to the missing value replenishment represented by the current instance.
            /// </summary>
            /// <param name="rowCount">The number of rows.</param>
            /// <param name="columnCount">The number of columns.</param>
            /// <param name="dataMatrix">The data matrix to replenish, provided column-by-column, i.e. contains at least <paramref name="rowCount"/> * <paramref name="columnCount"/> elements (in-/output).</param>
            /// <param name="xAxisLabeling">The labels of the x-axis in its <see cref="System.Double"/> representation, i.e. at least <paramref name="columnCount"/> elements.</param>
            /// <param name="yAxisLabeling">The labels of the y-axis in its <see cref="System.Double"/> representation, i.e. at least <paramref name="rowCount"/> elements.</param>
            /// <returns>A collection of null-based indices of the row and column of grid points in <paramref name="dataMatrix"/> which have been changed; <c>null</c> otherwise.</returns>
            public IEnumerable<(int RowIndex, int ColumnIndex)> Replenish(int rowCount, int columnCount, IList<double> dataMatrix, IList<double> xAxisLabeling, IList<double> yAxisLabeling)
            {
                return null;  // nothing to do 
            }
            #endregion
        }
        #endregion

        #region private members

        /// <summary>The name of the missing value replenishment.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the missing value replenishment.
        /// </summary>
        private IdentifierString m_LongName;

        /// <summary>The object that represents the none-replenishment.
        /// </summary>
        private IMissingValueReplenishment m_NoneReplenishment;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="MatrixReplenishRuleNone"/> class.
        /// </summary>
        internal MatrixReplenishRuleNone()
            : base(MissingValueReplenishmentResource.AnnotationNone)
        {
            m_Name = new IdentifierString("None");
            m_LongName = new IdentifierString(MissingValueReplenishmentResource.LongNameNone);
            m_NoneReplenishment = new Replenishment();
        }
        #endregion

        #region public methods

        /// <summary>Creates a <see cref="IMissingValueReplenishment"/> object that represents the implementation of the missing value replenishment.
        /// </summary>
        /// <returns>A <see cref="IMissingValueReplenishment"/> object that represents the implementation of the missing value replenishment.</returns>
        public override IMissingValueReplenishment Create()
        {
            return m_NoneReplenishment; // is thread-safe
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the missing value replenishment.
        /// </summary>
        /// <returns>The name of the missing value replenishment.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the missing value replenishment.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the missing value replenishment.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_LongName;
        }
        #endregion
    }
}