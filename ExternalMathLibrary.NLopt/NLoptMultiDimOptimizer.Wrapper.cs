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
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Optimizer;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Native.NLopt
{
    public partial class NLoptMultiDimOptimizer
    {
        /// <summary>Serves as wrapper of a specific NLopt optimization algorithm in its <see cref="IMultiDimOptimizerAlgorithm" /> representation.
        /// </summary>
        private class Wrapper : IMultiDimOptimizerAlgorithm
        {
            #region private members

            /// <summary>The low-level wrapper of the native NLopt dll.
            /// </summary>
            private NLoptPtr m_NLoptPtr;

            /// <summary>The <see cref="NLoptMultiDimOptimizer"/> object that has been used to create the current instance.
            /// </summary>
            private NLoptMultiDimOptimizer m_Factory;

            /// <summary>The objective function in its <see cref="NLoptFunction"/> representation.
            /// </summary>
            private NLoptFunction m_NLoptFunction;

            /// <summary>The logger in its <see cref="ILoggerStream"/> representation; perhaps <c>null</c> if no logger available.
            /// </summary>
            private ILogger m_LoggerStream = null;

            /// <summary>The <see cref="GCHandle"/> of <see cref="m_LoggerStream"/>.
            /// </summary>
            private GCHandle m_LoggerStreamGCHandle;
            #endregion

            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="Wrapper" /> class.
            /// </summary>
            /// <param name="nloptMultiDimOptimizer">The <see cref="NLoptMultiDimOptimizer"/> object that has been used to create the current instance.</param>
            /// <param name="dimension">The dimension.</param>
            public Wrapper(NLoptMultiDimOptimizer nloptMultiDimOptimizer, int dimension)
            {
                m_Factory = nloptMultiDimOptimizer;
                m_NLoptPtr = new NLoptPtr(nloptMultiDimOptimizer.Algorithm, dimension);

                // check whether the algorithm allows an unconstraint feasible region:
                if ((nloptMultiDimOptimizer.Configuration.BoxConstraintRequirement == NLoptBoundConstraintRequirement.Required) ||
                    (nloptMultiDimOptimizer.Configuration.NonlinearConstraintRequirement.HasFlag(NLoptNonlinearConstraintRequirement.RequiredInequalityConstraints)) ||
                    (nloptMultiDimOptimizer.Configuration.NonlinearConstraintRequirement.HasFlag(NLoptNonlinearConstraintRequirement.RequiredEqualityConstraints)))
                {
                    throw new InvalidOperationException("The optimizer does not support unconstraint feasible region.");
                }
                nloptMultiDimOptimizer.AbortCondition.ApplyTo(m_NLoptPtr);
                if (nloptMultiDimOptimizer.m_nloptPtrAdjustment != null)  // apply some optional individual adjustments
                {
                    nloptMultiDimOptimizer.m_nloptPtrAdjustment(m_NLoptPtr);
                }

                if (nloptMultiDimOptimizer.m_LoggerStreamFactory != null)
                {
                    m_LoggerStream = nloptMultiDimOptimizer.m_LoggerStreamFactory(this);
                    m_LoggerStreamGCHandle = GCHandle.Alloc(m_LoggerStream);
                }
            }

            /// <summary>Initializes a new instance of the <see cref="Wrapper" /> class.
            /// </summary>
            /// <param name="nloptMultiDimOptimizer">The <see cref="NLoptMultiDimOptimizer"/> object that has been used to create the current instance.</param>
            /// <param name="dimension">The dimension.</param>
            /// <param name="nloptConstraints">The constraints of the optimization algorithm in its <see cref="NLoptConstraint"/> representation.</param>
            internal Wrapper(NLoptMultiDimOptimizer nloptMultiDimOptimizer, int dimension, IEnumerable<NLoptConstraint> nloptConstraints)
            {
                m_Factory = nloptMultiDimOptimizer;
                m_NLoptPtr = new NLoptPtr(nloptMultiDimOptimizer.Algorithm, dimension);
                nloptMultiDimOptimizer.AbortCondition.ApplyTo(m_NLoptPtr);

                foreach (var constraint in nloptConstraints)
                {
                    if (constraint.Dimension != dimension)
                    {
                        throw new ArgumentException(String.Format(ExceptionMessages.ArgumentHasWrongDimension, constraint), nameof(nloptConstraints));
                    }
                    constraint.ApplyTo(m_NLoptPtr);
                }
                if (nloptMultiDimOptimizer.m_nloptPtrAdjustment != null)  // apply some optional individual adjustments
                {
                    nloptMultiDimOptimizer.m_nloptPtrAdjustment(m_NLoptPtr);
                }
                if (nloptMultiDimOptimizer.m_LoggerStreamFactory != null)
                {
                    m_LoggerStream = nloptMultiDimOptimizer.m_LoggerStreamFactory(this);
                    m_LoggerStreamGCHandle = GCHandle.Alloc(m_LoggerStream);
                }
            }
            #endregion

            #region public properties

            #region IMultiDimOptimizerAlgorithm Members

            /// <summary>Gets the factory for further <see cref="IMultiDimOptimizerAlgorithm" /> objects of the same type, i.e. with the same stopping condition etc.
            /// </summary>
            /// <value>The factory for further <see cref="IMultiDimOptimizerAlgorithm" /> objects of the same type.</value>
            MultiDimOptimizer IMultiDimOptimizerAlgorithm.Factory
            {
                get { return m_Factory; }
            }

            /// <summary>Gets the dimension of the feasible region.
            /// </summary>
            /// <value>The dimension.</value>
            public int Dimension
            {
                get { return m_NLoptPtr.Dimension; }
            }

            /// <summary>Gets or sets the objective function in its <see cref="MultiDimOptimizer.IFunction" /> representation.
            /// </summary>
            /// <value>The objective function.</value>
            IFunction IMultiDimOptimizerAlgorithm.Function
            {
                get { return m_NLoptFunction; }
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException("Objective function");
                    }
                    if (value is NLoptFunction)
                    {
                        this.Function = value as NLoptFunction;
                    }
                    else
                    {
                        throw new InvalidCastException("Must be of type 'NLoptFunction'.");
                    }
                }
            }
            #endregion

            /// <summary>Gets the factory for further <see cref="IMultiDimOptimizerAlgorithm" /> objects of the same type, i.e. with the same stopping condition etc.
            /// </summary>
            /// <value>The factory for further <see cref="IMultiDimOptimizerAlgorithm" /> objects of the same type.</value>
            public NLoptMultiDimOptimizer Factory
            {
                get { return m_Factory; }
            }

            /// <summary>Gets or sets the objective function in its <see cref="NLoptFunction" /> representation.
            /// </summary>
            /// <value>The objective function.</value>
            public NLoptFunction Function
            {
                get { return m_NLoptFunction; }
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException("Objective function");
                    }
                    m_NLoptFunction = value;
                    if (value.Dimension != Dimension)
                    {
                        throw new InvalidOperationException("Objective function and specified NLopt algorithm have different dimensions of the feasible region.");
                    }

                    if (m_LoggerStream == null)  // no logging
                    {
                        m_NLoptPtr.SetFunction(value.NLoptObjectiveFunction);
                    }
                    else
                    {
                        m_NLoptPtr.SetFunction(value.NLoptObjectiveFunction, m_LoggerStreamGCHandle);
                    }
                }
            }
            #endregion

            #region public methods

            #region IMultiDimOptimizerAlgorithm Members

            /// <summary>Finds the minimum and argmin of <see cref="IMultiDimOptimizerAlgorithm.Function" />.
            /// </summary>
            /// <param name="x">An array with at least <see cref="IMultiDimOptimizerAlgorithm.Dimension" /> elements which is perhaps taken into account as an initial guess of the algorithm; on exit this argument contains the argmin.</param>
            /// <param name="minimum">The minimum, i.e. the function value with respect to <paramref name="x" /> which represents the argmin (output).</param>
            /// <returns>The state of the algorithm, i.e. an indicating whether <paramref name="x" /> and <paramref name="minimum" /> contain valid data.</returns>
            /// <exception cref="NotOperableException">Thrown, if no objective function is specified.</exception>
            public State FindMinimum(double[] x, out double minimum)
            {
                if (m_NLoptFunction == null)
                {
                    throw new NotOperableException("No objective function specified.");
                }
                var nloptResultCode = m_NLoptPtr.FindMinimum(x, out minimum);

                switch (nloptResultCode)
                {
                    case NLoptResultCode.Success:
                    case NLoptResultCode.FToleranceReached:
                    case NLoptResultCode.XToleranceReached:
                        return MultiDimOptimizer.State.Create(OptimizerErrorClassification.ProperResult, minimum, details: InfoOutputProperty.Create("NLopt result code", nloptResultCode));

                    case NLoptResultCode.MaximalNumberOfFunctionEvaluationsReached:
                        return MultiDimOptimizer.State.Create(OptimizerErrorClassification.EvaluationLimitExceeded, minimum);

                    case NLoptResultCode.MaximalTimeReached:
                        return MultiDimOptimizer.State.Create(OptimizerErrorClassification.EvaluationTimeExceeded, minimum);

                    default:
                        return MultiDimOptimizer.State.Create(OptimizerErrorClassification.Unknown, minimum, details: InfoOutputProperty.Create("NLopt result code", nloptResultCode));
                }
            }
            #endregion

            #region IDisposable Members

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                m_NLoptPtr.Dispose();
                m_NLoptPtr = null;

                if (m_LoggerStream != null)
                {
                    m_LoggerStreamGCHandle.Free();
                    // m_LoggerStream.Dispose();
                    m_LoggerStream = null;
                }
            }
            #endregion

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return String.Format("{0}; dimension = {1}", Factory.Name.String, Dimension);
            }
            #endregion
        }
    }
}