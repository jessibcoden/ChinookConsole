using ChinookConsole.DataAccess.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ChinookConsole.DataAccess
{
    class EmployeeQuery
    {

        readonly string _connectionString = ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString;

        public Employee GetEmployeeById(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select e.FirstName, e.LastName
                                    from Employee e
                                    where e.EmployeeId = @EmployeeId";

                var employeeIdParam = new SqlParameter("@EmployeeId", SqlDbType.Int);
                employeeIdParam.Value = employeeId;
                cmd.Parameters.Add(employeeIdParam);

                connection.Open();
                var reader = cmd.ExecuteReader();

                var employee = new Employee();

                while (reader.Read())
                {
                    employee = new Employee
                    {
                        //EmployeeId = reader["EmployeeId"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString()
                    };


                }

                return employee;

            }

        }
    }
}
