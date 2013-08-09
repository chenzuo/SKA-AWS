using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirActTests
{
    class IAModelInheritance : IAModelWithIntProperty
    {
        public int NumberTwo { get; set; }

        public override string ToString()
        {
            return String.Format("IAModelInheritance[Number: {0}, NumberTwo: {1}]", Number, NumberTwo);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IAModelInheritance) == null)
            {
                return false;
            }

            IAModelInheritance model = (IAModelInheritance)obj;
            return (this.Number == model.Number) && (this.NumberTwo == model.NumberTwo);
        }

        public override int GetHashCode()
        {
            int hash = 2;
            hash = hash * 31 + this.Number.GetHashCode();
            hash = hash * 31 + this.NumberTwo.GetHashCode();
            return hash;
        }
    }
}
