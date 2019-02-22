using System;
using System.ComponentModel;

namespace CM
{
    public class Access
    {
        EGroup group;
        [DisplayName("Группа"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EGroup Group { get { return (group); } set { group = value; } }

        EUnit unit;
        [DisplayName("Установка"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EUnit Unit { get { return (unit); } set { unit = value; } }

        public Access()
        {
            group = EGroup.Operator;
            unit = EUnit.All;
        }
        public Access(Access _acc)
        {
            Copy(_acc);
        }
        public void Copy(Access _acc)
        {
            group = _acc.group;
            unit = _acc.unit;
        }
        public void Set(string _group, string _unit)
        {
            if (!Enum.TryParse<EGroup>(_group, out group))
                group = EGroup.Operator;
            if (!Enum.TryParse<EUnit>(_unit, out unit))
                unit = EUnit.All;
        }
        public bool CheckUser(User _user)
        {
            bool isBrosable = (unit == EUnit.All) | (_user.Unit == EUnit.All) | (unit == _user.Unit);
            isBrosable &= (_user.Group == EGroup.Master) | (group == _user.Group) | (group == EGroup.Operator);
            return (isBrosable);
        }
    }
    public enum EUnit
    {
        [Description("Дефектоскоп")]
        Defect,
        [Description("Толщиномер")]
        Thick,
        [Description("Все")]
        All
    }
    public enum EGroup
    {
        [Description("Мастер")]
        Master,
        [Description("Наладчик")]
        Setter,
        [Description("Оператор")]
        Operator
    }
}