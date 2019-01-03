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
namespace Dodoni.MathLibrary.ProbabilityTheory
{
    public partial class SapMatrixDecomposer
    {
        /// <summary>Serves as specific workspace container class.
        /// </summary>
        private class Workspace : WorkspaceContainer
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Workspace" /> class.
            /// </summary>
            /// <param name="dimension">The dimension of correlation matrices for which the workspace is suited.</param>
            /// <param name="sapDecomposer">The <see cref="SapMatrixDecomposer"/> object that serves as factory of the current object.</param>
            internal Workspace(int dimension, SapMatrixDecomposer sapDecomposer)
                : base(dimension)
            {
                InitalDecomposerWorkspace = sapDecomposer.m_InitialDecomposer.CreateWorkspaceContainer(dimension);
                CorrelationMatrixData = new double[dimension * dimension];
                ArgMinData = new double[dimension * (dimension - 1)];  // in general: n * (rank-1) with worst case 'rank = n'
                TempDifferences = new double[dimension * dimension];
            }
            #endregion

            #region internal properties

            /// <summary>Gets the <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object for the initial decomposer.
            /// </summary>
            /// <value>The <see cref="PseudoSqrtMatrixDecomposer.WorkspaceContainer"/> object for the initial decomposer.</value>
            internal WorkspaceContainer InitalDecomposerWorkspace
            {
                get;
                private set;
            }

            /// <summary>Gets the workspace array for the argMin of the optimization algorithm, i.e. for the initial guess and the final parameter set.
            /// </summary>
            /// <value>The workspace array for the argMin of the optimization algorithm, i.e. for the initial guess and the final parameter set.</value>
            internal double[] ArgMinData
            {
                get;
                private set;
            }

            /// <summary>Gets the workspace array for the correlation matrix calculated in each optimization step.
            /// </summary>
            /// <value>The workspace array for the correlation matrix calculated in each optimization step.</value>
            internal double[] CorrelationMatrixData
            {
                get;
                private set;
            }

            /// <summary>Gets the workspace array for differences between the specified (raw) correlation matrix and the estimated correlation matrix in each optimization step.
            /// </summary>
            /// <value>The workspace array for differences between the specified (raw) correlation matrix and the estimated correlation matrix in each optimization step.</value>
            internal double[] TempDifferences
            {
                get;
                private set;
            }
            #endregion
        }
    }
}