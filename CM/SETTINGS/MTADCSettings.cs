using Protocol;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CM
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MTADCSettings:ParBase
    {
        public ComPortSettings port { get; set; }
        public MTADCSettings()
        {
            #region Логирование 
            {
                string msg = string.Format("{0}", "Конструктор");
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            //port = new ComPortSettings();
        }
        public override string ToString()
        {
            return port.ToString();
        }
    }
}
