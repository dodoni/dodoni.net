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

namespace Dodoni.MathLibrary
{
    /// <summary>Represents a function returning a double-precision floating-point number that has one 
    /// double-precision floating-point number argument.
    /// </summary>
    /// <param name="x">The value where to evaluate the function.</param>
    /// <returns>The value of the function at <paramref name="x"/>.</returns>
    public delegate double tRealFunction(double x);

    /// <summary>Represents a function returning a double-precision floating-point number that has 
    /// a list of double-precision floating-point numbers as argument.
    /// </summary>
    /// <param name="x">The value where to evaluate the function.</param>
    /// <returns>The value of the function at <paramref name="x"/>.</returns>
    public delegate double tFunctionDoubleArrayToDouble(double[] x);

    /// <summary>Represents a function returning a double-precision floating-point number that has
    /// a list of double-precision floating-point numbers as argument. If desired the gradient, i.e.
    /// partial derivatives, will be computed as well.
    /// </summary>
    /// <param name="x">The point where to evaluate the function.</param>
    /// <param name="gradient">If != <c>null</c> this argument contains the gradient, i.e. the partial
    /// derivatives of the function at <paramref name="x"/>; otherwise this argument will be ignored.</param>
    /// <returns>The value of the function at <paramref name="x"/>.</returns>
    public delegate double tFunctionValueAndDerivative(double[] x, double[] gradient);
}