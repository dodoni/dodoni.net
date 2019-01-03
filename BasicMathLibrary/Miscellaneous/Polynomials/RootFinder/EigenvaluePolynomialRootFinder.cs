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
using Dodoni.MathLibrary.Basics;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents the root finder approach for Polynomials based on Eigenvalue calculation.
    /// </summary>
    /// <remarks>The implementation is based on William H. Press, Numerical Recipes in C, §9.5.</remarks>
    public class EigenvaluePolynomialRootFinder : IPolynomialRootFinder
    {
        #region private members

        /// <summary>The name of the root finder approach.
        /// </summary>
        private static IdentifierString sm_Name = new IdentifierString("Eigenvalue approach");
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get { return sm_Name; }
        }

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get { return sm_Name; }
        }
        #endregion

        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="EigenvaluePolynomialRootFinder"/> class.
        /// </summary>
        public EigenvaluePolynomialRootFinder()
        {
        }
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
            foreach (var root in GetRoots(degree, coefficients))
            {
                roots.Add(root);
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
            foreach (var root in GetRoots(degree, coefficients))
            {
                roots.Add(root);
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
            int numberOfRealRoots = 0;
            foreach (var root in GetRoots(degree, coefficients))
            {
                if (Math.Abs(root.Imaginary) < rootImaginaryZeroTolerance)
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
            int numberOfRealRoots = 0;
            foreach (var root in GetRoots(degree, coefficients))
            {
                if (Math.Abs(root.Imaginary) < rootImaginaryZeroTolerance)
                {
                    realRoots.Add(root.Real);
                    numberOfRealRoots++;
                }
            }
            return numberOfRealRoots;
        }
        #endregion

        #endregion

        #region protected methods

        /// <summary>Gets the roots via the eigenvalue method, i.e. the eigenvalues are the roots of the characteristic polynomial with respect to a given matrix.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The <paramref name="degree"/> + 1 coefficients of the polynomial.</param>
        /// <returns>The collection of roots, i.e. <paramref name="degree"/> elements.</returns>
        /// <remarks>The implementation is based on <para>William H. Press, Numerical Recipes in C, §9.5.</para></remarks>
        private IEnumerable<Complex> GetRoots(int degree, IList<Complex> coefficients)
        {
            var roots = new Complex[degree];

            // generate the upper Hessenberg matrix A with P(x) = \det(A -xI):
            var matrix = new Complex[degree * degree];
            var a_n = coefficients[degree];

            int k = 1;
            int coefficientIndex = degree - 1;
            for (int j = 0; j < degree - 1; j++)
            {
                matrix[j * degree] = -coefficients[coefficientIndex] / a_n;
                coefficientIndex--;

                matrix[k + j * degree] = 1.0;
                k++;
            }
            matrix[(degree - 1) * degree] = -coefficients[0] / a_n;

            // rebalance the matrix to avoid round-offs and compute the eigenvalues            
            var scale = new double[degree];

            LAPACK.EigenValues.NonSymmetric.zgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType.Scaled, degree, matrix, out int ilo, out int ihi, scale);

            int workspaceLength = LAPACK.EigenValues.NonSymmetric.zhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob.EigenvaluesOnly, LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors, degree, ilo, ihi);
            var workspace = new Complex[workspaceLength];
            LAPACK.EigenValues.NonSymmetric.zhseqr(LapackEigenvalues.NonSymmetricXhseqrJob.EigenvaluesOnly, LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors, degree, ilo, ihi, matrix, roots, null, workspace);

            return roots;
        }

        /// <summary>Gets the roots via the eigenvalue method, i.e. the eigenvalues are the roots of the characteristic polynomial with respect to a given matrix.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="coefficients">The <paramref name="degree"/> + 1 coefficients of the polynomial.</param>
        /// <returns>The roots collection of the polynomial.</returns>
        /// <remarks>The implementation is based on 
        /// <para>William H. Press, Numerical Recipes in C, §9.5.</para></remarks>
        private IEnumerable<Complex> GetRoots(int degree, IList<double> coefficients)
        {
            var rootsRealPart = new double[degree];
            var rootsImaginaryPart = new double[degree];

            // generate the upper Hessenberg matrix A with P(x) = \det(A -xI):
            var matrix = new double[degree * degree];
            double a_n = coefficients[degree];

            int k = 1;
            int coefficientIndex = degree - 1;
            for (int j = 0; j < degree - 1; j++)
            {
                matrix[j * degree] = -coefficients[coefficientIndex] / a_n;
                coefficientIndex--;

                matrix[k + j * degree] = 1.0;
                k++;
            }
            matrix[(degree - 1) * degree] = -coefficients[0] / a_n;

            // rebalance the matrix to avoid round-offs and compute the eigenvalues            
            double[] scale = new double[degree];

            LAPACK.EigenValues.NonSymmetric.dgebal(LapackEigenvalues.NonSymmetricMatrixBalancesType.Scaled, degree, matrix, out int ilo, out int ihi, scale);

            int workspaceLength = LAPACK.EigenValues.NonSymmetric.dhseqrQuery(LapackEigenvalues.NonSymmetricXhseqrJob.EigenvaluesOnly, LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors, degree, ilo, ihi);
            double[] workspace = new double[workspaceLength];
            LAPACK.EigenValues.NonSymmetric.dhseqr(LapackEigenvalues.NonSymmetricXhseqrJob.EigenvaluesOnly, LapackEigenvalues.NonSymmetricXhseqrOperation.NoSchurVectors, degree, ilo, ihi, matrix, rootsRealPart, rootsImaginaryPart, null, workspace);

            for (int j = 0; j < degree; j++)
            {
                yield return new Complex(rootsRealPart[j], rootsImaginaryPart[j]);
            }
        }
        #endregion
    }
}