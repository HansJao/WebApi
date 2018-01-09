using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using WebApi.Common;
using WebApi.DataClass;

namespace WebApi.Adapter
{
    public class DataAdapter
    {
        public List<DataObject> Get(int TableNameId)
        {
            var commandText = "select * from TableName Where TableNameId=@TableNameId";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TableNameId",SqlDbType.Int){Value = TableNameId}
            };
            var result = DapperHelper.QueryCollection<DataObject>(ConfigProvider.ConnectionString, CommandType.Text, commandText, parameters);

            return result.ToList();
        }
    }
}
