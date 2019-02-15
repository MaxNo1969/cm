using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FPS
{
    /// <summary>
    /// Класс для сохранения расположения формы
    /// </summary>
    [Serializable]
    public class FormAttrs
    {
        /// <summary>
        /// Имя формы для поиска (Поле Text)
        /// </summary>
        public string name;
        /// <summary>
        /// Признак видимости формы
        /// </summary>
        public bool visible { get; set; }
        /// <summary>
        /// Левый край 
        /// </summary>
        public int left { get; set; }
        /// <summary>
        /// Верх
        /// </summary>
        public int top { get; set; }
        /// <summary>
        /// Ширина
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// Высота
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// Дополнительные параметры для сохранения
        /// </summary>
        public string dop { get; set; } 
    }
    /// <summary>
    /// Класс для сохранения расположения форм
    /// </summary>
    [Serializable]
    public class FormPosSaver
    {
        Form frm;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_frm">Форма для сохранения</param>
        public FormPosSaver(Form _frm)
        {
            frm = _frm;
            frm.Load += new EventHandler(frm_Load);
            frm.FormClosing += new FormClosingEventHandler(frm_FormClosing);
        }

        void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            save(frm);
        }

        void frm_Load(object sender, EventArgs e)
        {
            load(frm);
        }
        private static List<FormAttrs> fl;
        /// <summary>
        /// Сохранение формы
        /// </summary>
        /// <param name="_frm">Форма</param>
        /// <param name="_dop">Дополнительные параметры</param>
        public static void save(Form _frm, string _dop=null)
        {
            FormAttrs fa = fl.Find(x => x.name == _frm.Name);
            if (fa == null)
            {
                fa = new FormAttrs();
                fa.name = _frm.Name;
                fa.visible = _frm.Visible;
                fa.left = _frm.Left;
                fa.top = _frm.Top;
                fa.width = _frm.Width;
                fa.height = _frm.Height;
                fa.dop = _dop;
                fl.Add(fa);
            }
            else
            {
                fa.visible = _frm.Visible;
                fa.left = _frm.Left;
                fa.top = _frm.Top;
                fa.width = _frm.Width;
                fa.height = _frm.Height;
                fa.dop = _dop;
            }
        }
        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="_frm">Форма</param>
        /// <param name="_dop">Дополнительные параметры</param>
        public static void load(Form _frm,out string _dop)
        {
            _dop = null;
            FormAttrs fa = fl.Find(x => x.name == _frm.Name);
            if (fa != null)
            {
                _frm.Visible = fa.visible;
                _frm.Left = fa.left;
                _frm.Top = fa.top;
                _frm.Width = fa.width;
                _frm.Height = fa.height;
                _dop = fa.dop;
            }
        }
        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="_frm">Форма</param>
        public static void load(Form _frm)
        {
            FormAttrs fa = fl.Find(x => x.name == _frm.Name);
            if (fa != null)
            {
                _frm.Visible = fa.visible;
                _frm.Left = fa.left;
                _frm.Top = fa.top;
                _frm.Width = fa.width;
                _frm.Height = fa.height;
            }
        }
        /// <summary>
        /// Проверка признака видимости формы
        /// </summary>
        /// <param name="_frm">Форма</param>
        /// <returns></returns>
        public static bool visible(Form _frm)
        {
            FormAttrs fa = fl.Find(x => x.name == _frm.Name);
            if (fa != null && fa.visible == true) return true;
            return false;
        }
        const string fileName = "Wins.xml";
        /// <summary>
        /// Сохранение списка координат форм в файл
        /// </summary>
        public static void ser()
        {
            //Обработка первого запуска - файла настроек ещё не записана
            string str = "FormPosSaver:ser()";            
            Debug.WriteLine(str);
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(List<FormAttrs>));            // десериализация
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(fs,fl);
            }
            return;            
        }
        /// <summary>
        /// Закгрузка списка координат форм из файла
        /// </summary>
        public static void deser()
        {
            string str = "FormPosSaver:deser()";
            Debug.WriteLine(str);
            try
            {
                // передаем в конструктор тип класса
                XmlSerializer formatter = new XmlSerializer(typeof(List<FormAttrs>));  
                // десериализация
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                   fl  = (List<FormAttrs>)formatter.Deserialize(fs);
                }
            }
            catch
            {
                //Обработка первого запуска - файла настроек ещё не записана
                str = string.Format("Нет файла {0}. Создаём пустой словарь.",fileName);
                Debug.WriteLine(str);
                fl = new List<FormAttrs>();
            }
        }
    }
}
