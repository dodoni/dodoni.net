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

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Represents the 'partial build-in' Special Function Library.
    /// </summary>
    internal class BuildInPartialSpecialFunctionLibrary : SpecialFunction.ILibrary
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="BuildInPartialSpecialFunctionLibrary" /> class.
        /// </summary>
        internal BuildInPartialSpecialFunctionLibrary()
        {
            Name = LongName = new IdentifierString("Build-In (partial)");

            PrimitiveIntegral = new PartialBuildInPrimitiveIntegralSpecialFunctions();
            Gamma = new DummyGammaSpecialFunctions();
            Hypergeometric = new DummyBuildInHypergeometricSpecialFunctions();
            IteratedExponential = new DummyBuildInIteratedExponentialSpecialFunctions();
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the current instance.
        /// </summary>
        /// <value>The language independent name of the current instance.</value>
        public IdentifierString Name
        {
            get;
            private set;
        }

        /// <summary>Gets the long name of the current instance.
        /// </summary>
        /// <value>The (perhaps) language dependent long name of the current instance.</value>
        public IdentifierString LongName
        {
            get;
            private set;
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        public bool HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets the annotation of the current instance.
        /// </summary>
        /// <value>The annotation of the current instance.</value>
        public string Annotation
        {
            get { return LowLevelMathConfigurationResources.DescriptionSpecialFunctionBuildInNone; }
        }
        #endregion

        #region ILibrary Members

        /// <summary>Gets a collection of methods for calculation of Gamma and related functions.
        /// </summary>
        /// <value>A collection of methods for calculation of Gamma and related functions.</value>
        public GammaSpecialFunctions Gamma
        {
            get;
            private set;
        }

        /// <summary>Gets a collection of methods for calculation of Hypergeometric and related functions.
        /// </summary>
        /// <value>A collection of methods for calculation of Hypergeometric and related functions.</value>
        public IHypergeometricSpecialFunctions Hypergeometric
        {
            get;
            private set;
        }

        /// <summary>Gets a collection of methods for calculation of Iterated Exponential and related functions.
        /// </summary>
        /// <value>A collection of methods for calculation of Iterated Exponential and related functions.</value>
        public IIteratedExponentialSpecialFunctions IteratedExponential
        {
            get;
            private set;
        }

        /// <summary>Gets a collection of methods for calculation of antiderivatives (i.e. Primitive Integral, Indefinite Integral) of elementary functions.
        /// </summary>
        /// <value>A collection of methods for calculation of antiderivatives (i.e. Primitive Integral, Indefinite Integral) of elementary functions.</value>
        public PrimitiveIntegralSpecialFunctions PrimitiveIntegral
        {
            get;
            private set;
        }
        #endregion

        #endregion

        #region public methods

        #region ILibrary Members

        /// <summary>Initializes the Library.
        /// </summary>
        /// <remarks>Call this method before using the Library the first time.</remarks>
        public void Initialize()
        {
            // nothing to do here
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation" /> has been changed.</returns>
        public bool TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        #endregion
    }
}