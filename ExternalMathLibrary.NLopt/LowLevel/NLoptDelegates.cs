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
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>The function delegate for an objective function with respect to the NLopt library, see http://ab-initio.mit.edu/wiki/index.php/NLopt.
    /// </summary>
    /// <param name="n">The dimension of the solving space.</param>
    /// <param name="x">The argument 'x' of the function to evaluate, where the first <paramref name="n"/> elements are taken into account.</param>
    /// <param name="grad">The optional gradient, i.e. if not <c>null</c> one has to store the gradient of the function
    /// at <paramref name="x"/> in this array thus \partial f / \partial x_j for j=1,\ldots, n.</param>
    /// <param name="data">Some generic optional parameter.</param>
    /// <returns>The value of the objective function at <paramref name="x"/>.</returns>
    [UnmanagedFunctionPointer(NLoptPtr.callingConvention)]
    public delegate double NLoptObjectiveFunction(int n, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] x, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), In, Out] double[] grad, IntPtr data);

    /// <summary>The function delegate for an inequality constraint function with respect to the NLopt library, see http://ab-initio.mit.edu/wiki/index.php/NLopt,
    /// i.e. a point x will be accepted if <c>g(x) &lt; \epsilon</c>, where \epsilon is a specific tolerance.
    /// </summary>
    /// <param name="n">The dimension of the solving space.</param>
    /// <param name="x">The argument 'x' of the function to evaluate, where the first <paramref name="n"/> elements are taken into account.</param>
    /// <param name="grad">The optional gradient, i.e. if not <c>null</c> one has to store the gradient of the function at <paramref name="x"/> in this array thus \partial g / \partial x_j for j=1,\ldots, n.</param>
    /// <param name="data">Some generic optional parameter.</param>
    /// <returns>The value of the constraint function at <paramref name="x"/>.</returns>
    [UnmanagedFunctionPointer(NLoptPtr.callingConvention)]
    public delegate double NLoptInequalityConstraintFunction(int n, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] x, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), In, Out] double[] grad, IntPtr data);

    /// <summary>The function delegate for a equality constraint function with respect to the NLopt library, see http://ab-initio.mit.edu/wiki/index.php/NLopt,
    /// i.e. a point x will be accepted if <c>|g(x)| &lt; \epsilon</c>, where \epsilon is a specific tolerance.
    /// </summary>
    /// <param name="n">The dimension of the solving space.</param>
    /// <param name="x">The argument 'x' of the function to evaluate, where the first <paramref name="n"/> elements are taken into account.</param>
    /// <param name="grad">The optional gradient, i.e. if not <c>null</c> one has to store the gradient of the function
    /// at <paramref name="x"/> in this array thus \partial g / \partial x_j for j=1,\ldots, n.</param>
    /// <param name="data">Some generic optional parameter.</param>
    /// <returns>The value of the constraint function at <paramref name="x"/>.</returns>
    [UnmanagedFunctionPointer(NLoptPtr.callingConvention)]
    public delegate double NLoptEqualityConstraintFunction(int n, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] x, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), In, Out] double[] grad, IntPtr data);

    /// <summary>The function delegate for a Vector-valued inequality constraint function g with respect to the NLopt library, see http://ab-initio.mit.edu/wiki/index.php/NLopt,
    /// i.a. a point x will be accepted if <c>g_j(x) &lt; \epsilon</c>, where \epsilon is a specific tolerance and g(x) = (g_1(x),...,g_m(x)).
    /// </summary>
    /// <param name="m">The number of components of the vector-valued inequality constraint function, i.e. the number of contraints.</param>
    /// <param name="result">The value of the vector-valued inequality constraint function.</param>
    /// <param name="n">The dimension of the solving space.</param>
    /// <param name="x">The argument 'x' of the function to evaluate, where the first <paramref name="n"/> elements are taken into account.</param>
    /// <param name="grad">The optional gradients of the single constraint functions, i.e. if not <c>null</c> one has to store the gradient of each real-value constraint function at <paramref name="x"/> in this array thus \partial g_i / \partial x_j = grad[i*n + j] for j=1,\ldots, n.</param>
    /// <param name="data">Some generic optional parameter.</param>
    [UnmanagedFunctionPointer(NLoptPtr.callingConvention)]
    public delegate void NLoptVectorInequalityConstraintFunction(int m, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out] double[] result, int n, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] x, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), In, Out] double[] grad, IntPtr data);

    /// <summary>The function delegate for a Vector-valued equality constraint function g with respect to the NLopt library, see http://ab-initio.mit.edu/wiki/index.php/NLopt,
    /// i.a. a point x will be accepted if <c>|g_j(x)| &lt; \epsilon</c>, where \epsilon is a specific tolerance and g(x) = (g_1(x),...,g_m(x)).
    /// </summary>
    /// <param name="m">The number of components of the vector-valued equality constraint function, i.e. the number of contraints.</param>
    /// <param name="result">The value of the vector-valued equality constraint function.</param>
    /// <param name="n">The dimension of the solving space.</param>
    /// <param name="x">The argument 'x' of the function to evaluate, where the first <paramref name="n"/> elements are taken into account.</param>
    /// <param name="grad">The optional gradient, i.e. if not <c>null</c> one has to store the gradient of the function at <paramref name="x"/> in this array thus \partial g_i / \partial x_j = grad[i*n + j] for j=1,\ldots, n.</param>
    /// <param name="data">Some generic optional parameter.</param>
    [UnmanagedFunctionPointer(NLoptPtr.callingConvention)]
    public delegate void NLoptVectorEqualityConstraintFunction(int m, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out] double[] result, int n, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] double[] x, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), In, Out] double[] grad, IntPtr data);
}