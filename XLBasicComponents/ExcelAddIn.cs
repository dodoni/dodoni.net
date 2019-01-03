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

using ExcelDna.Integration;

using Dodoni.BasicComponents.Logging;
using Dodoni.XLBasicComponents.Logging;
using Dodoni.XLBasicComponents.Utilities;
using ExcelDna.IntelliSense;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Represents the Excel-AddIn, i.e. the Excel-Menu entries for Dodoni.NET.
    /// </summary>
    /// <remarks>This class, especially the <c>AutoOpen</c> method must be extend to establish the functionallity for reading objects from a specific file.</remarks>
    public class ExcelAddIn : IExcelAddIn
    {
        #region public (readonly) members

        /// <summary>A reference to the Excel object. 
        /// </summary>
        public static readonly dynamic ExcelApplication;

        /// <summary>The configuration of the Excel-AddIn.
        /// </summary>
        public static readonly ExcelConfiguration Configuration;
        #endregion

        #region internal const members

        /// <summary>The name of the Excel menue, i.e. 'Dodoni'.
        /// </summary>
        internal const string MenuName = "Dodoni";

        /// <summary>The name of the Excel-category for general functions.
        /// </summary>
        internal const string GeneralCategoryName = "Dodoni";

        /// <summary>The name of the Excel-category for utility functions.
        /// </summary>
        internal const string UtilityCategoryName = "Dodoni.Utilities";

        /// <summary>The name of the Excel-category of math functions.
        /// </summary>
        internal const string MathCategoryName = "Dodoni.MathLib";

        /// <summary>The name of the Excel-category of basic finance functions.
        /// </summary>
        internal const string FinanceBasicCategoryName = "Dodoni.Finance";
        #endregion

        #region private static members

        /// <summary>The configuration form.
        /// </summary>
        private static ConfigurationForm sm_ConfigForm;

        /// <summary>The windows form which contains the global logfile.
        /// </summary>
        private static GlobalLogFileForm sm_GlobalLoggingForm;

        /// <summary>The pool inspector windows form.
        /// </summary>
        private static ExcelPoolInspectorForm sm_PoolInspector;
        #endregion

        #region constructors

        /// <summary>Initializes the <see cref="ExcelAddIn"/> class.
        /// </summary>
        static ExcelAddIn()
        {
            Configuration = ExcelConfiguration.Create();

            ExcelApplication = ExcelDnaUtil.Application;
            Application.EnableVisualStyles();  // required for Marquee progress bar style
        }

        /// <summary>Initializes a new instance of the <see cref="ExcelAddIn"/> class.
        /// </summary>
        public ExcelAddIn()
        {
        }
        #endregion

        #region public methods

        #region IExcelAddIn Members

        /// <summary>Initialize the <see cref="IExcelAddIn"/>.
        /// </summary>
        public void AutoOpen()
        {
            try
            {
                IntelliSenseServer.Install();
                ExcelIntegration.RegisterUnhandledExceptionHandler(UnhandledExceptionHandler);  // add a 'standard-output' if a try-catch block is missing
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("{0} Stack trace: {1}", e.Message, e.StackTrace), "Dodoni.net (XL-BasicComponents): Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Performs operations if Excel will be closed.
        /// </summary>
        public void AutoClose()
        {
        }
        #endregion

        #region Menu methods

        /// <summary>Shows the About box.
        /// </summary>
        [ExcelCommand(MenuText = "About", MenuName = ExcelAddIn.MenuName)]
        public static void ShowAboutBox()
        {
            AboutDodoniForm aboutForm = new AboutDodoniForm();
            aboutForm.ShowDialog();
        }

        /// <summary>Shows the configuration Windows form of Dodoni.NET.
        /// </summary>
        [ExcelCommand(MenuText = "Configuration", MenuName = ExcelAddIn.MenuName)]
        public static void ShowConfiguration()
        {
            if (sm_ConfigForm == null)
            {
                sm_ConfigForm = new ConfigurationForm();
            }
            sm_ConfigForm.ShowDialog();
        }

        /// <summary>Shows the global logging.
        /// </summary>
        [ExcelCommand(MenuText = "Global logging", MenuName = ExcelAddIn.MenuName)]
        public static void ShowGlobalLogFile()
        {
            if (sm_GlobalLoggingForm == null)
            {
                sm_GlobalLoggingForm = new GlobalLogFileForm();

                if (Logger.Stream is ExcelLogger)
                {
                    sm_GlobalLoggingForm.SetDataSources((Logger.Stream as ExcelLogger).DataSource);
                }
            }
            sm_GlobalLoggingForm.Show();
        }

        /// <summary>Shows the Pool Inspector Windows form.
        /// </summary>
        [ExcelCommand(MenuText = "Poolinspector", MenuName = ExcelAddIn.MenuName)]
        public static void ShowPoolInspector()
        {
            if (sm_PoolInspector == null)
            {
                sm_PoolInspector = new ExcelPoolInspectorForm();
            }
            sm_PoolInspector.Show();
        }
        #endregion

        #endregion

        #region private static methods

        /// <summary>Serves as method which will be called in a case that an exception is thrown.
        /// </summary>
        /// <param name="exceptionObject">The exception object.</param>
        /// <returns>The <see cref="System.String"/> representation of the exception.</returns>
        private static object UnhandledExceptionHandler(object exceptionObject)
        {
            try
            {
                if (exceptionObject != null)
                {
                    return ExcelDataConverter.GetExcelRangeErrorMessage(exceptionObject.ToString());
                }
                return ExcelDataConverter.GetExcelRangeErrorMessage("Unknown error.");
            }
            catch (Exception exception)
            {
                return exception.Message + ((exceptionObject != null) ? exceptionObject.ToString() : String.Empty);
            }
        }
        #endregion
    }
}