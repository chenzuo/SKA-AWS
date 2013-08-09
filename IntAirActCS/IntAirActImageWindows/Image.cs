using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirActImageWindows
{
    public class Image
    {
        public string identifier { get; set; }

        public override string ToString()
        {
            return String.Format("Image[identifier: {0}]", identifier);
        }
    }
}
