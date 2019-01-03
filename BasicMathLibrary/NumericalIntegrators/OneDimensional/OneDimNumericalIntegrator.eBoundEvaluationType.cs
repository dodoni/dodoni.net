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
namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class OneDimNumericalIntegrator
    {
        /// <summary>Represents the boundary type of a numerical integration, i.e. whether the function to integrate will be evaluate at the lower/upper bound in the algorithm.
        /// </summary>
        public enum BoundEvaluationType
        {
            /// <summary>The upper bound, lower boundary repectively, is unbounded.
            /// </summary>
            Unbounded,

            /// <summary>The numerical integration algorithm may evaluate the function to integrate at the (lower, upper respectively) boundary.
            /// </summary>
            Closed,

            /// <summary>The numerical integration algorithm does not evaluate the function to integrate at the (lower, upper respectively) boundary.
            /// </summary>
            Open
        }
    }
}