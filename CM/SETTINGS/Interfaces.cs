using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CM
{
    public interface IParentBase
    {
        string PropertyName { get; set; }
        object Parent { get; set; }
    }
    public interface IParent
    {
        object AddNew(PropertyInfo _pi);
        int PropertyIndex { get; set; }
    }
    public interface IParentList : IEnumerable
    {
        object AddNew();
        void RemoveOld(object _o);
        int ListCount();
        object GetItem(int _index);
        bool Move(int _index_src, int _index_trg);
    }
}
