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
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary
{
    /// <summary>Provides static methods for common mathematical functions similar to <see cref="System.Math"/>.
    /// </summary>
    public static partial class DoMath
    {
        #region static constructor

        /// <summary>Initializes the <see cref="DoMath" /> class.
        /// </summary>
        static DoMath()
        {
        }
        #endregion

        #region simple methods

        /// <summary>Returns the larger of two double-precision floating-point numbers; numbers that are <see cref="System.Double.NaN"/> will be ignored.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare; perhaps <see cref="System.Double.NaN"/>.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare; perhaps <see cref="System.Double.NaN"/>.</param>
        /// <returns>Parameter <paramref name="val1"/> or <paramref name="val2"/>, whichever is larger; or <see cref="System.Double.NaN"/> if both parameter are <see cref="System.Double.NaN"/>. If one of the parameter is <see cref="System.Double.NaN"/> it will be ignored.</returns>
        public static double Max(double val1, double val2)
        {
            if (Double.IsNaN(val1) == true)
            {
                return val2;
            }
            if (Double.IsNaN(val2) == true)
            {
                return val1;
            }
            return Math.Max(val1, val2);
        }

        /// <summary>Returns the smaller of two double-precision floating-point numbers; numbers that are <see cref="System.Double.NaN"/> will be ignored.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare; perhaps <see cref="System.Double.NaN"/>.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare; perhaps <see cref="System.Double.NaN"/>.</param>
        /// <returns>Parameter <paramref name="val1"/> or <paramref name="val2"/>, whichever is smaller; or <see cref="System.Double.NaN"/> if both parameter are <see cref="System.Double.NaN"/>. If one of the parameter is <see cref="System.Double.NaN"/> it will be ignored.</returns>
        public static double Min(double val1, double val2)
        {
            if (Double.IsNaN(val1) == true)
            {
                return val2;
            }
            if (Double.IsNaN(val2) == true)
            {
                return val1;
            }
            return Math.Min(val1, val2);
        }

        /// <summary>Computes the sum of a sequence of <see cref="System.Double"/> values in a numerical stable implementation.
        /// </summary>
        /// <param name="source">A sequence of System.Double values to calculate the sum</param>
        /// <returns>The sum of the sequence of values.</returns>
        /// <remarks>The compensated summation approach by Kahan is applied; see for example 'Numerische Mathematik 1', Tomas Sauer, 2005, Uni Giessen.</remarks>
        public static double Sum(IEnumerable<double> source)
        {
            var sum = source.First();
            var c = 0.0;

            foreach (var value in source.Skip(1))
            {
                var y = value - c;
                var t = sum + y;
                c = (t - sum) - y;
                sum = t;
            }
            return sum;
        }

        /// <summary>Computes the sum of a sequence of <see cref="System.Double"/> values in a numerical stable implementation.
        /// </summary>
        /// <param name="source">A sequence of System.Double values to calculate the sum</param>
        /// <returns>The sum of the sequence of values.</returns>
        /// <remarks>The compensated summation approach by Kahan (for complex numbers) is applied; see for example 'Numerische Mathematik 1', Tomas Sauer, 2005, Uni Giessen.</remarks>
        public static Complex Sum(IEnumerable<Complex> source)
        {
            var sum = source.First();
            var c = Complex.Zero;

            foreach (var value in source.Skip(1))
            {
                var y = value - c;
                var t = sum + y;
                c = (t - sum) - y;
                sum = t;
            }
            return sum;
        }

        /// <summary>Computes the average of a sequence of <see cref="System.Double"/> values in a numerical stable implementation.
        /// </summary>
        /// <param name="source">A sequence of System.Double values to calculate the average.</param>
        /// <returns>The average of the sequence of values.</returns>
        /// <remarks>The compensated summation approach by Kahan is applied; see for example 'Numerische Mathematik 1', Tomas Sauer, 2005, Uni Giessen.</remarks>
        public static double Average(IEnumerable<double> source)
        {
            var sum = source.First();
            var c = 0.0;

            int k = 1;
            foreach (var value in source.Skip(1))
            {
                var y = value - c;
                var t = sum + y;
                c = (t - sum) - y;
                sum = t;
                k++;
            }
            return sum / k;
        }

        /// <summary>Computes the average of a sequence of <see cref="System.Double"/> values in a numerical stable implementation.
        /// </summary>
        /// <param name="source">A sequence of System.Double values to calculate the average.</param>
        /// <returns>The average of the sequence of values.</returns>
        /// <remarks>The compensated summation approach by Kahan (for complex numbers) is applied; see for example 'Numerische Mathematik 1', Tomas Sauer, 2005, Uni Giessen.</remarks>
        public static Complex Average(IEnumerable<Complex> source)
        {
            var sum = source.First();
            var c = Complex.Zero;

            int k = 1;
            foreach (var value in source.Skip(1))
            {
                var y = value - c;
                var t = sum + y;
                c = (t - sum) - y;
                sum = t;
                k++;
            }
            return sum / k;
        }

        /// <summary>Gets the Euclidian distance between two <see cref="System.Double"/> objects, i.e. \sqrt( a^2 + b^2 ).
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The euclidian distance between <paramref name="a"/> and <paramref name="b"/>, i.e. 
        /// <para>
        ///   \sqrt( a^2 + b^2 ).
        /// </para>
        /// </returns>
        /// <remarks>This method avoids rounding errors.</remarks>
        public static double EuclidianDistance(double a, double b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a > b)
            {
                double t = b / a;
                return a * Math.Sqrt(1.0 + t * t);
            }
            else if (b > a)
            {
                double t = a / b;
                return b * Math.Sqrt(1.0 + t * t);
            }
            return a * MathConsts.Sqrt2;
        }

        /// <summary>Returns the value of the Heaviside function.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>0 if <paramref name="x"/> is less or equal 0.0; otherwise 1.</returns>
        /// <param name="tolerance">A positive tolerance to take into account.</param>
        /// <remarks>This function take into account rounding errors by first adding <paramref name="tolerance"/> to the argument <paramref name="x"/>.</remarks>
        public static int HeavisideFunction(double x, double tolerance = MachineConsts.Epsilon)
        {
            return (x + tolerance <= 0) ? 0 : 1;
        }

        /// <summary>Returns sqrt(a^2 + b^2), i.e. the hypotenuse of a triangle.
        /// </summary>
        /// <param name="a">The <see cref="System.Double"/> a.</param>
        /// <param name="b">The <see cref="System.Double"/> b.</param>
        /// <returns>The square root of a^2  + b^2.</returns>
        /// <remarks>This implementation avoids under- and overflows.</remarks>
        public static double Hypotenuse(double a, double b)
        {
            double r = 0.0;
            if (Math.Abs(a) > Math.Abs(b))
            {
                r = b / a;
                return Math.Abs(a) * Math.Sqrt(1 + r * r);
            }
            else if (b != 0)
            {
                r = a / b;
                return Math.Abs(b) * Math.Sqrt(1 + r * r);
            }
            return r;
        }
        #endregion

        #region analytic functions

        /// <summary>Returns a number raised to an integer power.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <param name="n">An integer exponent.</param>
        /// <returns>The real number <paramref name="x"/> raised to the specified exponent.</returns>
        public static double Pow(double x, int n)
        {
            int k = (n >= 0) ? n : -n;
            double value;

            switch (k)
            {
                case 0:
                    value = 1.0;
                    break;
                case 1:
                    value = x;
                    break;
                case 2:
                    value = x * x;
                    break;
                case 3:
                    value = x * x * x;
                    break;
                case 4:
                    double xSquared = x * x;
                    value = xSquared * xSquared;
                    break;

                default:
                    if (k <= 20)
                    {
                        double squaredX = x * x;

                        value = squaredX;
                        for (int i = 1; i <= (k / 2) - 1; i++)
                        {
                            value *= squaredX;
                        }
                        if ((n % 2) == 1)
                        {
                            value = value * x;
                        }
                    }
                    else if (k <= 60)
                    {
                        double powerXOfThree = x * x * x;

                        value = powerXOfThree;
                        for (int i = 1; i <= (k / 3) - 1; i++)
                        {
                            value *= powerXOfThree;
                        }
                        if ((k % 3) == 1)
                        {
                            value = value * x;
                        }
                        else if ((k % 3) == 2)
                        {
                            value = value * x * x;
                        }
                    }
                    else
                    {
                        value = Math.Pow(x, k);
                    }
                    break;
            }
            return (n >= 0) ? value : 1.0 / value;
        }
        #endregion

        #region number theory

        /// <summary>Returns the Greatest Common Divisor of two integers.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }
            return a;
        }

        /// <summary>Returns the Least Common Multiple of two integers.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The least common multiple of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static int LCM(int a, int b)
        {
            int gcd = GCD(a, b);
            return (a * b) / gcd;
        }
        #endregion
    }
}