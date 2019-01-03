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
namespace Dodoni.MathLibrary.Optimizer.OneDimensional
{
    /// <summary>Provides the parameter for the configuration of a Simulated Annealing optimization represented by <see cref="OneDimSAOptimizer"/>.
    /// </summary>
    public class OneDimSAOptimizerConfiguration
    {
        #region nested enumerations

        /// <summary>Represents the way how to generate points in the one-dimensional Simulated Annealing optimizer algorithm.
        /// </summary>
        public enum GenerationRule
        {
            /// <summary>If the randomly choosen point is outside the specified constraint (=bounded interval) reject the point and generate another point which is inside the domain. 
            /// Neither take some previous point into account nor consider some step lenght, just try to choose a uniformly distributed random variable in the specified domain.
            /// </summary>
            UseWholeDomain,

            /// <summary>If the point is inside the constraint (=bounded interval), the generation of the next randomly choosen point depends on some step lenght and the pevious point.
            /// </summary>
            /// <remarks>The next point is specified by
            /// <para>
            ///  x_{i+1} = x_i + \lambda * U, where U\R(-1,1), \lambda: step length; if x_i + \lambda, x_i - \lambda is inside the constraint.
            /// </para>
            /// Otherwise one has to be carefull with the constraints and one generate first a random variable S with values in \{-,+\} which indicates if the next value lies on the left or right side 
            /// of x_i and on the corresponding interval a random number is choosen.
            /// </remarks>
            TakeCurrentPositonIntoAccount,
        }
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="OneDimSAOptimizerConfiguration"/> class.
        /// </summary>
        /// <param name="initialTemperature">The initial temperature.</param>
        /// <param name="coolDownFactor">The cool down factor.</param>
        /// <param name="initialStepLength">The initial step size.</param>
        /// <param name="stepLengthControlFactor">The factor that controls the step length adjustment.</param>
        /// <param name="pointGenerationRule">A value that describes the way how to generate the next randomly choosen point.</param>
        public OneDimSAOptimizerConfiguration(double initialTemperature = 1.0, double coolDownFactor = 0.85, double initialStepLength = 1.0, double stepLengthControlFactor = 2.0, GenerationRule pointGenerationRule = GenerationRule.UseWholeDomain)
        {
            InitialTemperature = initialTemperature;
            CoolDownFactor = coolDownFactor;
            InitialStepLength = initialStepLength;
            StepLengthControlFactor = stepLengthControlFactor;
            PointGenerationRule = pointGenerationRule;
        }
        #endregion

        #region public properties

        /// <summary>Gets the initial temperature (= T_init).
        /// </summary>
        /// <value>The initial temperature.</value>
        public double InitialTemperature
        {
            get;
            private set;
        }

        /// <summary>Gets the cool down factor, i.e. the temperature is given by 'cool down factor * previous temperature'.
        /// </summary>
        /// <value>The cool down factor.</value>
        public double CoolDownFactor
        {
            get;
            private set;
        }

        /// <summary>Gets the initial step length.
        /// </summary>
        /// <value>The initial step length.</value>
        /// <remarks>The step length is a real value \lambda and the next point is given by
        /// <para>
        ///  x_{i+1} = x_i + \lambda * Z_i,
        /// </para>
        /// where Z_i is a random number in [-1,1]. If the resulting value is not inside the constraint, it will be rejected and the constraint will construct one feasible point itself.
        /// </remarks>
        public double InitialStepLength
        {
            get;
            private set;
        }

        /// <summary>Gets a factor that controls the step length adjustment.
        /// </summary>
        /// <remarks>This is the factor C in formula (5) and (6) of 'Use of a simulated annealing algorithm to fit compartmental models 
        /// with an application to fractal pharmacokinetics'., R. E. Marsh, T. A. Riauka, S. A. McQuarrie, 2007.</remarks>
        public double StepLengthControlFactor
        {
            get;
            private set;
        }

        /// <summary>Gets the constraint rule, i.e. represents the rule which is used if the next point of
        /// the probabilistic optimization algorithm is randomly choosen and some constraints are given.
        /// </summary>
        public GenerationRule PointGenerationRule
        {
            get;
            private set;
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="OneDimSAOptimizerConfiguration"/> object.
        /// </summary>          
        /// <param name="initialTemperature">The initial temperature.</param>
        /// <param name="coolDownFactor">The cool down factor.</param>
        /// <param name="initialStepLength">The initial step size.</param>
        /// <param name="stepLengthControlFactor">The factor that controls the step length adjustment.</param>
        /// <param name="pointGenerationRule">A value that describes the way how to generate the next randomly choosen point.</param>
        /// <returns>The specified <see cref="OneDimSAOptimizerConfiguration"/> object.</returns>
        public static OneDimSAOptimizerConfiguration Create(double initialTemperature = 1.0, double coolDownFactor = 0.85, double initialStepLength = 1.0, double stepLengthControlFactor = 2.0, GenerationRule pointGenerationRule = GenerationRule.UseWholeDomain)
        {
            return new OneDimSAOptimizerConfiguration(initialTemperature, coolDownFactor, initialStepLength, stepLengthControlFactor, pointGenerationRule);
        }
        #endregion
    }
}