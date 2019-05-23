using System;
using System.Collections;
using System.Windows.Forms;
using System.Reflection;

/// <summary>
/// Атрибут используемый для пометки матода и ассоциации с ним 
/// нужного идентификатора сообщения.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class WinMsgAttribute : Attribute
{
    public WinMsgAttribute(int msgID)
    {
        MsgID = msgID;
    }


    public int MsgID { get; set; }
}


delegate bool WinMsgHandler(ref Message msg);


/// <summary>
/// Вскрывальщик сообщений. Находит метода помеченные атрибутом
/// WinMsgAttribute и ассоциирует их с сообщениями идентификаторы
/// которых хранятся в WinMsgAttribute.
/// </summary>
public class MsgCrecker
{
    Hashtable _msgMap = new Hashtable();
    static Type _attrType = typeof(WinMsgAttribute);
    static Type _delegateType = typeof(WinMsgHandler);


    public MsgCrecker(Form form)
    {
        // Получаем список методов формы.
        MethodInfo[] methods = form.GetType().GetMethods(
            BindingFlags.Instance | BindingFlags.Public
            | BindingFlags.NonPublic);


        foreach (MethodInfo method in methods)
        {
            // Получаем список атрибутов WinMsgAttribute каждого метода.
            object[] attrs = method.GetCustomAttributes(_attrType, false);
            // Если атрибут WinMsgAttribute ассоциирован с методом...
            if (attrs.Length > 0)
            {
                // Перебираем их и...
                foreach (WinMsgAttribute attr in attrs)
                {
                    // Засандаливаем в хэш-таблицу, ассоциируя с делегатом содержащим
                    // ссылку на метод-обработчик.
                    _msgMap.Add(attr.MsgID,
                        Delegate.CreateDelegate(_delegateType, form, method.Name, true));
                }
            }
        }
    }


    /// <summary>
    /// Пытается обработать сообщение. Если не удается, возвращает true.
    /// Если удается, то возвращает значение возвращенное методом-обработчиком.
    /// </summary>
    /// <example>
    /// protected override void WndProc(ref Message msg)
    /// {
    ///     if (_msgCrecker.ProcessMsg(ref msg))
    ///         base.WndProc(ref msg);
    /// }
    /// </example>
    /// <param name="msg">Сообщение.</param>
    /// <returns>Если true, то нужно вызывать метод WndProc базового класса.</returns>
    public bool ProcessMsg(ref Message msg)
    {
        // Пытаемся нафти делегат ассоциированный с пришедшим сообщением.
        WinMsgHandler handler = (WinMsgHandler)_msgMap[msg.Msg];
        if (handler != null) // Если удается...
            return handler(ref msg); // Вызываем метод.
        return true; // Приказываем вызвать базовую реализацию WndProc.
    }
}
