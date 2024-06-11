using System;
using System.Configuration;

namespace MA2AAPI
{
    public class Logger
    {
        public static void writeLog(string msg, ref log4net.ILog log)
        {
            try
            {
                if (ConfigurationManager.AppSettings["isFileLog"].ToString().Equals("Y"))
                    log.Info(msg);
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }
        }
    }
}