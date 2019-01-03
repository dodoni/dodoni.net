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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Represents the implementation of the inverse (complementary) error function based on a "nested derivatives" approach for an initial guess and Halley's Iteration.
    /// </summary>
    /// <remarks>This implementation is based on "Nested Derivatives: A Simple method for computing series expansions of inverse functions", D. Dominici, February 2008.</remarks>
    public class InverseErrorFunctionNestedDerivativesAlgorithm
    {
        #region private members

        /// <summary>The coefficients A_n * ( \sqrt{PI}/2 )^{2n +1} / (2n+1)! used for the calculation of the initial value of InvErf(x) [see Example 4.9 in the reference].
        /// </summary>
        /// <remarks>The coefficients have been calculated with Sage (http://sagemath.com).</remarks>
        private double[] m_CoefficientsInitialGuessInvErf = new double[]{
            0.8862269254527580136490837416705725914,
            0.23201366653465449355353408258828482092,
            0.12755617530559795825399974141573007964,
            0.08655212924154753372964179262906019467,
            0.0649596177453854133820146686203173992,
            0.05173128198461637411263188588995263494,
            0.042836720651797349844651488262505239778748614494577,
            0.036465929308531626325579767243837255197189652833816,
            0.031689005021605446809609468464282901899488416427716,
            0.027980632964995224733430667242687604018208353124197,
            0.025022275841198349457169309105600322973899140592551,
            0.022609863318897574432816586571825165781991413545509,
            0.020606780379059001718768799653504673904926580322217,
            0.018918217250778854463498776390977373648034147062102,
            0.017476370562856546190429615204363584668515222571904,
            0.016231500987685251275294964378105742488956128532596,
            0.015146315063247805520385389047805864944448606003764,
            0.014192316002509964151153600056745641681784598884799
        };
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="InverseErrorFunctionNestedDerivativesAlgorithm" /> class.
        /// </summary>
        internal InverseErrorFunctionNestedDerivativesAlgorithm()
            : this(ErrorFunction.CoarseAlgorithm)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InverseErrorFunctionNestedDerivativesAlgorithm" /> class.
        /// </summary>
        /// <param name="errorFunctionAlgorithm">The algorithm for the calculation of the (complementary) error function.</param>
        /// <param name="maxNumberOfIterations">The maximal number of iterations.</param>
        internal InverseErrorFunctionNestedDerivativesAlgorithm(ErrorFunction errorFunctionAlgorithm, int maxNumberOfIterations = 25)
        {
            if (errorFunctionAlgorithm == null)
            {
                throw new ArgumentNullException("errorFunctionAlgorithm");
            }
            ErrorFunction = errorFunctionAlgorithm;

            if (maxNumberOfIterations < 1)
            {
                throw new ArgumentOutOfRangeException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "maximal Number Of Iterations", 1));
            }
            MaxNumberOfIterations = maxNumberOfIterations;
        }
        #endregion

        #region public properties

        /// <summary>Gets the maximal number of iterations in Halley's method.
        /// </summary>
        /// <value>The maximal number of iterations in Halley's method.</value>
        public int MaxNumberOfIterations
        {
            get;
            private set;
        }

        /// <summary>Gets the algorithm for (complementary) the error function.
        /// </summary>
        /// <value>The algorithm for the (complementary) error function.</value>
        public ErrorFunction ErrorFunction
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Gets the value of InvErf(x), i.e. the inverse error function.
        /// </summary>
        /// <param name="x">The argument, i.e. a value inside [0, 1[.</param>
        /// <returns>The estimated value for InvErf(x).</returns>
        /// <remarks>For approximal [0, 0.5] the initial guess has a deviation to the correct value up to 1.0E-16. For values near 1.0 the difference is about 1E-2. Even in this
        /// case Halley's method required not more than 5 Iteration for an accurate result.</remarks>
        public double GetValue(double x)
        {
            if (x == 1)
            {
                return Double.PositiveInfinity;
            }
            var z = GetInvErfInitialGuess(x);


            //// ist diese Fallunterscheidung zielführend und numerisch stabiler?

            //if (x <= 0.65)  // apply Halley's method directly to erf(z_n) - x
            //{
            //    // apply Halley's method (more specific Bailey's method) to erf(z_n) - x
            //    for (int k = 0; k < MaxNumberOfIterations; k++)
            //    {
            //        var erf = DoMath.PrimitiveIntegral.Erf(z);
            //        var exp = Math.Exp(-z * z);

            //        var diff = (erf - x) / (2.0 * MathConsts.OneOverSqrtPi * exp + z * (erf - x));
            //        z = z - diff;

            //        if (Math.Abs(diff) < MachineConsts.ExtremeTinyEpsilon * (2.0 + z))
            //        {
            //            return z;
            //        }
            //    }
            //}
            //else  // apply Halley's method to erfc(z_n) - (1-x) and use the fact InvErf(x) = InvErfc(1-x)
            //{
            //    double w = 1 - x;

            //  //  z = z-1;
            //    for (int k = 0; k < MaxNumberOfIterations; k++)
            //    {
            //        var erfc = DoMath.PrimitiveIntegral.Erfc(z);
            //        //var exp = Math.Exp(-z * z);
            //        var exp = ExpOfMinusXSquare(z);
            //        exp /= w;  // die Ableitung noch durch w teilen

            //        // das w kann man wieder kürzen, erhält dann aber das gleiche Resultat

            //        double h = (erfc - w) / w;  // warum -1.0 ?

            //        //var diff = (erfc - w) / (-2.0 * MathConsts.OneOverSqrtPi * exp + z * (erfc - w));
            //        var diff = h / (-2.0 * MathConsts.OneOverSqrtPi * exp  + z * h);
            //        z = z - diff;

            //        //if (Math.Abs(diff) < MachineConsts.ExtremeTinyEpsilon * (2.0 + z))
            //        //{
            //        //    return z;
            //        //}
            //    }

            //}
            return z;
        }

        public double GetCValue(double x)
        {
            return GetValue(1.0 - x);
            throw new NotImplementedException();  // InvErfc(1-x) = InvErf(x)
        }
        #endregion

        // diese Methode stammt aus Coarse Algorithm  und sollte allgemein zugänglich gemacht werden!

        /// <summary>Contains the values e^{k^2} for k = 0,1,2,... up to machine precision
        /// </summary>
        private double[] m_PowerOfE2 ={
               1,                          //  e^(-0^2)
               0.367879441171442321,       // e^(-1^2)
               1.83156388887341802e-2,     //  e^(-2^2)
               1.23409804086679549e-4,   
               1.12535174719259114e-7,
               1.38879438649640205e-11,
               2.319522830243569388e-16,
               5.24288566336346393e-22,
               1.60381089054863785e-28,
               6.63967719958073440e-36,
               3.72007597602083596e-44,
               2.82077008846013540e-53,
               2.89464031164830028e-63,
               4.02006021574335524e-74,
               7.55581901971196035e-86,
               1.92194772782384906e-98,
               6.61626105670948526e-112,
               3.08244069694909841e-126,
               1.94351485004929273e-141,
               1.65841047768114514e-157,
               1.91516959671400569e-174,
               2.99318445226019273e-192,
               6.33097733621059136e-211,
               1.81225402579399232e-230,
               7.02066779850473471e-251,
               3.68085585480180060e-272,
               2.61174176128405547e-294,   // e^(-26^2)
               4.62639185846956420e-298};  // e^(-27^2)*2^64                                

        /// <summary>Gets exp(-x*x) with low relative error.
        /// </summary>
        /// <param name="x">The argument x.</param>
        /// <returns>The value of exp(-x*x) with low relative error.</returns>
        private double ExpOfMinusXSquare(double x)
        {
            if (Math.Sign(x) < 0)
            {
                x = -x;
            }
            int z = (int)Math.Truncate(x);
            var m = x - z;

            if (m > 0.5)
            {
                z++;
                m--;
            }
            var t = m_PowerOfE2[z] * Math.Exp(-(z + z) * m) * Math.Exp(-m * m);
            if (z >= 27)
            {
                return t * Math.Pow(2, -64); // In the XSC-implementation a rounding to the nearest 'grid number' was used, but .net does not (directly) supported the access to mantisse and exponent component of a real number
            }
            return t;
        }





        #region private methods

        /// <summary>Gets an initial value for InvErf(x).
        /// </summary>
        /// <param name="x">The argument, i.e. inside [0, 1].</param>
        /// <returns>The initial guess for InvErf(x).</returns>
        private double GetInvErfInitialGuess(double x)
        {
            /* for values near 1.0 we use the approximation of InvErf(x) with respect to formula (13) in 
             *  "On the Calculation of the Inverse of the Error Function", A. J. Strecok, 1966.
             *  This reduces the number of Iteration in Halley's method. */
            if (x >= 0.999)
            {
                return Math.Pow(-Math.Log((1 - x) * (1 + x)), 0.5);
            }
            var value = 0.0;

            double powerOfX = x;
            for (int n = 0; n < m_CoefficientsInitialGuessInvErf.Length; n++)
            {
                value += powerOfX * m_CoefficientsInitialGuessInvErf[n];
                powerOfX *= x * x;
            }
            return value;
        }
        #endregion
    }
}