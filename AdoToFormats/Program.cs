/*-------------------------------------------------------
'	프로그램명	: AdoToFormats.exe
'	작성자		: DevOpsFlux
'	작성일		: 2015-01-20
'	설명		: DB Ado Procedure-> XML/JSON
' -------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using AdoToFormats.Lib;

namespace AdoToFormats
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                [필수]
                /F [FORMAT]                 : XML, JSON
                /C [CONNECTION DB]		    : 연결 DB -> Config [ TESTDB / STAGINGDB ]
                /S [SQLCOMMAND]			    : SQL Query 문자열
                /O [OUTPUT_FILEPATH]		: 파일 저장 경로
                
                [옵션]
                /I [INDENTED OPTION]        : XML,JSON 들여쓰기 Y/N [Default:N]
                /D [CDATA OPTION]           : XML CDATA Y/M(Filed PreFix:CDATA_)/N [Default:N]
                /T [COMMAND TIMEOUT]		: SQL TimeOut 설정 [Default:180(초:3분)]
                /X [EMPTY]			        : 빈 파일생성 Y/N [Default:N (빈 파일생성 안함)]             
                /H [EXE INFO]               : 실행 옵션 정보

            */
            //Console.WriteLine("args.Length : " +  args.Length.ToString());
            //GLASS.TraceLog tlog = new GLASS.TraceLog();
            //tlog.WriteLine("DEBUG Start 0 ");

            // [필수]
            string strFormat = string.Empty;
            string strConFlag = string.Empty;
            string strSql = string.Empty;
            string strSavePath = string.Empty;
            // [옵션]
            string strIndentedYN = "N";
            string strCDataYN = "N";
            string strDBTimeOutSec = "180";
            string strEmptyYN = "N";
            string strHelpYN = "N";

            bool bSuccess = false;
            string strErrMsg = string.Empty;

            try
            {
                if (args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].ToUpper() == "/F")
                            strFormat = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/C")
                            strConFlag = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/S")
                            strSql = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/O")
                            strSavePath = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/I")
                            strIndentedYN = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/D")
                            strCDataYN = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/T")
                            strDBTimeOutSec = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/X")
                            strEmptyYN = args[i + 1].ToString();
                        else if (args[i].ToUpper() == "/H")
                            strHelpYN = "Y";

                        //Console.WriteLine("args["+i.ToString()+"] : " + args[i].ToString());
                    }
                }

                if (strHelpYN == "Y")
                {
                    GetHelpInfo();
                }
                else
                {
                    // 필수항목 체크
                    if (strFormat == string.Empty || strConFlag == string.Empty || strSql == string.Empty || strSavePath == string.Empty)
                    {
                        Console.WriteLine("Args Input Error");
                    }
                    else
                    {
                        ////Debug
                        //GetArgsInfo(strFormat, strConFlag, strSql, strSavePath, strIndentedYN, strCDataYN, strDBTimeOutSec, strEmptyYN);

                        /* */
                        IMakeFormats imf = new ToFormats();
                        imf.ConFlag = strConFlag;
                        imf.SqlStr = strSql;
                        imf.SavePath = strSavePath;
                        imf.IndentedYN = strIndentedYN;
                        imf.CDataYN = strCDataYN;
                        imf.DBTimeOutSec = strDBTimeOutSec;
                        imf.EmptyYN = strEmptyYN;

                        if (strFormat == "XML")
                        {
                            bSuccess = imf.MakeXML();
                        }
                        else if (strFormat == "JSON")
                        {
                            bSuccess = imf.MakeJSON();
                        }
                        else
                        {
                            bSuccess = false;
                        }
                        /* */

                        ////Debug
                        //if (bSuccess)
                        //    Console.WriteLine("Success");
                        //else
                        //    Console.WriteLine("Fail");
                    }
                }
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.ToString());
                //Console.WriteLine("Exception Error");
                strErrMsg = "Exception Error";
            }
            finally
            {
                if (!bSuccess || bSuccess)
                {                    
                    string strLog = string.Empty;
                    FileLog log = new FileLog(Config.GetLogFilePath());
                    strLog = string.Format("bSuccess : {0} , ErrMsg : {1}", bSuccess.ToString(), strErrMsg.ToString());
                    log.WriteLine(strLog.ToString());
                    strLog = GetArgsInfoStr(strFormat, strConFlag, strSql, strSavePath, strIndentedYN, strCDataYN, strDBTimeOutSec, strEmptyYN);
                    log.WriteLine(strLog.ToString());
                }
            }

        }

        #region # static BaseInfo       
        public static void GetHelpInfo()
        {
            Console.WriteLine("[필수]");
            Console.WriteLine("/F [FORMAT]              : XML, JSON");
            Console.WriteLine("/C [CONNECTION DB]       : 연결 DB -> Config [ TESTDB / STAGINGDB ]");
            Console.WriteLine("/S [SQLCOMMAND]          : SQL Query 문자열");
            Console.WriteLine("/O [OUTPUT_FILEPATH]     : 파일 저장 경로");
            Console.WriteLine("[옵션]");
            Console.WriteLine("/I [INDENTED OPTION]     : XML,JSON 들여쓰기 Y/N [Default:N]");
            Console.WriteLine("/D [CDATA OPTION]        : XML CDATA Y/M(Filed PreFix:CDATA_)/N [Default:N]");
            Console.WriteLine("/T [COMMAND TIMEOUT]     : SQL TimeOut 설정 [Default:180(초:3분)]");
            Console.WriteLine("/X [EMPTY]               : 빈 파일생성 Y/N [Default:N (빈 파일생성 안함)]");
            Console.WriteLine("/H [EXE INFO]            : 실행 옵션 정보");
        }

        public static void GetArgsInfo(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8)
        {
            Console.WriteLine("strFormat : " + p1.ToString());
            Console.WriteLine("strConFlag : " + p2.ToString());
            Console.WriteLine("strSql : " + p3.ToString());
            Console.WriteLine("strSavePath : " + p4.ToString());

            Console.WriteLine("strIndentedYN : " + p5.ToString());
            Console.WriteLine("strCDataYN : " + p6.ToString());
            Console.WriteLine("strDBTimeOutSec : " + p7.ToString());
            Console.WriteLine("strEmptyYN : " + p8.ToString());
        }

        public static string GetArgsInfoStr(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8)
        {
            string strTmp = string.Empty;
            strTmp = "/F : " + p1.ToString() + "  ";
            strTmp += "/C : " + p2.ToString() + "  ";
            strTmp += "/S : " + p3.ToString() + "  ";
            strTmp += "/O : " + p4.ToString() + "  ";

            strTmp += "/I : " + p5.ToString() + "  ";
            strTmp += "/D : " + p6.ToString() + "  ";
            strTmp += "/T : " + p7.ToString() + "  ";
            strTmp += "/X : " + p8.ToString();
            /*
            strTmp = "/F : " + p1.ToString() + Environment.NewLine;
            strTmp += "/C : " + p2.ToString() + Environment.NewLine;
            strTmp += "/S : " + p3.ToString() + Environment.NewLine;
            strTmp += "/O : " + p4.ToString() + Environment.NewLine;

            strTmp += "/I : " + p5.ToString() + Environment.NewLine;
            strTmp += "/D : " + p6.ToString() + Environment.NewLine;
            strTmp += "/T : " + p7.ToString() + Environment.NewLine;
            strTmp += "/X : " + p8.ToString();
            */
            return strTmp;
        }
        #endregion

    }



    #region class Test Employee     
    class Employee
    {
        int _id;
        string _firstName;
        string _lastName;
        int _salary;

        public Employee(int id, string firstName, string lastName, int salary)
        {
            this._id = id;
            this._firstName = firstName;
            this._lastName = lastName;
            this._salary = salary;
        }

        public int Id { get { return _id; } }
        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public int Salary { get { return _salary; } }
    }
    #endregion

}
