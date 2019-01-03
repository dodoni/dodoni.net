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
    public partial class MklLapackEigenvalues
    {
        /// <summary>A value indicating what kind of job to do by the LAPACK function 'dsyrdb'.
        /// </summary>
        public enum DsyrdbJob
        {
            /// <summary>Reduce the specified symmetric matrix only.
            /// </summary>
            None,

            /// <summary>Reduce the specified symmetric matrix and the parameter 'a' contains the orthogonal matrix Q on exit.
            /// </summary>
            V,

            /// <summary>Reduce the specified symmetric matrix and the parameter 'a' contains Z * Q on exit, where Q is the orthogonal matrix.
            /// </summary>
            U
        }

        /// <summary>A value indicating what kind of job to do by the LAPACK function 'zherdb'.
        /// </summary>
        public enum ZherdbJob
        {
            /// <summary>Reduce the specified symmetric matrix only.
            /// </summary>
            None,

            /// <summary>Reduce the specified symmetric matrix and the parameter 'a' contains the orthogonal matrix Q on exit.
            /// </summary>
            V,

            /// <summary>Reduce the specified symmetric matrix and the parameter 'a' contains Z * Q on exit, where Q is the orthogonal matrix.
            /// </summary>
            U
        }
    }
}