using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using FormsExtras;

namespace Protocol
{
    /// <summary>
    /// Форма вывода протокола
    /// </summary>
    public partial class FRProt : Form, IDisposable
    {
        /// <summary>
        /// Делегат скрытия формы 
        /// </summary>
        public delegate void OnHideForm();
        /// <summary>
        /// Обработка скрытия формы
        /// </summary>
        public OnHideForm onHide = null;

        private enum UpdateMethod { _timer, _event };
        UpdateMethod updateMethod = UpdateMethod._event;
        private Timer timer;

        /// <summary>
        /// Методы сохранения протокола
        /// </summary>
        public enum SaveMethod {
            /// <summary>
            /// Не сохранять
            /// </summary>
            _none,
            /// <summary>
            /// Сохранять в файл
            /// </summary>
            _tofile,
            /// <summary>
            /// Сохранять в таблицу БД (не реализовано)
            /// </summary>
            _todb
        };
        private SaveMethod _saveMethod = SaveMethod._none;
        /// <summary>
        /// Текущий метод сохранения лога
        /// </summary>
        public SaveMethod saveMethod
        {
            get
            {
                return _saveMethod;
            }
            set
            {
                switch (value)
                {
                    case SaveMethod._none:
                        _saveMethod = SaveMethod._none;
                        //Если будем как-то отдельно открывать коннекцию к базе, то надо её закрыть
                        closeDB();
                        //Если у нас запись в файл, то надо его закрыть.
                        closeFile();
                        break;
                    case SaveMethod._todb:
                        _saveMethod = SaveMethod._todb;
                        //Если у нас запись в файл, то надо его закрыть.
                        closeFile();
                        openDB();
                        break;
                    case SaveMethod._tofile:
                        _saveMethod = SaveMethod._tofile;
                        closeDB();
                        openFile();
                        break;
                }
            }
        }
        //Последняя записанная запись из протокола
        private static int lastRecordedIndex = 0;
        //Для записи в файл
        private StreamWriter streamWriter = null;
        private bool openFile(string fName = null)
        {
            //Прверяем может файл уже открыт
            if (streamWriter != null) return true;
            if(fName==null)
                fName = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\log.txt";
            try
            {
                streamWriter = new StreamWriter(fName, true);
                #region Логирование 
                {
                    string msg = string.Format("{0}", "Открыли файл протокола.");
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message );
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");

                }
                #endregion
                return false;
            }
        }
        private void closeFile()
        {
            if (streamWriter != null)
            {
                try
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", "Закрываем файл протокола.");
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
                catch (Exception ex)
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", ex.Message);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr, "Error");

                    }
                    #endregion                }
                }
            }
        }

        private void checkLogTable()
        {
            /*
            string SQL = "select count(*) as nn from INFORMATION_SCHEMA.TABLES where table_type='BASE TABLE' and TABLE_SCHEMA='Uran' and table_name='logtable'";
            Select S = new Select(SQL);
            if (!S.Read())
                throw new ArgumentException("RAGLIB:DBSPar:CheckTable: " + SQL + " - не нашли записей");
            if (S.AsInt("nn") == 1)
                return;
            //Если таблица не существует, создаем её
            SQL = "CREATE TABLE [Uran].[logtable](" +
                "[id] [int] IDENTITY(1,1) NOT NULL," +
                "[tstamp] [varchar](20) NOT NULL," +
                "[reason] [varchar](20) NOT NULL," +
                "[text] [varchar](200) NOT NULL," +
                "CONSTRAINT [PK_logtable] PRIMARY KEY CLUSTERED " +
                "   (" +
                "       [id] ASC" +
                "   ) ON [PRIMARY]" +
                ") ON [PRIMARY]";
            ExecSQL E = new ExecSQL(SQL);
             */ 
        }
        //Если будем предпринимать какие-то действия с базой, то здесь можно надо будет всё закрыть
        private void openDB()
        {
            checkLogTable();
        }
        //Если будем предпринимать какие-то действия с базой, то здесь можно надо будет всё закрыть
        private void closeDB()
        {
        }

        int[] colSizes = { 200, 800 };
        /// <summary>
        /// Конструктор
        /// </summary>
        public FRProt(/*Form _frm*/)
        {
            InitializeComponent();
            //Owner = _frm;
            KeyPreview = true;
            KeyDown += new KeyEventHandler(FRProt_KeyDown);

            //Настраиваем таймер
            timer = new Timer()
            {
                Interval = 500,
            };
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        /// <summary>
        /// Очистка
        /// </summary>
        public new void Dispose()
        {
            timer.Stop();
            timer.Dispose();
            timer = null;
            closeFile();
            closeDB();
            base.Dispose();
        }

        void FRProt_Load(object sender, EventArgs e)
        {
            {
                FormPosSaver.load(this, out string s);
                if (s != null)
                {
                    string[] scs = s.Split(new char[] { ',' });
                    colSizes[0] = Convert.ToInt32(scs[0]);
                    colSizes[1] = Convert.ToInt32(scs[1]);
                }
            }
            //Настраиваем колонки
            lvProt.Columns.Add("Время");
            lvProt.Columns.Add("Событие");

            lvProt.Columns[0].Width = colSizes[0];
            lvProt.Columns[1].Width = colSizes[1];
            if (updateMethod == UpdateMethod._event)
            {
                Log.onLogChanged += new Log.OnLogChanged(invokeUpdateList);
                logToList();
            }
        }
        
        void invokeUpdateList()
        {
            try
            {
                if (InvokeRequired)
                    BeginInvoke(new Action(() => logToList()));
                else
                    logToList();
            }
            catch (Exception)
            {
            }
        }

        void logToList()
        {
            while (Log.size() > 0)
            {
                LogRecord rec = Log.get();
                if (rec != null)
                {
                    string[] subitems = { rec.dt.ToString("dd/MM/yy HH:mm:ss.ff"), rec.text };
                    ListViewItem item = new ListViewItem(subitems, (int)rec.reason);
                    lvProt.EnsureVisible(lvProt.Items.Add(item).Index);
                }
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            
            if (updateMethod == UpdateMethod._timer)
            {
                while (Log.size() > 0)
                {
                    LogRecord rec = Log.get();
                    if (rec != null)
                    {
                        string[] subitems = { rec.dt.ToString("dd/MM/yy HH:mm:ss.ff"), rec.text };
                        ListViewItem item = new ListViewItem(subitems, (int)rec.reason);
                        lvProt.EnsureVisible(lvProt.Items.Add(item).Index);
                    }
                }
            }
            if (_saveMethod == SaveMethod._tofile)
            {
                while (lastRecordedIndex < lvProt.Items.Count)
                {
                    streamWriter.WriteLine(string.Format("{0} -> {1}", lvProt.Items[lastRecordedIndex].Text, lvProt.Items[lastRecordedIndex].SubItems[1].Text));
                    lastRecordedIndex++;
                }
                streamWriter.Flush();
            }
            /*
            if (_saveMethod == SaveMethod._todb)
            {
                while (lastRecordedIndex < lvProt.Items.Count)
                {
                    string SQL = "insert into [Uran].[logtable] values ('" + lvProt.Items[lastRecordedIndex].Text + "','" +
                       ((LogRecord.LogReason)lvProt.Items[lastRecordedIndex].ImageIndex).ToString() + "','" +
                       lvProt.Items[lastRecordedIndex].SubItems[1].Text + "')";
                    ExecSQL E = new ExecSQL(SQL);
                    lastRecordedIndex++;
                }
            }
             */ 
        }
        
        void FRProt_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F11:
                case Keys.Escape:
                    Visible = false;
                    onHide?.Invoke();
                    break;
            }
        }
        private void FRProt_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.onLogChanged -= invokeUpdateList;
            FormPosSaver.save(this, string.Format("{0},{1}", lvProt.Columns[0].Width, lvProt.Columns[1].Width));
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    Visible = false;
                    onHide?.Invoke();
                    break;
            }
        }
    }
}
