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
    /// <summary>Represents a numerical integrator that implements the Gauss-Tschebyscheff integration rule,
    /// i.e. numerical integrator of 
    /// <para>
    /// \int_{-1}^1  f(x) / Sqrt(1-x^2) dx 
    /// </para>
    /// The integral can be transformed to arbitrary integration bounds, i.e.
    /// <para>
    ///   \int_a^b f(x) * w(x) dx, 
    /// </para>
    /// where w(x) = 1 / \sqrt[ 1 - { ( x - (b+a)/2) * 2 / (b-a) }^2 ].
    /// </summary>
    /// <remarks>The implementation is based on
    /// <para>'Numerical recipes', §4.5, W. H. Press.</para>
    /// </remarks>
    public partial class GaussTschebyscheffIntegrator : OneDimNumericalIntegrator
    {
        #region public static (readonly) members

        /// <summary>Represents a standard exit condition.
        /// </summary>
        public static readonly ExitCondition StandardExitCondition = ExitCondition.Create(50, absoluteTolerance: MachineConsts.Epsilon, relativeTolerance: MachineConsts.Epsilon);
        #endregion

        #region private members

        /// <summary>The name of the numerical integrator.
        /// </summary>
        private IdentifierString m_Name;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="GaussTschebyscheffIntegrator"/> class.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        /// <param name="exitCondition">The exit condition.</param>
        public GaussTschebyscheffIntegrator(int initialOrder, int orderStepSize, ExitCondition exitCondition)
            : base(BoundDescriptor.Open, BoundDescriptor.Open)
        {
            ExitCondition = exitCondition ?? throw new ArgumentNullException(nameof(exitCondition));

            if (initialOrder < 2)
            {
                throw new ArgumentOutOfRangeException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "Initial order", 2));
            }
            InitialOrder = initialOrder;

            if (orderStepSize < 1)
            {
                throw new ArgumentOutOfRangeException(String.Format(ExceptionMessages.ArgumentOutOfRangeGreaterEqual, "Order step size", 1));
            }
            OrderStepSize = orderStepSize;

            m_Name = new IdentifierString("Gauss-Tschebyscheff Integrator");
        }

        /// <summary>Initializes a new instance of the <see cref="GaussTschebyscheffIntegrator"/> class.
        /// </summary>
        /// <param name="initialOrder">The initial order of the Gauss-Tschebyscheff approach, i.e. the order in the first iteration step.</param>
        /// <param name="orderStepSize">The step size of the order, i.e. in each iteration step the order will be increased by the specified number.</param>
        /// <remarks>The <see cref="StandardExitCondition"/> is taken into account.</remarks>
        public GaussTschebyscheffIntegrator(int initialOrder = 50, int orderStepSize = 10)
            : this(initialOrder, orderStepSize, StandardExitCondition)
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets the exit condition.
        /// </summary>
        /// <value>The exit condition.</value>
        public ExitCondition ExitCondition
        {
            get;
            private set;
        }

        /// <summary>Gets the initial order of the Gauss-Hermite approach, i.e. the order in the first iteration step.
        /// </summary>
        /// <value>The initial order.</value>
        public int InitialOrder
        {
            get;
            private set;
        }

        /// <summary>Gets the step size of the order, i.e. in each iteration step the order will be increased by the specified number.
        /// </summary>
        /// <value>The order step size.</value>
        public int OrderStepSize
        {
            get;
            private set;
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.
        /// </summary>
        /// <returns>A new <see cref="IOneDimNumericalIntegratorAlgorithm"/> object.</returns>
        public override IOneDimNumericalIntegratorAlgorithm Create()
        {
            return new Algorithm(this);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput"/> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public override void FillInfoOutput(InfoOutput infoOutput, string categoryName = "General")
        {
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            infoOutputPackage.Add("Dimension", 1);
            infoOutputPackage.Add("Weight function is 1.0", false);
            infoOutputPackage.Add("Initial Order", InitialOrder);
            infoOutputPackage.Add("Order stepsize", OrderStepSize);

            LowerBoundDescriptor.FillInfoOutput(infoOutput, categoryName + ".LowerBoundDescriptor");
            UpperBoundDescriptor.FillInfoOutput(infoOutput, categoryName + ".UpperBoundDescriptor");
            ExitCondition.FillInfoOutput(infoOutput, categoryName + ".ExitCondition");
        }

        /// <summary>Sets the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> property.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
        /// <returns>A value indicating whether the <see cref="P:Dodoni.BasicComponents.Containers.IInfoOutputQueriable.InfoOutputDetailLevel"/> has been set to <paramref name="infoOutputDetailLevel"/>.
        /// </returns>
        public override bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
        {
            return (infoOutputDetailLevel == InfoOutputDetailLevel.Full);
        }
        #endregion

        #region protected methods

        /// <summary>Gets the long name of the numerical integrator.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the numerical integrator.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_Name;
        }

        /// <summary>Gets the name of the numerical integrator.
        /// </summary>
        /// <returns>The name of the numerical integrator.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }
        #endregion
    }
}