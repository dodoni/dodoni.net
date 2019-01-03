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
using System.Data;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Basics;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    /// <summary>Serves as abstract base class to find the "nearest" correlation matrix with respect to a specific symmetric, normalized matrix, perhaps applying some rank reduction technique.
    /// </summary>
    public abstract partial class PseudoSqrtMatrixDecomposer : IIdentifierNameable, IAnnotatable
    {
        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="PseudoSqrtMatrixDecomposer"/> class.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        protected PseudoSqrtMatrixDecomposer(InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
        {
            Annotation = String.Empty;
            InfoOutputDetailLevel = infoOutputDetailLevel;
        }

        /// <summary>Initializes a new instance of the <see cref="PseudoSqrtMatrixDecomposer"/> class.
        /// </summary>
        /// <param name="annotation">The annotation, i.e. short description, of the correlation matrix decomposer.</param>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        protected PseudoSqrtMatrixDecomposer(string annotation, InfoOutputDetailLevel infoOutputDetailLevel = InfoOutputDetailLevel.Middle)
        {
            Annotation = (annotation != null) ? annotation : String.Empty;
            InfoOutputDetailLevel = infoOutputDetailLevel;
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the correlation matrix decomposer.
        /// </summary>
        /// <value>The language independent name of the correlation matrix decomposer.</value>
        public IdentifierString Name
        {
            get { return GetName(); }
        }

        /// <summary>Gets the long name of the correlation matrix decomposer.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the correlation matrix decomposer.</value>
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
            get;
            private set;
        }
        #endregion

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get;
            private set;
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
            Annotation = annotation ?? String.Empty;
            return true;
        }
        #endregion

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public virtual bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            InfoOutputDetailLevel = infoOutputDetailLevel;
            return true;
        }

        /// <summary>Creates a specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object preferable for frequently operation of the same type. 
        /// </summary>
        /// <param name="dimension">The dimension of correlation matrices for which the workspace should be suited.</param>
        /// <returns>A specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object.</returns>
        public abstract WorkspaceContainer CreateWorkspaceContainer(int dimension);

        /// <summary>Creates a n x r matrix B such that B * B^t is a correlation matrix and "near" to the specified symmetric, normalized matrix of dimension n. A rank reduction will apply if r is strict less than n.
        /// </summary>
        /// <param name="rawCorrelationMatrix">The symmetric, normalized matrix where to find the 'nearest' correlation matrix.</param>
        /// <param name="state">The state of the operation in its <see cref="PseudoSqrtMatrixDecomposer.State"/> representation (output).</param>
        /// <param name="outputEntries">This argument will be used to store the matrix entries of the resulting matrix B, i.e. the return value array points to this array if != <c>null</c>; otherwise a memory allocation will be done.</param>
        /// <param name="worksspaceContainer">A specific <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object to reduce memory allocation; ignored if <c>null</c>.</param>
        /// <param name="triangularMatrixType">A value indicating which part of <paramref name="rawCorrelationMatrix"/> to take into account.</param>
        /// <returns>A <see cref="DenseMatrix"/> object that represents a matrix B such that B * B^t is the "nearest" correlation matrix with respect to <paramref name="rawCorrelationMatrix"/>.</returns>
        /// <remarks>In general the return object does <b>not</b> represents the pseudo-root of <paramref name="rawCorrelationMatrix"/>, i.e. output of the Cholesky decomposition.
        /// <para>The parameters <paramref name="outputEntries"/>, <paramref name="worksspaceContainer"/> allows to avoid memory allocation and to re-use arrays if the calculation of correlation matrices will be done often.</para></remarks>
        public abstract DenseMatrix Create(DenseMatrix rawCorrelationMatrix, out State state, double[] outputEntries = null, WorkspaceContainer worksspaceContainer = null, BLAS.TriangularMatrixType triangularMatrixType = BLAS.TriangularMatrixType.LowerTriangularMatrix);

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name.String;
        }
        #endregion

        #region protected methods

        /// <summary>Gets the name of the correlation matrix decomposer.
        /// </summary>
        /// <returns>The name of the correlation matrix decomposer.</returns>
        protected abstract IdentifierString GetName();

        /// <summary>Gets the long name of the correlation matrix decomposer.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the correlation matrix decomposer.</returns>
        protected abstract IdentifierString GetLongName();
        #endregion

        #region protected static methods

        /// <summary>Creates a <see cref="DataTable"/> object that contains eigenvalues of a specific matrix.
        /// </summary>
        /// <param name="dataTableName">The name of the <see cref="DataTable"/> object to create.</param>
        /// <param name="n">The number of elements to take into account in <paramref name="eigenvalues"/>.</param>
        /// <param name="eigenvalues">The eigenvalues.</param>
        /// <returns>The specified <see cref="DataTable"/> object that contains the eigenvalues.</returns>
        protected static DataTable CreateDataTableWithEigenvalues(string dataTableName, int n, double[] eigenvalues)
        {
            var dataTable = new DataTable(dataTableName);
            dataTable.Columns.Add("No", typeof(int));
            dataTable.Columns.Add("Value", typeof(double));
            dataTable.Columns.Add("Proportion", typeof(double));

            var total = BLAS.Level1.dasum(n, eigenvalues);

            for (int k = 0; k < n; k++)
            {
                var row = dataTable.NewRow();
                row[0] = k;
                row[1] = eigenvalues[k];                
                row[2] = Math.Abs(eigenvalues[k]) / total;

                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
        #endregion
    }
}