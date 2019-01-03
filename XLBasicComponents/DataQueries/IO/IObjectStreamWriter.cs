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
using System.IO;
using System.Text;
using System.Collections.Generic;

using Dodoni.BasicComponents;

namespace Dodoni.XLBasicComponents.IO
{
    /// <summary>Serves as interface for writing objects into a specific stream.
    /// </summary>
    public interface IObjectStreamWriter : IOperable
    {
        /// <summary>Opens the <see cref="IObjectStreamWriter"/> object with a specific <see cref="StreamWriter"/> object.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <remarks>Except for the first time call <see cref="IObjectStreamWriter.Close(bool)"/> before using a other <see cref="StreamWriter"/> object.</remarks>
        void Open(StreamWriter streamWriter);

        /// <summary>Closes the <see cref="IObjectStreamWriter"/> object and the underlying stream,
        /// and releases any system resources associated with the reader.
        /// </summary>
        /// <param name="closeEmbeddedStream">A value indicating whether the underlying <see cref="StreamWriter"/> should be closed as well.</param>
        void Close(bool closeEmbeddedStream = true);

        /// <summary>Writes the <see cref="GuidedExcelDataQuery"/> representation of a specific <see cref="ExcelPoolItem"/> object into the stream.
        /// </summary>
        /// <param name="value">The <see cref="ExcelPoolItem"/> to store.</param>
        /// <remarks>The stream must be filled in a valid order taken into account the dependency structure, i.e. first 'independent' objects will be stored, afterwards
        /// objects which needes as input 'independent' objects will be stored etc. However implementations stores the <see cref="GuidedExcelDataQuery"/> representation 
        /// of <paramref name="value"/> only, thus the dependency structure, must be managed by the caller of the method.
        /// </remarks>
        void WriteObject(ExcelPoolItem value);
    }
}