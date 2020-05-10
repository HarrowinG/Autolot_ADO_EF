using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoLotDal.Models;

namespace AutoLotDal.DataOperations
{
    //need IDisposable, Exceptions for production
    public class InventoryDal
    {
        private readonly string _connectionString;
        private SqlConnection _sqlConnection = null;

        public InventoryDal() : this(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=AutoLot;Integrated Security=True") { }

        public InventoryDal(string connectionString) => _connectionString = connectionString;

        public List<Car> GetAllInventory()
        {
            OpenConnection();
            var inventory = new List<Car>();
            string sql = "Select * from dbo.Inventory";
            using (var command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dataReader.Read())
                    {
                        inventory.Add(new Car
                        {
                            CarId = (int)dataReader["CarId"],
                            Color = (string)dataReader["Color"],
                            Make = (string)dataReader["Make"],
                            PetName = (string)dataReader["PetName"]
                        });
                    }
                }
            }

            return inventory;
        }

        public Car GetCar(int id)
        {
            OpenConnection();
            Car car = null;
            string sql = $"Select * From dbo.Inventory where CarId = {id}";
            using (var command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                using (var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dataReader.Read())
                    {
                        car = new Car
                        {
                            CarId = (int)dataReader["CarId"],
                            Color = (string)dataReader["Color"],
                            Make = (string)dataReader["Make"],
                            PetName = (string)dataReader["PetName"]
                        };
                    }
                }
            }

            return car;
        }

        public void InsertAuto(string color, string make, string petName)
        {
            OpenConnection();
            string sql = $"Insert Into dbo.Inventory(Make, Color, PetName) values('{make}', '{color}', '{petName}')";
            using (var command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public void InsertAuto(Car car)
        {
            OpenConnection();
            string sql = "Insert Into dbo.Inventory(Make, Color, PetName) values(@Make, @Color, @PetName)";
            using (var command = new SqlCommand(sql, _sqlConnection))
            {
                var parameter = new SqlParameter
                {
                    ParameterName = "@Make",
                    Value = car.Make,
                    SqlDbType = SqlDbType.Char
                };

                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@Color",
                    Value = car.Color,
                    SqlDbType = SqlDbType.Char
                };

                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@PetName",
                    Value = car.PetName,
                    SqlDbType = SqlDbType.Char
                };

                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void DeleteCar(int id)
        {
            OpenConnection();
            string sql = $"Delete From dbo.Inventory where CarId = {id}";
            using (var command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    var error = new Exception("Sorry! That car is on order!", ex);
                    throw error;
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        public void UpdateCarPetName(int id, string petName)
        {
            OpenConnection();
            string sql = $"Update dbo.Inventory Set PetName = '{petName}' Where CarId = {id}";
            using (var command = new SqlCommand(sql, _sqlConnection))
            {
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public string LookUpPetName(int carId)
        {
            OpenConnection();
            string carPetName;
            using (var command = new SqlCommand("dbo.GetPetName", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                var parameter = new SqlParameter
                {
                    ParameterName = "@carId",
                    Value = carId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@petName",
                    SqlDbType = SqlDbType.Char,
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                carPetName = (string) command.Parameters["@petName"].Value;
                CloseConnection();
            }

            return carPetName;
        }

        public void ProcessCreditRisk(bool throwEx, int custId)
        {
            OpenConnection();
            string fName;
            string lName;
            using (var cmdSelect = new SqlCommand($"Select * from Customers where CustId = {custId}", _sqlConnection))
            using (var dataReader = cmdSelect.ExecuteReader())
            {
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    fName = (string) dataReader["FirstName"];
                    lName = (string)dataReader["LastName"];
                }
                else
                {
                    CloseConnection();
                    return;
                }
            }

            var cmdInsert = new SqlCommand($"Insert into CreditRisks(FirstName, LastName) values('{fName}', '{lName}')", _sqlConnection);
            var cmdRemove = new SqlCommand($"Delete from Customers where CustId = {custId}", _sqlConnection);
            

            SqlTransaction tx = null;
            try
            {
                tx = _sqlConnection.BeginTransaction();
                //don't forget to set in transaction context
                cmdInsert.Transaction = tx;
                cmdRemove.Transaction = tx;
                cmdInsert.ExecuteNonQuery();
                cmdRemove.ExecuteNonQuery();

                if (throwEx)
                {
                    throw new Exception("Error Raised initially to fail transaction");
                }

                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                tx?.Rollback();
            }
            finally
            {
                CloseConnection();
            }
        }

        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection(_connectionString);
            _sqlConnection.Open();
        }

        private void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }
    }
}
