using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logging
{
    public class RDIFileLoggerOptions
    {
        public virtual string ErrorFilePath { get; set; }
        public virtual string InformationFilePath { get; set; }
        public virtual string FolderPath { get; set; }
    }
}
