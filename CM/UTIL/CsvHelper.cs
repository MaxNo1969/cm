using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FormsExtras;
using Protocol;

namespace CM
{
    public static class CsvHelper
    {
        public static List<double> readCsvFile(string _fName)
        {
            List<double> data = new List<double>();
            string s;
            try
            {
                using (StreamReader reader = new StreamReader(_fName))
                {
                    while ((s = reader.ReadLine()) != null)
                    {
                        try
                        {
                            double val = Convert.ToDouble(s);
                            data.Add(val);
                        }
                        catch (Exception ex)
                        {
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", ex.Message );
                                string logstr = string.Format("{0}: {1}: {2}", "CsvHelper", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.error);
                                Debug.WriteLine(logstr, "Error");
                            }
                            #endregion                            return null;
                        }
                    }
                    reader.Close();
                }
                return data;
            }
            catch
            {
                return null;
            }
        }
        public static bool writeCsvFile(string _fName, List<double> _dump)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_fName, false))
                {

                    foreach(double val in _dump)
                    {
                        writer.WriteLine(string.Format("{0:F3}", val));
                    }
                    writer.Close();
                }
            }
            catch 
            {
                return false;
            }
            return true;
        }
    }
}
