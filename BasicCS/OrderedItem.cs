using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BasicCS
{
    public class OrderedItem
    {
        [XmlElement(Namespace = "http://www.cpandl.com")]
        public string ItemName;
        [XmlElement(Namespace = "http://www.cpandl.com")]
        public string Description;
        [XmlElement(Namespace = "http://www.cohowinery.com")]
        public decimal UnitPrice;
        [XmlElement(Namespace = "http://www.cpandl.com")]
        public int Quantity;
        [XmlElement(Namespace = "http://www.cohowinery.com")]
        public decimal LineTotal;

        public string Genre;

        // A custom method used to calculate price per item.
        public void Calculate()
        {
            LineTotal = UnitPrice * Quantity;
        }

    }
}
