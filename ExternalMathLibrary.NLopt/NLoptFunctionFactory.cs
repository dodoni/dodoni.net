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
using Microsoft.Extensions.Logging;

using System.Runtime.InteropServices;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as factory for <see cref="NLoptFunction"/> objects with respect to a specific NLopt optimization algorithm.
    /// </summary>
    public class NLoptFunctionFactory : MultiDimOptimizer.IFunctionFactory, OrdinaryMultiDimOptimizer.IFunctionFactory
    {
        #region private (readonly) members

        /// <summary>The <see cref="NLoptMultiDimOptimizer"/> object that represents the NLopt optimization algorithm.
        /// </summary>
        private readonly NLoptMultiDimOptimizer nloptMultiDimOptimizer;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptFunctionFactory" /> class.
        /// </summary>
        /// <param name="nloptMultiDimOptimizer">The <see cref="NLoptMultiDimOptimizer"/> object that represents the NLopt optimization algorithm.</param>
        internal NLoptFunctionFactory(NLoptMultiDimOptimizer nloptMultiDimOptimizer)
        {
            this.nloptMultiDimOptimizer = nloptMultiDimOptimizer;
        }
        #endregion

        #region public properties

        #region OrdinaryMultiDimOptimizer.IFunctionFactory Members

        /// <summary>Gets a value indicating whether the gradient is required in the optimization algorithm.
        /// </summary>
        /// <value>The gradient requirement.</value>
        public OrdinaryMultiDimOptimizer.GradientMethod GradientRequirement
        {
            get { return nloptMultiDimOptimizer.Configuration.GradientRequirement; }
        }

        /// <summary>Gets a lower bound of all function values if the optimization algorithm assumed function values greater than a specific number (for example positive values only); otherwise <see cref="System.Double.NaN" />.
        /// </summary>
        /// <value>The lower bound of all function values if the optimization algorithm assumed function values greater than a specific number (for example positive values only); otherwise <see cref="System.Double.NaN" />.</value>
        public double LowerBound
        {
            get { return Double.NaN; }
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Gets the info-output level of detail.
        /// </summary>
        /// <value>The info-output level of detail.</value>
        public InfoOutputDetailLevel InfoOutputDetailLevel
        {
            get { return InfoOutputDetailLevel.Full; }
        }
        #endregion

        #endregion

        #region public methods

        #region OrdinaryMultiDimOptimizer.IFunctionFactory Members

        /// <summary>Creates a specific <see cref="MultiDimOptimizer.IFunction"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="objectiveFunction">The objective function.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="OrdinaryMultiDimOptimizer.IFunctionFactory.GradientRequirement"/> indicates that a gradient is required.</exception>
        MultiDimOptimizer.IFunction OrdinaryMultiDimOptimizer.IFunctionFactory.Create(int dimension, Func<double[], double> objectiveFunction)
        {
            return this.Create(dimension, objectiveFunction);
        }

        /// <summary>Creates a specific <see cref="MultiDimOptimizer.IFunction"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evalute and the second argument contains the gradient, 
        /// i.e. the partial derivatives of the function at the first argument; the return value is the value of the function at the first argument.</param>
        /// <returns>A specific <see cref="MultiDimOptimizer.IFunction"/> object with respect to the specified optimization algorithm.</returns>
        MultiDimOptimizer.IFunction OrdinaryMultiDimOptimizer.IFunctionFactory.Create(int dimension, Func<double[], double[], double> objectiveFunction)
        {
            return this.Create(dimension, objectiveFunction);
        }
        #endregion

        #region IInfoOutputQueriable Members

        /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.</returns>
        public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return infoOutputDetailLevel == InfoOutputDetailLevel.Full;
        }

        /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Gradient requirement", GradientRequirement);
            infoOutputPackage.Add("Lower bound", LowerBound);
        }
        #endregion

        /// <summary>Creates a specific <see cref="NLoptFunction"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="objectiveFunction">The objective function.</param>
        /// <returns>A specific <see cref="NLoptFunction"/> object with respect to the specified optimization algorithm.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if <see cref="OrdinaryMultiDimOptimizer.IFunctionFactory.GradientRequirement"/> indicates that a gradient is required.</exception>
        public NLoptFunction Create(int dimension, Func<double[], double> objectiveFunction)
        {
            if (GradientRequirement == OrdinaryMultiDimOptimizer.GradientMethod.Required)
            {
                throw new InvalidOperationException("Gradient required for NLopt algorithm");
            }
            if (nloptMultiDimOptimizer.IsLogging == false)
            {
                return new NLoptFunction(this, dimension, (n, x, grad, data) => objectiveFunction(x));
            }
            else
            {
                int numberOfFunctionEvaluations = 0;

                return new NLoptFunction(this, dimension,
                    (n, x, grad, data) =>
                    {
                        double value = objectiveFunction(x);
                        numberOfFunctionEvaluations++;

                        if (data != IntPtr.Zero)
                        {
                            var loggerStream = GCHandle.FromIntPtr(data).Target as ILogger;
                            if (loggerStream != null)
                            {
                                LogObjectiveFunctionEvaluation(loggerStream, value, numberOfFunctionEvaluations, dimension, x);
                            }
                        }
                        return value;
                    }
                 );
            }
        }

        /// <summary>Creates a specific <see cref="NLoptFunction"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="objectiveFunction">The objective function, where the first argument is the point where to evalute and the second argument contains the gradient (perhaps <c>null</c>), 
        /// i.e. the partial derivatives of the function at the first argument; the return value is the value of the function at the first argument.</param>
        /// <returns>A specific <see cref="NLoptFunction"/> object with respect to the specified optimization algorithm.</returns>
        public NLoptFunction Create(int dimension, Func<double[], double[], double> objectiveFunction)
        {
            if (nloptMultiDimOptimizer.IsLogging == false)
            {
                return new NLoptFunction(this, dimension, (n, x, grad, data) => objectiveFunction(x, grad));
            }
            else
            {
                int numberOfFunctionEvaluations = 0;

                return new NLoptFunction(this, dimension,
                    (n, x, grad, data) =>
                    {
                        double value = objectiveFunction(x, grad);
                        numberOfFunctionEvaluations++;

                        if (data != IntPtr.Zero)
                        {
                            var loggerStream = GCHandle.FromIntPtr(data).Target as ILogger;
                            if (loggerStream != null)
                            {
                                LogObjectiveFunctionEvaluation(loggerStream, value, numberOfFunctionEvaluations, dimension, x);
                            }
                        }
                        return value;
                    }
                 );
            }
        }

        /// <summary>Creates a specific <see cref="NLoptFunction"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the feasible region.</param>
        /// <param name="objectiveFunction">The objective function in its <see cref="NLoptObjectiveFunction"/> representation.</param>
        /// <returns>A specific <see cref="NLoptFunction"/> object with respect to the specified optimization algorithm.</returns>
        public NLoptFunction Create(int dimension, NLoptObjectiveFunction objectiveFunction)
        {
            return new NLoptFunction(this, dimension, objectiveFunction);
        }
        #endregion

        #region private methods

        /// <summary>Adds a logging (info) message after objective function evaluation.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="value">The value after objective function evaluation.</param>
        /// <param name="numberOfFunctionEvaluations">The number of function evaluations.</param>
        /// <param name="dimension">The dimension.</param>
        /// <param name="argument">The argument.</param>
        private void LogObjectiveFunctionEvaluation(ILogger logger, double value, int numberOfFunctionEvaluations, int dimension, double[] argument)
        {
            if ((argument == null) || (dimension < 1))
            {
                logger.LogInformation(String.Format("NLopt objective function evaluation {0}; value: {1}; number of function evaluations: {2}.", this.nloptMultiDimOptimizer.Algorithm.ToFormatString(), value, numberOfFunctionEvaluations));
            }
            else
            {
                var strBuilder = new StringBuilder(); 

                strBuilder.Append("{");
                strBuilder.AppendFormat("{0}", argument[0]);
                for (int j = 1; j < dimension; j++)
                {
                    strBuilder.AppendFormat(";{0}", argument[j]);
                }
                strBuilder.Append("}");

                logger.LogInformation(String.Format("NLopt objective function evaluation {0}; value: {1}; number of function evaluations: {2}; argument: {3}.", this.nloptMultiDimOptimizer.Algorithm.ToFormatString(), value, numberOfFunctionEvaluations, strBuilder.ToString()));
            }
        }
        #endregion
    }
}