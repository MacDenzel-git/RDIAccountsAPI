using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class RDIExtensions
    {
       
            public static string GetInitials(this string value)
               => string.Concat(value
                  .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                  .Where(x => x.Length >= 1 && char.IsLetter(x[0]))
                  .Select(x => char.ToUpper(x[0])));
        
    }
}
