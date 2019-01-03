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

namespace Dodoni.MathLibrary.Miscellaneous
{
    public partial class MultiDimRegion
    {
        public abstract partial class Polynomial
        {
            /// <summary>Represents a specific monomial, i.e. <para>'x_j^k',</para> where x = (x_0,... ,x_n) is some element in \R^{n+1}.
            /// </summary>
            public class Monomial
            {
                /// <summary>The null-based index of the argument, i.e. the index 'j' with respect to 'x_j', where x = (x_0,..., x_n).
                /// </summary>
                public readonly int ArgumentIndex;

                /// <summary>The power of the argument, i.e. represents 'k' with respect to 'x_j^k', where x = (x_0,..., x_n).
                /// </summary>
                public readonly int Power;

                /// <summary>Initializes a new instance of the <see cref="Monomial"/> class.
                /// </summary>
                /// <param name="argumentIndex">The null-based index of the argument, i.e. the index 'j' with respect to 'x_j', where x = (x_0,..., x_n).</param>
                /// <param name="power">The power of the argument, i.e. represents 'k' with respect to 'c * x_j^k', where x = (x_0,..., x_n).</param>
                public Monomial(int argumentIndex, int power)
                {
                    ArgumentIndex = argumentIndex;
                    Power = power;
                }

                /// <summary>Returns a <see cref="System.String"/> that represents this instance.
                /// </summary>
                /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
                public override string ToString()
                {
                    return String.Format("x_{0}^{1}", ArgumentIndex, Power);
                }

                /// <summary>Creates a new <see cref="Monomial"/> object.
                /// </summary>
                /// <param name="argumentIndex">The null-based index of the argument, i.e. the index 'j' with respect to 'x_j', where x = (x_0,..., x_n).</param>
                /// <param name="power">The power of the argument, i.e. represents 'k' with respect to 'x_j^k', where x = (x_0,..., x_n).</param>
                /// <returns>The specified <see cref="Monomial"/> object.</returns>
                public static Monomial Create(int argumentIndex, int power)
                {
                    return new Monomial(argumentIndex, power);
                }
            }
        }
    }
}