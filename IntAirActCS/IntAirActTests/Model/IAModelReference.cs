using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirActTests
{
    class IAModelReference
    {
        public IAModelWithIntProperty Number { get; set; }

        public override string ToString()
        {
            return String.Format("IAModelReference[Number: {0}]", Number);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IAModelReference) == null)
            {
                return false;
            }

            IAModelReference model = (IAModelReference)obj;
            return (this.Number == model.Number) || (this.Number != null && this.Number.Equals(model.Number));
        }

        public override int GetHashCode()
        {
            int hash = 3;
            hash = hash * 31 + this.Number.GetHashCode();
            return hash;
        }
    }
}
