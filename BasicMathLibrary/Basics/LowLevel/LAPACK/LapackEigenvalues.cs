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
namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides routines of the Linear Algebra PACKage, see http://www.netlib.org/lapack/index.html, that are related to Least Square and Eigenvalue calculations.
    /// </summary>
    public partial class LapackEigenvalues
    {
        #region public (readonly) members

        /// <summary>Provides routines for generalized non-symmetric Eigenvalue problems.
        /// </summary>
        public readonly IGeneralizedNonsymmetricEigenvalueProblems GeneralizedNonSymmetric = null;

        /// <summary>Provides routines for generalized symmetric Eigenvalue problems.
        /// </summary>
        public readonly IGeneralizedSymmetricEigenvalueProblems GeneralizedSymmetric = null;

        /// <summary>Provides routines for non-symmetric Eigenvalue problems.
        /// </summary>
        public readonly INonSymmetricEigenvalueProblems NonSymmetric;

        /// <summary>Provides routines for symmetric Eigenvalue problems.
        /// </summary>
        public readonly ISymmetricEigenvalueProblems Symmetric;

        /// <summary>Provides routines for the Singular Value Decomposition of a specified matrix.
        /// </summary>
        public readonly ISingularValueDecomposition SingularValueDecomposition;

        /// <summary>Provides routines for the generalized Singular Value Decomposition of two matrices.
        /// </summary>
        public readonly IGeneralizedSingularValueDecomposition GeneralizedSingularValueDecomposition = null;

        /// <summary>Provides routines for Linear Least Squares Problems.
        /// </summary>
        public readonly ILinearLeastSquaresProblems LinearLeastSquaresProblem;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LapackEigenvalues"/> class.
        /// </summary>
        /// <param name="generalizedNonsymmetricEigenvalueProblems">The generalized nonsymmetric eigenvalue problems.</param>
        /// <param name="generalizedSymmetricEigenvalueProblems">The generalized symmetric eigenvalue problems.</param>
        /// <param name="nonSymmetricEigenvalueProblems">The non symmetric eigenvalue problems.</param>
        /// <param name="symmetricEigenvalueProblems">The symmetric eigenvalue problems.</param>
        /// <param name="singularValueDecomposition">The singular value decomposition.</param>
        /// <param name="generalizedSingularValueDecomposition">The generalized singular value decomposition.</param>
        /// <param name="linearLeastSquaresProblems">The linear least squares problems.</param>
        public LapackEigenvalues(
            IGeneralizedNonsymmetricEigenvalueProblems generalizedNonsymmetricEigenvalueProblems,
            IGeneralizedSymmetricEigenvalueProblems generalizedSymmetricEigenvalueProblems,
            INonSymmetricEigenvalueProblems nonSymmetricEigenvalueProblems,
            ISymmetricEigenvalueProblems symmetricEigenvalueProblems,
            ISingularValueDecomposition singularValueDecomposition,
            IGeneralizedSingularValueDecomposition generalizedSingularValueDecomposition,
            ILinearLeastSquaresProblems linearLeastSquaresProblems)
        {
            GeneralizedNonSymmetric = generalizedNonsymmetricEigenvalueProblems;
            GeneralizedSymmetric = generalizedSymmetricEigenvalueProblems;
            NonSymmetric = nonSymmetricEigenvalueProblems;
            Symmetric = symmetricEigenvalueProblems;
            SingularValueDecomposition = singularValueDecomposition;
            GeneralizedSingularValueDecomposition = generalizedSingularValueDecomposition;
            LinearLeastSquaresProblem = linearLeastSquaresProblems;
        }
        #endregion
    }
}