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
using System.Windows.Forms;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>A Windows form which shows a progress bar for time consuming calculations.
    /// </summary>
    internal partial class ExcelBackgroundWorkerForm : Form
    {
        #region internal constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelBackgroundWorkerForm"/> class.
        /// </summary>
        internal ExcelBackgroundWorkerForm()
        {
            InitializeComponent();
        }
        #endregion

        #region internal methods

        /// <summary>Initializes the current <see cref="ExcelBackgroundWorkerForm"/>.
        /// </summary>
        /// <param name="title">The title of the Windows form.</param>
        /// <param name="progressBarMaximum">The maximum of the progress bar; <c>0</c> if unknown, <see cref="ProgressBarStyle.Marquee"/> is used in this case.</param>
        /// <param name="progressBarStepSize">The step size of the progress bar.</param>
        internal void Initialize(string title, int progressBarMaximum = 0, int progressBarStepSize = 1)
        {
            progressBar.Value = 0;

            this.Text = title;
            if (progressBarMaximum == 0)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.MarqueeAnimationSpeed = 40;
            }
            else if (progressBarMaximum > 0)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Maximum = progressBarMaximum;
                progressBar.Step = progressBarStepSize;
            }
            else
            {
                throw new ArgumentOutOfRangeException("progressBarMaximum");
            }
        }

        /// <summary>Show a description and if the progress bar style is not <see cref="ProgressBarStyle.Marquee"/> advances the current position of the progress bar by the amount of the step size.
        /// </summary>
        /// <param name="description">The description.</param>
        internal void PerformStep(string description = null)
        {
            label.Text = description;
            if (progressBar.Style != ProgressBarStyle.Marquee)
            {
                progressBar.PerformStep();
            }
        }
        #endregion

        #region internal properties

        /// <summary>Occurs when the abort button is clicked.
        /// </summary>
        /// <value>The abort event.</value>
        internal event EventHandler AbortClick
        {
            add { bAbort.Click += value; }
            remove { bAbort.Click -= value; }
        }
        #endregion
    }
}