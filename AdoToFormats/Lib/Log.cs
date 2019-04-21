using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;

namespace AdoToFormats.Lib
{
    public interface ILog
    {
        void WriteLine(string strLog);
    }

    public class FileLog : ILog
    {
        public FileLog()
        {
        }

        public FileLog(string strFileName)
        {
            strLogFile_ = strFileName;
        }

        public void WriteLine(string strLog)
        {
            System.IO.StreamWriter sw = null;

            try
            {
                string strLogFile = MakeLogFileName(strLogFile_);
                sw = new System.IO.StreamWriter(strLogFile, true);

                sw.WriteLine("==============================");
                sw.WriteLine(DateTime.Now.ToString());
                sw.WriteLine("==============================");
                sw.WriteLine(strLog);
                sw.WriteLine(sw.NewLine);
                sw.Flush();
            }
            catch (Exception) { }
            finally
            {
                if (sw != null) sw.Close();
            }
        }

        static string MakeLogFileName(string strFileName)
        {
            string strName = string.Format(
                "{0}\\{1}_{2}{3}",
                Path.GetDirectoryName(strFileName),
                Path.GetFileNameWithoutExtension(strFileName),
                DateTime.Now.ToShortDateString().Replace("-", ""),
                Path.GetExtension(strFileName)
                );

            return strName;
        }

        public void WriteProcLog(SqlCommand cmd)
        {
            string sFile = Config.GetLogFilePath();
            FileLog webLog = new FileLog(sFile);
            StringBuilder websb = new StringBuilder();

            string query = string.Format("Exec {0} ", cmd.CommandText);

            int i = 0;
            int paraCnt = cmd.Parameters.Count;
            string paraValue = string.Empty;

            foreach (SqlParameter para in cmd.Parameters)
            {
                paraValue = string.Empty;

                if ((para.SqlDbType == SqlDbType.VarChar) || (para.SqlDbType == SqlDbType.Char))
                {
                    paraValue = "'" + (para.Value == null ? "" : para.Value.ToString()) + "'";
                }
                else
                {
                    paraValue = (para.Value == null ? "" : para.Value.ToString());
                }

                i++;
                if (paraCnt > i)
                {
                    paraValue += ", ";
                }
                query += string.Format("{0}", paraValue);

            }
            websb.AppendLine(query);

            webLog.WriteLine(websb.ToString());
        }

        private string strLogFile_ = "log.txt";
    };

    public class TraceLog : ILog
    {
        public TraceLog()
        {
        }

        public TraceLog(string strFileName)
        {
        }

        public void WriteLine(string strLog)
        {
            Trace.WriteLine(strLog);
        }
    };
}
