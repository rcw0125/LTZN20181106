using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Rcw.Data;

namespace LTZN
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DbContext.AddDataSource("ltzn", DbContext.DbType.Oracle, "192.168.2.204", "orcl", "LF", "LF");
            DbContext.AddDataSource("ltzn", DbContext.DbType.Oracle, "192.168.2.3", "xgmes", "LF", "LF");
            DbContext.DefaultDataSourceName = "ltzn";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new 炼铁智能主窗体());
        }

        public static void Log(string content)
        {
            File.WriteAllText(@"c:\temp.txt", content);
        }
    }
}