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
namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Represents a bracketing triple, i.e. points a,b,c with
    /// <para>
    ///     a  &lt; b &lt; c (or c &lt; b &lt; a) and f(b) &lt; f(a), f(b) &lt; f(c).
    /// </para>
    /// where a and b are inside the domain of a specific real-valued function f.
    /// </summary>
    public class BracketingTriple
    {
        /// <summary>The first element of the bracketing triple (a,b,c).
        /// </summary>
        public double A;

        /// <summary>The second element of the bracketing triple (a,b,c).
        /// </summary>
        public double B;

        /// <summary>The third element of the bracketing triple (a,b,c).
        /// </summary>
        public double C;

        /// <summary>Initializes a new instance of the <see cref="BracketingTriple"/> class.
        /// </summary>
        /// <param name="a">The first element of the bracketing triple (a,b,c).</param>
        /// <param name="b">The second element of the bracketing triple (a,b,c).</param>
        /// <param name="c">The third element of the bracketing triple (a,b,c).</param>
        public BracketingTriple(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}