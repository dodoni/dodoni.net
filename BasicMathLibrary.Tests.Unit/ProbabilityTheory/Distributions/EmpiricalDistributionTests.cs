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
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

namespace Dodoni.MathLibrary.ProbabilityTheory.Distributions
{
    /// <summary>Serves as unit test class for <see cref="EmpiricalDistribution"/>.
    /// </summary>
    /// <remarks>Microsoft Excel has been used to calculate benchmark values.</remarks>
    public class EmpiricalDistributionTests
    {
        #region private members

        /// <summary>Sample data (1.000 values).
        /// </summary>
        private static double[] m_SampleData = new[]{
                 0.910092791,
                 0.812833636,
                 0.112801054,
                 0.860301368,
                 0.570536061,
                 0.562496489,
                 0.831006528,
                 0.288677619,
                 0.020518653,
                 0.568313039,
                 0.538112037,
                 0.07523679,
                 0.932673923,
                 0.631669204,
                 0.140420745,
                 0.894716012,
                 0.075256541,
                 0.488257293,
                 0.473571078,
                 0.431757422,
                 0.075685298,
                 0.755117628,
                 0.46174792,
                 0.544219291,
                 0.723250663,
                 0.538952446,
                 0.278581993,
                 0.345704158,
                 0.613743251,
                 0.014054077,
                 0.895110874,
                 0.138067537,
                 0.384418686,
                 0.594670683,
                 0.494663339,
                 0.773044263,
                 0.56688269,
                 0.440660938,
                 0.538426876,
                 0.130315927,
                 0.735488758,
                 0.544762121,
                 0.149870875,
                 0.258167543,
                 0.725428397,
                 0.648468097,
                 0.181283614,
                 0.75055416,
                 0.44635641,
                 0.753040351,
                 0.512048566,
                 0.694380458,
                 0.989113091,
                 0.274758247,
                 0.688444648,
                 0.895286429,
                 0.51418802,
                 0.197168269,
                 0.635234309,
                 0.062888124,
                 0.447109988,
                 0.496118267,
                 0.539686474,
                 0.837703108,
                 0.671011971,
                 0.123832995,
                 0.393584233,
                 0.769971995,
                 0.039611083,
                 0.668830735,
                 0.918783442,
                 0.461478972,
                 0.881626175,
                 0.455727511,
                 0.012818792,
                 0.054353126,
                 0.273655256,
                 0.580708406,
                 0.277527354,
                 0.318995206,
                 0.488809078,
                 0.671908749,
                 0.223501215,
                 0.453372832,
                 0.377373548,
                 0.537384669,
                 0.386879829,
                 0.961815362,
                 0.477359353,
                 0.043988836,
                 0.014564552,
                 0.312013424,
                 0.644085715,
                 0.76499965,
                 0.965857705,
                 0.558372908,
                 0.440162547,
                 0.909587981,
                 0.917982227,
                 0.545844079,
                 0.66299846,
                 0.315187118,
                 0.872729624,
                 0.054175819,
                 0.887683598,
                 0.533221495,
                 0.637060637,
                 0.817729989,
                 0.810475887,
                 0.036235158,
                 0.73247813,
                 0.379131391,
                 0.927713555,
                 0.941385218,
                 0.568950696,
                 0.099941206,
                 0.955637122,
                 0.96170462,
                 0.824671987,
                 0.184211019,
                 0.465743746,
                 0.324188253,
                 0.261156184,
                 0.304955514,
                 0.314239323,
                 0.278427495,
                 0.420877793,
                 0.807631398,
                 0.129028485,
                 0.290879979,
                 0.525948806,
                 0.127401665,
                 0.266482053,
                 0.329358615,
                 0.670299614,
                 0.137118638,
                 0.031402804,
                 0.477732438,
                 0.47775198,
                 0.496263768,
                 0.258158054,
                 0.382451777,
                 0.332958513,
                 0.350548161,
                 0.135283936,
                 0.247464756,
                 0.312844301,
                 0.122435368,
                 0.125972628,
                 0.233993519,
                 0.347399348,
                 0.561770258,
                 0.640895321,
                 0.517694276,
                 0.841327196,
                 0.070917883,
                 0.710169286,
                 0.371918347,
                 0.117415547,
                 0.638595141,
                 0.035325796,
                 0.417652634,
                 0.943939628,
                 0.998724543,
                 0.218278046,
                 0.043470988,
                 0.611955559,
                 0.173307626,
                 0.289456657,
                 0.918829108,
                 0.019995932,
                 0.239330823,
                 0.227189557,
                 0.33902774,
                 0.257586574,
                 0.712104981,
                 0.298299762,
                 0.723582017,
                 0.592118905,
                 0.709808762,
                 0.639367922,
                 0.668743155,
                 0.305868229,
                 0.347423183,
                 0.981621311,
                 0.38231442,
                 0.705266801,
                 0.454592167,
                 0.946894377,
                 0.908068301,
                 0.337141743,
                 0.561689006,
                 0.766834989,
                 0.7873688,
                 0.662453047,
                 0.234222948,
                 0.978494644,
                 0.253347084,
                 0.075949966,
                 0.396328047,
                 0.399679029,
                 0.119555478,
                 0.840300656,
                 0.783851842,
                 0.345312336,
                 0.16303025,
                 0.776631551,
                 0.976531475,
                 0.448069374,
                 0.837928138,
                 0.130376409,
                 0.071471176,
                 0.769472969,
                 0.444338104,
                 0.805747018,
                 0.193036748,
                 0.545109385,
                 0.896197134,
                 0.960094417,
                 0.010144618,
                 0.882739509,
                 0.119729623,
                 0.719964976,
                 0.642228556,
                 0.893729155,
                 0.112183482,
                 0.038352888,
                 0.784141915,
                 0.030703406,
                 0.106168817,
                 0.089089536,
                 0.058913417,
                 0.218135828,
                 0.029143662,
                 0.966693353,
                 0.27029706,
                 0.955972986,
                 0.079683301,
                 0.906757133,
                 0.363576532,
                 0.28642858,
                 0.971266137,
                 0.721730354,
                 0.181982962,
                 0.645497689,
                 0.983026409,
                 0.234952779,
                 0.253542461,
                 0.683328873,
                 0.697058214,
                 0.457939332,
                 0.631289635,
                 0.441487826,
                 0.322011961,
                 0.365682816,
                 0.515159682,
                 0.9147031,
                 0.72814595,
                 0.168978709,
                 0.763216703,
                 0.041984706,
                 0.159415205,
                 0.251680147,
                 0.251438992,
                 0.398928833,
                 0.353330037,
                 0.429689764,
                 0.225912429,
                 0.47839173,
                 0.967449852,
                 0.969652938,
                 0.416683346,
                 0.484831988,
                 0.572596633,
                 0.012660803,
                 0.446036169,
                 0.758978655,
                 0.066942093,
                 0.278093199,
                 0.425548821,
                 0.523954152,
                 0.242028572,
                 0.959509607,
                 0.551675468,
                 0.451902737,
                 0.997484016,
                 0.861808057,
                 0.338795322,
                 0.785143794,
                 0.205871324,
                 0.894030305,
                 0.825246039,
                 0.943939501,
                 0.169057871,
                 0.050036237,
                 0.374566054,
                 0.610864223,
                 0.731018962,
                 0.0008025,
                 0.789174325,
                 0.76062067,
                 0.619251144,
                 0.959220512,
                 0.756352667,
                 0.110924962,
                 0.818461286,
                 0.069854821,
                 0.463039787,
                 0.776760512,
                 0.393929921,
                 0.864388717,
                 0.028336494,
                 0.881104996,
                 0.892029294,
                 0.402650622,
                 0.595570779,
                 0.538247923,
                 0.20752068,
                 0.458127909,
                 0.47937911,
                 0.333677232,
                 0.300054323,
                 0.485260049,
                 0.683700643,
                 0.250999323,
                 0.114049727,
                 0.375943054,
                 0.957411446,
                 0.524138121,
                 0.997749423,
                 0.780957943,
                 0.814969459,
                 0.696190747,
                 0.14030825,
                 0.507024213,
                 0.36812447,
                 0.06746133,
                 0.593826678,
                 0.484511281,
                 0.182797722,
                 0.897014928,
                 0.811737505,
                 0.130554989,
                 0.535142517,
                 0.00181196,
                 0.53030304,
                 0.979844889,
                 0.435743642,
                 0.703597945,
                 0.815232895,
                 0.935668788,
                 0.941532297,
                 0.905122377,
                 0.61539132,
                 0.198340197,
                 0.022477654,
                 0.777651738,
                 0.252168286,
                 0.422078401,
                 0.077220755,
                 0.243454947,
                 0.921990764,
                 0.416978286,
                 0.809233032,
                 0.16805447,
                 0.746448583,
                 0.467223847,
                 0.035599426,
                 0.381035127,
                 0.739621925,
                 0.113300269,
                 0.109005101,
                 0.444838749,
                 0.571996146,
                 0.389905009,
                 0.295633515,
                 0.942833592,
                 0.092677354,
                 0.965478913,
                 0.332863322,
                 0.169525694,
                 0.103522249,
                 0.730575816,
                 0.344053627,
                 0.305072869,
                 0.984474809,
                 0.851526503,
                 0.223884064,
                 0.696655759,
                 0.404008906,
                 0.12167558,
                 0.735998832,
                 0.751157862,
                 0.390530111,
                 0.814385485,
                 0.867329762,
                 0.420480776,
                 0.409994651,
                 0.758642574,
                 0.467516145,
                 0.388252994,
                 0.107568413,
                 0.063379682,
                 0.870181516,
                 0.000348822,
                 0.211783498,
                 0.687028795,
                 0.6907528,
                 0.044557864,
                 0.674707017,
                 0.935369961,
                 0.001445808,
                 0.15793052,
                 0.505597758,
                 0.649139533,
                 0.282900518,
                 0.599071089,
                 0.184696678,
                 0.5089482,
                 0.544417375,
                 0.462145742,
                 0.457945138,
                 0.257227443,
                 0.559897968,
                 0.956671282,
                 0.432684402,
                 0.42773057,
                 0.101462002,
                 0.423254467,
                 0.983687231,
                 0.009671728,
                 0.745440168,
                 0.371785246,
                 0.380848449,
                 0.94053279,
                 0.606766832,
                 0.342880817,
                 0.586725624,
                 0.651521866,
                 0.805967297,
                 0.673157751,
                 0.168958972,
                 0.429056396,
                 0.05077576,
                 0.269182165,
                 0.557877721,
                 0.022494787,
                 0.573374857,
                 0.785041774,
                 0.285820521,
                 0.513058176,
                 0.514180783,
                 0.375748249,
                 0.731600654,
                 0.365454558,
                 0.782921495,
                 0.760958262,
                 0.722756001,
                 0.332670557,
                 0.009971797,
                 0.014512147,
                 0.21094364,
                 0.858381325,
                 0.047249325,
                 0.067133468,
                 0.990613512,
                 0.770411381,
                 0.167420988,
                 0.264466132,
                 0.105567082,
                 0.332540888,
                 0.049558233,
                 0.820347796,
                 0.034298861,
                 0.025352295,
                 0.937466685,
                 0.767752538,
                 0.416100859,
                 0.787963728,
                 0.07657436,
                 0.483117526,
                 0.098691132,
                 0.773943514,
                 0.960532846,
                 0.261068236,
                 0.765134216,
                 0.741831577,
                 0.493721448,
                 0.23267433,
                 0.251180324,
                 0.389062226,
                 0.353088474,
                 0.910382069,
                 0.509525871,
                 0.769288802,
                 0.189754243,
                 0.106339356,
                 0.378849618,
                 0.019296593,
                 0.935478648,
                 0.502026326,
                 0.205754924,
                 0.451115818,
                 0.171946297,
                 0.691049134,
                 0.11433126,
                 0.455445554,
                 0.015983151,
                 0.346394206,
                 0.245224513,
                 0.955160409,
                 0.518439108,
                 0.272627489,
                 0.143317039,
                 0.854041053,
                 0.343171929,
                 0.783938442,
                 0.252989388,
                 0.110783912,
                 0.899585447,
                 0.757831817,
                 0.074742867,
                 0.448734097,
                 0.762975004,
                 0.394706341,
                 0.060984772,
                 0.819164309,
                 0.475350593,
                 0.973672525,
                 0.290167271,
                 0.290493133,
                 0.636706301,
                 0.233211205,
                 0.834165193,
                 0.001885712,
                 0.874779598,
                 0.778712983,
                 0.548101963,
                 0.936597769,
                 0.049518932,
                 0.809414013,
                 0.083211716,
                 0.038477483,
                 0.58559948,
                 0.820948854,
                 0.319172678,
                 0.00391898,
                 0.681615988,
                 0.343125611,
                 0.385361678,
                 0.814566952,
                 0.730485445,
                 0.129193737,
                 0.860296568,
                 0.076284844,
                 0.40271625,
                 0.41634633,
                 0.234008057,
                 0.656628742,
                 0.001479872,
                 0.596725856,
                 0.459846431,
                 0.797235776,
                 0.177752549,
                 0.267596505,
                 0.576509577,
                 0.570908459,
                 0.083819143,
                 0.11454742,
                 0.791765982,
                 0.783225714,
                 0.54651632,
                 0.089651315,
                 0.299309478,
                 0.941781974,
                 0.134160801,
                 0.759704388,
                 0.334061319,
                 0.156887962,
                 0.209022415,
                 0.320451752,
                 0.716870093,
                 0.838174334,
                 0.557089784,
                 0.284083146,
                 0.998810624,
                 0.681223237,
                 0.464370021,
                 0.671535401,
                 0.272846288,
                 0.847920777,
                 0.141753421,
                 0.642485065,
                 0.007685474,
                 0.57867712,
                 0.209724049,
                 0.513212649,
                 0.065559147,
                 0.362230377,
                 0.062758399,
                 0.545066726,
                 0.133387593,
                 0.557174955,
                 0.121915664,
                 0.235671659,
                 0.191983743,
                 0.507322913,
                 0.879094488,
                 0.075357158,
                 0.344714409,
                 0.857304646,
                 0.625237367,
                 0.54965313,
                 0.476901309,
                 0.597023277,
                 0.196972728,
                 0.847020452,
                 0.299416863,
                 0.16824838,
                 0.422225668,
                 0.189195906,
                 0.290876648,
                 0.558603563,
                 0.997049468,
                 0.848335024,
                 0.228352623,
                 0.113253466,
                 0.609272091,
                 0.906969572,
                 0.765068384,
                 0.467621429,
                 0.947372337,
                 0.531895833,
                 0.104699316,
                 0.624046833,
                 0.904234406,
                 0.636908366,
                 0.297732941,
                 0.310359896,
                 0.048934655,
                 0.563924661,
                 0.951944129,
                 0.343993486,
                 0.065193414,
                 0.363866137,
                 0.627858294,
                 0.290596701,
                 0.310720638,
                 0.678238412,
                 0.788761946,
                 0.538825036,
                 0.15793107,
                 0.970096406,
                 0.087471744,
                 0.328052978,
                 0.711370621,
                 0.192854305,
                 0.215888336,
                 0.789174805,
                 0.482836201,
                 0.640165754,
                 0.911720842,
                 0.149549543,
                 0.86918985,
                 0.235248446,
                 0.844912962,
                 0.598452906,
                 0.842906692,
                 0.34609095,
                 0.620914166,
                 0.688664371,
                 0.07454831,
                 0.51351879,
                 0.378908308,
                 0.452802545,
                 0.671833235,
                 0.537670497,
                 0.832203173,
                 0.898289308,
                 0.929702231,
                 0.171255659,
                 0.326714187,
                 0.548817754,
                 0.112105283,
                 0.432766315,
                 0.772303671,
                 0.966667763,
                 0.274303197,
                 0.841650952,
                 0.313910817,
                 0.170113011,
                 0.759673344,
                 0.943798281,
                 0.737782259,
                 0.172223325,
                 0.779042628,
                 0.775578464,
                 0.329717127,
                 0.910468314,
                 0.249673646,
                 0.127935337,
                 0.818806759,
                 0.103052307,
                 0.092369356,
                 0.800978626,
                 0.772288991,
                 0.033831901,
                 0.261897102,
                 0.638834427,
                 0.351188134,
                 0.891474302,
                 0.766019398,
                 0.503626674,
                 0.90605059,
                 0.331042854,
                 0.571379926,
                 0.127465145,
                 0.037850186,
                 0.420073428,
                 0.394951716,
                 0.186919159,
                 0.807132764,
                 0.415553469,
                 0.418393376,
                 0.664413525,
                 0.459619002,
                 0.269459563,
                 0.293130095,
                 0.577184936,
                 0.908893175,
                 0.747095719,
                 0.391912282,
                 0.110941824,
                 0.604640245,
                 0.000745788,
                 0.682858857,
                 0.166659889,
                 0.211819231,
                 0.868703523,
                 0.888936706,
                 0.826703279,
                 0.843222248,
                 0.726470046,
                 0.164317945,
                 0.863780612,
                 0.20683336,
                 0.574572207,
                 0.978503291,
                 0.614346268,
                 0.624916379,
                 0.017981661,
                 0.475579573,
                 0.861470963,
                 0.744880173,
                 0.132550756,
                 0.923912676,
                 0.469622643,
                 0.617735975,
                 0.30660111,
                 0.354598495,
                 0.788256521,
                 0.497096067,
                 0.343421417,
                 0.327682605,
                 0.298274875,
                 0.861759834,
                 0.467001489,
                 0.748831403,
                 0.375203038,
                 0.691963027,
                 0.798993924,
                 0.240874814,
                 0.444106301,
                 0.164641019,
                 0.090431277,
                 0.744467204,
                 0.776769818,
                 0.582003934,
                 0.715634887,
                 0.132694591,
                 0.117449291,
                 0.491269822,
                 0.835578218,
                 0.361305388,
                 0.726960014,
                 0.46056067,
                 0.321950195,
                 0.826620625,
                 0.601032758,
                 0.259239093,
                 0.91046897,
                 0.77481638,
                 0.946882735,
                 0.160738748,
                 0.890247807,
                 0.801645718,
                 0.242474692,
                 0.078072697,
                 0.96497839,
                 0.674396159,
                 0.024029996,
                 0.442847132,
                 0.998437929,
                 0.643406872,
                 0.416475466,
                 0.221243954,
                 0.7008558,
                 0.01785315,
                 0.310946789,
                 0.845697481,
                 0.897432405,
                 0.327764554,
                 0.825032906,
                 0.754535521,
                 0.681309547,
                 0.345673281,
                 0.926764091,
                 0.581106,
                 0.012674342,
                 0.703298969,
                 0.598772701,
                 0.945373448,
                 0.883566604,
                 0.362483772,
                 0.274354062,
                 0.46399197,
                 0.160490997,
                 0.224430819,
                 0.112357555,
                 0.952867012,
                 0.634635813,
                 0.111686799,
                 0.367020064,
                 0.557858189,
                 0.883295587,
                 0.660279699,
                 0.44497679,
                 0.011988034,
                 0.947814413,
                 0.581396121,
                 0.510308457,
                 0.978939363,
                 0.576663703,
                 0.067555045,
                 0.966916643,
                 0.165791721,
                 0.535968657,
                 0.469286948,
                 0.351621972,
                 0.5869076,
                 0.096888707,
                 0.400744845,
                 0.531895086,
                 0.298618029,
                 0.425208714,
                 0.281700739,
                 0.54974694,
                 0.228290579,
                 0.196865845,
                 0.821273111,
                 0.452893866,
                 0.237420015,
                 0.432739755,
                 0.393194366,
                 0.166011159,
                 0.3349004,
                 0.24552676,
                 0.537334698,
                 0.537711204,
                 0.364841488,
                 0.98216278,
                 0.908988782,
                 0.362786372,
                 0.150733503,
                 0.346585025,
                 0.379334173,
                 0.605603081,
                 0.560562931,
                 0.545805424,
                 0.180893527,
                 0.376790465,
                 0.485344046,
                 0.827553792,
                 0.162192574,
                 0.880394367,
                 0.282349987,
                 0.623633864,
                 0.364446626,
                 0.126165928,
                 0.236700695,
                 0.351436577,
                 0.972993925,
                 0.282954118,
                 0.031472749,
                 0.95939686,
                 0.850003143,
                 0.827049602,
                 0.055344111,
                 0.956197917,
                 0.710557242,
                 0.354243302,
                 0.734774961,
                 0.42062128,
                 0.646995483,
                 0.425428241,
                 0.112411631,
                 0.045165027,
                 0.305667493,
                 0.928129191,
                 0.108153393,
                 0.078387289,
                 0.639387934,
                 0.335046242,
                 0.43181037,
                 0.831141874,
                 0.185042394,
                 0.768359768,
                 0.647549258,
                 0.544161124,
                 0.309466179,
                 0.941035719,
                 0.43788736,
                 0.28403273,
                 0.583922984,
                 0.307768509,
                 0.400534131,
                 0.720448075,
                 0.579945156,
                 0.040041949,
                 0.280017692,
                 0.85292748,
                 0.744544676,
                 0.70464322,
                 0.61381811,
                 0.532775313,
                 0.168294587,
                 0.995692634,
                 0.955954467,
                 0.935676474,
                 0.212210113,
                 0.855766197,
                 0.630369496,
                 0.321538136,
                 0.191676519,
                 0.294290919,
                 0.72379667,
                 0.293032316,
                 0.928632663,
                 0.775320745,
                 0.728548635,
                 0.359663141,
                 0.41912609,
                 0.515731221,
                 0.659027664,
                 0.875029653,
                 0.029897501,
                 0.378890134,
                 0.022745436,
                 0.663761575,
                 0.890773488,
                 0.374744781,
                 0.248993374,
                 0.416555861,
                 0.949548783,
                 0.898868925,
                 0.236350308,
                 0.618266735,
                 0.643095683,
                 0.910201016,
                 0.394325376,
                 0.774465871,
                 0.643381147,
                 0.082604239,
                 0.123556629,
                 0.917311489,
                 0.553751782,
                 0.890453211,
                 0.811978122,
                 0.023782092,
                 0.636193009,
                 0.732256199,
                 0.816131515,
                 0.968070832,
                 0.308559114,
                 0.553838911,
                 0.619647116,
                 0.07586198,
                 0.54504528,
                 0.589048888,
                 0.028318623,
                 0.972047948,
                 0.756309536,
                 0.298634898,
                 0.285005268,
                 0.386992452,
                 0.212886516,
                 0.958312429,
                 0.426229407,
                 0.336633121,
                 0.930455929
               };
        #endregion

