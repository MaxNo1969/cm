using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Делегат при тревожном сигнале
    /// </summary>
    public delegate void OnAlarm();
    /// <summary>
    /// Статический класс для PCIE1730
    /// Работает все время работы программы
    /// При выходе в том числе при аварии снимаем все выходные сигналы
    /// </summary>
    //public static class SL 
    //{
    //    private static SignalListDef sl = null;
    //    /// <summary>
    //    /// Получить список сигналов
    //    /// </summary>
    //    /// <returns></returns>
    //    public static SignalListDef getInst()
    //    {
    //        if(sl==null)sl = new SignalListDef();
    //        return sl;
    //    }
    //    /// <summary>
    //    /// Снимаем все выходные сигналы и чистим
    //    /// </summary>
    //    public static void Destroy()
    //    {
    //        if (sl != null)
    //        {
    //            sl.ClearAllSignals();
    //            sl.Dispose();
    //        }
    //    }
    //}

    /// <summary>
    /// Именованый список сигналов для нашего приложения
    /// </summary>
    public class SignalListDef : SignalList
    {
        private readonly Signal iCC_;
        /// <summary>
        /// 0 ЦУП-Наличие сигнала показывает, что цепи управления влючены.(Digital= True, EOn=, EOff=, Timeout= 00:00:00, No_reset= False, Verbal= False)
        /// </summary>
        public SignalIn iCC;
        private readonly Signal iCYC_;
        /// <summary>
        /// 3 ЦИКЛ3-Наличие сигнала показывает, что в циклк контроля необходимо использовать МНК1(Digital= True, EOn=, EOff=, Timeout= 00:00:00, No_reset= False, Verbal= False)
        /// </summary>
        public SignalIn iCYC;
        private readonly Signal iSTRB_;
        /// <summary>
        /// 5 СТРОБ3-Фронт сигнала(переход из 0 в 1) показывает, что труба переместилась на одну зону при движении её через установку.(Digital=True,EOn=,EOff=,Timeout=00:00:00,No_reset=False,Verbal=False)
        /// </summary>
        public SignalIn iSTRB;
        private readonly Signal iREADY_;
        /// <summary>
        /// 6 ГОТ3-Наличие сигнала показывает, что на входе в установку имеется готовая к контролю труба.(Digital= True, EOn=, EOff=, Timeout= 00:00:00, No_reset= False, Verbal= False)
        /// </summary>
        public SignalIn iREADY;
        private readonly Signal iCNTR_;
        /// <summary>
        /// 7 КОНТР3-Сигнал устанавливается при поступлении трубы на вход МНК3, снимается при при уходе трубы с выхода из МНК3.(Digital= True, EOn=, EOff=, Timeout= 00:00:00, No_reset= False, Verbal= False)
        /// </summary>
        public SignalIn iCNTR;
        private readonly Signal iSOL_;
        /// <summary>
        /// 9 СОЛ-Признак работы соленоида(Digital= True, EOn=, EOff=, Timeout= 00:00:00, No_reset= False, Verbal= False)
        /// </summary>
        public SignalIn iSOL;

        private readonly Signal oWRK_;
        /// <summary>
        /// 1 РАБ3-МНК1 готов к контролю трубы.(Digital=True,EOn=,EOff=,Timeout=00:00:00,No_reset=False,Verbal=False)
        /// </summary>
        public SignalOut oWRK;

        private readonly Signal oPOWER_;
        /// <summary>
        /// 2 Питание датчиков (Включать при запуске программы и выключать при выходе).(Digital=True,EOn=,EOff=,Timeout=00:00:00,No_reset=False,Verbal=False)
        /// </summary>
        public SignalOut oPOWER;

        private readonly Signal oGLOBRES_;
        /// <summary>
        /// 3 ПЕРЕКЛ-Установленый сигнал является признаком завершения цикла контроля очередной трубы.(Digital=True,EOn=,EOff=,Timeout=00:00:00,No_reset=False,Verbal=False)
        /// </summary>
        public SignalOut oGLOBRES;
        private readonly Signal oSTRB_;
        /// <summary>
        /// 4 СТРОБВ-Сигнал устанавливается для подтверждения контроля очередной зоны трубы.(Digital= True, EOn=, EOff=, Timeout= 00:00:00, No_reset= False, Verbal= False)
        /// </summary>
        public SignalOut oSTRB;
        private readonly Signal oZONRES_;
        /// <summary>
        /// 5 РЕЗУЛТ-Сигнал определяет результаты для очередной зоны трубы.(Digital=True,EOn=,EOff=,Timeout=00:00:00,No_reset=False,Verbal=False)
        /// </summary>
        public SignalOut oZONRES;

        /// <summary>
        /// Событие "Авария"
        /// </summary>
        public event OnAlarm onAlarm = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        public SignalListDef() : base()
        {

            string s = "SignalListDef: Constructor";
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);
            try
            {
                iCC_ = Find("ЦУП", true); iCC = new SignalIn(iCC_); MIn.Add(iCC);
                iCYC_ = Find("ЦИКЛ3", true); iCYC = new SignalIn(iCYC_); MIn.Add(iCYC);
                iSTRB_ = Find("СТРОБ3", true); iSTRB = new SignalIn(iSTRB_); MIn.Add(iSTRB);
                iREADY_ = Find("ГОТ3", true); iREADY = new SignalIn(iREADY_); MIn.Add(iREADY);
                iCNTR_ = Find("КОНТР3", true); iCNTR = new SignalIn(iCNTR_); MIn.Add(iCNTR);
                iSOL_ = Find("СОЛ", true); iSOL = new SignalIn(iSOL_); MIn.Add(iSOL);

                oWRK_ = Find("РАБ3", false); oWRK = new SignalOut(oWRK_); MOut.Add(oWRK);
                oPOWER_ = Find("ПИТАНИЕ", false); oPOWER = new SignalOut(oPOWER_); MOut.Add(oPOWER);
                oGLOBRES_ = Find("ОБЩРЕЗ", false); oGLOBRES = new SignalOut(oGLOBRES_); MOut.Add(oGLOBRES);
                oSTRB_ = Find("СТРОБВ", false); oSTRB = new SignalOut(oSTRB_); MOut.Add(oSTRB);
                oZONRES_ = Find("ЗОНРЕЗ", false); oZONRES = new SignalOut(oZONRES_); MOut.Add(oZONRES);

                Start();
            }
            catch (KeyNotFoundException)
            {
                s = "SignalListDef: Ошибка причтении списка сигналов. Проверте настройки.";
                Log.add(s, LogRecord.LogReason.info);
                Debug.WriteLine(s);
                MessageBox.Show(s, "Ошибка");
            }
            catch
            {
                throw new Exception("Ошибка в конструкторк SignalListDef");
            }
        }
        /// <summary>
        /// Проверяем необходимые сигналы
        /// </summary>
        /// <returns></returns>
        public bool checkSignals()
        {
            if (iCC.Val == false)
            {
                throw new Exception("Пропал сигнал цепи управления");
                //return false;
            }
            else if (iCYC.Val == false)
            {
                //return false;
                throw new Exception("Пропал сигнал \"ЦИКЛ3\"");
            }
            else
                return true;
        }
        /// <summary>
        /// Проверяем на состояние "Авария"
        /// </summary>
        protected override void CheckAlarm()
        {
            onAlarm?.Invoke();
        }
        /// <summary>
        /// Снимаем все выходные сигналы
        /// </summary>
        public void ClearAllSignals()
        {
            string s = "SL: " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Снимаем выходные сигналы";
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);

            if (oWRK != null) oWRK.Val = false;
            if (oGLOBRES != null) oGLOBRES.Val = false;
            if (oSTRB != null) oSTRB.Val = false;
            if (oZONRES != null) oZONRES.Val = false;
        }
        /// <summary>
        /// Снимаем все входные сигналы (для эмулятора)
        /// </summary>
        /// <param name="_bIccOff">Надо ли выключать "Цепи управления"</param>
        public void ClearAllInputSignals(bool _bIccOff = false)
        {
            if (_bIccOff)
                set(iCC, false);
            set(iREADY, false);
            set(iCYC, false);
            set(iCNTR, false);
            set(iSTRB, false);
        }

        //Сообщаем результат контроля
        public void ControlResult(Tube.TubeRes _res, bool isGlobRes = false)
        {
            switch (_res)
            {
                case Tube.TubeRes.Good:
                    oSTRB.Val = true;
                    oZONRES.Val = true;
                    WaitHelper.Wait(200);
                    oSTRB.Val = false;
                    oZONRES.Val = false;
                    break;
                case Tube.TubeRes.Class2:
                    oSTRB.Val = false;
                    oZONRES.Val = true;
                    WaitHelper.Wait(200);
                    oZONRES.Val = false;
                    break;
                case Tube.TubeRes.Bad:
                    oSTRB.Val = true;
                    oZONRES.Val = false;
                    WaitHelper.Wait(200);
                    oSTRB.Val = false;
                    break;
            }
            if (isGlobRes)
                oGLOBRES.Val = true;
            #region Логирование 
            {
                string msg = string.Format("Результат={0}", _res.ToString());
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
        }
    }
}
