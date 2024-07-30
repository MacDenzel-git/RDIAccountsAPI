using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logging
{
    
    public class RDIFileLogger : ILogger
    {
        public RDIFileLoggerProvider _roundTheCodeFileLoggerProvider;
        public RDIFileLogger([NotNull] RDIFileLoggerProvider roundTheCodeFileLoggerProvider)
        {
                _roundTheCodeFileLoggerProvider = roundTheCodeFileLoggerProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

            if (!IsEnabled(logLevel))
            {
                return;
            }

            var folder = "";
            var fullFilePath = "";
            if (logLevel.ToString().ToLower().Equals("error"))
            {
                folder = string.Format("{0}", _roundTheCodeFileLoggerProvider._options.FolderPath.Replace("{year}", DateTime.Now.Year.ToString()).Replace("{month}", DateTime.Now.Month.ToString().Replace("{day}", DateTime.Now.Month.ToString())));
                fullFilePath = string.Format("{0}/{1}", folder,$"ERROR_LOG_{DateTime.UtcNow.ToString("yyyyMMdd")}.log");

            }
            else if (logLevel.Equals("info")) { }
            {
                folder = string.Format("{0}", _roundTheCodeFileLoggerProvider._options.FolderPath.Replace("{year}", DateTime.Now.Year.ToString()).Replace("{month}", DateTime.Now.Month.ToString().Replace("{day}", DateTime.Now.Month.ToString())));
                fullFilePath = string.Format("{0}/{1}", folder, $"INFO_LOG_{DateTime.Now.ToString("yyyyMMdd")}.log");
                //fullFilePath = string.Format("{0}/{1}",folder, _roundTheCodeFileLoggerProvider._options.InformationFilePath.Replace("{date}", DateTime.UtcNow.ToString("yyyyMMdd")));
            }
            //setup patj

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            //actual log
            var logRecord = string.Format("{0} [{1}] {2} {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logLevel.ToString(),formatter(state,exception),(exception != null ? exception.StackTrace :"")) ;

            using (var streamWriter = new StreamWriter(fullFilePath,true))
            {
                //Append to file 
                streamWriter.WriteLine(logRecord);
            }
        }
    }
}
