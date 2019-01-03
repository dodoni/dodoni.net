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
using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.ProbabilityTheory
{
    public partial class EziMatrixDecomposer
    {
        /// <summary>Serves as specific workspace container class.
        /// </summary>
        private class Workspace : WorkspaceContainer
        {
            #region internal members (for LAPACK function driver_dsyevrQuery etc.)

            internal double[] work;
            internal int[] iwork;
            internal int[] isuppz;
            internal double[] eigenValues;

            /// <summary>Matrix p in the EZI algorithm.
            /// </summary>
            internal double[] p;

            /// <summary>The normalized correlation matrix p in the EZI algorithm.
            /// </summary>
            internal double[] qNormalized;

            /// <summary>Corresponds to matrix p_hat in the EZI algorithm.
            /// </summary>
            internal double[] q;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Workspace" /> class.
            /// </summary>
            /// <param name="dimension">The dimension of correlation matrices for which the workspace is suited.</param>
            internal Workspace(int dimension)
                : base(dimension)
            {
                int n = dimension;

                int lwork, liwork;
                LAPACK.EigenValues.Symmetric.driver_dsyevrQuery(LapackEigenvalues.SymmetricGeneralJob.All, n, out lwork, out liwork);

                work = new double[lwork];
                iwork = new int[liwork];
                isuppz = new int[2 * n];
                eigenValues = new double[n];
                q = new double[n * n];
                p = new double[n * n];
                qNormalized = new double[n * n];
            }
            #endregion
        }
    }
}