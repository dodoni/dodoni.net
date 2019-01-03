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
    /// <summary>Contains various mathematical constants in some <see cref="System.Double"/> representations.
    /// </summary>
    public static class MathConsts
    {
        /// <summary>Represents the base for the natural logarithm, e.
        /// </summary>
        public const double E = 2.71828182845904523536028747135266249;

        /// <summary>Represents the natural logarithm of 10.
        /// </summary>
        public const double Log10 = 2.302585092994045684017991454684364;

        /// <summary>Represents the natural logarithm of 17.
        /// </summary>
        public const double Log17 = 2.8332133440562160802495346178731;

        /// <summary>Represents the natural logarithm of 2.
        /// </summary>
        public const double Log2 = 0.6931471805599453094172321214581765681;

        /// <summary>Represents the natural logarithm of 3.
        /// </summary>
        public const double Log3 = 1.0986122886681096913952452369225;

        /// <summary>Represents the natural logarithm of <see cref="System.Math.PI"/>.</summary>
        public const double LogPi = 1.1447298858494001741434273513530587116472948129153d;

        /// <summary>Represents the natural logarithm of two times <see cref="System.Math.PI"/>.
        /// </summary>
        public const double LogTwoPi = 1.837877066409345483560659472811235279723;

        /// <summary>Represents the natural logarithm of two times the square-root of <see cref="System.Math.E"/> over <see cref="System.Math.PI"/>.</summary>
        public const double LogTwoSqrtEOverPi = 0.6207822376352452223455184457816472122518527279025978;

        /// <summary>Represents the natural logarithm of the square-root of two <see cref="System.Math.PI"/>.
        /// </summary>
        public const double LogSqrtTwoPi = 0.9189385332046727418;

        /// <summary>Represents the inverse of the square root of <see cref="System.Math.PI"/>.
        /// </summary>
        public const double OneOverSqrtPi = 0.56418958354775628694807945156082;

        /// <summary>Represents one over <c>3</c>.
        /// </summary>
        public const double OneOverThree = 0.33333333333333333333333333333333333;

        /// <summary>Represents the inverse of the square root of two times <see cref="System.Math.PI"/>.
        /// </summary>
        public const double OneOverSqrtTwoPi = 0.39894228040143267793994605993439;

        /// <summary>Represents 0.25 times <see cref="System.Math.PI"/>.
        /// </summary>
        public const double PiOverFour = 0.785398163397448309615660845819876;

        /// <summary>Represents 0.5 times <see cref="System.Math.PI"/>.
        /// </summary>
        public const double PiOverTwo = 1.570796326794896619231321691639751;

        /// <summary>Represents <see cref="System.Math.PI"/> squared.
        /// </summary>
        public const double PiSquared = 9.869604401089358618834490999873;

        /// <summary>Represents the square root of 17.
        /// </summary>
        public const double Sqrt17 = 4.1231056256176605498214098559741;

        /// <summary>Represents the square root of 2.
        /// </summary>
        public const double Sqrt2 = 1.414213562373095048801688724209698;

        /// <summary>Represents the square root of 3.
        /// </summary>
        public const double Sqrt3 = 1.732050807568877293527446341505872;

        /// <summary>Represents the square root of 5.
        /// </summary>
        public const double Sqrt5 = 2.236067977499789696409173668731276;

        /// <summary>Represents the square root of 7.
        /// </summary>
        public const double Sqrt7 = 2.645751311064590590501615753639260;

        /// <summary>Represents the square root of <see cref="System.Math.PI"/>.
        /// </summary>
        public const double SqrtPi = 1.772453850905516027298167483341145;

        /// <summary>Represents the square root of two times <see cref="System.Math.PI"/>.
        /// </summary>
        public const double SqrtTwoPi = 2.506628274631000502415765284811045;

        /// <summary>Represents the square root of <c>2.0/3.0</c>.
        /// </summary>
        public const double SqrtTwoOverThree = 0.816496580927726;

        /// <summary>Represents the square root of 0.5 * <see cref="System.Math.PI"/>.
        /// </summary>
        public const double SqrtPiOverTwo = 1.2533141373155002512078826424052;

        /// <summary>Represent one over the square root of 5.
        /// </summary>
        public const double OneOverSqrt5 = 0.44721359549995793;

        /// <summary>Represents one over ln(10).
        /// </summary>
        public const double OneOverLn10 = 0.43429448190325176;

        /// <summary>Represents one over <see cref="System.Math.E"/>.
        /// </summary>
        public const double OneOverE = 1.0 / Math.E;

        /// <summary>Represents two times <see cref="System.Math.PI"/>.
        /// </summary>
        public const double TwoPi = 6.283185307179586476925286766558;

        /// <summary>Represents two times the square-root of <see cref="System.Math.E"/> over <see cref="System.Math.PI"/>.</summary>
        public const double TwoSqrtEOverPi = 1.8603827342052657173362492472666631120594218414085755;

        /// <summary>Represents <see cref="System.Math.PI"/> to the power of -0.25 = -1/4.
        /// </summary>
        public const double PiToMinusOneOverFour = 0.7511255444649424828587030047762276930523650667560542957663902323579491645976658005519911356954776315;

        /// <summary>Represents the golden ratio which is equal to (1 + sqrt(5))/2 = 1.618033...
        /// </summary>
        public const double GoldenRatio = 1.61803398874989484820458683436563811772;

        /// <summary>Represents the golden ratio minus one which is equal to (1 + sqrt(5))/2 - 1 = (-1 + sqrt(5))/2 = 0.618033...
        /// </summary>
        public const double GoldenRatioMinusOne = GoldenRatio - 1.0;

        /// <summary>Represents two minus the golden ratio wich is equal to 2-(1 + sqrt(5))/2 = (3 - sqrt(5))/2 = 0.38196601... 
        /// </summary>
        public const double TwoMinusGoldenRatio = 2.0 - GoldenRatio;

        /// <summary>Represents two to the power of 32, i.e. 2^32 as <see cref="System.Double"/> instance.
        /// </summary>
        public const double TwoToThePowerOf32 = 4294967296.0;
    }
}