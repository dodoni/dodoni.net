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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.NumericalIntegrators
{
    public partial class AdaptiveGaussLobattoIntegrator
    {
        /// <summary>Provides exit condition for <see cref="AdaptiveGaussLobattoIntegrator"/> objects.
        /// </summary>
        public class StoppingCriteria : IInfoOutputQueriable
        {
            #region public constructors

            /// <summary>Initializes a new instance of the <see cref="StoppingCriteria"/> class.
            /// </summary>
            /// <param name="maxIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance to take into account as exit condition; or <see cref="System.Double.NaN"/>.</param>
            /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
            public StoppingCriteria(int maxIterations, double tolerance = Double.NaN, int maxEvaluations = Int32.MaxValue)
            {
                MaxIterations = maxIterations;
                MaxEvaluations = maxEvaluations;
                Tolerance = tolerance;
            }
            #endregion

            #region public properties

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return InfoOutputDetailLevel.Full; }
            }
            #endregion

            /// <summary>Gets the maximal number of function evaluations.
            /// </summary>
            /// <value>The maximal number of function evaluations.</value>
            public int MaxEvaluations
            {
                get;
                private set;
            }

            /// <summary>Gets the maximal number of iterations.
            /// </summary>
            /// <value>The maximal number of iterations.</value>
            public int MaxIterations
            {
                get;
                private set;
            }

            /// <summary>Gets the tolerance to take into account as exit condition; or <see cref="System.Double.NaN"/> if no tolerance available.
            /// </summary>
            /// <value>The tolerance.</value>
            public double Tolerance
            {
                get;
                private set;
            }
            #endregion

            #region public methods

            #region IInfoOutputQueriable Members

            /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public virtual void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
            {
                var infoOutputPackage = infoOutput.AcquirePackage(categoryName);

                infoOutputPackage.Add("Maximal number of iterations", MaxIterations);
                infoOutputPackage.Add("Maximal function evaluations", MaxEvaluations);
                infoOutputPackage.Add("Tolerance", Tolerance);
            }

            /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
            /// </returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
            }
            #endregion

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return String.Format("Maximal number of iterations: {0}; maximal number of function evaluations: {1}; tolerance: {2}", MaxIterations, MaxEvaluations, Tolerance);
            }
            #endregion

            #region public static methods

            /// <summary>Creates a new <see cref="StoppingCriteria"/> object.
            /// </summary>
            /// <param name="maxIterations">The maximal number of iterations.</param>
            /// <param name="tolerance">The tolerance to take into account as exit condition; or <see cref="System.Double.NaN"/>.</param>
            /// <param name="maxEvaluations">The maximal number of function evaluations.</param>
            /// <returns>A new <see cref="StoppingCriteria"/> object.</returns>
            public static StoppingCriteria Create(int maxIterations, double tolerance = Double.NaN, int maxEvaluations = Int32.MaxValue)
            {
                return new StoppingCriteria(maxIterations, tolerance, maxEvaluations);
            }
            #endregion
        }
    }
}