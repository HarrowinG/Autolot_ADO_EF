using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AutoLotDal.BulkImport
{
    public static class ProcessBulkImport
    {
        private static readonly string _connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=AutoLot;Integrated Security=True";
        private static SqlConnection _sqlConnection = null;

        public static void ExecuteBulkCopy<T>(IEnumerable<T> records, string tableName)
        {
            OpenConnection();
            using (var connection = _sqlConnection)
            {
                var bc = new SqlBulkCopy(connection)
                {
                    DestinationTableName = tableName
                };

                var dataReader = new MyDataReader<T> { Records = records.ToList() };
                try
                {
                    bc.WriteToServer(dataReader);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        private static void OpenConnection()
        {
            _sqlConnection = new SqlConnection(_connectionString);
            _sqlConnection.Open();
        }

        private static void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }
    }
}
