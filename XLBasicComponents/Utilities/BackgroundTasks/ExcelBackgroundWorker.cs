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
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Represents a function used to create a <see cref="ExcelPoolItem"/> object in the background.
    /// </summary>
    /// <param name="value">The output objects (output).</param>
    /// <param name="errorMessage">The error message or undefined if no error occurs (output).</param>
    /// <returns>A value indicating whether <paramref name="value"/> contains valid data; otherwise <paramref name="errorMessage"/> contains an error message.</returns>
    public delegate bool TryCreateExcelPoolItemConcurrently(out ExcelPoolItem value, out string errorMessage);

    /// <summary>Represents a function used to create a collection of <see cref="ExcelPoolItem"/> object in the background.
    /// </summary>
    /// <param name="values">The output objects (output).</param>
    /// <param name="errorMessage">The error message or undefined if no error occurs (output).</param>
    /// <returns>A value indicating whether <paramref name="values"/> contains valid data; otherwise <paramref name="errorMessage"/> contains an error message.</returns>
    public delegate bool TryCreateExcelPoolItemsConcurrently(out IEnumerable<ExcelPoolItem> values, out string errorMessage);

    /// <summary>Creates Excel pool item objects in a separate thread.
    /// </summary>
    public class ExcelBackgroundWorker
    {
        #region private members

        /// <summary>Signals to the <see cref="CancellationToken"/> that it should be canceled
        /// </summary>
        private CancellationTokenSource m_CancellationTokenSource;

        /// <summary>Propagates notification that operations should be canceled.
        /// </summary>
        private CancellationToken m_CancellationToken;

        /// <summary>The Windows form containing a <see cref="ProgressBar"/>.
        /// </summary>
        private ExcelBackgroundWorkerForm m_Form;
        #endregion

        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelBackgroundWorker"/> class.
        /// </summary>
        public ExcelBackgroundWorker()
        {
            m_CancellationTokenSource = new CancellationTokenSource();
            m_CancellationToken = m_CancellationTokenSource.Token;

            m_Form = new ExcelBackgroundWorkerForm();
            m_Form.AbortClick += new EventHandler(m_Form_AbortClick);
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelBackgroundWorker"/> class.
        /// </summary>
        /// <param name="title">The title of the Windows form.</param>
        /// <param name="progressBarMaximum">The maximum of the progress bar; <c>0</c> if unknown, <see cref="ProgressBarStyle.Marquee"/> is used in this case.</param>
        /// <param name="progressBarStepSize">The step size of the progress bar.</param>
        public ExcelBackgroundWorker(string title, int progressBarMaximum = 0, int progressBarStepSize = 1)
            : this()
        {
            m_Form.Initialize(title, progressBarMaximum, progressBarStepSize);
        }
        #endregion

        #region public properties

        /// <summary>Gets a value that indicates whether the currently creation of <see cref="ExcelPoolItem"/> objects is to be canceled. 
        /// </summary>
        /// <value>A <see cref="CancellationToken"/> object that propagates notification that operations should be canceled.
        /// </value>
        public CancellationToken CancellationToken
        {
            get { return m_CancellationToken; }
        }
        #endregion

        #region public methods

        /// <summary>Creates a specific <see cref="ExcelPoolItem"/> object. 
        /// </summary>
        /// <param name="value">The value (output).</param>
        /// <param name="errorMessage">The error message or undefined if no error occurs (output).</param>
        /// <param name="calculationMethod">The calculation method.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data; otherwise <paramref name="errorMessage"/> contains an error message.</returns>
        public bool TryCreateItem(out ExcelPoolItem value, out string errorMessage, TryCreateExcelPoolItemConcurrently calculationMethod)
        {
            IAsyncResult asyncResult = calculationMethod.BeginInvoke(out value, out errorMessage, callBack => { this.Close(); }, null);

            switch (m_Form.ShowDialog())
            {
                case DialogResult.Abort:
                    calculationMethod.EndInvoke(out value, out errorMessage, asyncResult);
                    errorMessage = "Error! Abort by the user";
                    return false;

                case DialogResult.OK:
                    return calculationMethod.EndInvoke(out value, out errorMessage, asyncResult);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Creates a collection of <see cref="ExcelPoolItem"/> objects. 
        /// </summary>
        /// <param name="values">The values (output).</param>
        /// <param name="errorMessage">The error message or undefined if no error occurs (output).</param>
        /// <param name="calculationMethod">The calculation method.</param>
        /// <returns>A value indicating whether <paramref name="values"/> contains valid data; otherwise <paramref name="errorMessage"/> contains an error message.</returns>
        public bool TryCreateItems(out IEnumerable< ExcelPoolItem> values, out string errorMessage, TryCreateExcelPoolItemsConcurrently calculationMethod)
        {
            IAsyncResult asyncResult = calculationMethod.BeginInvoke(out values, out errorMessage, callBack => { this.Close(); }, null);

            switch (m_Form.ShowDialog())
            {
                case DialogResult.Abort:
                    calculationMethod.EndInvoke(out values, out errorMessage, asyncResult);
                    errorMessage = "Error! Abort by the user";
                    return false;

                case DialogResult.OK:
                    return calculationMethod.EndInvoke(out values, out errorMessage, asyncResult);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Initializes the current <see cref="ExcelBackgroundWorker"/> object.
        /// </summary>
        /// <param name="title">The title of the time consuming background task.</param>
        public void Initialize(string title)
        {
            if (m_Form.InvokeRequired == true)
            {
                m_Form.Invoke(new Action<string, int, int>(m_Form.Initialize), title, 0, 0);
            }
        }

        /// <summary>Initializes the current <see cref="ExcelBackgroundWorker"/> object.
        /// </summary>
        /// <param name="title">The title of the time consuming background task.</param>
        /// <param name="maximalNumberOfCalculationSteps">The maximal number of calculation steps.</param>
        /// <param name="calculationStepSize">The step size of the calculation.</param>
        public void Initialize(string title, int maximalNumberOfCalculationSteps, int calculationStepSize = 1)
        {
            if (m_Form.InvokeRequired == true)
            {
                m_Form.Invoke(new Action<string, int, int>(m_Form.Initialize), title, maximalNumberOfCalculationSteps, calculationStepSize);
            }
        }

        /// <summary>Show a description and if the progress bar style is not <see cref="ProgressBarStyle.Marquee"/> advances the current position of the progress bar by the amount of the step size.
        /// </summary>
        /// <param name="description">The description.</param>
        public void PerformCalculationStep(string description = null)
        {
            if ((m_CancellationToken.IsCancellationRequested == false) && (m_Form.InvokeRequired == true))
            {
                m_Form.Invoke(new Action<string>(m_Form.PerformStep), description);
            }
        }
        #endregion

        #region private methods

        /// <summary>Close the Windows form after a sucessfully calculation.
        /// </summary>
        private void Close()
        {
            if (m_CancellationToken.IsCancellationRequested == false)  // perhaps the Form is already closed
            {
                if (m_Form.InvokeRequired == true)
                {
                    m_Form.Invoke(new MethodInvoker(() => { m_Form.DialogResult = DialogResult.OK; }));
                }
            }
        }

        /// <summary>Handles the AbortClick event of the control with the progress bar.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void m_Form_AbortClick(object sender, EventArgs e)
        {
            m_CancellationTokenSource.Cancel();
        }
        #endregion

    }
}