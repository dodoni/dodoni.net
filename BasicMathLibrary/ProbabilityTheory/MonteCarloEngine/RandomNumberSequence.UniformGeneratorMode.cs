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
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

namespace Dodoni.MathLibrary.ProbabilityTheory.MonteCarloEngine
{
    /// <summary>Provides interfaces and structures etc. for handling Random Number sequences.
    /// </summary>
    public partial class RandomNumberSequence
    {
        /// <summary>Serves as abstract class which represents a library specific generation mode for uniform distributed random numbers.
        /// </summary>
        public abstract class UniformGenerationMode : IIdentifierNameable, IAnnotatable
        {
            #region private members

            /// <summary>The name of the generation method.
            /// </summary>
            private IdentifierString m_Name;

            /// <summary>The annotation of the generation method.
            /// </summary>
            private string m_Annotation;
            #endregion

            #region protected constructors

            /// <summary>Initializes a new instance of the <see cref="UniformGenerationMode"/> class.
            /// </summary>
            /// <param name="name">The name of the generation method for uniform distributed random numbers.</param>
            /// <param name="annotation">The annotation (description) of the generation method.</param>
            protected UniformGenerationMode(IdentifierString name, string annotation)
            {
                m_Name = name ?? throw new ArgumentNullException(nameof(name));
                m_Annotation = annotation;
            }
            #endregion

            #region public properties / methods

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the current instance.
            /// </summary>
            /// <value>The language independent name of the current instance.</value>
            public IdentifierString Name
            {
                get { return m_Name; }
            }

            /// <summary>Gets the long name of the current instance.
            /// </summary>
            /// <value>The language dependent long name of the current instance.</value>
            public IdentifierString LongName
            {
                get { return m_Name; }
            }
            #endregion

            #region IAnnotatable Members

            /// <summary>Gets a value indicating whether the annotation is readonly.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.
            /// </value>
            bool IAnnotatable.HasReadOnlyAnnotation
            {
                get { return true; }
            }

            /// <summary>Gets the annotation of the current instance.
            /// </summary>
            /// <value>The annotation of the current instance.</value>
            public string Annotation
            {
                get { return m_Annotation; }
            }

            /// <summary>Sets the annotation of the current instance.
            /// </summary>
            /// <param name="annotation">The annotation.</param>
            /// <returns>A value indicating whether the <see cref="Annotation"/> has been changed.
            /// </returns>
            bool IAnnotatable.TrySetAnnotation(string annotation)
            {
                return false;
            }
            #endregion

            /// <summary>Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return m_Name.String;
            }
            #endregion
        }
    }
}
