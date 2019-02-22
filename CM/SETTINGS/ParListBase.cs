using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace CM
{
    [TypeConverter(typeof(CollectionTypeConverter))]
    [Editor(typeof(MyListEditor), typeof(UITypeEditor))]
    public class ParListBase<T> : List<T>, IParentList, IParentBase
    {
        [Browsable(false)]
        public object Parent { get; set; }
        [Browsable(false)]
        public string PropertyName { get; set; }

        /// <summary>
        /// Выбор элемента по имени
        /// </summary>
        /// <param name="_val"></param>
        /// <returns></returns>
        public virtual T this[string _num] { get { return (this[Convert.ToInt32(_num)]); } }
        //        public new T this[int _num] { get { return (_num < Count ? base[_num] : default(T)); } }

        public virtual object AddNew()
        {
            Type tp = typeof(T);
            if (tp.GetInterface("IParent") == null)
                return (null);
            object o = Activator.CreateInstance(tp, null);
            Add((T)o);
            IParentBase p = o as IParentBase;
            p.Parent = this;
            p.PropertyName = null;
            (p as IParent).PropertyIndex = this.IndexOf((T)o);
            return (o);
        }
        public virtual object AddNewTree()
        {
            object o = AddNew();
            return (o);
        }
        public void RemoveOld(object _o)
        {
            Remove((T)_o);
            for (int i = 0; i < Count; i++)
            {
                IParent p = this[i] as IParent;
                p.PropertyIndex = i;
            }
        }
        public override string ToString()
        {
            return ("<" + Count.ToString() + ">");
        }
        public int ListCount()
        {
            return (base.Count);
        }
        public object GetItem(int _index)
        {
            return (this[_index]);
        }
        public bool Move(int _index_src, int _index_trg)
        {
            if (_index_src < 0 || _index_src >= base.Count)
                return (false);
            if (_index_trg < 0 || _index_trg >= base.Count)
                return (false);
            if (_index_src == _index_trg)
                return (true);
            T o = this[_index_src];
            RemoveAt(_index_src);
            Insert(_index_trg, o);
            foreach (T oo in this)
                (oo as IParent).PropertyIndex = this.IndexOf(oo);
            return (true);
        }
    }
}