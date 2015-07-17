using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTest_CSharp
{
    [Serializable]
    public class DBConfiguration
    {
        public DBConfiguration()
        {
            DBName = "csWebCrawler";
            ContentTable = "c";
            ContentTableSchema = "id Integer IDENTITY(1,1) PRIMARY KEY, uri VARCHAR(255), content TEXT";
            //Leave this blank for windows auth
            Username = "";
            Password = "";
            Host = "localhost\\sqlexpress";

        }

        //Delegates here. No need for types if we have these.
        #region dbgetters
        /// <summary>
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        /// </summary>
        public String DBName { get; set; }

        /// <summary>
        /// </summary>
        public String ContentTable { get; set; }

        /// <summary>
        /// </summary>
        public String ContentTableSchema { get; set; }

        /// <summary>
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// </summary>
        public String Password { get; set; }

        #endregion dbgetters
    }
}
