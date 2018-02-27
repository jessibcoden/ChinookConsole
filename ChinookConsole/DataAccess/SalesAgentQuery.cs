using ChinookConsole.DataAccess.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ChinookConsole.DataAccess
{
    class SalesAgentQuery
    {
        readonly string _connectionString = ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString;

        public List<SalesAgent> GetAllSalesReps()
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select e.FirstName + ' ' + e.LastName  AgentName, e.EmployeeId AgentId
                                    from Employee e
                                    where e.Title like 'Sales % Agent'";

                connection.Open();
                var reader = cmd.ExecuteReader();


                var salesAgents = new List<SalesAgent>();

                while (reader.Read())
                {
                    var agent = new SalesAgent
                    {
                        AgentName = reader["AgentName"].ToString(),
                        AgentId = reader["AgentId"].ToString()
                    };

                    salesAgents.Add(agent);

                }

                return salesAgents;

            }

        }
    }
}
