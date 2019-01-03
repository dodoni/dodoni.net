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

namespace Dodoni.XLBasicComponents
{
    /// <summary>Serves as interface for a user control of the configuration of Dodoni.NET.
    /// </summary>
    public interface IConfigurationUserControl : IContainerControl
    {
        /// <summary>Gets the title of the user control in its <see cref="System.String"/> representation.
        /// </summary>
        /// <value>The title of the user control.</value>
        string Title
        {
            get;
        }

        /// <summary>Gets a value indicating whether the user changed the configuration.
        /// </summary>
        /// <value><c>true</c> if the user changed the configuration; otherwise, <c>false</c>.</value>
        bool ConfigurationChanged
        {
            get;
        }

        /// <summary>Store the configuration, i.e. write a specific configuration file.
        /// </summary>
        void StoreConfiguration();

        /// <summary>Restores the configuration, i.e. reads the original configuration from some
        /// configuration file or take some default values and show these configuration.
        /// </summary>
        void RestoreConfiguration();
    }
}