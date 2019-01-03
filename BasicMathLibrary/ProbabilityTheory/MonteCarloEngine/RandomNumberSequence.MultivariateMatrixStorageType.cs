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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides interfaces and structures etc. for handling Random Number sequences.
    /// </summary>
    public partial class RandomNumberSequence
    {
        /// <summary>The matrix storage type for the generation of multivariate random vectors.
        /// </summary>
        public enum MultivariateMatrixStorageType
        {
            /// <summary>The whole matrix Q' is stored, supplied column-by-column, thus the transposed matrix Q is supplied row-by-row.
            /// </summary>
            Full = 0,

            /// <summary>The upper triangular matrix is packed in 1-dimensional array, i.e. the upper triangle elements of Q' are packed by rows into a one-dimensional array,
            /// where Q * Q' is the covariance matrix.
            /// </summary>
            /// <remarks>We used the Fortran interface which supports the upper triangular matrix only.</remarks>
            TriangularPackaged = 1,

            /// <summary>Diagonal elements are packed in 1-dimensional array only, i.e. each element represents the standard deviation of the j-th projection.
            /// </summary>
            Diagonal = 2
        }
    }
}