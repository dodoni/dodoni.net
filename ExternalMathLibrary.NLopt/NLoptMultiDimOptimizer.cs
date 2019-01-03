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

using Dodoni.BasicComponents;
using Dodoni.MathLibrary.Optimizer;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Utilities;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;
using Microsoft.Extensions.Logging;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>Serves as wrapper for optimization algorithms based on the NLopt library 2.4.x, see
    /// <para>http://ab-initio.mit.edu/wiki/index.php/NLopt.</para>
    /// NLopt combines several free/open-source nonlinear optimization libraries by various authors.  See the COPYING, COPYRIGHT, and README
    /// files in the subdirectories for the original copyright and licensing information of these packages.
    /// 
    /// The compiled NLopt library, i.e. the combined work of all of the included optimization routines, is licensed under the conjunction of
    /// all of these licensing terms.  Currently, the most restrictive terms are for the code in the "luksan" directory, which is licensed under
    /// the GNU Lesser General Public License (GNU LGPL), version 2.1 or later (see luksan/COPYRIGHT).
    /// 
    /// That means that the compiled NLopt library is governed by the terms of the LGPL.
    /// 
    /// Other portions of NLopt, including any modifications to the abovementioned packages, are licensed under the standard "MIT License:"
    /// 
    /// Copyright (c) 2007-2011 Massachusetts Institute of Technology
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the
    /// "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish,
    /// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
    /// the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    /// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    /// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    /// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </summary>           
    public partial class NLoptMultiDimOptimizer : OrdinaryMultiDimOptimizer
    {
        #region public static (readonly) members

        /// <summary>Represents a standard abort (stopping) condition of the NLopt algorithm.
        /// </summary>
        public static readonly NLoptAbortCondition StandardAbortCondition = new NLoptAbortCondition();
        #endregion

        #region private members

        /// <summary>The name of the NLopt algorithm in its <see cref="IdentifierString"/> representation.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The long name of the NLopt algorithm in its <see cref="IdentifierString"/> representation.
        /// </summary>
        private IdentifierString m_LongName;

        /// <summary>An optional delegate which will be called in the <c>Create</c> methods for <see cref="IMultiDimOptimizerAlgorithm"/> objects that allows individual adjustments of the internal <see cref="NLoptPtr"/> representation.
        /// </summary>
        private Action<NLoptPtr> m_nloptPtrAdjustment;

        /// <summary>A factory for <see cref="ILoggerStream"/> objects, i.e. for a logging. Each <see cref="IMultiDimOptimizerAlgorithm"/> object will track the function values in the specified logger.
        /// </summary>
        private Func<IMultiDimOptimizerAlgorithm, ILogger> m_LoggerStreamFactory;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="NLoptMultiDimOptimizer" /> class.
        /// </summary>
        /// <param name="algorithm">A value indicating the specific NLopt algorithm.</param>
        /// <param name="abortCondition">The abort (stopping) condition of the NLopt algorithm.</param>
        /// <param name="nloptPtrAdjustment">An optional delegate which will be called in the <c>Create</c> methods for <see cref="IMultiDimOptimizerAlgorithm"/> objects that allows individual adjustments of the internal <see cref="NLoptPtr"/> representation.</param>
        /// <param name="loggerStreamFactory">A factory for <see cref="ILoggerStream"/> objects, i.e. for a logging. Each <see cref="IMultiDimOptimizerAlgorithm"/> object will track the function values in the specified logger.</param>
        /// <remarks>One can use <paramref name="nloptPtrAdjustment"/> to change the Initial step size, initial "population" of random points, set Local/subsidiary optimization algorithm etc. See 
        /// the documentation of the NLopt library http://ab-initio.mit.edu/wiki/index.php/NLopt for further details.</remarks>
        public NLoptMultiDimOptimizer(NLoptAlgorithm algorithm, NLoptAbortCondition abortCondition, Action<NLoptPtr> nloptPtrAdjustment = null, Func<IMultiDimOptimizerAlgorithm, ILogger> loggerStreamFactory = null)
        {
            Algorithm = algorithm;
            Configuration = NLoptConfiguration.Create(algorithm);

            if (abortCondition == null)
            {
                throw new ArgumentNullException("abortCondition");
            }
            AbortCondition = abortCondition;

            m_LongName = new IdentifierString(NLoptPtr.GetName(algorithm));
            m_Name = new IdentifierString(algorithm.ToFormatString(EnumStringRepresentationUsage.StringAttribute));
            Constraint = new NLoptConstraintFactory(this);
            Function = new NLoptFunctionFactory(this);
            m_nloptPtrAdjustment = nloptPtrAdjustment;
            m_LoggerStreamFactory = loggerStreamFactory;
        }

        /// <summary>Initializes a new instance of the <see cref="NLoptMultiDimOptimizer" /> class.
        /// </summary>
        /// <param name="algorithm">A value indicating the specific NLopt algorithm.</param>
        /// <param name="nloptPtrAdjustment">An optional delegate which will be called in the <c>Create</c> methods for <see cref="IMultiDimOptimizerAlgorithm"/> objects that allows individual adjustments of the internal <see cref="NLoptPtr"/> representation.</param>
        /// <param name="loggerStreamFactory">A factory for <see cref="ILoggerStream"/> objects, i.e. for a logging. Each <see cref="IMultiDimOptimizerAlgorithm"/> object will track the function values in the specified logger.</param>
        /// <remarks>
        /// <para>The <see cref="NLoptMultiDimOptimizer.StandardAbortCondition"/> is taken into account.</para>
        /// One can use <paramref name="nloptPtrAdjustment"/> to change the Initial step size, initial "population" of random points, set Local/subsidiary optimization algorithm etc. See 
        /// the documentation of the NLopt library http://ab-initio.mit.edu/wiki/index.php/NLopt for further details.</remarks>
        public NLoptMultiDimOptimizer(NLoptAlgorithm algorithm, Action<NLoptPtr> nloptPtrAdjustment = null, Func<IMultiDimOptimizerAlgorithm, ILogger> loggerStreamFactory = null)
            : this(algorithm, StandardAbortCondition, nloptPtrAdjustment, loggerStreamFactory)
        {
        }
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="NLoptMultiDimOptimizer" /> class.
        /// </summary>
        static NLoptMultiDimOptimizer()
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets a factory for constraints of the algorithm represented by the current instance.
        /// </summary>
        /// <value>A factory for constraints of the algorithm represented by the current instance.</value>
        public new NLoptConstraintFactory Constraint
        {
            get;
            private set;
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction" /> objects that encapsulate the function to optimize.
        /// </summary>
        /// <value>A factory for <see cref="MultiDimOptimizer.IFunction" /> objects that encapsulate the function to optimize.</value>
        public new NLoptFunctionFactory Function
        {
            get;
            private set;
        }

        /// <summary>Gets the NLopt algorithm in its <see cref="NLoptAlgorithm"/> representation.
        /// </summary>
        /// <value>The NLopt algorithm in its <see cref="NLoptAlgorithm"/> representation.</value>
        public NLoptAlgorithm Algorithm
        {
            get;
            private set;
        }

        /// <summary>Gets the configuration of the NLopt algorithm in its <see cref="NLoptConfiguration"/> representation.
        /// </summary>
        /// <value>The configuration of the NLopt algorithm in its <see cref="NLoptConfiguration"/> representation.</value>
        public NLoptConfiguration Configuration
        {
            get;
            private set;
        }

        /// <summary>Gets the abort (stopping) condition of the NLopt algorithm.
        /// </summary>
        /// <value>The abort (stopping) condition of the NLopt algorithm.</value>
        public NLoptAbortCondition AbortCondition
        {
            get;
            private set;
        }
        #endregion

        #region internal properties

        /// <summary>Gets a value indicating whether each <see cref="IMultiDimOptimizerAlgorithm"/> object created by the current instance is logged via a specific <see cref="ILoggerStream"/> instance.
        /// </summary>
        /// <value><c>true</c> if each <see cref="IMultiDimOptimizerAlgorithm"/> object created by the current instance is logged via a specific <see cref="ILoggerStream"/> instance; otherwise, <c>false</c>.</value>
        internal bool IsLogging
        {
            get { return m_LoggerStreamFactory != null; }
        }
        #endregion

        #region public methods

        /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm"/> object.
        /// </summary>
        /// <param name="dimension">The dimension of the unconstrainted feasible region.</param>
        /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm"/> object.</returns>
        /// <exception cref="InvalidOperationException">Thrown, if the optimization algorithm does not support a unconstraint feasible region.</exception>
        public override IMultiDimOptimizerAlgorithm Create(int dimension)
        {
            if (dimension < Constraint.MinimumDimension)
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
            else if (dimension > Constraint.MaximumDimension)
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
            return new Wrapper(this, dimension);
        }

        /// <summary>Creates a new <see cref="IMultiDimOptimizerAlgorithm" /> object.
        /// </summary>
        /// <param name="constraints">A collection of contraints for the optimization algorithm, where each constraint has been created via the <see cref="MultiDimOptimizer.Constraint"/> factory.</param>
        /// <returns>A new <see cref="IMultiDimOptimizerAlgorithm" /> object.</returns>
        public override IMultiDimOptimizerAlgorithm Create(params MultiDimOptimizer.IConstraint[] constraints)
        {
            if ((constraints == null) || (constraints.Length < 1))
            {
                throw new ArgumentNullException("constraints");
            }

            int dimension = constraints[0].Dimension;
            if (dimension < Constraint.MinimumDimension)
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
            else if (dimension > Constraint.MaximumDimension)
            {
                throw new ArgumentOutOfRangeException("dimension");
            }

            var nloptConstraints = new List<NLoptConstraint>();
            foreach (var constraint in constraints)
            {
                if ((constraint is NLoptConstraint) == false)
                {
                    throw new ArgumentException("constraints");
                }
                nloptConstraints.Add(constraint as NLoptConstraint);
            }
            return new Wrapper(this, dimension, nloptConstraints);
        }

        /// <summary>Gets informations of the current object as a specific <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput" /> instance.
        /// </summary>
        /// <param name="infoOutput">The <see cref="T:Dodoni.BasicComponents.Containers.InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
        /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
        public override void FillInfoOutput(BasicComponents.Containers.InfoOutput infoOutput, string categoryName = "General")
        {
            base.FillInfoOutput(infoOutput, categoryName);
            var infoOutputPackage = infoOutput.AcquirePackage(categoryName);
            Configuration.FillInfoOutput(infoOutput, categoryName + ".Configuration");
        }
        #endregion

        #region protected methods

        /// <summary>Gets descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="MultiDimOptimizer.IConstraintFactory" /> representation.
        /// </summary>
        /// <returns>Descriptor and factory of constraints for the algorithm represented by the current instance in its <see cref="MultiDimOptimizer.IConstraintFactory" /> representation.</returns>
        protected override MultiDimOptimizer.IConstraintFactory GetConstraintFactory()
        {
            return this.Constraint;
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction" /> objects that encapsulate the function to optimize.
        /// </summary>
        /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction" /> objects that encapsulate the function to optimize.</returns>
        protected override MultiDimOptimizer.IFunctionFactory GetFunctionFactory()
        {
            return this.Function;
        }

        /// <summary>Gets a factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.
        /// </summary>
        /// <returns>A factory for <see cref="MultiDimOptimizer.IFunction"/> objects that encapsulate the function to optimize.</returns>
        protected override OrdinaryMultiDimOptimizer.IFunctionFactory GetOrdinaryFunctionDescriptor()
        {
            return this.Function;
        }

        /// <summary>Gets a value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.
        /// </summary>
        /// <returns>A value indicating whether this instance is a random algorithm, i.e. applying the algorithm to the same initial value may yields to different results.</returns>
        protected override bool GetIsRandomAlgorithm()
        {
            return Configuration.IsRandomAlgorithm;
        }

        /// <summary>Gets the name of the multi-dimensional optimizer.
        /// </summary>
        /// <returns>The name of the multi-dimensional optimizer.</returns>
        protected override IdentifierString GetName()
        {
            return m_Name;
        }

        /// <summary>Gets the long name of the multi-dimensional optimizer.
        /// </summary>
        /// <returns>The (perhaps) language dependent long name of the multi-dimensional optimizer.</returns>
        protected override IdentifierString GetLongName()
        {
            return m_LongName;
        }
        #endregion
    }
}