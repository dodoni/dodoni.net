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

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    /// <summary>Serves as abstract basis class for a numerical integrator that uses a non-adaptive Gauss-Kronrod integration rule, i.e.
    /// applies the 1-, 3-, 7-, 15-, 31-, 63-, 127- and if neccessary the 255-point Kronrod integration rule.
    /// </summary>
    /// <remarks>Based on 'Algorithm 699: A new representation of Patterson's quadature formulae', F. T. Krogh and W. van Snyder,
    /// ACM Transaction on Mathematical Software Vol. 17, No. 4, December 1991, p. 457-461.</remarks>
    internal abstract class GaussKronrodPatterson255Quadrature : IOperable, IInfoOutputQueriable
    {
        #region public/protected static members

        /// <summary>The maximal number of iterations.
        /// </summary>
        public const int MaxIterations = 7;

        /// <summary>The maximal number of function evaluations.
        /// </summary>
        public const int MaxEvaluations = 255;

        /// <summary>Lower bounds for the subscript sequences.
        /// </summary>
        protected static readonly int[] sm_FL = { 1, 2, 4, 8, 11, 13, 0 };

        /// <summary>Upper bounds for the subscript sequences.
        /// </summary>
        protected static readonly int[] sm_FH = { 1, 3, 7, 15, 16, 16, -1 };

        /// <summary>Lower bounds for the subscript sequences.
        /// </summary>
        protected static readonly int[] sm_KL = { 0, 0, 0, 0, 0, 2, 4, 8, 4, 8, 11 };

        /// <summary>Upper bounds for the subscript sequences.
        /// </summary>
        protected static readonly int[] sm_KH = { 0, 1, 3, 7, 15, 2, 5, 16, 4, 8, 16 };

        /// <summary>Contains indiexes into <see cref="sm_KL"/> and <see cref="sm_KH"/> used to indicates a list therein:
        /// The k-th list in <see cref="sm_KL"/> and <see cref="sm_KH"/> consists of elements at positions given by
        /// KX[k-1] through KX[k] inclusively.
        /// </summary>
        protected static readonly int[] sm_KX = { 0, 1, 2, 3, 4, 5, 8, 11 };

        /// <summary>The corrections, nodes and weights for the different Kronrod rules.
        /// </summary>
        /// <remarks>The data are given in the same manner as in the implementation of 'algorithm 699', i.e. 
        /// first at all corrections, and the abscissae as 1-|abscissa|. See the original implementation for
        /// further informations. </remarks>
        protected static readonly double[] sm_Coefficients = {

           #region coefficients for the 3-point Kronrod rule

            /* Corrections */
           -.11111111111111111111e+00,
            /* nodes and weights */
           +.22540333075851662296e+00,
           +.55555555555555555556e+00,
           #endregion

           #region coefficients for the 7-point Kronrod rule
            
            /* Corrections */
           +.647209421402969791e-02,
           -.928968790944433705e-02,
            /* nodes and weights */
           +.39508731291979716579e-01,
           +.10465622602646726519e+00,
           +.56575625065319744200e+00,
           +.40139741477596222291e+00,
           #endregion

           #region coefficients for the 15-point Kronrod rule
           
            /* Corrections */
           +.5223046896961622e-04,
           +.17121030961750000e-03,
           -.724830016153892898e-03,
           -.7017801099209042e-04,
            /* nodes and weights */
           +.61680367872449777899e-02,
           +.17001719629940260339e-01,
           +.11154076712774300110e+00,
           +.92927195315124537686e-01,
           +.37889705326277359705e+00,
           +.17151190913639138079e+00,
           +.77661331357103311837e+00,
           +.21915685840158749640e+00,
           #endregion

           #region coefficients for the 31-point Kronrod rule

           /* corrections */
           +.682166534792e-08,
           +.12667409859336e-06,
           +.59565976367837165e-05,
           +.1392330106826e-07,
           -.6629407564902392e-04,
           -.704395804282302e-06,
           -.34518205339241e-07,
           -.814486910996e-08,
            /* nodes and weights */
           +.90187503233240234038e-03,
           +.25447807915618744154e-02,
           +.18468850446259893130e-01,
           +.16446049854387810934e-01,
           +.70345142570259943330e-01,
           +.35957103307129322097e-01,
           +.16327406183113126449e+00,
           +.56979509494123357412e-01,
           +.29750379350847292139e+00,
           +.76879620499003531043e-01,
           +.46868025635562437602e+00,
           +.93627109981264473617e-01,
           +.66886460674202316691e+00,
           +.10566989358023480974e+00,
           +.88751105686681337425e+00,
           +.11195687302095345688e+00,
           #endregion

           #region coefficients for the 63-point Kronrod rule

           /* corrections */
           +.371583e-15,
           +.21237877e-12,
           +.10522629388435e-08,
           +.1748029e-14,
           +.3475718983017160e-06,
           +.90312761725e-11,
           +.12558916e-13,
           +.54591e-15,
           -.72338395508691963e-05,
           -.169699579757977e-07,
           -.854363907155e-10,
           -.12281300930e-11,
           -.462334825e-13,
           -.42244055e-14,
           -.88501e-15,
           -.40904e-15,
            /* nodes and weights */
           +.12711187964238806027e-03,
           +.36322148184553065969e-03,
           +.27937406277780409196e-02,
           +.25790497946856882724e-02,
           +.11315242452570520059e-01,
           +.61155068221172463397e-02,
           +.27817125251418203419e-01,
           +.10498246909621321898e-01,
           +.53657141626597094849e-01,
           +.15406750466559497802e-01,
           +.89628843042995707499e-01,
           +.20594233915912711149e-01,
           +.13609206180630952284e+00,
           +.25869679327214746911e-01,
           +.19305946804978238813e+00,
           +.31073551111687964880e-01,
           +.26024395564730524132e+00,
           +.36064432780782572640e-01,
           +.33709033997521940454e+00,
           +.40715510116944318934e-01,
           +.42280428994795418516e+00,
           +.44914531653632197414e-01,
           +.51638197305415897244e+00,
           +.48564330406673198716e-01,
           +.61664067580126965307e+00,
           +.51583253952048458777e-01,
           +.72225017797817568492e+00,
           +.53905499335266063927e-01,
           +.83176474844779253501e+00,
           +.55481404356559363988e-01,
           +.94365568695340721002e+00,
           +.56277699831254301273e-01,
           #endregion

           #region coefficients for the 127-point Kronrod rule
 
           /* corrections */
           +.1041098e-15,
           +.249472054598e-10,
           +.55e-20,
           +.290412475995385e-07,
           +.367282126e-13,
           +.5568e-18,
           -.871176477376972025e-06,
           -.8147324267441e-09,
           -.8830920337e-12,
           -.18018239e-14,
           -.70528e-17,
           -.506e-19,
           /* nodes and weights */
           +.17569645108401419961e-04,
           +.50536095207862517625e-04,
           +.40120032808931675009e-03,
           +.37774664632698466027e-03,
           +.16833646815926074696e-02,
           +.93836984854238150079e-03,
           +.42758953015928114900e-02,
           +.16811428654214699063e-02,
           +.85042788218938676006e-02,
           +.25687649437940203731e-02,
           +.14628500401479628890e-01,
           +.35728927835172996494e-02,
           +.22858485360294285840e-01,
           +.46710503721143217474e-02,
           +.33362148441583432910e-01,
           +.58434498758356395076e-02,
           +.46269993574238863589e-01,
           +.70724899954335554680e-02,
           +.61679602220407116350e-01,
           +.83428387539681577056e-02,
           +.79659974529987579270e-01,
           +.96411777297025366953e-02,
           +.10025510022305996335e+00,
           +.10955733387837901648e-01,
           +.12348658551529473026e+00,
           +.12275830560082770087e-01,
           +.14935550523164972024e+00,
           +.13591571009765546790e-01,
           +.17784374563501959262e+00,
           +.14893641664815182035e-01,
           +.20891506620015163857e+00,
           +.16173218729577719942e-01,
           +.24251603361948636206e+00,
           +.17421930159464173747e-01,
           +.27857691462990108452e+00,
           +.18631848256138790186e-01,
           +.31701256890892077191e+00,
           +.19795495048097499488e-01,
           +.35772335749024048622e+00,
           +.20905851445812023852e-01,
           +.40059606975775710702e+00,
           +.21956366305317824939e-01,
           +.44550486736806745112e+00,
           +.22940964229387748761e-01,
           +.49231224246628339785e+00,
           +.23854052106038540080e-01,
           +.54086998801016766712e+00,
           +.24690524744487676909e-01,
           +.59102017877011132759e+00,
           +.25445769965464765813e-01,
           +.64259616216846784762e+00,
           +.26115673376706097680e-01,
           +.69542355844328595666e+00,
           +.26696622927450359906e-01,
           +.74932126969651682339e+00,
           +.27185513229624791819e-01,
           +.80410249728889984607e+00,
           +.27579749566481873035e-01,
           +.85957576684743982540e+00,
           +.27877251476613701609e-01,
           +.91554595991628911629e+00,
           +.28076455793817246607e-01,
           +.97181535105025430566e+00,
           +.28176319033016602131e-01,
           #endregion

           #region coefficients for the 255-point Kronrod rule
 
            /* corrections */
           +.3326e-18,
           +.114094770478e-11,
           +.2952436056970351e-08,
           +.51608328e-15,
           -.110177219650597323e-06,
           -.58656987416475e-10,
           -.23340340645e-13,
           -.1248950e-16,
           /* nodes and weights */
           +.24036202515353807630e-05,
           +.69379364324108267170e-05,
           +.56003792945624240417e-04,
           +.53275293669780613125e-04,
           +.23950907556795267013e-03,
           +.13575491094922871973e-03,
           +.61966197497641806982e-03,
           +.24921240048299729402e-03,
           +.12543855319048853002e-02,
           +.38974528447328229322e-03,
           +.21946455040427254399e-02,
           +.55429531493037471492e-03,
           +.34858540851097261500e-02,
           +.74028280424450333046e-03,
           +.51684971993789994803e-02,
           +.94536151685852538246e-03,
           +.72786557172113846706e-02,
           +.11674841174299594077e-02,
           +.98486295992298408193e-02,
           +.14049079956551446427e-02,
           +.12907472045965932809e-01,
           +.16561127281544526052e-02,
           +.16481342421367271240e-01,
           +.19197129710138724125e-02,
           +.20593718329137316189e-01,
           +.21944069253638388388e-02,
           +.25265540247597332240e-01,
           +.24789582266575679307e-02,
           +.30515340497540768229e-01,
           +.27721957645934509940e-02,
           +.36359378430187867480e-01,
           +.30730184347025783234e-02,
           +.42811783890139037259e-01,
           +.33803979910869203823e-02,
           +.49884702478705123440e-01,
           +.36933779170256508183e-02,
           +.57588434808916940190e-01,
           +.40110687240750233989e-02,
           +.65931563842274211999e-01,
           +.43326409680929828545e-02,
           +.74921067092924347640e-01,
           +.46573172997568547773e-02,
           +.84562412844234959360e-01,
           +.49843645647655386012e-02,
           +.94859641186738404810e-01,
           +.53130866051870565663e-02,
           +.10581543166444097714e+00,
           +.56428181013844441585e-02,
           +.11743115975265809315e+00,
           +.59729195655081658049e-02,
           +.12970694445188609414e+00,
           +.63027734490857587172e-02,
           +.14264168911376784347e+00,
           +.66317812429018878941e-02,
           +.15623311732729139895e+00,
           +.69593614093904229394e-02,
           +.17047780536259859981e+00,
           +.72849479805538070639e-02,
           +.18537121234486258656e+00,
           +.76079896657190565832e-02,
           +.20090770903915859819e+00,
           +.79279493342948491103e-02,
           +.21708060588171698360e+00,
           +.82443037630328680306e-02,
           +.23388218069623990928e+00,
           +.85565435613076896192e-02,
           +.25130370638306339718e+00,
           +.88641732094824942641e-02,
           +.26933547875781873867e+00,
           +.91667111635607884067e-02,
           +.28796684463774796540e+00,
           +.94636899938300652943e-02,
           +.30718623022088529711e+00,
           +.97546565363174114611e-02,
           +.32698116976958152079e+00,
           +.10039172044056840798e-01,
           +.34733833458998250389e+00,
           +.10316812330947621682e-01,
           +.36824356228880576959e+00,
           +.10587167904885197931e-01,
           +.38968188628481359983e+00,
           +.10849844089337314099e-01,
           +.41163756555233745857e+00,
           +.11104461134006926537e-01,
           +.43409411457634557737e+00,
           +.11350654315980596602e-01,
           +.45703433350168850951e+00,
           +.11588074033043952568e-01,
           +.48044033846254297801e+00,
           +.11816385890830235763e-01,
           +.50429359208123853983e+00,
           +.12035270785279562630e-01,
           +.52857493412834112307e+00,
           +.12244424981611985899e-01,
           +.55326461233797152625e+00,
           +.12443560190714035263e-01,
           +.57834231337383669993e+00,
           +.12632403643542078765e-01,
           +.60378719394238406082e+00,
           +.12810698163877361967e-01,
           +.62957791204992176986e+00,
           +.12978202239537399286e-01,
           +.65569265840056197721e+00,
           +.13134690091960152836e-01,
           +.68210918793152331682e+00,
           +.13279951743930530650e-01,
           +.70880485148175331803e+00,
           +.13413793085110098513e-01,
           +.73575662758907323806e+00,
           +.13536035934956213614e-01,
           +.76294115441017027278e+00,
           +.13646518102571291428e-01,
           +.79033476175681880523e+00,
           +.13745093443001896632e-01,
           +.81791350324074780175e+00,
           +.13831631909506428676e-01,
           +.84565318851862189130e+00,
           +.13906019601325461264e-01,
           +.87352941562769803314e+00,
           +.13968158806516938516e-01,
           +.90151760340188079791e+00,
           +.14017968039456608810e-01,
           +.92959302395714482093e+00,
           +.14055382072649964277e-01,
           +.95773083523463639678e+00,
           +.14080351962553661325e-01,
           +.98590611358921753738e+00,
           +.14092845069160408355e-01};
           #endregion

        #endregion

        #region protected members

        /// <summary>The lower integration bound.
        /// </summary>
        protected double m_LowerBound = Double.NaN;

        /// <summary>The upper integration bound.
        /// </summary>
        protected double m_UpperBound = Double.NaN;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="GaussKronrodPatterson255Quadrature"/> class.
        /// </summary>
        protected GaussKronrodPatterson255Quadrature()
        {
        }
        #endregion

        #region public properties

        #region IOperable Members

        /// <summary>Gets a value indicating whether this instance is operable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is operable; otherwise, <c>false</c>.
        /// </value>
        abstract public bool IsOperable { get; }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.
        /// </value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        /// <summary>Gets the lower integration bound.
        /// </summary>
        /// <value>The lower integration bound.</value>
        public double LowerBound
        {
            get { return m_LowerBound; }
        }

        /// <summary>Gets the upper integration bound.
        /// </summary>
        /// <value>The upper integration bound.</value>
        public double UpperBound
        {
            get { return m_UpperBound; }
        }
        #endregion

        #region public methods

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.
        /// </returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public abstract void FillInfoOutput(InfoOutput infoOutput, string categoryName);
        #endregion

        /// <summary>Sets the lower integration bound.
        /// </summary>
        /// <param name="lowerBound">The lower integration bound.</param>
        /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound"/> has been set to <paramref name="lowerBound"/>.</returns>
        public bool TrySetLowerBound(double lowerBound)
        {
            if ((Double.IsNaN(lowerBound) == true) || (Double.IsInfinity(lowerBound) == true))
            {
                return false;
            }
            m_LowerBound = lowerBound;
            return true;
        }

        /// <summary>Sets the upper integration bound.
        /// </summary>
        /// <param name="upperBound">The upper integration bound.</param>
        /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound"/> has been set to <paramref name="upperBound"/>.</returns>
        public bool TrySetUpperBound(double upperBound)
        {
            if ((Double.IsNaN(upperBound) == true) || (Double.IsInfinity(upperBound) == true))
            {
                return false;
            }
            m_UpperBound = upperBound;
            return true;
        }

        /// <summary>Sets the lower and upper integration bounds.
        /// </summary>
        /// <param name="lowerBound">The lower integration bound.</param>
        /// <param name="upperBound">The upper integration bound.</param>
        /// <returns>A value indicating whether <see cref="IOneDimNumericalIntegratorAlgorithm.LowerBound"/> and <see cref="IOneDimNumericalIntegratorAlgorithm.UpperBound"/> have been changed.</returns>
        public bool TrySetBounds(double lowerBound, double upperBound)
        {
            if ((Double.IsNaN(lowerBound) == true) || (Double.IsInfinity(lowerBound) == true))
            {
                return false;
            }
            if ((Double.IsNaN(upperBound) == true) || (Double.IsInfinity(upperBound) == true))
            {
                return false;
            }
            m_LowerBound = lowerBound;
            m_UpperBound = upperBound;
            return true;
        }
        #endregion
    }
}