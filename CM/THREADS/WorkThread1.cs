using Protocol;
using System;
using System.Diagnostics;
using System.Threading;

namespace CM
{
    /// <summary>
    /// Основной поток работы программы
    /// </summary>
    class WorkThread1 : IDisposable
    {
        public bool isRunning { get; private set; }
        private Thread thread;
        SignalListDef sl = Program.signals;

        /// <summary>
        /// Блокировка
        /// </summary>
        private readonly object block = new object();
        FRMain frm;
        readonly Tube tube;
        ReadDataThread readDataThread = null;
        StrobeThread strobeThread = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        public WorkThread1(Tube _tube, FRMain _frm)
        {
            #region Логирование 
            {
                string msg = string.Format("{0}", "");
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            tube = _tube;
            frm = _frm;
        }


        //17.04.2019, 13:29 - Алексей Червонцев: Нет в цикле сперва ждем готовность и соленоид, а при появлении ждем контроль
        //17.04.2019, 13:30 - Алексей Червонцев: Контроль появится когда труба булет уже в модуле на входе и выходе одновременно
        //17.04.2019, 13:30 - Алексей Червонцев: И снимется когда труба выйдет с датчика на входе
        //17.04.2019, 13:31 - Алексей Червонцев: Т.е.Контрольль стоит когда оба датчика на входе и выходе замкнуты
        //17.04.2019, 13:31 - Алексей Червонцев: С пропаданием контроля снимается и сигнал соленоид
        //17.04.2019, 13:32 - Алексей Червонцев: Чтобы резко снять магнитеое поле, если его снимать по команде пойдут искажения сигнала
        //17.04.2019, 13:34 - Алексей Червонцев: По появлению контроля уже все включено, надо только запустить п217
        //17.04.2019, 13:35 - Алексей Червонцев: Сбор пойдет
        //17.04.2019, 13:35 - Алексей Червонцев: И выключить п217 при пропадании контроля
        //17.04.2019, 13:35 - Алексей Червонцев: Ну потом выключить питание
        //17.04.2019, 13:37 - Алексей Червонцев: Скорость мерить по сигналу соленоид до появления контроля
        //17.04.2019, 13:37 - Алексей Червонцев: Расстояние я скажу

        //Перечисление для текущуго состояния конечного автомата
        public enum WrkStates
        {
            none, //Не установлено
            startWorkCycle, //Начало рабочего цикла по трубе
            waitNextTube, //Ждем следующую трубу
            waitCntrl, //Ждем появления сигнала "Контроль"
            work, //Работа - крутим цикл приема данных до снятия сигнала "Контроль"
            endWork, //Работа закончена
            error //Приключилась ошибка
        }
        public WrkStates curState;
        private WrkStates prevState = WrkStates.none;
        private TimeSpan waitControlStarted;
        private void threadFunc(object _params)
        {
            try
            {
                //начинаем отсчет времени
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string errStr = string.Empty;
                string s;
                curState = WrkStates.startWorkCycle;
                while (isRunning)
                {
                    //Проверяем сигналы ICC и  CYCLE3 - они должны быть выставлены воё время работы
                    if (!sl.checkSignals())
                    {
                        errStr = "Отсутствуют сигналы";
                        curState = WrkStates.error;
                    }
                    //Состояние изменилось
                    if (prevState != curState)
                    {
                        string msg = string.Format("{0} -> {1}", Enum.GetName(typeof(WrkStates), prevState), Enum.GetName(typeof(WrkStates), curState));
                        #region Логирование
                        {
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr);
                        }
                        #endregion 
                        //frm.setSb("Info",msg);
                        prevState = curState;
                    }
                    switch (curState)
                    {
                        //Проверяем наличие ошибки - если выставлено, то закрываем всё и выходим из цикла
                        case WrkStates.error:
                            #region Логирование
                            {
                                s = string.Format("{0}: {1}: Ошибка: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, errStr);
                                Log.add(errStr, LogRecord.LogReason.error);
                                Debug.WriteLine(errStr);
                            }
                            #endregion Логирование
                            isRunning = false;
                            break;
                        //Начало рабочего цикла - снимаем сигнал "" для сигнализации того, что
                        //установка ждет следующую трубу
                        case WrkStates.startWorkCycle:
                            sl.oGLOBRES.Val = false;
                            curState = WrkStates.waitNextTube;
                            break;
                        case WrkStates.waitNextTube:
                            //Сперва ждем готовность и соленоид
                            if (sl.iREADY.Val == true && sl.iSOL.Val==true)
                            {
                                //Если 
                                sl.oGLOBRES.Val = false;
                                //Здесь подготовка модуля к работе
                                {
                                    //Включить намагничивание модуля. Проверить сопротивление соленоидов. 
                                    //Если перегрев – выход из режима с соответствующим сообщением. 
                                    //Далее в цикле контроля трубы выводить на экран ток и напряжение соленоидов (пока намагничивание включено).
                                }
                                //Выставляем сигнал "РАБОТА3"
                                sl.oWRK.Val = true;
                                waitControlStarted = sw.Elapsed;
                                curState = WrkStates.waitCntrl;
                            }
                            break;
                        //При появлении сигнала КОНТРОЛЬ начать анализировать сигнал СТРОБ3. 
                        //Если сигнал КОНТРОЛЬ не появился за определенное время (10 секунд) – 
                        //аварийное завершение режима с выводом со-ответствующего сообщения.
                        case WrkStates.waitCntrl:
                            if ((sw.Elapsed - waitControlStarted).Seconds > 10)
                            {
                                errStr = "Не дождались трубы на входе в модуль";
                                curState = WrkStates.error;
                                break;
                            }
                            if (sl.iCNTR.Val == true)
                            {
                                //Включить сбор данных с модуля контроля. Ожидать появления сигнала КОНТРОЛЬ. 
                                {
                                    //Запускаем поток чтения стробов
                                    strobeThread = new StrobeThread(tube);
                                    strobeThread.start();
                                    //Запускаем поток чтения данных
                                    readDataThread = new ReadDataThread(Program.lCard, tube);
                                    readDataThread.Start();
                                }
                                curState = WrkStates.work;
                            }
                            break;
                        case WrkStates.work:
                            //Пропал сигнал контроль
                            if (sl.iCNTR.Val == false)
                            {
                                //Останавливаем поток обработки стробов
                                strobeThread.stop();
                                //strobeThread = null;
                                //Останавливаем поток чтения данных
                                readDataThread.Stop();
                                //readDataThread = null;
                                sl.oWRK.Val = false;
                                curState = WrkStates.endWork;
                                break;
                            }

                            break;
                        case WrkStates.endWork:
                            //По окончании сбора, обработки и передачи результата снять сигнал КОНТРОЛЬ. 
                            //Выключить намагничивание соленоидов при снятии соответствующих сигналов КОНТРОЛЬ. 
                            //Выключить генерацию синхросигнала платой УС.
                            curState = WrkStates.startWorkCycle;
                            //По идее надо наверное выходить из цикла работы для следующей трубы уже запускать новый цикл
                            isRunning = false;
                            break;
                        default:
                            break;

                    }
                    //Thread.Sleep(100);
                }
                string _s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Вышли";
                Log.add(_s, LogRecord.LogReason.info);
                //Останавливаем поток обработки стробов
                strobeThread.stop();
                //Останавливаем поток чтения данных если он запущен
                readDataThread.Stop();
            }
            catch (Exception e)
            {
                curState = WrkStates.error;
                string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + e.Message;
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
                frm.setSb("Info", s + ". Аварийное завершение.");
                //Останавливаем поток обработки стробов
                if(strobeThread!=null)strobeThread.stop();
                //Останавливаем поток чтения данных если он запущен
                if(readDataThread!=null)readDataThread.Stop();
                //readDataThread = null;
                //Снимем все выходные сигналы
                sl.ClearAllSignals();
                isRunning = false;
            }
        }
        public void start()
        {
            string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name;
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);
            if (!isRunning)
            {
                frm.setSb("Info", "Готов к работе");
                isRunning = true;
                thread = new Thread(threadFunc)
                {
                    Name = "WorkThread",
                    IsBackground = true,
                };
                thread.Start();
            }
            else
            {
                return;
            }
        }
        public void stop()
        {
            #region Логирование 
            {
                string msg = string.Format("{0}", @"stop");
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            if (isRunning)
            {
                isRunning = false;
                thread.Join();
                thread = null;
            }
        }

        void IDisposable.Dispose()
        {
            stop();
        }
    }
}
