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
using System.Numerics;
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents Laguerre's root finder method for Polynomials.
    /// </summary>
    /// <remarks>The implementation is based on<para>Numerical Recipes in C, §9.5, William H. Press.</para>
    /// </remarks>
    public class LaguerrePolynomialRootFinder
    {
        #region private static members

        /// <summary>The period to break a limit cycle in Laguerre's root finding method.
        /// </summary>
        private const int sm_LaguerreCylePeriod = 10;

        /// <summary>Fractions used to break a limit cycle in Laguerre's root finding method.
        /// </summary>
        private static double[] sm_LaguerreFrac = new double[] { 0.0, 0.5, 0.25, 0.75, 0.13, 0.38, 0.62, 0.88, 1.0 };
        #endregion

        #region public (readonly) members

        /// <summary>Laguerre's root finder method with polishing and standard settings.
        /// </summary>
        public readonly IPolynomialRootFinder StandardPolishing;

        /// <summary>Laguerre's root finder method without polishing and standard settings.
        /// </summary>
        public readonly IPolynomialRootFinder StandardNonPolishing;
        #endregion

        #region nested classes

        /// <summary>The implementation of the root finder.
        /// </summary>
        private class RootFinder : IPolynomialRootFinder
        {
            #region private (readonly) members

            /// <summary>A value indicating whether to polishing.
            /// </summary>
            private readonly bool m_Polish;

            /// <summary>The tolerance of the algorithm.
            /// </summary>
            private readonly double m_Tolerance;

            /// <summary>The maximal number of iterations of the algorithm.
            /// </summary>
            private readonly int m_MaxNumberOfIterations;

            /// <summary>A positive tolerance taken into account to indicate whether a complex number is a real number (for the algorithm only, will not be applied to the real roots).
            /// </summary>
            private readonly double m_AlgorithmImaginaryZeroTolerance;

            /// <summary>The name of the root finder approach.
            /// </summary>
            private readonly IdentifierString m_Name;
            #endregion

            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="RootFinder"/> class.
            /// </summary>
            /// <param name="polishing">A value indicating whether the roots are polishing by Laguerre's method.</param>
            /// <param name="tolerance">A (positive) tolerance needed for the iterated algorithm.</param>
            /// <param name="maxNumberOfIterations">The maximal number of iterations for each root.</param>
            /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account for the algorithm to indicate whether a complex number is a real number (this argument 
            /// will be used for the algorithm but not to indicate whether a specific root is a real number).</param>
            public RootFinder(bool polishing, double tolerance = MachineConsts.TinyEpsilon, int maxNumberOfIterations = 500, double imaginaryZeroTolerance = MachineConsts.Epsilon)
            {
                m_Polish = polishing;
                m_Tolerance = tolerance;
                m_MaxNumberOfIterations = maxNumberOfIterations;
                m_AlgorithmImaginaryZeroTolerance = imaginaryZeroTolerance;
                m_Name = new IdentifierString(String.Format("Laguerre root finding {{polishing:{0};tolerance:{1};maxNumberOfIterations:{2}}})", polishing, tolerance, maxNumberOfIterations));
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the long name of the current instance.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the current instance.</value>
            public IdentifierString LongName
            {
                get { return m_Name; }
            }

            /// <summary>Gets the name of the current instance.
            /// </summary>
            /// <value>The language independent name of the current instance.</value>
            public IdentifierString Name
            {
                get { return m_Name; }
            }
            #endregion

            #endregion

            #region public methods

            #region IPolynomialRootFinder Members

            /// <summary>Gets the roots of a specified polynomial.
            /// </summary>
            /// <param name="degree">The degree of the polynomial.</param>
            /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds
            /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
            /// <param name="roots">The <paramref name="degree"/> roots (output).</param>
            /// <remarks><paramref name="roots"/> will <c>not</c> be cleared before adding roots.</remarks>
            public void GetRoots(int degree, IList<Complex> coefficients, IList<Complex> roots)
            {
                var workingCoefficients = new Complex[degree + 1];
                coefficients.CopyTo(workingCoefficients, 0);  // use a copy of the coefficients

                if (m_Polish)
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);

                    for (int j = 0; j < degree; j++) // Polish the roots using the undeflated coefficients
                    {
                        Complex root = roots[j];
                        LaguerreSingleStepRootFinder(coefficients, degree, m_MaxNumberOfIterations, m_Tolerance, ref root);
                        roots[j] = root;
                    }
                }
                else
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);
                }
            }

            /// <summary>Gets the roots of a specified (real-valued) polynomial.
            /// </summary>
            /// <param name="degree">The degree of the polynomial.</param>
            /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds
            /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
            /// <param name="roots">The <paramref name="degree"/> roots (output).</param>
            /// <remarks><paramref name="roots"/> will <c>not</c> be cleared before adding roots.</remarks>
            public void GetRoots(int degree, IList<double> coefficients, IList<Complex> roots)
            {
                var workingCoefficients = new Complex[degree + 1];  // use a copy of the coefficients
                for (int j = 0; j <= degree; j++)
                {
                    workingCoefficients[j] = new Complex(coefficients[j], 0.0);
                }

                if (m_Polish)
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);

                    for (int j = 0; j <= degree; j++)
                    {
                        workingCoefficients[j] = new Complex(coefficients[j], 0.0);
                    }

                    for (int j = 0; j < degree; j++) // Polish the roots using the undeflated coefficients
                    {
                        Complex root = roots[j];
                        LaguerreSingleStepRootFinder(workingCoefficients, degree, m_MaxNumberOfIterations, m_Tolerance, ref root);
                        roots[j] = root;
                    }
                }
                else
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);
                }
            }

            /// <summary>Gets the real roots of a specified polynomial.
            /// </summary>
            /// <param name="degree">The degree of the polynomial.</param>
            /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds
            /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
            /// <param name="realRoots">The real-valued roots (output).</param>
            /// <param name="rootImaginaryZeroTolerance">The tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
            /// <returns>The number or real roots added to <paramref name="realRoots"/>.</returns>
            /// <remarks>Roots are added with respect to their multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
            /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.</para>
            /// </remarks>
            public int GetRealRoots(int degree, IList<Complex> coefficients, IList<double> realRoots, double rootImaginaryZeroTolerance = MachineConsts.Epsilon)
            {
                var workingCoefficients = new Complex[degree + 1];
                coefficients.CopyTo(workingCoefficients, 0); // use a copy of the coefficients

                // get the (complex) roots first:
                var roots = new List<Complex>(degree + 1);
                if (m_Polish)
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);

                    for (int j = 0; j < degree; j++) // Polish the roots using the undeflated coefficients
                    {
                        Complex root = roots[j];
                        LaguerreSingleStepRootFinder(coefficients, degree, m_MaxNumberOfIterations, m_Tolerance, ref root);
                        roots[j] = root;
                    }
                }
                else
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);
                }

                // filter the real roots:
                int numberOfRealRoots = 0;
                for (int j = 0; j < roots.Count; j++)
                {
                    Complex root = roots[j];
                    if (Math.Abs(root.Imaginary) <= rootImaginaryZeroTolerance * Math.Abs(root.Real))
                    {
                        realRoots.Add(root.Real);
                        numberOfRealRoots++;
                    }
                }
                return numberOfRealRoots;
            }

            /// <summary>Gets the real roots of a specified polynomial.
            /// </summary>
            /// <param name="degree">The degree of the polynomial.</param>
            /// <param name="coefficients">The coefficients of the polynmial P(z) = a_0 + a_1 * z + a_2 * z^2 + ... + a_n * z^n where the null-based index corresponds
            /// to the power of the argument, i.e. starting from the absolute coefficient, first oder coefficient etc.</param>
            /// <param name="realRoots">The real-valued roots (output).</param>
            /// <param name="rootImaginaryZeroTolerance">The tolerance taken into account to indicate whether a complex number is interpreted as real number.</param>
            /// <returns>The number or real roots added to <paramref name="realRoots"/>.</returns>
            /// <remarks>Roots are added with respect to their multiplicity, i.e. a root may add more than once in <paramref name="realRoots"/>.
            /// <para><paramref name="realRoots"/> will <c>not</c> be cleared before adding real roots.</para>
            /// </remarks>
            public int GetRealRoots(int degree, IList<double> coefficients, IList<double> realRoots, double rootImaginaryZeroTolerance = MachineConsts.Epsilon)
            {
                var workingCoefficients = new Complex[degree + 1]; // use a copy of the coefficients
                for (int j = 0; j <= degree; j++)
                {
                    workingCoefficients[j] = new Complex(coefficients[j], 0.0);
                }

                // get the (complex) roots first:
                var roots = new List<Complex>(degree + 1);
                if (m_Polish)
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);
                    for (int j = 0; j <= degree; j++)
                    {
                        workingCoefficients[j] = new Complex(coefficients[j], 0.0);
                    }

                    for (int j = 0; j < degree; j++) // Polish the roots using the undeflated coefficients
                    {
                        Complex root = roots[j];
                        LaguerreSingleStepRootFinder(workingCoefficients, degree, m_MaxNumberOfIterations, m_Tolerance, ref root);
                        roots[j] = root;
                    }
                }
                else
                {
                    GetRootsViaLaguerre(workingCoefficients, degree, roots, m_MaxNumberOfIterations, m_Tolerance, m_AlgorithmImaginaryZeroTolerance);
                }

                // filter the real roots:
                int numberOfRealRoots = 0;
                for (int j = 0; j < roots.Count; j++)
                {
                    Complex root = roots[j];
                    if (Math.Abs(root.Imaginary) <= rootImaginaryZeroTolerance * Math.Abs(root.Real))
                    {
                        realRoots.Add(root.Real);
                        numberOfRealRoots++;
                    }
                }
                return numberOfRealRoots;
            }
            #endregion

            #endregion
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="LaguerrePolynomialRootFinder"/> class.
        /// </summary>
        public LaguerrePolynomialRootFinder()
        {
            StandardPolishing = new RootFinder(true);
            StandardNonPolishing = new RootFinder(false);
        }
        #endregion

        #region public methods

        /// <summary>Creates a new instance of the <see cref="RootFinder"/> class.
        /// </summary>
        /// <param name="polishing">A value indicating whether the roots are polishing by Laguerre's method.</param>
        /// <param name="tolerance">A (positive) tolerance needed for the iterated algorithm.</param>
        /// <param name="maxNumberOfIterations">The maximal number of iterations for each root.</param>
        /// <param name="imaginaryZeroTolerance">The (positive) tolerance taken into account for the algorithm to indicate whether a complex number is a real number (this argument 
        /// will be used for the algorithm but not to indicate whether a specific root is a real number).</param>
        public IPolynomialRootFinder Create(bool polishing, double tolerance = MachineConsts.Epsilon, int maxNumberOfIterations = 500, double imaginaryZeroTolerance = MachineConsts.Epsilon)
        {
            return new RootFinder(polishing, tolerance, maxNumberOfIterations, imaginaryZeroTolerance);
        }
        #endregion

        #region protected (static) methods

        /// <summary>Gets the roots of a polynomial with complex coefficients via Laguerre's method.
        /// </summary>
        /// <param name="coefficients">The coefficients of the polynomial (will be changed after evaluation call).</param>
        /// <param name="degree">The degree.</param>
        /// <param name="roots">The roots (output).</param>
        /// <param name="maxNumberOfIterations">The maximal number of iterations for each root.</param>
        /// <param name="tolerance">A (positive) tolerance needed for the iterated algorithm.</param>
        /// <param name="algorithmImaginaryZeroTolerance">The (positive) tolerance taken into account to indicate whether some complex number is interpreted as real.</param>
        /// <remarks>See<para>Numerical Recipes in C, §9.5, William H. Press.</para>
        /// </remarks>
        protected static void GetRootsViaLaguerre(IList<Complex> coefficients, int degree, IList<Complex> roots, int maxNumberOfIterations, double tolerance, double algorithmImaginaryZeroTolerance = MachineConsts.Epsilon)
        {
            for (int j = degree; j >= 1; j--)
            {
                var x = Complex.Zero; // start at zero to favor convergence to smallest remaining  root, and nd the root.
                if (LaguerreSingleStepRootFinder(coefficients, j, maxNumberOfIterations, tolerance, ref x) > maxNumberOfIterations)
                {
                    throw new Exception("To many iterations needed.");
                }
                if (Math.Abs(x.Imaginary) <= algorithmImaginaryZeroTolerance * Math.Abs(x.Real))
                {
                    x = new Complex(x.Real, 0.0); // i.e.  x.ImaginaryPart = 0.0;
                }
                roots.Add(x);
                var b = coefficients[j]; // Forward deflation
                for (int k = j - 1; k >= 0; k--)
                {
                    var c = coefficients[k];
                    coefficients[k] = b;
                    b = (x * b) + c;
                }
            }
        }

        /// <summary>Use Laguerre's method to find a root of a polynomial.
        /// </summary>
        /// <param name="coefficients">The coefficients of the polynomial.</param>
        /// <param name="degree">The degree.</param>
        /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
        /// <param name="tolerance">A (positive) tolerance needed for the iterated algorithm.</param>
        /// <param name="root">The root (initial guess and output).</param>
        /// <returns>The number of iterations needed for the computation.</returns>
        /// <remarks>See<para>Numerical Recipes in C, §9.5, William H. Press.</para></remarks>
        protected static int LaguerreSingleStepRootFinder(IList<Complex> coefficients, int degree, int maxNumberOfIterations, double tolerance, ref Complex root)
        {
            for (int n = 1; n <= maxNumberOfIterations; n++)
            {
                double absRoot = Complex.Abs(root);

                var functionValue = coefficients[degree];
                double err = Complex.Abs(functionValue);

                // Effcient computation of the polynomial and its first two derivatives:
                var firstDerivative = Complex.Zero;
                var secondDerivativeDivTwo = Complex.Zero;
                for (int j = degree - 1; j >= 0; j--)
                {
                    secondDerivativeDivTwo = (root * secondDerivativeDivTwo) + firstDerivative;
                    firstDerivative = (root * firstDerivative) + functionValue;
                    functionValue = (root * functionValue) + coefficients[j];
                    err = Complex.Abs(functionValue) + absRoot * err;
                }
                err *= tolerance; //Estimate of roundof error in evaluating polynomial:
                if (Complex.Abs(functionValue) <= err)
                {
                    return n; // We are on the root.
                }

                var derivativeDivFunctionValue = firstDerivative / functionValue; //The generic case: use Laguerre's formula.
                var squareOfDerivativeDivFunctionValue = derivativeDivFunctionValue * derivativeDivFunctionValue;
                var h = squareOfDerivativeDivFunctionValue - 2.0 * secondDerivativeDivTwo / functionValue;  // with respect to formula (9.5.7)

                // choose the correct denominator, see formula (9.5.11):
                var radikand = Complex.Sqrt((degree - 1) * ((degree * h) - squareOfDerivativeDivFunctionValue));
                var denominatorPlus = derivativeDivFunctionValue + radikand;
                var denominatorMinus = derivativeDivFunctionValue - radikand;
                var absDenominatorPlus = Complex.Abs(denominatorPlus);
                var absDenominatorMinus = Complex.Abs(denominatorMinus);
                if (absDenominatorPlus < absDenominatorMinus)
                {
                    denominatorPlus = denominatorMinus;
                }
                var a = (Math.Max(absDenominatorPlus, absDenominatorMinus) > 0.0 ? (degree / denominatorPlus) : (1 + absRoot) * new Complex(Math.Cos(n), Math.Sin(n)));
                var x1 = root - a;

                if (root.Real == x1.Real && root.Imaginary == x1.Imaginary)
                {
                    return n; //Converged.
                }
                if (n % sm_LaguerreCylePeriod == 0)
                {
                    root = x1;
                }
                else
                {
                    root = root - (sm_LaguerreFrac[n / sm_LaguerreCylePeriod] * a);
                }
                //Every so often we take a fractional step, to break any limit cycle (itself a rare occurrence).
            }
            throw new Exception("Laguerre's root finding method doesn't work. Too many iterations needed.");
        }
        #endregion
    }
}