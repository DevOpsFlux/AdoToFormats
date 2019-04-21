using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Text;

namespace AdoToFormats.Lib
{
    [JustInTimeActivation(true)]
    [Transaction(TransactionOption.Supported)]
    public class DBData //: ServicedComponent
    {
        public DBData() { }

        [AutoComplete(true)]
        public DataSet ExecuteDataSet(SqlCommand cmd)
        {
            DataSet ds = null;

            try
            {

                using (AdoToFormats.Lib.AdoNetSql ado = AdoToFormats.Lib.DBUtil.OpenAdoNetSql())
                {
                    ds = ado.ExecuteDataSet(cmd);
                    ado.Dispose();
                }
            }
            catch (Exception ex)
            {

                try
                {
                    AdoToFormats.Lib.FileLog log = new AdoToFormats.Lib.FileLog(AdoToFormats.Lib.Config.GetLogFilePath());
                    log.WriteLine(ex.ToString());
                }
                catch (Exception) { }
            }


            return ds;
        }

        [AutoComplete(true)]
        public int Execute(SqlCommand cmd)
        {
            int nRows = 0;

            try
            {
                using (AdoToFormats.Lib.AdoNetSql ado = AdoToFormats.Lib.DBUtil.OpenAdoNetSql())
                {
                    nRows = ado.Execute(cmd);
                    ado.Dispose();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    AdoToFormats.Lib.FileLog log = new AdoToFormats.Lib.FileLog(AdoToFormats.Lib.Config.GetLogFilePath());
                    log.WriteLine(ex.ToString());
                }
                catch (Exception) { }
            }

            return nRows;
        }

    }

    class DBUtil
    {
        public static AdoToFormats.Lib.AdoNetSql OpenAdoNetSql()
        {
            AdoToFormats.Lib.AdoNetSql ado = new AdoToFormats.Lib.AdoNetSql();
            ado.Open(Config.GetConnectionString());

            return ado;
        }
    }
}
