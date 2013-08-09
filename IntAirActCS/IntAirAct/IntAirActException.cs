using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirAct
{
    class IntAirActException : Exception
    {
        public IntAirActException() { }
        public IntAirActException(string message) : base(message) { }
        public IntAirActException(string message, Exception inner) : base(message, inner) { }
    }
}
