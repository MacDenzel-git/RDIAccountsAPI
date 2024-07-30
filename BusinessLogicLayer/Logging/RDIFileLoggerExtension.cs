using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logging
{
    public static class RDIFileLoggerExtension
    {
        public static ILoggingBuilder AddRoundCodeFileLogger(this ILoggingBuilder builder, Action<RDIFileLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, RDIFileLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
