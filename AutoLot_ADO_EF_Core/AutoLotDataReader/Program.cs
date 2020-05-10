using System;
using System.Data.SqlClient;

namespace AutoLotDataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=AutoLot;Integrated Security=True";
                connection.Open();

                string sql = "Select * From dbo.Inventory; Select * from dbo.Customers";
                SqlCommand command = new SqlCommand(sql, connection);
                using (var dataReader = command.ExecuteReader())
                {
                    do
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine("***Record***");
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                Console.WriteLine($"{dataReader.GetName(i)} = {dataReader.GetValue(i)} ");
                            }
                            Console.WriteLine();
                        }
                    } while (dataReader.NextResult());
                }
            }

            Console.ReadLine();
        }
    }
}
