using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace CM
{
    public class De : Attribute
    {
        public De()
        {
            acc = new Access();
            Browsable = true;
        }
        public bool Browsable { get; private set; }
        public De(bool _Browsable)
        {
            acc = new Access();
            Browsable = _Browsable;
        }
        Access acc;
        public Access Acc { get { return (acc); } set { acc = value; } }
        public string Description { get; set; }
    }
    public class NoCopyAttribute : Attribute { }
    [Serializable]
    public class ParBase : IParent, IParentBase
    {
        [Browsable(false)]
        [XmlIgnore]
        public object Parent { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public string PropertyName { get; set; }
        [Browsable(false)]
        [XmlIgnore]
        public int PropertyIndex { get; set; }
        public object AddNew(PropertyInfo _pi)
        {
            Type tp = _pi.PropertyType;
            if (tp.GetInterface("IParentBase") == null)
                return (null);
            object o = Activator.CreateInstance(_pi.PropertyType, null);
            _pi.SetValue(this, o, null);
            IParentBase p = o as IParentBase;
            p.Parent = this;
            p.PropertyName = _pi.Name;
            if (o is IParent)
                (p as IParent).PropertyIndex = -1;
            return (o);
        }
        public override string ToString() { return (""); }

        public ParBase()
        {
            Type t = GetType();
            PropertyInfo[] pii = t.GetProperties();
            foreach (PropertyInfo pi in pii)
            {
                AddNew(pi);
            }
        }

        public void SimpleCopy(ParBase _src)
        {
            Type tp_dst = GetType();
            Type tp_src = _src.GetType();
            if (tp_dst != tp_src)
                return;
            foreach (PropertyInfo pi_src in _src.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi_src, typeof(De)) == null)
                    continue;
                if (Attribute.GetCustomAttribute(pi_src, typeof(NoCopyAttribute)) is NoCopyAttribute no_copy)
                    continue;
                PropertyInfo pi_dst = tp_dst.GetProperty(pi_src.Name);
                pi_dst.SetValue(this, pi_src.GetValue(_src, null), null);
            }
        }
    }
}
