using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirActTests
{
    class IAModelWithIntProperty
    {
        public int Number { get; set; }

        public override string ToString()
        {
            return String.Format("IAModelWithIntProperty[Number: {0}]", Number);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IAModelWithIntProperty) == null)
            {
                return false;
            }

            IAModelWithIntProperty model = (IAModelWithIntProperty)obj;
            return (this.Number == model.Number);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.Number.GetHashCode();
            return hash;
        }
    }
}
