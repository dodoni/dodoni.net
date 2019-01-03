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

using NUnit.Framework;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public abstract partial class PrimitiveIntegralSpecialFunctionsTests
    {
        /// <summary>A test function for erf(x).
        /// </summary>
        /// <param name="x">The argument of erf(x).</param>
        /// <param name="expected">The expected value of erf(x).</param>
        /// <remarks>The benchmark values are taken from http://functions.wolfram.com/webMathematica </remarks>
        [TestCase(-1.0, -0.84270079294971486934122063508260925929606699796630291)]
        [TestCase(0.0, 0.0)]
        [TestCase(1E-16, 1.1283791670955125738961589031215414104242109402828E-16)]
        [TestCase(1e-15, 0.0000000000000011283791670955126615773132947717431253912942469337536)]
        [TestCase(0.1, 0.1124629160182848984047122510143040617233925185058162)]
        [TestCase(0.2, 0.22270258921047846617645303120925671669511570710081967)]
        [TestCase(0.3, 0.32862675945912741618961798531820303325847175931290341)]
        [TestCase(0.4, 0.42839235504666847645410962730772853743532927705981257)]
        [TestCase(0.5, 0.5204998778130465376827466538919645287364515757579637)]
        [TestCase(1.0, 0.84270079294971486934122063508260925929606699796630291)]
        [TestCase(1.25, 0.92290012825645823013652348119728114042360143870223)]
        [TestCase(1.5, 0.96610514647531072706697626164594785868141047925763678)]
        [TestCase(2.0, 0.99532226501895273416206925636725292861089179704006008)]
        [TestCase(2.5, 0.99959304798255504106043578426002508727965132259628658)]
        [TestCase(3.0, 0.99997790950300141455862722387041767962015229291260075)]
        [TestCase(4.0, 0.99999998458274209971998114784032651311595142785474641)]
        [TestCase(5.0, 0.99999999999846254020557196514981165651461662110988195)]
        [TestCase(6.0, 0.99999999999999997848026328750108688340664960081261537)]
        [TestCase(6.01, 0.9999999999999999809465442097035401034463664973157)]
        [TestCase(6.13, 0.9999999999999999956456211315721088051919271148645)]
        [TestCase(6.52, 0.9999999999999999999704757158540424448184694016319)]
        [TestCase(7.01, 0.9999999999999999999999636820794627402388226152807)]
        [TestCase(Double.PositiveInfinity, 1.0)]
        [TestCase(Double.NegativeInfinity, -1.0)]
        [TestCase(Double.NaN, Double.NaN)]
        public void Erf_TestArgument_BenchmarkResult(double x, double expected)
        {
            var specialFunctions = GetTestObject();
            var actual = specialFunctions.Erf(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-13).Percent, String.Format("x={0}; erf(x)={1} is not equal to expected value {2}!", x, actual, expected));
        }

        /// <summary>A test function for erfc(x).
        /// </summary>
        /// <param name="x">The argument of erfc(x).</param>
        /// <param name="expected">The expected value of erfc(x).</param>
        /// <remarks>The benchmark values are taken from http://functions.wolfram.com/webMathematica </remarks>
        [TestCase(-1.0, 1.8427007929497148693412206350826092592960669979663028)]
        [TestCase(0.0, 1.0)]
        [TestCase(0.1, 0.88753708398171510159528774898569593827660748149418343)]
        [TestCase(0.2, 0.77729741078952153382354696879074328330488429289918085)]
        [TestCase(0.3, 0.67137324054087258381038201468179696674152824068709621)]
        [TestCase(0.4, 0.57160764495333152354589037269227146256467072294018715)]
        [TestCase(0.5, 0.47950012218695346231725334610803547126354842424203654)]
        [TestCase(1.0, 0.15729920705028513065877936491739074070393300203369719)]
        [TestCase(1.5, 0.033894853524689272933023738354052141318589520742363247)]
        [TestCase(2.0, 0.0046777349810472658379307436327470713891082029599399245)]
        [TestCase(2.5, 0.00040695201744495893956421573997491272034867740371342016)]
        [TestCase(3.0, 0.00002209049699858544137277612958232037984770708739924966)]
        [TestCase(4.0, 0.000000015417257900280018852159673486884048572145253589191167)]
        [TestCase(5.0, 0.0000000000015374597944280348501883434853833788901180503147233804)]
        [TestCase(6.0, 2.1519736712498913116593350399187384630477514061688559e-17)]
        [TestCase(10.0, 2.0884875837625447570007862949577886115608181193211634e-45)]
        [TestCase(15.0, 7.2129941724512066665650665586929271099340909298253858e-100)]
        [TestCase(20.0, 5.3958656116079009289349991679053456040882726709236071e-176)]
        [TestCase(30.0, 2.5646562037561116000333972775014471465488897227786155e-393)]
        [TestCase(50.0, 2.0709207788416560484484478751657887929322509209953988e-1088)]
        [TestCase(80.0, 2.3100265595063985852034904366341042118385080919280966e-2782)]
        [TestCase(Double.PositiveInfinity, 0.0)]
        [TestCase(Double.NegativeInfinity, 2.0)]
        [TestCase(Double.NaN, Double.NaN)]
        public void Erfc_TestArgument_BenchmarkResult(double x, double expected)
        {
            var specialFunctions = GetTestObject();
            var actual = specialFunctions.Erfc(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-13).Percent, String.Format("x={0}; erfc(x)={1} is not equal to expected value {2}!", x, actual, expected));
        }

        ///// <summary>A test function for erfce(x).
        ///// </summary>
        ///// <param name="x">The argument of erfce(x).</param>
        ///// <param name="expected">The expected value of erfce(x).</param>
        //public void GetExponentialScaledCValue_Argument_BenchmarkResult(double x, double expected)
        //{
        //    var codyAlgorithm = new ErrorFunctionCodyAlgorithm();
        //    var actual = codyAlgorithm.GetExponentialScaledCValue(x);

        //    Assert.That(actual, Is.EqualTo(expected).Within(1E-13).Percent, String.Format("x={0}; erfce(x)={1} is not equal to expected value {2}!", x, actual, expected));
        //}


        /// <summary>A test function for InvErf(x).
        /// </summary>
        /// <param name="x">The argument of InvErf(x).</param>
        /// <param name="expected">The expected value of InvErf(x).</param>
        /// <remarks>The benchmark values are taken from http://functions.wolfram.com/webMathematica </remarks>
        [TestCase(double.NaN, double.NaN)]
        [TestCase(0.0, 0.0)]
        [TestCase(1E-50, 8.8622692545275801364908374167057259139877472806119E-51)]
        [TestCase(1E-25, 8.8622692545275801364908374167057259139877472806120E-26)]
        [TestCase(1e-15, 8.8622692545275801364908374167080460506530938255475E-16)]
        [TestCase(0.1, 0.088855990494257687015737250567791777572052244333197)]
        [TestCase(0.2, 0.17914345462129167649274901662647187030390927701953)]
        [TestCase(0.3, 0.27246271472675435562195759858756581266755846463101)]
        [TestCase(0.4, 0.37080715859355792905824947752244913860430488316293)]
        [TestCase(0.5, 0.47693627620446987338141835364313055980896974905947)]
        [TestCase(0.9, 1.1630871536766740867262542605629475934779325500021)]
        [TestCase(1.0 - 1E-10, 4.5728249673894852787410436731406723099870217238688)]   // 1 - 1E-10  = 0.9999999999
        //[TestCase(1.0 - 1.0E-25, 7.4148420426459366847870247680130300319210663851057)]
        //[TestCase(1.0 - 1.0E-50, 10.592090169527365189021663925329799115594206455416)]
        //[TestCase(1.0 - 1.0E-100, 15.065574702592645704404610541368897995968009540407)]
        [TestCase(1.0, Double.PositiveInfinity)]
        public void InvErf_TestArgument_BenchmarkResult(double x, double expected)
        {
            var specialFunctions = GetTestObject();
            var actual = specialFunctions.InvErf(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6).Percent, String.Format("x={0:E20}; InvErfc(x)={1} is not equal to expected value {2}!", x, actual, expected));
        }

        /// <summary>A test function for InvErfc(x).
        /// </summary>
        /// <param name="x">The argument of InvErfc(x).</param>
        /// <param name="expected">The expected value of InvErfc(x).</param>
        /// <remarks>The benchmark values are taken from http://functions.wolfram.com/webMathematica </remarks>
        [TestCase(0.0, double.PositiveInfinity)]
        [TestCase(1e-100, 15.065574702592645704404610541368897995968009540407)]
        [TestCase(1E-99, 14.989130087739213052813288208984209500645098174465)]
        [TestCase(1E-50, 10.592090169527365189021663925329799115594206455417)]
        [TestCase(1e-30, 8.1486162231698646073845666606481005383128634868542)]
        [TestCase(1E-28, 7.8631987627881460765103084577660807783305129271263)]
        [TestCase(1E-22, 6.9381083753289986173561653711149718074367012596187)]
        [TestCase(1e-20, 6.6015806223551425656238419307518653973860819532614)]
        [TestCase(1E-13, 5.2615123688647851038359223408126729480540549170055)]
        [TestCase(1e-10, 4.5728249585449249378479309946884581365517663258840893)]
        [TestCase(1E-8, 4.0522372438713892052307738897049963633528359357532)]
        [TestCase(1e-5, 3.1234132743415708640270717579666062107939039971365252)]
        [TestCase(0.1, 1.1630871536766741628440954340547000483801487126688552)]
        [TestCase(0.2, 0.90619380243682330953597079527631536107443494091638384)]
        [TestCase(0.5, 0.47693627620446987338141835364313055980896974905947083)]
        [TestCase(0.999999, 8.8622692545299002731561852372030143116587339612346E-7)]
        [TestCase(1.0, 0.0)]
        [TestCase(1.5, -0.47693627620446987338141835364313055980896974905947083)]
        [TestCase(1.9999999, -3.7665625815708380737812488872178302925928485872071)]
        [TestCase(2.0, double.NegativeInfinity)]
        public void InvErfc_TestArgument_BenchmarkResult(double x, double expected)
        {
            var specialFunctions = GetTestObject();
            var actual = specialFunctions.InvErfc(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-13).Percent, String.Format("x={0}; InvErf(x)={1} is not equal to expected value {2}!", x, actual, expected));
        }
    }
}