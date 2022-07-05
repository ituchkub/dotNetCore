using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ITuChClass
{
    public class MsSQLConnection : IConnection
    {
        private OleDbConnection _Conn;
        private OleDbTransaction _Trans = null;
        private String _ConnStr;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of connection in IT-CAT solution.
        /// </summary>
        public MsSQLConnection()
        {
        }

        /// <summary>
        /// Initializes a new instance of connection and open it automatically.
        /// </summary>
        /// <param name="pConnStr">The connection string for create connection.</param>
        public MsSQLConnection(String pConnStr)
        {
            _ConnStr = pConnStr;
            this.open();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the connection string of this instance.
        /// </summary>
        /// <value>Connection string of connection.</value>
        /// <remarks> The value must be match conntection string.
        /// </remarks>
        public string ConnectionString
        {
            get
            {
                return _ConnStr;
            }
            set
            {
                _ConnStr = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Open connection to target database that assign with connection string. 
        /// </summary>
        /// <example> This sample shows how to manual open connection. 
        /// <code>
        /// 
        ///IConnection Conn = new MsSQLConnection();
        ///Conn.ConnectionString = ConnectionStr;
        ///Conn.open();
        ///DataTable tbl = Conn.exeQuery(SqlStr);
        ///Conn.close();
        ///
        ///</code>
        ///</example>
        /// <remarks> Not recommended to create connection by yourself. 
        /// You can use DbMgr class to manage your connection
        /// </remarks>
        ///
        public void open()
        {
            _Conn = new OleDbConnection(_ConnStr);
            _Conn.Open();
        }

        /// <summary>
        /// Initiates a nested database transaction in this connection. After you call this method, 
        /// All of activity will bind with transaction until you call commit.
        /// </summary>
        /// <example> This sample shows how to use transaction on this connection. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///Conn.beginTrans();
        ///try{
        ///	Conn.exeNonQuery("UPDATE mytable SET col = 'xyz'");
        ///	Conn.exeNonQuery("DELETE FROM mytable WHERE col2 is null");
        ///	Conn.commit();
        ///}catch(Exception ex){
        ///	Conn.rollback();
        ///	throw(ex);
        ///}finally{
        ///	Conn.close();
        ///}
        ///
        ///</code>
        ///</example>
        public void beginTrans()
        {
            _Trans = _Conn.BeginTransaction();
        }

        /// <summary>
        /// Query your SQL statement and return set of records to DataTable. 
        /// </summary>
        /// <returns>DataTable that contain set of results.</returns>
        /// <param name="pSqlStr">The SQL statement or stored procedure used to select records in the data source.</param>
        /// <example> This sample shows how to query from datasource. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///DataTable tbl = Conn.exeQuery("SELECT * FROM mytable WHERE col = 'xyz'");
        ///Conn.close();
        ///
        ///</code>
        ///</example>
        public DataTable exeQuery(String pSqlStr)
        {
            try
            {
                OleDbCommand cmd;
                if (_Trans != null)
                    cmd = new OleDbCommand(pSqlStr, _Conn, _Trans);
                else
                    cmd = new OleDbCommand(pSqlStr, _Conn);
                cmd.CommandTimeout = _Conn.ConnectionTimeout;
                DataTable Table = new DataTable();
                new OleDbDataAdapter(cmd).Fill(Table);
                return Table;
            }
            catch (OleDbException sqlEx)
            {
                throw this.GenException(pSqlStr, sqlEx);
            }
        }

        /// <summary>
        /// Query your SQL statement and return set of records to DataTable. 
        /// </summary>
        /// <returns>DataTable that contain set of results.</returns>
        /// <param name="pSqlStr">The SQL statement or stored procedure used to select records in the data source.</param>
        /// <param name="pNumberRecord">Number of record that you want to query</param>
        /// <example> This sample shows how to query from datasource. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///Connection Conn = DbMgr.GetConnection();
        ///DataTable tbl = Conn.exeQuery("SELECT * FROM mytable WHERE col = 'xyz'");
        ///Conn.close();
        ///
        ///</code>
        ///</example>
        public DataTable exeQuery(String pSqlStr, int pNumberRecord)
        {
            try
            {
                pSqlStr = pSqlStr.Replace("SELECT", "SELECT TOP " + pNumberRecord.ToString());
                return exeQuery(pSqlStr);
            }
            catch (OleDbException sqlEx)
            {
                throw this.GenException(pSqlStr, sqlEx);
            }
        }

        /// <summary>
        /// Executes a SQL statement and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        /// <param name="pSqlStr">The SQL statement or stored procedure to execute at the data source.</param>
        /// <example> This sample shows how to query from datasource. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///int roweffect = Conn.exeNonQuery("UPDATE mytable SET col = 'xyz'");
        ///Conn.close();
        ///
        ///</code>
        ///</example>
        public int exeNonQuery(String pSqlStr)
        {
            try
            {
                OleDbCommand cmd;
                if (_Trans == null)
                    cmd = new OleDbCommand(pSqlStr, _Conn);
                else
                    cmd = new OleDbCommand(pSqlStr, _Conn, _Trans);
                cmd.CommandTimeout = _Conn.ConnectionTimeout;
                return cmd.ExecuteNonQuery();
            }
            catch (OleDbException sqlEx)
            {
                throw this.GenException(pSqlStr, sqlEx);
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set 
        /// returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        /// <param name="pSqlStr">The SQL statement or stored procedure to execute at the data source.</param>
        /// <example> This sample shows how to query from datasource. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///int Cnt = DbMgr.convertInt(Conn.exeScalar("SELECT COUNT(*) FROM mytable"));
        ///Conn.close();
        ///
        ///</code>
        ///</example>
        public object exeScalar(String pSqlStr)
        {
            try
            {
                OleDbCommand cmd;
                if (_Trans == null)
                    cmd = new OleDbCommand(pSqlStr, _Conn);
                else
                    cmd = new OleDbCommand(pSqlStr, _Conn, _Trans);
                cmd.CommandTimeout = _Conn.ConnectionTimeout;
                return cmd.ExecuteScalar();
            }
            catch (OleDbException sqlEx)
            {
                throw this.GenException(pSqlStr, sqlEx);
            }
        }

        /// <summary>
        /// Executes the insert statement which insert into table that have auto number on primary key, 
        /// and returns the primary key of the row that insert to table.
        /// </summary>
        /// <returns>The primary key of the row that insert to table.</returns>
        /// <param name="pSqlStr">The SQL insert statement or stored procedure to execute at the data source.</param>
        /// <example> This sample shows how to insert to table with auto number on primary key. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///Conn.beginTrans();
        ///try{
        ///	int ID = Conn.exeInsertQuery("INSERT INTO mytable VALUES('xyz')");
        ///	Conn.exeNonQuery("INSERT INTO mytable2 (MyTableID) VALUES(" + ID.ToString() + ")");
        ///	Conn.commit();
        ///}catch(Exception ex){
        ///	Conn.rollback();
        ///	throw(ex);
        ///}finally{
        ///	Conn.close();
        ///}
        ///
        ///</code>
        ///</example>
        public int exeInsertQuery(String pSqlStr)
        {
            try
            {
                bool haveTrans = false;
                if (_Trans == null)
                    _Trans = _Conn.BeginTransaction();
                else
                    haveTrans = true;
                OleDbCommand cmd = new OleDbCommand(pSqlStr, _Conn, _Trans);
                cmd.CommandTimeout = _Conn.ConnectionTimeout;
                cmd.ExecuteNonQuery();
                DataTable Table = new DataTable();
                //pSqlStr = "SELECT @@IDENTITY AS NewID";
                pSqlStr = "SELECT SCOPE_IDENTITY() AS NewID";
                OleDbDataAdapter adap = new OleDbDataAdapter(pSqlStr, _Conn);
                adap.SelectCommand.Transaction = _Trans;
                adap.Fill(Table);
                if (!haveTrans)
                    _Trans.Commit();
                return Convert.ToInt32(Table.Rows[0][0]);
            }
            catch (OleDbException sqlEx)
            {
                throw this.GenException(pSqlStr, sqlEx);
            }
        }

        /// <summary>
        /// Executes the insert statement which insert into table that have auto number on primary key, 
        /// and returns the primary key of the row that insert to table.
        /// </summary>
        public int exeInsertQuery(String pSqlStr, string pTableName, string pPrimaryKey)
        {
            return this.exeInsertQuery(pSqlStr);
        }

        /// <summary>
        /// Rolls back a transaction from a pending state. 
        /// </summary>
        /// <example> This sample shows how to use transaction on this connection. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///Conn.beginTrans();
        ///try{
        ///	Conn.exeNonQuery("UPDATE mytable SET col = 'xyz'");
        ///	Conn.exeNonQuery("DELETE FROM mytable WHERE col2 is null");
        ///	Conn.commit();
        ///}catch(Exception ex){
        ///	Conn.rollback();
        ///	throw(ex);
        ///}finally{
        ///	Conn.close();
        ///}
        ///
        ///</code>
        ///</example>
        public void rollback()
        {
            _Trans.Rollback();
            _Trans = null;
        }
        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <example> This sample shows how to use transaction on this connection. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///Conn.beginTrans();
        ///try{
        ///	Conn.exeNonQuery("UPDATE mytable SET col = 'xyz'");
        ///	Conn.exeNonQuery("DELETE FROM mytable WHERE col2 is null");
        ///	Conn.commit();
        ///}catch(Exception ex){
        ///	Conn.rollback();
        ///	throw(ex);
        ///}finally{
        ///	Conn.close();
        ///}
        ///
        ///</code>
        ///</example>
        public void commit()
        {
            _Trans.Commit();
            _Trans = null;
        }

        /// <summary>
        /// Closes the connection to the data source. This is the preferred method of closing any open connection.
        /// </summary>
        /// <example> This sample shows how to use connection and closing it. 
        /// <code>
        /// 
        ///DbMgr.ConnectionString = ConnectionStr; //call only one time on your application
        ///IConnection Conn = DbMgr.GetConnection();
        ///Conn.beginTrans();
        ///try{
        ///	Conn.exeNonQuery("UPDATE mytable SET col = 'xyz'");
        ///	Conn.exeNonQuery("DELETE FROM mytable WHERE col2 is null");
        ///	Conn.commit();
        ///}catch(Exception ex){
        ///	Conn.rollback();
        ///	throw(ex);
        ///}finally{
        ///	Conn.close();
        ///}
        ///
        ///</code>
        ///</example>
        /// <remarks> You must close your connection when you do not use it. 
        /// Do not afraid to open and close connection many times,
        /// because we call them over connection pooling.
        /// </remarks>
        ///
        public void close()
        {
            _Conn.Close();
        }

        /// <summary>
        /// Create exception that contain firendly message. 
        /// </summary>
        private Exception GenException(string pSqlStr, OleDbException pOleDbEx)
        {
            if (pOleDbEx.Message.IndexOf("UNIQUE KEY") > -1 || pOleDbEx.Message.IndexOf("duplicate key") > -1)
            {
                return new Exception("Your are trying to enter duplicate item. Please enter new item.");
            }
            else if (pOleDbEx.Message.IndexOf("DELETE statement conflicted") > -1)
            {
                return new Exception("Your are trying to delete item that use in another place. Please delete relative item first.");
            }
            else if (pOleDbEx.Message.IndexOf("Cannot insert the value NULL") > -1)
            {
                return new Exception("Please enter all of require information.");
            }
            else if (pOleDbEx.Message.IndexOf("CustomErr") > -1)
            {
                string[] Str = pOleDbEx.Message.Substring(10).Split(':');
                return new Exception(Str[0]);
            }
            else
                return new Exception( "Database Error. {0}");
        }
        #endregion

        #region Command
        /// <summary>
        /// getStoreCommand
        /// </summary>
        /// <param name="pStoredName"></param>
        /// <returns></returns>
        public ICommand getStoreCommand(string pStoredName)
        {
            return new MsSQLCommand(pStoredName);
        }

        /// <summary>
        /// exeQuery
        /// </summary>
        /// <param name="pCmd"></param>
        /// <returns></returns>
        public DataTable exeQuery(ICommand pCmd)
        {
            OleDbCommand cmd = ((MsSQLCommand)pCmd).getInternalCmd();
            cmd.Connection = _Conn;
            cmd.CommandTimeout = _Conn.ConnectionTimeout;
            if (_Trans != null)
                cmd.Transaction = _Trans;
            OleDbDataAdapter adap = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds, TblCnt.ToString());
            TblCnt++;
            return ds.Tables[0].Copy();
        }

        /// <summary>
        /// exeNonQuery
        /// </summary>
        /// <param name="pCmd"></param>
        /// <returns></returns>
        public int exeNonQuery(ICommand pCmd)
        {
            OleDbCommand cmd = ((MsSQLCommand)pCmd).getInternalCmd();
            cmd.Connection = _Conn;
            cmd.CommandTimeout = _Conn.ConnectionTimeout;
            if (_Trans != null)
                cmd.Transaction = _Trans;
            return cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// exeScalar
        /// </summary>
        /// <param name="pCmd"></param>
        /// <returns></returns>
        public object exeScalar(ICommand pCmd)
        {
            OleDbCommand cmd = ((MsSQLCommand)pCmd).getInternalCmd();
            cmd.Connection = _Conn;
            cmd.CommandTimeout = _Conn.ConnectionTimeout;
            if (_Trans != null)
                cmd.Transaction = _Trans;
            return cmd.ExecuteScalar();
        }

        private static int TblCnt = 0;
        #endregion
    }
}
