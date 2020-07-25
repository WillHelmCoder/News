using System;
using System.IO;
using System.Web;
using Dal.Interfaces;
using Dal.Models;

namespace Dal.Classes
{
    public class Logger : ILog
    {
        private readonly static Object Locker = new Object();

        public void Write(String message, LogTypes type)
        {
            var category = type.ToString();

            var fileName = String.Format("{0}-{1}.log", DateTime.Now.ToString("dd-MM-yyyy"), category);
            var path = HttpContext.Current.Server.MapPath("~/Logs");

            var FileName = Path.Combine(path, fileName);

            lock (Locker)
            {
                File.AppendAllText(FileName,
                String.Format("{0}  |  {1}\r\n", DateTime.Now.ToString("[HH:mm:ss]"), message));
            }
        }
    }
}