using System;
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

        public int Insert(string area, string name, int quantity, string userName)
        {
            var commandText = @"insert into dbo.TextileStore
                                (Area,Name,Quantity,ModifyUser,ModifyDate)
                                VALUES
                                (@Area,@Name,@Quantity,@ModifyUser,@ModifyDate)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Area",SqlDbType.Text){Value = area},
                new SqlParameter("@Name",SqlDbType.Text){Value = name},
                new SqlParameter("@Quantity",SqlDbType.Int){Value = quantity},
                new SqlParameter("@ModifyUser",SqlDbType.Text){Value = userName},
                new SqlParameter("@ModifyDate",SqlDbType.DateTime){Value = DateTime.Now},
            };
            var result = DapperHelper.QueryCollection<int>(ConfigProvider.ConnectionString, CommandType.Text, commandText, parameters).FirstOrDefault();

            return result;
        }

        public IEnumerable<TextileStore> SearchArea(string area)
        {
            var commandText = @"select * from TextileStore Where Area like @Area";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Area",SqlDbType.Text){Value = string.Concat("%",area,"%")}
            };
            var result = DapperHelper.QueryCollection<TextileStore>(ConfigProvider.ConnectionString, CommandType.Text, commandText, parameters);

            return result;
        }
    }
}
