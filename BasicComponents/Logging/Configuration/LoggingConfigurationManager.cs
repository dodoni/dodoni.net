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
using System.Configuration;
using System.Composition;
using System.Composition.Hosting;

using Microsoft.Extensions.Logging;

namespace Dodoni.BasicComponents.Logging.Configuration
{
    /// <summary>Serves as factory for a specific <see cref="ILogger"/> object with respect to Managed Extensibility Framework (MEF) and the configuration file.
    /// </summary>
    public class LoggingConfigurationManager
    {
        /// <summary>Gets the logger in its <see cref="IloggerStreamFactory"/> representation.
        /// </summary>
        /// <value>The logger in its <see cref="IloggerStreamFactory"/> representation.</value>
        [Import]
        public IloggerStreamFactory Value { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="LoggingConfigurationManager" /> class.
        /// </summary>
        internal LoggingConfigurationManager()
        {
            try
            {
                var settings = ConfigurationManager.GetSection("LoggingSetting") as LoggingConfigurationFileSection;

                var type = Type.GetType(settings.TypeName, throwOnError: true, ignoreCase: true);

                var configuration = new ContainerConfiguration().WithAssembly(type.Assembly);
                var container = configuration.CreateContainer();
                Value = container.GetExport<IloggerStreamFactory>();
            }
            catch (Exception e)
            {
                IloggerStreamFactory standardLogger = new StandardLoggerStreamFactory();
                standardLogger.Log(LogLevel.Critical, e, "Configuration file corrupt or invalid, use standard logger implementation.");

                Value = standardLogger;
            }
        }
    }
}