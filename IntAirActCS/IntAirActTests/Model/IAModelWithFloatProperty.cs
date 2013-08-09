using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirActTests
{
    class IAModelWithFloatProperty
    {
        public float Number { get; set; }

        public override string ToString()
        {
            return String.Format("IAModelWithFloatProperty[Number: {0}]", Number);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IAModelWithFloatProperty) == null)
            {
                return false;
            }

            IAModelWithFloatProperty model = (IAModelWithFloatProperty)obj;
            return (this.Number == model.Number);
        }

        public override int GetHashCode()
        {
            int hash = 3;
            hash = hash * 31 + this.Number.GetHashCode();
            return hash;
        }
    }
}
