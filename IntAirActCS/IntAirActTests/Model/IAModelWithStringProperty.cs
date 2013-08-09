using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntAirActTests
{
    class IAModelWithStringProperty
    {
        public string StringProperty { get; set; }

        public override string ToString()
        {
            return String.Format("IAModelWithStringProperty[StringProperty: {0}]", StringProperty);
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || (obj as IAModelWithStringProperty) == null)
            {
                return false;
            }

            IAModelWithStringProperty model = (IAModelWithStringProperty)obj;
            return (this.StringProperty != null && this.StringProperty.Equals(model.StringProperty));
        }

        public override int GetHashCode()
        {
            int hash = 2;
            hash = hash * 31 + this.StringProperty.GetHashCode();
            return hash;
        }
    }
}
