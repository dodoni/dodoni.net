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
namespace Dodoni.MathLibrary
{
    public partial class DenseMatrix
    {
        /// <summary>Provides methods for unit matrices.
        /// </summary>
        public class Unity
        {
            /// <summary>Creates a specific unit matrix.
            /// </summary>
            /// <param name="dimension">The dimension of the (quadratic) unit matrix.</param>
            /// <returns>The specified unit matrix in its <see cref="DenseMatrix"/> representation.</returns>
            public static DenseMatrix Create(int dimension)
            {
                var unityMatrix = new DenseMatrix(dimension, dimension);
                for (int j = 0; j < dimension; j++)
                {
                    unityMatrix.Data[j * (1+dimension)] = 1.0;
                }
                return unityMatrix;
            }
        }
    }
}