using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataTransferObjects
{
    public class TestOutputHandler
    {
        public bool _IsErrorOccured { get; set; }

        public object Result { get; set; }

        public bool IsErrorKnown { get; set; }

        public string Message { get; set; }

        public bool IsErrorOccured
        {
            get => _IsErrorOccured;
              set {
                if (value == true)
                {
                    Debug.WriteLine(Message);
                }
            }
        }
    }
}
