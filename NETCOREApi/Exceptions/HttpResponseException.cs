using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREApi.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 600;
        public object Value { get; set; }
    }
}
