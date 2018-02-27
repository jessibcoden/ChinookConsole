using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ChinookConsole.DataAccess
{
    class EmployeeModifier
    {

        readonly string _connectionString = ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString;

        public bool UpdateName(int employeeId, string firstName, string lastName)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"Update Employee
                                    set Employee.FirstName = @UpdatedFirstName, Employee.LastName = @UpdatedLastName
                                     where Employee.EmployeeId = @EmployeeId";

                var employeeIdParam = new SqlParameter("@EmployeeId", SqlDbType.Int);
                employeeIdParam.Value = employeeId;
                cmd.Parameters.Add(employeeIdParam);

                var updatedFirstName = new SqlParameter("@UpdatedFirstName", SqlDbType.VarChar);
                updatedFirstName.Value = firstName;
                cmd.Parameters.Add(updatedFirstName);

                var updatedLastName = new SqlParameter("@UpdatedLastName", SqlDbType.VarChar);
                updatedLastName.Value = lastName;
                cmd.Parameters.Add(updatedLastName);

                connection.Open();

                var result = cmd.ExecuteNonQuery();

                return result == 1;

            }
        }

    }
}
