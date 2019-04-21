using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Text;

namespace GLASS
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

                using (GLASS.AdoNetSql ado = GLASS.DBUtil.OpenAdoNetSql())
                {
                    ds = ado.ExecuteDataSet(cmd);
                    ado.Dispose();
                }
            }
            catch (Exception ex)
            {

                try
                {
                    GLASS.FileLog log = new GLASS.FileLog(GLASS.Config.GetLogFilePath());
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
                using (GLASS.AdoNetSql ado = GLASS.DBUtil.OpenAdoNetSql())
                {
                    nRows = ado.Execute(cmd);
                    ado.Dispose();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    GLASS.FileLog log = new GLASS.FileLog(GLASS.Config.GetLogFilePath());
                    log.WriteLine(ex.ToString());
                }
                catch (Exception) { }
            }

            return nRows;
        }

    }

    class DBUtil
    {
        public static GLASS.AdoNetSql OpenAdoNetSql()
        {
            GLASS.AdoNetSql ado = new GLASS.AdoNetSql();
            ado.Open(Config.GetConnectionString());

            return ado;
        }
    }
}
