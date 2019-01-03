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
using System.Composition;
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Represents a wrapper for BLAS operation with respect to Intel's MKL Library 10.3.
    /// </summary>
    [Export(typeof(BLAS.ILibrary))]
    public partial class MklBlasNativeWrapper : BlasNativeWrapper
    {
        /// <summary>Initializes a new instance of the <see cref="MklBlasNativeWrapper" /> class.
        /// </summary>
        public MklBlasNativeWrapper()
            : base(
            new IdentifierString("MKL BLAS"),
            new MklLevel1BLAS(),
            new Level2BLAS(),
            new MklLevel3BLAS())
        {
        }

        /// <summary>Gets a description of the BLAS Library.
        /// </summary>
        /// <value>The description of the BLAS Library.</value>
        public override string Annotation
        {
            get { return String.Format(ResourceFile.DescriptionBlas, BlasNativeWrapper.dllName); }
        }
    }
}