using Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    public partial class FRSettings : FormSp
    {
        AppSettings settings;
        public FRSettings(ref AppSettings _settings)
        {
            settings = _settings;
            InitializeComponent();
            pg.PropertySort = PropertySort.Categorized;
            pg.SelectedObject = _settings;
            pg.PropertyValueChanged += Pg_PropertyValueChanged;
            settings.changed = true;
        }

        private void Pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            #region Логирование
            {
                string msg = string.Format("Изменено {0} {1}=>{2}", e.ChangedItem.Label, e.OldValue, e.ChangedItem.Value);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion
            //ToDo - изменения настроек
            settings.onChangeSettings?.Invoke(new object[] { e.ChangedItem.Label, e.ChangedItem.Value });
            settings.changed = true;
        }
    }
}
