using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using WebApi.DataClass;

namespace WebApi.Adapter
{
    public class DataAdapter
    {

        private string conStr = "Server=tcp:hansdb.database.windows.net,1433;Initial Catalog=HansDB;Persist Security Info=False;User ID=vigorhan;Password=yuiop_7410;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public List<DataObject> Get()
        {
            var result = new List<DataObject>();
           using(var con = new SqlConnection(conStr))
           {
               result = con.Query<DataObject>("select * from TableName").ToList();
           }

           return result;
        } 
    }    
}
