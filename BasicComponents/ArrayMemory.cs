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
namespace Dodoni.BasicComponents
{
    /// <summary>Provides functions concerning memory allocation.
    /// </summary>
    public static class ArrayMemory
    {
        /// <summary>(Re-)Allocate memory for a specified array length.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="values">If <paramref name="values"/> contains less than <paramref name="minimalSize"/> elements an array with at least <paramref name="minimalSize"/> elements will be returned.</param>
        /// <param name="minimalSize">The minimal size of the array.</param>
        /// <param name="pufferSize">The size of the puffer.</param>
        /// <returns>A value indicating whether the elements of <paramref name="values"/> has been changed.</returns>
        /// <remarks>In the case of a memory allocation a array with at least <paramref name="minimalSize"/> plus <paramref name="pufferSize"/> will be created.</remarks>
        public static bool Reallocate<T>(ref T[] values, int minimalSize, int pufferSize = 0)
        {            
            if ((values == null) || (values.Length < minimalSize))
            {
                values = new T[minimalSize + pufferSize];
                return true;
            }
            return false;
        }
    }
}