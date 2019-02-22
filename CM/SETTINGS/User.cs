using System.ComponentModel;

namespace CM
{
    public class User : ParBase
    {
        [DisplayName("Имя"), Browsable(true), De]
        public string Name { get; set; }

        [DisplayName("Группа"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EGroup Group { get; set; }

        [DisplayName("Установка"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EUnit Unit { get; set; }

        [DisplayName("Пароль"), PasswordPropertyText(true), Browsable(true), De]
        public string Pwd { get; set; }

        public override string ToString() { return (Name); }

        static User Default()
        {
            User u = new User()
            {
                Name = "Master",
                Group = EGroup.Master,
                Unit = EUnit.All,
            };
            return (u);
        }

        public static User current = Default();
    }
}