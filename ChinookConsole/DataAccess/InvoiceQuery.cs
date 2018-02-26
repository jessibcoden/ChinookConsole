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

        public List<InvoiceBySalesRep> GetInvoiceBySalesAgent(string agentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select i.InvoiceId InvoiceId, e.FirstName + ' ' + e.LastName AgentName
                                    from Invoice i
                                    inner join Customer c on i.CustomerId = c.CustomerId
                                    inner join Employee e on c.SupportRepId = e.EmployeeId
                                    where e.EmployeeId = @AgentId (select e.FirstName + ' ' + e.LastName from Employee e)";

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
                        AgentName = reader["AgentName"].ToString(),
                        InvoiceId = int.Parse(reader["InvoiceId"].ToString()),
                    };

                    invoices.Add(invoice);
                }

                return invoices;

            }
        }

        public List<Invoice> GetAllInvoices()
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select InvoiceCustomer.Total, InvoiceCustomer.CustomerName, InvoiceCustomer.CustomerCountry, Agent.SalesAgent
	                                from 
	                                    (select i.Total Total, c.FirstName + ' ' + c.LastName CustomerName, c.Country CustomerCountry, c.SupportRepId AgentId 
		                                from Invoice i
		                                inner join Customer c on i.CustomerId = c.CustomerId) InvoiceCustomer
	                                join 
	                                    (select e.FirstName + ' ' + e.LastName SalesAgent, e.EmployeeId EmployeeId
	                                    from Employee e
	                                    inner join Customer c on e.EmployeeId = c.SupportRepId) Agent 
	                                on Agent.EmployeeId = InvoiceCustomer.AgentId";

                connection.Open();
                var reader = cmd.ExecuteReader();

                var allInvoices = new List<Invoice>();

                while (reader.Read())
                {
                    var invoice = new Invoice
                    {
                        Total = double.Parse(reader["Total"].ToString()),
                        CustomerName = reader["CustomerName"].ToString(),
                        CustomerCountry = reader["CustomerCountry"].ToString(),
                        SalesAgent = reader["SalesAgent"].ToString(),
                    };

                    allInvoices.Add(invoice);
                }

                return allInvoices;

            }
        }


    }
}
