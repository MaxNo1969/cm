using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace CM
{
    /// <summary>
    /// Список сигналов 
    /// </summary>
    [TypeConverter(typeof(CollectionTypeConverter))]
    [Editor(typeof(MyListEditor), typeof(UITypeEditor))]
    [DisplayName("Сигналы")]
    public class L_SignalSettings : ParListBase<SignalSettings>
    {
        /// <summary>
        /// Преобразовние в строку для отображения в PropertyGrid
        /// </summary>
        /// <returns>строка отображения</returns>
        public override string ToString() { return (string.Format("<{0}>", Count)); }
        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <returns>Ссылка на добавленный элемент</returns>
        public override object AddNew()
        {
            int position_new = FindNewPosition();
            SignalSettings p = base.AddNew() as SignalSettings;
            p.Name = FindNewName();
            p.Position = position_new;
            return (p);
        }

        private int FindNewPosition()
        {
            int position = -1;
            foreach (SignalSettings p in this)
            {
                if (p.Position >= position)
                    position = p.Position;
            }
            return (position + 1);
        }
        /// <summary>
        /// Индексатор по имени сигнала
        /// </summary>
        /// <param name="_name">Имя сигнала</param>
        /// <returns>Ссылка на параметры выбранного сигнала</returns>
        public override SignalSettings this[string _name]
        {
            get
            {
                foreach (SignalSettings p in this)
                    if (p.Name == _name)
                        return (p);
                return (null);
            }
        }
        string FindNewName()
        {
            for (int i = 0; ; i++)
            {
                if (this["Новый" + i.ToString()] == null)
                    return ("Новый" + i.ToString());
            }
        }
        /// <summary>
        /// Вывод сигналов в список строк
        /// </summary>
        /// <returns>Список сигналов</returns>
        public List<string> ToCS()
        {
            List<string> L = new List<string>();
            foreach (SignalSettings p in this)
            {
                if (!p.Input)
                    continue;
                L.AddRange(p.ToCS());
                L.Add("");
            }
            L.Add("");
            foreach (SignalSettings p in this)
            {
                if (p.Input)
                    continue;
                L.AddRange(p.ToCS());
                L.Add("");
            }
            return (L);
        }
    }
}
