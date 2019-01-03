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
using System.Numerics;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Provides routines of the Linear Algebra PACKage, see http://www.netlib.org/lapack/index.html, that are related to Linear Equations.
    /// </summary>
    public partial class LapackLinearEquations
    {
        #region public (readonly) members

        /// <summary>Provides routines to estimate conditional numbers of a specified Matrix.
        /// </summary>
        public readonly IMatrixConditionalNumbers MatrixConditionalNumbers;

        /// <summary>Provide routines to compute scaling factors to equilibrate a specified Matrix.
        /// </summary>
        public readonly IMatrixEquilibration MatrixEquilibration;

        /// <summary>Provide routines for Matrix factorization.
        /// </summary>
        public readonly IMatrixFactorization MatrixFactorization;

        /// <summary>Provide routines for Matrix inversion.
        /// </summary>
        public readonly IMatrixInversion MatrixInversion;

        /// <summary>Provides routines to solve Linear Equations.
        /// </summary>
        public readonly ISolver Solver;

        /// <summary>Provides routines to solve Linear Equations and returns some error estimation.
        /// </summary>
        public readonly IErrorEstimationSolver ErrorEstimationSolver;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LapackLinearEquations"/> class.
        /// </summary>
        /// <param name="matrixConditionalNumbers">The matrix conditional numbers.</param>
        /// <param name="matrixEquilibration">The matrix equilibration.</param>
        /// <param name="matrixFactorization">The matrix factorization.</param>
        /// <param name="matrixInversion">The matrix inversion.</param>
        /// <param name="solver">The solver.</param>
        /// <param name="errorEstimationSolver">The error estimation solver.</param>
        public LapackLinearEquations(
            IMatrixConditionalNumbers matrixConditionalNumbers,
            IMatrixEquilibration matrixEquilibration,
            IMatrixFactorization matrixFactorization,
            IMatrixInversion matrixInversion,
            ISolver solver,
            IErrorEstimationSolver errorEstimationSolver)
        {
            MatrixConditionalNumbers = matrixConditionalNumbers;
            MatrixEquilibration = matrixEquilibration;
            MatrixFactorization = matrixFactorization;
            MatrixInversion = matrixInversion;
            Solver = solver;
            ErrorEstimationSolver = errorEstimationSolver;
        }
        #endregion
    }
}