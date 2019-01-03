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
    public partial class LapackEigenvalues
    {
        /// <summary>Provides methods for solving generalized symmetric-definite eigenvalue problems, reordering the generalized Schur factorization of a pair of matrices, as well 
        /// as performing a number of related computational tasks. For more information see the documentation of the Linear Algebra PACKage http://www.netlib.org/lapack/index.html.
        /// </summary>
        /// <remarks>A generalized symmetric-definite eigenvalue problem is as follows: Find the eigenvalues λ and the corresponding eigenvectors z that satisfy one of these equations: 
        /// Az = λBz, ABz = λz, or BAz = λz, where A is an n-by-n symmetric or Hermitian matrix, and B is an n-by-n symmetric positive-definite or Hermitian positive-definite matrix.</remarks>
        public interface IGeneralizedSymmetricEigenvalueProblems
        {
            // todo: add further LAPACK functions on demand
        }
    }
}