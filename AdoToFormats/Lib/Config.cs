using System;
using System.Text;
using System.IO;
using System.Xml;
//using System.Configuration;

namespace AdoToFormats.Lib
{
    class Config
    {
        public Config() { }

        public static string strDBName = "Test";
        public static int intDBTimeOut = 5; // 10초 Timeout -> 5초로 수정 

        public static string GetConfigFile()
        {
            string strPath = Environment.SystemDirectory;
            strPath += "\\" + "AdoToFormats.config";
            return strPath;
        }

        public static string GetConnectionString()
        {
            string CONNECTION_STRING_XPATH = "/Config/" + strDBName.ToUpper() + "DB/";
            int DB_CONNECTION_TIMEOUT = intDBTimeOut;

            XmlDocument xmlDom = new XmlDocument();
            xmlDom.Load(GetConfigFile());

            string strServer = xmlDom.SelectSingleNode(CONNECTION_STRING_XPATH + "Server").InnerText;
            string strUID = xmlDom.SelectSingleNode(CONNECTION_STRING_XPATH + "UID").InnerText;
            string strPWD = xmlDom.SelectSingleNode(CONNECTION_STRING_XPATH + "PWD").InnerText;
            string strDatabase = xmlDom.SelectSingleNode(CONNECTION_STRING_XPATH + "Database").InnerText;

            return string.Format("Server={0};UID={1};PWD={2};Database={3};Connection Timeout={4}",
                        strServer, strUID, strPWD, strDatabase, DB_CONNECTION_TIMEOUT
                    );
            //return ConfigurationManager.ConnectionStrings["Play"].ConnectionString;
        }

        public static string GetLogFilePath()
        {
            string LOG_STRING_XPATH = "/Config/Log/FileName";
            string strFilePath = "";

            XmlDocument xmlDom = new XmlDocument();
            xmlDom.Load(GetConfigFile());

            if (xmlDom.SelectSingleNode(LOG_STRING_XPATH) != null)
            {
                strFilePath = xmlDom.SelectSingleNode(LOG_STRING_XPATH).InnerText;
            }

            return strFilePath;
            //return ConfigurationManager.AppSettings["Log"];
        }
    }
}