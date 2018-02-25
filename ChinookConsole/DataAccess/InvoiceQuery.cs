using ChinookConsole.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookConsole.DataAccess
{
    class InvoiceQuery
    {

        readonly string _connectionString = ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString;

        public List<InvoiceBySalesRep> GetInvoiceBySalesAgent(int agentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select e.FirstName + ' ' + e.LastName AgentName, i.InvoiceId
                                    from Customer c
                                    inner join Invoice i on c.CustomerId = i.CustomerId
                                    where exists (select e.FirstName + ' ' + e.LastName AgentName from Employee
                                                  where EmployeeId like @AgentId + '%' and EmployeeId = c.SupportRepId)";

                var AgentIdParam = new SqlParameter("@AgentId", System.Data.SqlDbType.VarChar);
                AgentIdParam.Value = agentId;
                cmd.Parameters.Add(AgentIdParam);

                connection.Open();
                var reader = cmd.ExecuteReader();


                var invoices = new List<InvoiceBySalesRep>();

                while (reader.Read())
                {
                    var invoice = new InvoiceBySalesRep
                    {

                        //Takes in the column (0 would be first common)
                        AgentName = reader["AgentName"].ToString(),
                        InvoiceId = int.Parse(reader["InvoiceId"].ToString()),

                    };

                    invoices.Add(invoice);

                }

                return invoices;

            }
        }
        

    }
}
