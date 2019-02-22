using System;
using System.ComponentModel;

namespace CM
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MTADCSettings
    {
        public ComPortSettings port { get; set; }
        public MTADCSettings()
        {
            port = new ComPortSettings();
        }
        public override string ToString()
        {
            return port.ToString();
        }
    }
}