        #region public methods

        /// <summary>A test function for the calculation of a specific empirical quantile w.r.t. a specific test case.
        /// </summary>
        /// <param name="probability">The probability to take into account.</param>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(EmpiricalQuantileTestCaseData))]
        public void Quantile_TestCaseData_BenchmarkResult(double probability, int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));

            double actual = empiricalDistribution.Quantile[probability];

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the quantile.
        /// </summary>
        /// <value>The kurtosis test case data.</value>
        public static IEnumerable<TestCaseData> EmpiricalQuantileTestCaseData
        {
            get
            {
                yield return new TestCaseData(0.0005, 1000, m_SampleData, 0, 1, 0.00054710652);
                yield return new TestCaseData(0.01, 1000, m_SampleData, 0, 1, 0.00996879611);
                yield return new TestCaseData(0.05, 1000, m_SampleData, 0, 1, 0.04396294390);
                yield return new TestCaseData(0.95, 1000, m_SampleData, 0, 1, 0.94966855023);
                yield return new TestCaseData(0.995, 1000, m_SampleData, 0, 1, 0.99705164116);


                yield return new TestCaseData(0.0005, 332, m_SampleData, 5, 3, 0.00041452016);
                yield return new TestCaseData(0.01, 332, m_SampleData, 5, 3, 0.00923202346);
                yield return new TestCaseData(0.05, 332, m_SampleData, 5, 3, 0.03910096312);
                yield return new TestCaseData(0.95, 332, m_SampleData, 5, 3, 0.93567224649);
                yield return new TestCaseData(0.995, 332, m_SampleData, 5, 3, 0.98786436442);

                yield return new TestCaseData(0.0005, 291, m_SampleData, 5, 3, 0.00040638238);
                yield return new TestCaseData(0.01, 291, m_SampleData, 5, 3, 0.00709812289);
                yield return new TestCaseData(0.05, 291, m_SampleData, 5, 3, 0.03904428312);
                yield return new TestCaseData(0.95, 291, m_SampleData, 5, 3, 0.93810078879);
                yield return new TestCaseData(0.995, 291, m_SampleData, 5, 3, 0.99073909163);
            }
        }

