using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DataAccess.EFCore
{
    public static class DbContextExtensions
    {
        /*
         * need
            Microsoft.EntityFrameworkCore
            Microsoft.EntityFrameworkCore.Relational
         */
        public static DataTable DataTable(this DbContext context,
       string sqlQuery, params DbParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            DbConnection connection = context.Database.GetDbConnection();
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }


        public static DataSet DataSet(this DbContext context,
      string sqlQuery, params DbParameter[] parameters)
        {
            DataSet dset = new DataSet();
            DbConnection connection = context.Database.GetDbConnection();
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dset);
                }
            }
            return dset;
        }
    }
}
