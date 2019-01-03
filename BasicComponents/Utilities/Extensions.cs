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

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Serves as collection of general extension methods.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class Extensions
    {
        /// <summary>Gets a raw distance between two dates normalized by the number of days per year, i.e. a generic day count fraction.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The number of days in the time period over the number of days per year.</returns>
        public static double GetRawDistanceTo(this DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                DateTime temp = startDate;
                startDate = endDate;
                endDate = temp;
            }
            if (startDate.Year == endDate.Year)
            {
                return endDate.Subtract(startDate).Days / ((DateTime.IsLeapYear(startDate.Year) == true) ? 366.0 : 365.0);
            }
            int numberOfYearsInBetween;
            DateTime adjEndDate;
            if (endDate.AddYears(-1) < startDate)
            {
                numberOfYearsInBetween = 0;
                adjEndDate = endDate;
            }
            else
            {
                numberOfYearsInBetween = endDate.Year - startDate.Year;
                adjEndDate = endDate.AddMonths(-12 * numberOfYearsInBetween);
            }

            // compute denominator:
            double denominator = 365.0;
            if (DateTime.IsLeapYear(adjEndDate.Year))
            {
                DateTime temp = new DateTime(adjEndDate.Year, 2, 29);
                if ((startDate <= temp) && (temp < adjEndDate))
                {
                    denominator += 1.0;
                }
            }
            else if (DateTime.IsLeapYear(startDate.Year))
            {
                DateTime temp = new DateTime(startDate.Year, 2, 29);
                if ((startDate <= temp) && (temp < adjEndDate))
                {
                    denominator += 1.0;
                }
            }
            return numberOfYearsInBetween + adjEndDate.Subtract(startDate).Days / denominator;
        }

        /// <summary>Shrinks a specific enumeration.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="elementsToSkipAtTheBeginning">The number of elements to skip at the beginning of the enumeration.</param>
        /// <param name="elementsToSkipAtTheEnd">The number of elements to skip at the end of the enumeration.</param>
        /// <returns>The subset of the <paramref name="source"/> enumeration.</returns>
        public static IEnumerable<T> Shrink<T>(this IEnumerable<T> source, int elementsToSkipAtTheBeginning, int elementsToSkipAtTheEnd)
        {
            int i = 0;
            var buffer = new Queue<T>(elementsToSkipAtTheEnd + 1);

            foreach (T item in source)
            {
                if (i >= elementsToSkipAtTheBeginning) // Read past left many elements at the start 
                {
                    buffer.Enqueue(item);
                    if (buffer.Count > elementsToSkipAtTheEnd) // Build a buffer to drop right many elements at the end 
                    {
                        yield return buffer.Dequeue();
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>Searches an entire one-dimensional sorted IList for a specific element, using the generic IComparable interface 
        /// implemented by each element of the list and by the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The sorted one-dimensional, zero-based list to search.</param>
        /// <param name="value">The object to search for.</param>
        /// <returns>The index of the specified value in the specified list, if <paramref name="value"/> is found. If <paramref name="value"/> is not 
        /// found and <paramref name="value"/> is less than one or more elements in <paramref name="list"/>, a negative number which is the bitwise 
        /// complement of the index of the first element that is larger than <paramref name="value"/>. 
        /// If <paramref name="value"/> is not found and <paramref name="value"/> is greater than any of the elements in <paramref name="list"/>, 
        /// a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
        public static int BinarySearch<T>(this IList<T> list, T value) where T : IComparable<T>
        {
            return BinarySearch(list, 0, list.Count, value);
        }

        /// <summary>Searches an entire one-dimensional sorted IList for a specific element, using the generic IComparable interface 
        /// implemented by each element of the list and by the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The sorted one-dimensional, zero-based list to search.</param>
        /// <param name="index">The starting index of the range to search.</param>
        /// <param name="length">The length of the range to search.</param>
        /// <param name="value">The object to search for.</param>
        /// <returns>The index of the specified value in the specified list, if <paramref name="value"/> is found. If <paramref name="value"/> is not 
        /// found and <paramref name="value"/> is less than one or more elements in <paramref name="list"/>, a negative number which is the bitwise 
        /// complement of the index of the first element that is larger than <paramref name="value"/>. 
        /// If <paramref name="value"/> is not found and <paramref name="value"/> is greater than any of the elements in <paramref name="list"/>, 
        /// a negative number which is the bitwise complement of (the index of the last element plus 1).</returns>
        public static int BinarySearch<T>(this IList<T> list, int index, int length, T value) where T : IComparable<T>
        {
            /* do a binary search: */
            int a = index;
            int b = length;
            int mid;

            while (a < b)
            {
                mid = (a + b) >> 1;
                if (list[mid].CompareTo(value) < 0) // Keys[mid] < key
                {
                    a = mid + 1;
                }
                else
                {
                    b = mid;
                }
            }
            if ((a < list.Count) && (list[a].CompareTo(value) == 0))
            {
                return a;
            }
            return ~b;
        }

        /// <summary>Adds a collection of items to the strongly typed list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The strongly typed list.</param>
        /// <param name="values">The items to add to the list.</param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                list.Add(value);
            }
        }
    }
}