        /// <summary>A test function for the calculation of the kurtosis w.r.t. a specific test case.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(KurtosisTestCaseData))]
        public void Kurtosis_TestCaseData_BenchmarkResult(int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.Moment.Kurtosis;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the kurtosis.
        /// </summary>
        /// <value>The kurtosis test case data.</value>
        public static IEnumerable<TestCaseData> KurtosisTestCaseData
        {
            get
            {
                yield return new TestCaseData(1000, m_SampleData, 0, 1, -1.19029884586);
                yield return new TestCaseData(332, m_SampleData, 5, 3, -1.16924552189);
                yield return new TestCaseData(291, m_SampleData, 5, 3, -1.14231766212);
            }
        }

        /// <summary>A test function for the calculation of the maximum w.r.t. a specific test case.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(MaximumTestCaseData))]
        public void Maximum_TestCaseData_BenchmarkResult(int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.Maximum;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the maximum.
        /// </summary>
        /// <value>The maximum test case data.</value>
        public static IEnumerable<TestCaseData> MaximumTestCaseData
        {
            get
            {
                yield return new TestCaseData(1000, m_SampleData, 0, 1, 0.998810624);
                yield return new TestCaseData(332, m_SampleData, 5, 3, 0.99774942349);
                yield return new TestCaseData(291, m_SampleData, 5, 3, 0.99774942349);
            }
        }

        /// <summary>A test function for the calculation of the mean w.r.t. a specific test case.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(MeanTestCaseData))]
        public void Mean_TestCaseData_BenchmarkResult(int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.Moment.Expectation;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the mean.
        /// </summary>
        /// <value>The mean test case data.</value>
        public static IEnumerable<TestCaseData> MeanTestCaseData
        {
            get
            {
                yield return new TestCaseData(1000, m_SampleData, 0, 1, 0.491469224);
                yield return new TestCaseData(332, m_SampleData, 5, 3, 0.46887200118);
            }
        }

        /// <summary>A test function for the calculation of the median w.r.t. a specific test case.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(MedianTestCaseData))]
        public void Median_TestCaseData_BenchmarkResult(int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.Median;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the median.
        /// </summary>
        /// <value>The median test case data.</value>
        public static IEnumerable<TestCaseData> MedianTestCaseData
        {
            get
            {
                yield return new TestCaseData(1000, m_SampleData, 0, 1, 0.47446083553);
                yield return new TestCaseData(332, m_SampleData, 5, 3, 0.43118708314);
                yield return new TestCaseData(291, m_SampleData, 5, 3, 0.42325446709);
            }
        }

        /// <summary>A test function for the calculation of the minimum w.r.t. a specific test case.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(MinimumTestCaseData))]
        public void Minimum_TestCaseData_BenchmarkResult(int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.Minimum;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the minimum.
        /// </summary>
        /// <value>The minimum test case data.</value>
        public static IEnumerable<TestCaseData> MinimumTestCaseData
        {
            get
            {
                yield return new TestCaseData(1000, m_SampleData, 0, 1, 0.00034882243);
                yield return new TestCaseData(332, m_SampleData, 5, 3, 0.00034882243);
            }
        }

        /// <summary>A test function for the calculation of the normalized number of elements in a specific range w.r.t. a specific test case.
        /// </summary>
        /// <param name="lowerBound">The lower bound of the range.</param>
        /// <param name="upperBound">The upper bound of the range.</param>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(RangeTestCaseData))]
        public void GetRangeStatistics_TestCaseData_BenchmarkResult(double lowerBound, double upperBound, int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.GetRangeStatistics(lowerBound, upperBound);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the number of elements in a specific range.
        /// </summary>
        /// <value>The range test case data.</value>
        public static IEnumerable<TestCaseData> RangeTestCaseData
        {
            get
            {
                yield return new TestCaseData(0.0, 1.0, 1000, m_SampleData, 0, 1, 1.0);
                yield return new TestCaseData(0.05, 0.8, 1000, m_SampleData, 0, 1, 748 / 1000.0);
                yield return new TestCaseData(0.175, 0.65, 1000, m_SampleData, 0, 1, 484 / 1000.0);


                yield return new TestCaseData(0.0, 1.0, 332, m_SampleData, 5, 3, 1.0);
                yield return new TestCaseData(0.05, 0.8, 332, m_SampleData, 5, 3, 256 / 332.0);
                yield return new TestCaseData(0.175, 0.65, 332, m_SampleData, 5, 3, 164 / 332.0);
            }
        }

        /// <summary>A test function for the calculation of the skewness w.r.t. a specific test case.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(SkewnessTestCaseData))]
        public void Skewness_TestCaseData_BenchmarkResult(int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.Moment.Skewness;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the skewness.
        /// </summary>
        /// <value>The skewness test case data.</value>
        public static IEnumerable<TestCaseData> SkewnessTestCaseData
        {
            get
            {
                yield return new TestCaseData(1000, m_SampleData, 0, 1, 0.05200434239);
                yield return new TestCaseData(332, m_SampleData, 5, 3, 0.13309300428);
                yield return new TestCaseData(291, m_SampleData, 5, 3, 0.17333979414);
            }
        }

        /// <summary>A test function for the calculation of the standard deviation w.r.t. a specific test case.
        /// </summary>
        /// <param name="n">The number of elements in <paramref name="x"/> to take into account.</param>
        /// <param name="x">The argument (sample), where <c>x[<paramref name="startIndex"/> + k * <paramref name="increment"/>]</c> for k=0,...,<paramref name="n"/>-1 are taken into account.</param>
        /// <param name="startIndex">The null-based start index (offset) for <paramref name="x"/>.</param>
        /// <param name="increment">The increment for <paramref name="x"/>.</param>
        /// <param name="expected">The expected value.</param>
        [TestCaseSource(nameof(StandardDeviationTestCaseData))]
        public void StandardDeviation_TestCaseData_BenchmarkResult(int n, double[] x, int startIndex, int increment, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(x.Skip(startIndex).Where((z, m) => (m == 0) || (m % increment == 0)).TakeWhile((z, m) => m < n));
            double actual = empiricalDistribution.Moment.StandardDeviation;

            Assert.That(actual, Is.EqualTo(expected).Within(1E-6));
        }

        /// <summary>Gets the test case data for the calculation of the standard deviation.
        /// </summary>
        /// <value>The standard deviation  test case data.</value>
        public static IEnumerable<TestCaseData> StandardDeviationTestCaseData
        {
            get
            {
                yield return new TestCaseData(1000, m_SampleData, 0, 1, 0.28971459906);
                yield return new TestCaseData(332, m_SampleData, 5, 3, 0.28603044026);
            }
        }

        /// <summary>A test function for the calculation of the cummulative distribution function.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="x">The argument for the calculation of the cummulative distribution function.</param>
        /// <param name="expected">The expected value of the cummulative distribution function at <paramref name="x"/>.</param>
        [TestCaseSource(nameof(GetCdfValueTestCaseData))]
        public void GetCdfValue_TestCaseData_BenchmarkResult(double[] sample, double x, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(sample);
            double actual = empiricalDistribution.GetCdfValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Gets the test case data for the calculation of the cummulative distribution function.
        /// </summary>
        /// <value>The test case data for the calculation of the cummulative distribution function.</value>
        public static IEnumerable<TestCaseData> GetCdfValueTestCaseData
        {
            get
            {
                /* values are calculated with Excel */
                yield return new TestCaseData(m_SampleData, 0.000348822, 0.001);
                yield return new TestCaseData(m_SampleData, 0.000348822, 0.001);
                yield return new TestCaseData(m_SampleData, 0.032557267, 0.038);
                yield return new TestCaseData(m_SampleData, 0.064765712, 0.067);
                yield return new TestCaseData(m_SampleData, 0.096974158, 0.1);
                yield return new TestCaseData(m_SampleData, 0.129182603, 0.141);
                yield return new TestCaseData(m_SampleData, 0.161391048, 0.165);
                yield return new TestCaseData(m_SampleData, 0.193599493, 0.2);
                yield return new TestCaseData(m_SampleData, 0.225807939, 0.222);
                yield return new TestCaseData(m_SampleData, 0.258016384, 0.257);
                yield return new TestCaseData(m_SampleData, 0.290224829, 0.292);
                yield return new TestCaseData(m_SampleData, 0.322433274, 0.33);
                yield return new TestCaseData(m_SampleData, 0.354641719, 0.373);
                yield return new TestCaseData(m_SampleData, 0.386850165, 0.406);
                yield return new TestCaseData(m_SampleData, 0.41905861, 0.437);
                yield return new TestCaseData(m_SampleData, 0.451267055, 0.473);
                yield return new TestCaseData(m_SampleData, 0.4834755, 0.51);
                yield return new TestCaseData(m_SampleData, 0.515683946, 0.537);
                yield return new TestCaseData(m_SampleData, 0.547892391, 0.57);
                yield return new TestCaseData(m_SampleData, 0.580100836, 0.604);
                yield return new TestCaseData(m_SampleData, 0.612309281, 0.629);
                yield return new TestCaseData(m_SampleData, 0.644517727, 0.664);
                yield return new TestCaseData(m_SampleData, 0.676726172, 0.687);
                yield return new TestCaseData(m_SampleData, 0.708934617, 0.709);
                yield return new TestCaseData(m_SampleData, 0.741143062, 0.739);
                yield return new TestCaseData(m_SampleData, 0.773351507, 0.777);
                yield return new TestCaseData(m_SampleData, 0.805559953, 0.807);
                yield return new TestCaseData(m_SampleData, 0.837768398, 0.842);
                yield return new TestCaseData(m_SampleData, 0.869976843, 0.871);
                yield return new TestCaseData(m_SampleData, 0.902185288, 0.9);
                yield return new TestCaseData(m_SampleData, 0.934393734, 0.929);
                yield return new TestCaseData(m_SampleData, 0.966602179, 0.97);

                // a simple benchmark, calculated with R
                var sample2 = new double[]{
                    0.720486887,
                    0.928154392,
                    1.311728638,
                    -0.311430237,
                    0.086620481,
                    -1.071620241,
                    1.824661519,
                    -0.835600084,
                    2.849126887,
                    -1.315829176,
                    -1.131318457,
                    0.01590839,
                    -0.335315233,
                    1.630871029,
                    1.052407562,
                    0.012433983,
                    -0.290857768,
                    0.140721067,
                    -1.627914542,
                    1.640033635,
                    1.825361161,
                    0.169950474,
                    3.084260513,
                    -0.774076731,
                    0.705925472,
                    -0.944602302,
                    2.513457814,
                    -0.741599539,
                    1.19000394,
                    0.21948804,
                    0.056124991,
                    -0.440227497,
                    1.100454744,
                    -0.749153422,
                    -2.264566575,
                    -0.052394162,
                    0.012484287,
                    -0.493953982,
                    -0.4464941,
                    -0.607556012,
                    -0.027845157,
                    0.634586701,
                    -0.505463377,
                    -0.03819564,
                    -0.261959175,
                    0.320003398,
                    -0.747551032,
                    -0.605316354,
                    0.512984171,
                    -0.193600946,
                    1.851675352,
                    -0.039832474,
                    0.397999714,
                    -1.328447769,
                    0.46218549,
                    -0.758612764,
                    -0.167630927,
                    0.658343528,
                    0.274253697,
                    1.199306143,
                    -0.093757905,
                    0.816365282,
                    0.562019812,
                    -0.550592847,
                    3.318783724,
                    0.479627758,
                    -2.407814538,
                    -0.547498121,
                    0.773467167,
                    -0.053354835,
                    -2.015518099,
                    0.157989915,
                    -0.234750301,
                    -0.048788195,
                    -1.395994029,
                    0.033990504,
                    -0.269634613,
                    0.531617907,
                    0.45909188,
                    2.053098012,
                    -2.990586901,
                    0.474540277,
                    -0.40376558,
                    -0.651317613,
                    0.654146712,
                    0.921068753,
                    0.006965487,
                    0.202977973,
                    0.017263269,
                    0.793783647,
                    0.052416213,
                    -0.13892392,
                    -0.877675585,
                    -0.226390163,
                    1.717537086,
                    0.061048576,
                    1.569114541,
                    0.045511928,
                    -0.963079925,
                    0.34321391};

                yield return new TestCaseData(sample2, -1.25, 0.08);
                yield return new TestCaseData(sample2, -1.1, 0.09);
                yield return new TestCaseData(sample2, -0.876, 0.13);
                yield return new TestCaseData(sample2, -0.25, 0.34);
                yield return new TestCaseData(sample2, 0, 0.46);
                yield return new TestCaseData(sample2, 0.657, 0.75);
                yield return new TestCaseData(sample2, 0.99181, 0.83);
                yield return new TestCaseData(sample2, 1.27, 0.87);
                yield return new TestCaseData(sample2, 2.18, 0.96);
                yield return new TestCaseData(sample2, 3.18, 0.99);
                yield return new TestCaseData(sample2, 3.3, 0.99);
                yield return new TestCaseData(sample2, 3.4, 1);
            }
        }

        /// <summary>A test function for the calculation of the probability density function.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="x">The argument for the calculation of the probability density function.</param>
        /// <param name="expected">The expected value of the probability density function at <paramref name="x"/>.</param>
        [TestCaseSource(nameof(GetPdfValueTestCaseData))]
        public void GetPdfValue_TestCaseData_BenchmarkResult(double[] sample, double x, double expected)
        {
            var empiricalDistribution = new EmpiricalDistribution(sample);
            double actual = empiricalDistribution.GetPdfValue(x);

            Assert.That(actual, Is.EqualTo(expected).Within(1E-9));
        }

        /// <summary>Gets the test case data for the calculation of the probability density function.
        /// </summary>
        /// <value>The test case data for the calculation of the probability density function.</value>
        public static IEnumerable<TestCaseData> GetPdfValueTestCaseData
        {
            get
            {
                /* values are calculated with Microsoft Excel 2007; because of rounding problems, the bins taken from Excel are adjusted by minus 1E-11:  */
                yield return new TestCaseData(m_SampleData, 0.0003488220000000, 0.001);
                yield return new TestCaseData(m_SampleData, 0.0325572672158064, 0.037);
                yield return new TestCaseData(m_SampleData, 0.0647657124416129, 0.029);
                yield return new TestCaseData(m_SampleData, 0.0969741576674193, 0.033);
                yield return new TestCaseData(m_SampleData, 0.1291826028932260, 0.041);
                yield return new TestCaseData(m_SampleData, 0.1613910481190320, 0.024);
                yield return new TestCaseData(m_SampleData, 0.1935994933448390, 0.035);
                yield return new TestCaseData(m_SampleData, 0.2258079385706450, 0.022);
                yield return new TestCaseData(m_SampleData, 0.2580163837964520, 0.035);
                yield return new TestCaseData(m_SampleData, 0.2902248290222580, 0.035);
                yield return new TestCaseData(m_SampleData, 0.3224332742480650, 0.038);
                yield return new TestCaseData(m_SampleData, 0.3546417194738710, 0.043);
                yield return new TestCaseData(m_SampleData, 0.3868501646996770, 0.033);
                yield return new TestCaseData(m_SampleData, 0.4190586099254840, 0.031);
                yield return new TestCaseData(m_SampleData, 0.4512670551512900, 0.036);
                yield return new TestCaseData(m_SampleData, 0.4834755003770970, 0.037);
                yield return new TestCaseData(m_SampleData, 0.5156839456029030, 0.027);
                yield return new TestCaseData(m_SampleData, 0.5478923908287100, 0.033);
                yield return new TestCaseData(m_SampleData, 0.5801008360545160, 0.034);
                yield return new TestCaseData(m_SampleData, 0.6123092812803230, 0.025);
                yield return new TestCaseData(m_SampleData, 0.6445177265061290, 0.035);
                yield return new TestCaseData(m_SampleData, 0.6767261717319350, 0.023);
                yield return new TestCaseData(m_SampleData, 0.7089346169577420, 0.022);
                yield return new TestCaseData(m_SampleData, 0.7411430621835480, 0.03);
                yield return new TestCaseData(m_SampleData, 0.7733515074093550, 0.038);
                yield return new TestCaseData(m_SampleData, 0.8055599526351610, 0.03);
                yield return new TestCaseData(m_SampleData, 0.8377683978609680, 0.035);
                yield return new TestCaseData(m_SampleData, 0.8699768430867740, 0.029);
                yield return new TestCaseData(m_SampleData, 0.9021852883125810, 0.029);
                yield return new TestCaseData(m_SampleData, 0.9343937335383870, 0.029);
                yield return new TestCaseData(m_SampleData, 0.9666021787641940, 0.041);
                yield return new TestCaseData(m_SampleData, 0.998810624, 0.03); // argument is max of the sample
            }
        }
        #endregion
    }
}