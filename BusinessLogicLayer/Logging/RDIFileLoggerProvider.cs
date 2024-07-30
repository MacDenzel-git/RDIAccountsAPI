using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logging
{
    [ProviderAlias("RoundTheCodeFile")]
    public class RDIFileLoggerProvider : ILoggerProvider
    {
        public readonly RDIFileLoggerOptions _options;
        public RDIFileLoggerProvider(IOptions<RDIFileLoggerOptions> options)
        {
            _options = options.Value;
            if (_options.FolderPath.ToLower().Contains("year") && _options.FolderPath.ToLower().Contains("year"))
            {

            }
            else
            {
                if (!Directory.Exists(_options.FolderPath))
                {
                    Directory.CreateDirectory(_options.FolderPath);
                }
            }
           
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new RDIFileLogger(this);
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
