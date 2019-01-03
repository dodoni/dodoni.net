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
using Dodoni.BasicComponents.Containers;

namespace Dodoni.XLBasicComponents.IO
{
    /// <summary>Serves as interface for reading objects from a specific stream.
    /// </summary>
    public interface IObjectStreamReader : IOperable
    {
        /// <summary>Opens the <see cref="IObjectStreamReader"/> object with a specific <see cref="StreamReader"/> object.
        /// </summary>
        /// <param name="streamReader">The stream reader.</param>
        /// <remarks>Except for the first time call <see cref="IObjectStreamReader.Close(bool)"/> before using a other <see cref="StreamReader"/> object.</remarks>
        void Open(StreamReader streamReader);

        /// <summary>Closes the <see cref="IObjectStreamReader"/> object and the underlying stream, and releases any system resources associated with the reader.
        /// </summary>
        /// <param name="closeEmbeddedStream">A value indicating whether the underlying <see cref="StreamReader"/> should be closed as well.</param>
        void Close(bool closeEmbeddedStream = true);

        /// <summary>Gets the name of the objects as well as the object type.
        /// </summary>
        /// <returns>A collection, where the first component is the object type in its <see cref="ExcelPoolItemType"/> representation and the second component
        /// is the name of the object itself.</returns>
        /// <exception cref="Exception">Thrown, if the stream has a invalid format.</exception>
        IEnumerable<Tuple<ExcelPoolItemType, string>> GetObjectNames();

        /// <summary>Gets the <see cref="IExcelDataQuery"/> representation of all elements in the stream.
        /// </summary>
        /// <returns>A collection of objects taken into account the dependency structure, where the first component is the object type in its <see cref="ExcelPoolItemType"/> 
        /// representation, the second component is the name of the object and the third component is the data needed to construct the corresponding object.</returns>
        /// <exception cref="Exception">Thrown, if the stream has a invalid format.</exception>
        IEnumerable<Tuple<ExcelPoolItemType, string, IIdentifierStringDictionary<GuidedExcelDataQuery>>> GetObjects();

        /// <summary>Gets the <see cref="IExcelDataQuery"/> representation of specific objects in the stream.
        /// </summary>
        /// <param name="objectNames">The names of the objects to read from the stream. If <c>null</c> or empty the method is equal to <see cref="IObjectStreamReader.GetObjects()"/>.</param>
        /// <returns>A collection of objects taken into account the dependency structure, where the first component is the object type in its <see cref="ExcelPoolItemType"/> representation, the second component
        /// is the name of the object and the third component is the data needed to construct the corresponding object; or the first and third component is <c>null</c> if no entry with the 
        /// desired object name is available.</returns>
        /// <exception cref="Exception">Thrown, if the stream has a invalid format.</exception>
        /// <remarks>Performs a case-insensitive search. Perhaps more objects are returned because of the dependency structure, i.e. objects may need further input objects.</remarks>
        IEnumerable<Tuple<ExcelPoolItemType, string, IIdentifierStringDictionary<GuidedExcelDataQuery>>> GetObjectsByName(IEnumerable<string> objectNames);

        /// <summary>Gets the <see cref="IExcelDataQuery"/> representation of specific objects in the stream.
        /// </summary>
        /// <param name="excelPoolItemTypes">The <see cref="ExcelPoolItemType"/> objects to search;  if <c>null</c> or empty the method is equal to <see cref="IObjectStreamReader.GetObjects()"/>.</param>
        /// <returns>A collection of objects taken into account the dependency structure, where the first component is the object type in its <see cref="ExcelPoolItemType"/> representation, the second component
        /// is the name of the object and the third component is the data needed to construct the corresponding object; or the second and third component is <c>null</c> if no entry with the 
        /// desired object type is available.</returns>     
        /// <exception cref="Exception">Thrown, if the stream has a invalid format.</exception>
        /// <remarks>Perhaps more objects are returned because of dependency cascading structure, i.e. objects may need further input objects.</remarks>
        IEnumerable<Tuple<ExcelPoolItemType, string, IIdentifierStringDictionary<GuidedExcelDataQuery>>> GetObjectsByTypeName(IEnumerable<ExcelPoolItemType> excelPoolItemTypes);
    }
}