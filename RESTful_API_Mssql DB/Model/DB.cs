using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITuChClass
{
    public class ITuChDB
    {

        private static string _Port = "";
        private static string _SERVER = "";
        private static string _DATABASE = "";
        private static string _UserID = "";
        private static string _PASSWORD = "";
        private static string _ConnStr = "";
       // private static enumDBProviderType _DBProvider = enumDBProviderType.MySQL;


        #region Properties

        public static void SetConnection(string pServerName, string pDATABASE, string pUserID, string pPASSWORD)
        {
            _SERVER = pServerName;
            _DATABASE = pDATABASE;
            _UserID = pUserID;
            _PASSWORD = pPASSWORD;
         //   _DBProvider = Type;

        }

        public static string CreateConnection()
        {
            
         
               
                    _ConnStr = "Provider=SQLOLEDB;Data Source=" + _SERVER +
                                           ";User Id=" + _UserID + ";Password=" + _PASSWORD +
                                           "; Initial Catalog=" + _DATABASE + ";Connect Timeout=60;";
               
            
            return _ConnStr;
        }

        public static IConnection GetConnection()
        {

            CreateConnection();

         
                    return new MsSQLConnection(_ConnStr);
               
        }
        #endregion
    }
}
