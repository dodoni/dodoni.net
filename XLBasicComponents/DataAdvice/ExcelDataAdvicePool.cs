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

namespace Dodoni.XLBasicComponents
{
    /// <summary>Serves as collection for <see cref="IExcelDataAdvice"/> objects.
    /// </summary>
    public class ExcelDataAdvicePool
    {
        #region internal (readonly) members

        /// <summary>Represents the <see cref="IExcelDataAdvice"/> for Boolean values, i.e. represents 'false' and 'true' with respect to a user configuration.
        /// </summary>
        internal readonly BooleanDataAdvice m_BooleanAdvice = new BooleanDataAdvice();
        #endregion

        #region private members

        /// <summary>The Excel data advice.
        /// </summary>
        private IdentifierNameableDictionary<IExcelDataAdvice> m_ExcelDataAdvice = new IdentifierNameableDictionary<IExcelDataAdvice>(capacity: 50, isReadOnlyExceptAdding: false);

        /// <summary>Occurs quering an element of the pool.
        /// </summary>
        private event Action<ExcelDataAdvice.InitializeEventArgs> m_Initialize;

        /// <summary>A value indicating whether the user has added or removed at least one event to the <see cref="Initialize"/> event handler
        /// since the last call of the event-handler.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private bool m_DataAdviceInitializeChanged = true;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelDataAdvicePool"/> class.
        /// </summary>
        internal ExcelDataAdvicePool()
        {
            m_ExcelDataAdvice.Add(m_BooleanAdvice);
        }
        #endregion

        #region public properties

        /// <summary>Gets the number of elements.
        /// </summary>
        /// <value>The number of elements.</value>
        public int Count
        {
            get { return m_ExcelDataAdvice.Count; }
        }

        /// <summary>Occurs before quering an element of the pool.
        /// </summary>
        /// <value>The initialize event handler.</value>
        public event Action<ExcelDataAdvice.InitializeEventArgs> Initialize
        {
            add
            {
                m_DataAdviceInitializeChanged = true;
                m_Initialize += value;
            }
            remove
            {
                m_DataAdviceInitializeChanged = true;
                m_Initialize -= value;
            }
        }

        /// <summary>Gets the <see cref="IExcelDataAdvice"/> for Boolean values, i.e. represents 'false' and 'true' with respect to a user configuration.
        /// </summary>
        /// <value>The <see cref="IExcelDataAdvice"/> for Boolean values.</value>
        public IExcelDataAdvice Booleans
        {
            get { return m_BooleanAdvice; }
        }
        #endregion

        #region internal methods

        /// <summary>Add a specific <see cref="IExcelDataAdvice"/> object into the <see cref="ExcelDataAdvicePool"/>.
        /// </summary>
        /// <param name="value">The excel data advice to register.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
        internal ItemAddedState Add(IExcelDataAdvice value)
        {
            return m_ExcelDataAdvice.Add(value);
        }
        #endregion

        #region public methods

        /// <summary>Gets a specific <see cref="IExcelDataAdvice"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetValue(string name, out IExcelDataAdvice value)
        {
            OnInitialize();
            return m_ExcelDataAdvice.TryGetValue(name, out value);
        }

        /// <summary>Gets a specific <see cref="IExcelDataAdvice"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public bool TryGetValue(IdentifierString name, out IExcelDataAdvice value)
        {
            OnInitialize();
            return m_ExcelDataAdvice.TryGetValue(name, out value);
        }

        /// <summary>Gets a collection of the pool item names.
        /// </summary>
        /// <returns>A collection of the pool item names.</returns>
        public IEnumerable<string> GetNames()
        {
            OnInitialize();
            return m_ExcelDataAdvice.Names;
        }
        #endregion

        #region private methods

        /// <summary>Raises the <see cref="Initialize"/> event.
        /// </summary>
        /// <remarks>Adds elements to the pool, if needed.</remarks>
        private void OnInitialize()
        {
            if (m_DataAdviceInitializeChanged == true)
            {
                if (m_Initialize != null)
                {
                    ExcelDataAdvice.InitializeEventArgs eventArgs = new ExcelDataAdvice.InitializeEventArgs();
                    m_Initialize(eventArgs);
                }
                m_DataAdviceInitializeChanged = true;
            }
        }
        #endregion
    }
}