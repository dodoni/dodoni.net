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
using System.Composition;
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Represents the Fast-Fourier transformations with respect to the FFTW library, release 3.3.x.
    /// </summary>
    /// <remarks>Non-free lizenses of the FFTW library are available, for more informations, see http://www.fftw.org
    /// Copyright (c) 2003, 2007-8 Matteo Frigo 
    /// Copyright (c) 2003, 2007-8 Massachusetts Institute of Technology
    /// </remarks>    
    [Export(typeof(FFT.ILibrary))]
    public partial class FftwNativeWrapper :  FFT.ILibrary
    {
        #region public const members

        /// <summary>The name of the native dll.
        /// </summary>
        public const string dllName = "libfftw3_3.dll";

        /// <summary>The calling convention of the external dll.
        /// </summary>
        public const CallingConvention callingConvention = CallingConvention.Cdecl;
        #endregion

        #region private members

        /// <summary>The name of the Library.
        /// </summary>
        private IdentifierString m_Name;

        /// <summary>The factory for one-dimensional Fast-Fourier Transformations.
        /// </summary>
        private IOneDimFourierTransformationFactory m_OneDimensionalFactory;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="FftwNativeWrapper" /> class.
        /// </summary>
        public FftwNativeWrapper()
        {
            m_Name = new IdentifierString("FFTW 3.3.x");
            m_OneDimensionalFactory = new FftwOneDimFourierTransformationFactory();
        }
        #endregion

        #region public properties

        #region IIdentifierNameable Members

        /// <summary>Gets the name of the Fast-Fourier Transformation Library.
        /// </summary>
        /// <value>The name of the Fast-Fourier Transformation Library.</value>
        public IdentifierString Name
        {
            get { return m_Name; }
        }

        /// <summary>Gets the long name of the Fast-Fourier Transformation Library.
        /// </summary>
        /// <value>The long name of the Fast-Fourier Transformation Library.</value>
        public IdentifierString LongName
        {
            get { return m_Name; }
        }
        #endregion

        #region IAnnotatable Members

        /// <summary>Gets a value indicating whether the annotation is read-only.
        /// </summary>
        /// <value><c>true</c> if the annotation of this instance is readonly; otherwise, <c>false</c>.</value>
        bool IAnnotatable.HasReadOnlyAnnotation
        {
            get { return true; }
        }

        /// <summary>Gets a description of the Fast-Fourier Transformation Library.
        /// </summary>
        /// <value>The description of the Fast-Fourier Transformation Library.</value>
        public string Annotation
        {
            get { return String.Format(ResourceFile.Description, dllName); }
        }
        #endregion

        #region ILibrary Members

        /// <summary>Gets the factory for one dimensional Fast-Fourier Transformation.
        /// </summary>
        /// <value>The factory for one dimensional Fast-Fourier Transformation.
        /// </value>
        public IOneDimFourierTransformationFactory OneDimensionalFactory
        {
            get { return m_OneDimensionalFactory; }
        }
        #endregion

        #endregion

        #region public methods

        #region IAnnotatable Members

        /// <summary>Sets the annotation of the current instance.
        /// </summary>
        /// <param name="annotation">The annotation.</param>
        /// <returns>A value indicating whether the <see cref="Annotation" /> has been changed.</returns>
        bool IAnnotatable.TrySetAnnotation(string annotation)
        {
            return false;
        }
        #endregion

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return m_Name.String;
        }
        #endregion
    }
}