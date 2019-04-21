using System;
using System.Text;
using System.Data;
using System.Data.Common;

namespace AdoToFormats.Lib
{
    class AdoNet<ConnectionT, AdaptorT, DataReaderT> : IDisposable
        where ConnectionT : class, IDbConnection, new()
        where AdaptorT : DbDataAdapter, new()
        where DataReaderT : class, IDataReader
    {
        protected ConnectionT con_ = null;
        private bool disposed = false;

        IDbTransaction tran_ = null;

        public AdoNet()
        {
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing && con_ != null)
            {
                con_.Dispose();
            }

            con_ = null;
            disposed = true;
        }

        public virtual void Open(string strConnectionString)
        {
            con_ = new ConnectionT();
            con_.ConnectionString = strConnectionString;
            con_.Open();
        }

        public virtual void BeginTransaction()
        {
            tran_ = con_.BeginTransaction();
        }

        public virtual void BeginTransaction(IsolationLevel lv)
        {
            tran_ = con_.BeginTransaction(lv);
        }

        public virtual void Commit()
        {
            tran_.Commit();
            tran_ = null;
        }

        public virtual void Rollback()
        {
            tran_.Rollback();
            tran_ = null;
        }

        public virtual void ChangeDatabae(string strDatabase)
        {
            con_.ChangeDatabase(strDatabase);
        }

        public virtual int Execute(IDbCommand cmd)
        {
            SetConnection(cmd);
            return cmd.ExecuteNonQuery();
        }

        public virtual object ExecuteScalar(IDbCommand cmd)
        {
            SetConnection(cmd);
            return cmd.ExecuteScalar();
        }

        public virtual DataReaderT ExecuteReader(IDbCommand cmd)
        {
            SetConnection(cmd);
            DataReaderT reader = (DataReaderT)cmd.ExecuteReader();

            return reader;
        }

        public virtual DataSet ExecuteDataSet(IDbCommand cmd)
        {
            AdaptorT adp = CreateDataAdaptor(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds;
        }

        public virtual DataSet ExecuteDataSet(IDbCommand cmd, string strTable)
        {
            AdaptorT adp = CreateDataAdaptor(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds, strTable);

            return ds;
        }

        protected virtual AdaptorT CreateDataAdaptor(IDbCommand cmd)
        {
            SetConnection(cmd);

            AdaptorT adp = new AdaptorT();
            adp.SelectCommand = (DbCommand)cmd;

            return adp;
        }

        protected virtual void SetConnection(IDbCommand cmd)
        {
            cmd.Connection = con_;
            if (tran_ != null)
            {
                cmd.Transaction = tran_;
            }
        }

    }

    class AdoNetSql :
        AdoToFormats.Lib.AdoNet<
            System.Data.SqlClient.SqlConnection,
            System.Data.SqlClient.SqlDataAdapter,
            System.Data.SqlClient.SqlDataReader
        >
    {
    }

    class AdoNetOdbc :
        AdoToFormats.Lib.AdoNet<
            System.Data.Odbc.OdbcConnection,
            System.Data.Odbc.OdbcDataAdapter,
            System.Data.Odbc.OdbcDataReader
        >
    {
    }

    class AdoNetOleDb :
        AdoToFormats.Lib.AdoNet<
            System.Data.OleDb.OleDbConnection,
            System.Data.OleDb.OleDbDataAdapter,
            System.Data.OleDb.OleDbDataReader
        >
    {
    }
}
