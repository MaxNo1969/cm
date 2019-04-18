using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Serialization;

namespace CM
{
    class LCardChannelsListConverter : TypeConverter
    {
        /// <summary>
        /// Только в строку
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        /// <summary>
        /// И только так
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            return "<" + (value as ICollection).Count + ">";
        }
    }

    /// <summary>
    /// Список типоразмеров
    /// </summary>
    [TypeConverter(typeof(LCardChannelsListConverter))]
    [Editor(typeof(MyListEditor), typeof(UITypeEditor))]
    [Serializable]
    public class L_LCardChannels : ParListBase<LCardChannelSettings>
    {
        /// <summary>
        /// Строковое представление списка (отображается выбранный типоразмер)
        /// </summary>
        /// <returns></returns>
        //public override string ToString() { return (Current?.Name); }
    }
}