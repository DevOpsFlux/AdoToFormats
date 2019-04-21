/*-------------------------------------------------------
'	프로그램명	: ToFormats Class
'	작성자		: DevOpsFlux
'	작성일		: 2015-01-20
'	설명		: ToFormats Common Class
' -------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using Newtonsoft.Json;
using AdoToFormats.Lib;

namespace AdoToFormats.Lib
{
    #region # interface
    interface IMakeFormats
    {
        string ConFlag { get; set; }
        string SqlStr { get; set; }
        string SavePath { get; set; }
        string IndentedYN { get; set; }
        string CDataYN { get; set; }
        string DBTimeOutSec { get; set; }
        string EmptyYN { get; set; }
        bool MakeXML();
        bool MakeJSON();
    }
    #endregion

    #region # class ToFormats
    class ToFormats : IMakeFormats
    {
        #region # private member
        private string strConFlag = string.Empty;
        private string strSql = string.Empty;
        private string strSavePath = string.Empty;
        private string strIndentedYN = "N";
        private string strCDataYN = "N";
        private string strDBTimeOutSec = "180";
        private string strEmptyYN = "N";
        #endregion 

        #region # MakeXML   
        public bool MakeXML()
        {            
            // http://xml.interpark.com/Movie/Main/TestXML.xml

            bool bSuccess = false;
            string strLog = string.Empty;
            FileLog log = new FileLog(Config.GetLogFilePath());
            //log.WriteLine("DEBUG Start 1 ");
            //GLASS.TraceLog tlog = new GLASS.TraceLog();
            //tlog.WriteLine("DEBUG Start 1 ");

            try
            {
                if (strDBTimeOutSec == string.Empty || strDBTimeOutSec == "")
                {
                    strDBTimeOutSec = "180";
                }
                Config.strDBName = strConFlag.ToString();
                Config.intDBTimeOut = Convert.ToInt32(strDBTimeOutSec.ToString());

                //tlog.WriteLine("DEBUG Start 2 ");

                DBData db = new DBData();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = Convert.ToInt32(strDBTimeOutSec.ToString());
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSql.ToString();
                //log.WriteProcLog(cmd);

                //tlog.WriteLine("DEBUG Start 3 ");

                DataSet ds = db.ExecuteDataSet(cmd);

                //tlog.WriteLine("DEBUG Start 3.5 ");
                //tlog.WriteLine("DEBUG Start 3.5.1 " + ds.ToString());

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        //tlog.WriteLine("DEBUG Start 4 ");

                        DataTable dt = ds.Tables[0];
                        DataRow[] dr = dt.Select();
                        
                        if (dr.Length > 0)
                        {                            
                            XmlTextWriter writer = new XmlTextWriter(strSavePath.ToString(), Encoding.UTF8);

                            if (strIndentedYN == "Y")
                                writer.Formatting = System.Xml.Formatting.Indented;

                            writer.WriteStartDocument();
                            writer.WriteStartElement("NewDataSet");

                            for (int i = 0; i < dr.Length; i++)
                            {
                                writer.WriteStartElement("Table");

                                foreach (DataColumn dc in dt.Columns)
                                {
                                    SetMakeElement(writer, dc.ColumnName.ToString(), dr[i][dc.ColumnName].ToString(), strCDataYN);
                                }

                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                            writer.WriteEndDocument();

                            writer.Flush();
                            writer.Close();

                            //tlog.WriteLine("DEBUG Start 5 ");
                        }
                        else
                        {
                            if (strEmptyYN == "Y")
                                GetEmpty("XML");

                            //tlog.WriteLine("DEBUG Start 6 ");
                        }
                    }
                    else
                    {
                        if (strEmptyYN == "Y")
                            GetEmpty("XML");

                        //tlog.WriteLine("DEBUG Start 7 ");
                    }
                }
                else
                {
                    if (strEmptyYN == "Y")
                        GetEmpty("XML");
                    //strLog = string.Format("ErrCode : {0} , ErrMsg : {1}", "0001", "ds null");
                    //log.WriteLine(strLog.ToString());

                    //tlog.WriteLine("DEBUG Start 8 ");
                }

                bSuccess = true;

                #region #  Test     
                /*
                Employee[] employees = new Employee[4];
                employees[0] = new Employee(1, "David", "Smith", 10000);
                employees[1] = new Employee(3, "Mark", "Drinkwater", 30000);
                employees[2] = new Employee(4, "Norah", "Miller", 20000);
                employees[3] = new Employee(12, "Cecil", "Walker", 120000);

                //XmlTextWriter writer = new XmlTextWriter(Environment.CurrentDirectory + @"\TestXML.xml", Encoding.UTF8);
                XmlTextWriter writer = new XmlTextWriter(strSavePath.ToString(), Encoding.UTF8);

                if(strIndentedYN == "Y")
                    writer.Formatting = System.Xml.Formatting.Indented;

                writer.WriteStartDocument();
                writer.WriteStartElement("NewDataSet");

                foreach (Employee employee in employees)
                {
                    writer.WriteStartElement("Table");

                    SetMakeElement(writer, "ID", employee.Id.ToString(), "N");
                    SetMakeElement(writer, "FirstName", employee.FirstName.ToString(), strCDataYN);
                    SetMakeElement(writer, "CDATA_LastName", employee.LastName.ToString(), strCDataYN);
                    SetMakeElement(writer, "Salary", employee.Salary.ToString(), "N");

                    //
                    //writer.WriteElementString("ID", employee.Id.ToString());
                    ////writer.WriteElementString("FirstName", employee.FirstName);
                    //writer.WriteStartElement("FirstName");
                    //writer.WriteCData(employee.FirstName);
                    //writer.WriteEndElement();

                    //writer.WriteElementString("LastName", employee.LastName);
                    //writer.WriteElementString("Salary", employee.Salary.ToString());
                    //

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();
                writer.Close();
                
                bSuccess = true;
                */
                #endregion

            }
            catch (Exception ex)
            {
                //tlog.WriteLine("DEBUG Start 9 ");

                //Console.WriteLine(ex.ToString());
                strLog = string.Format("ErrCode : {0} , ErrMsg : {1}", "1001", ex.ToString());
                log.WriteLine(strLog.ToString());
            }
            return bSuccess;
        }

        private XmlTextWriter SetMakeElement(XmlTextWriter writer, string strID, string strValue, string strCDataYN)
        {
            if (strCDataYN == "Y")
            {
                writer.WriteStartElement(strID.ToString());
                writer.WriteCData(strValue.ToString());
                writer.WriteEndElement();
            }
            else if (strCDataYN == "M" && strID.Length >= 6)
            {
                if (strID.Substring(0, 6) == "CDATA_")
                {
                    writer.WriteStartElement(strID.ToString());
                    writer.WriteCData(strValue.ToString());
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteElementString(strID.ToString(), strValue.ToString());
                }
            }
            else
            {
                writer.WriteElementString(strID.ToString(), strValue.ToString());
            }
            return writer;
        }
        #endregion

        #region # MakeJSON  
        public bool MakeJSON()
        {
            // http://xml.interpark.com/Movie/Main/TestJSON.json

            bool bSuccess = false;
            string strLog = string.Empty;
            FileLog log = new FileLog(Config.GetLogFilePath());

            try
            {
                if (strDBTimeOutSec == string.Empty || strDBTimeOutSec == "")
                {
                    strDBTimeOutSec = "180";
                }
                Config.strDBName = strConFlag.ToString();
                Config.intDBTimeOut = Convert.ToInt32(strDBTimeOutSec.ToString());

                DBData db = new DBData();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = Convert.ToInt32(strDBTimeOutSec.ToString());
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSql.ToString();
                //log.WriteProcLog(cmd);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow[] dr = dt.Select();

                    if (dr.Length > 0)
                    {
                        string strJSON = string.Empty;

                        StringBuilder sb = new StringBuilder();
                        StringWriter sw = new StringWriter(sb);
                        JsonWriter writer = new JsonTextWriter(sw);

                        if (strIndentedYN == "Y")
                            writer.Formatting = Newtonsoft.Json.Formatting.Indented;

                        writer.WriteStartArray();

                        for (int i = 0; i < dr.Length; i++)
                        {
                            writer.WriteStartObject();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                writer.WritePropertyName(dc.ColumnName.ToString());
                                writer.WriteValue(dr[i][dc.ColumnName].ToString());
                            }
                            writer.WriteEndObject();
                        }

                        writer.WriteEndArray();
                        strJSON = sb.ToString();

                        writer.Close();
                        sw.Close();

                        //string strPath = Directory.GetCurrentDirectory().ToString() + @"\" + "TestJSON.json";
                        string strPath = strSavePath.ToString();
                        StreamWriter sw2 = new StreamWriter(strPath, false);
                        sw2.Write(strJSON.ToString());

                        sw2.Flush();
                        sw2.Close();
                    }
                    else
                    {
                        if (strEmptyYN == "Y")
                            GetEmpty("JSON");
                    }
                }
                else
                {
                    if (strEmptyYN == "Y")
                        GetEmpty("JSON");
                }

                ds.Dispose();

                bSuccess = true;

                #region # Test      
                /*
                string strJSON = string.Empty;

                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                JsonWriter writer = new JsonTextWriter(sw);

                if (strIndentedYN == "Y")
                    writer.Formatting = Newtonsoft.Json.Formatting.Indented;

                writer.WriteStartArray();

                foreach (Employee employee in employees)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("ID");
                    writer.WriteValue(employee.Id.ToString());
                    writer.WritePropertyName("First Name");                    
                    writer.WriteValue(employee.FirstName.ToString());
                    writer.WritePropertyName("LastName");
                    writer.WriteValue(employee.LastName.ToString());
                    writer.WritePropertyName("Salary");
                    writer.WriteValue(employee.Salary.ToString());
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
                strJSON = sb.ToString();

                writer.Close();
                sw.Close();
                                
                //string strPath = Directory.GetCurrentDirectory().ToString() + @"\" + "TestJSON.json";
                string strPath = strSavePath.ToString();
                StreamWriter sw2 = new StreamWriter(strPath, false);
                sw2.Write(strJSON.ToString());

                sw2.Flush();
                sw2.Close();

                bSuccess = true;
                */
                #endregion

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                strLog = string.Format("ErrCode : {0} , ErrMsg : {1}", "1001", ex.ToString());
                log.WriteLine(strLog.ToString());
            }
            return bSuccess;
        }
        #endregion

        #region # GetEmpty
        private void GetEmpty(string strKind)
        {
            try
            {
                if (strKind == "XML")
                {
                    XmlTextWriter writer = new XmlTextWriter(strSavePath.ToString(), Encoding.UTF8);
                    writer.WriteStartDocument();
                    writer.WriteStartElement("NewDataSet");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }
                else if (strKind == "JSON")
                {
                    string strJSON = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);
                    JsonWriter writer = new JsonTextWriter(sw);

                    writer.WriteStartArray();
                    writer.WriteNull();
                    writer.WriteEndArray();
                    strJSON = sb.ToString();

                    writer.Close();
                    sw.Close();

                    //string strPath = Directory.GetCurrentDirectory().ToString() + @"\" + "TestJSON.json";
                    string strPath = strSavePath.ToString();
                    StreamWriter sw2 = new StreamWriter(strPath, false);
                    sw2.Write(strJSON.ToString());

                    sw2.Flush();
                    sw2.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string GetEmptyStr(string strKind)
        {
            string strEmpty = string.Empty;

            if (strKind == "XML")
            {
                strEmpty = @"<?xml version=""1.0"" encoding=""utf-8""?><NewDataSet/>";
            }
            else if(strKind == "JSON")
            {
                strEmpty = @"[]";
            }

            return strEmpty;
        }
        #endregion

        #region # property
        public string ConFlag
        {
            get { return strConFlag; }
            set { this.strConFlag = value; }
        }
        public string SqlStr
        {
            get { return strSql; }
            set { this.strSql = value; }
        }
        public string SavePath
        {
            get { return strSavePath; }
            set { this.strSavePath = value; }
        }
        public string IndentedYN
        {
            get { return strIndentedYN; }
            set { this.strIndentedYN = value; }
        }
        public string CDataYN
        {
            get { return strCDataYN; }
            set { this.strCDataYN = value; }
        }
        public string DBTimeOutSec
        {
            get { return strDBTimeOutSec; }
            set { this.strDBTimeOutSec = value; }
        }
        public string EmptyYN
        {
            get { return strEmptyYN; }
            set { this.strEmptyYN = value; }
        }
        #endregion
    }
    #endregion
}
