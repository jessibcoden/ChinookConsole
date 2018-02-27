using ChinookConsole.DataAccess.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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

        public int GetCountOfLineItems(int invoiceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select count (il.InvoiceLineId)
                                    from InvoiceLine il
                                    inner join Invoice i on il.InvoiceId = i.InvoiceId
                                    where i.InvoiceId = @InvoiceId";

                var InvoiceIdParam = new SqlParameter("@InvoiceId", System.Data.SqlDbType.Int);
                InvoiceIdParam.Value = invoiceId;
                cmd.Parameters.Add(InvoiceIdParam);

                connection.Open();
                var count = (int.Parse(cmd.ExecuteScalar().ToString()));

                return count;

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
                    var invoice = new Invoice();

                    if (!reader.IsDBNull(reader.GetOrdinal("Total")))
                    {
                        invoice.Total = double.Parse(reader["Total"].ToString());
                    }
                    else
                    {
                        invoice.Total = 0.00;
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("CustomerName")))
                    {
                        invoice.CustomerName = reader["CustomerName"].ToString();
                    }
                    else
                    {
                        invoice.CustomerName = "Who Dat";
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("CustomerCountry")))
                    {
                        invoice.CustomerCountry = reader["CustomerCountry"].ToString();
                    }
                    else
                    {
                        invoice.CustomerCountry = "Doofusland";
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("SalesAgent")))
                    {
                        invoice.SalesAgent = reader["SalesAgent"].ToString();
                    }
                    else
                    {
                        invoice.SalesAgent = "Smarmy Bastard";
                    }

                    allInvoices.Add(invoice);
                }

                return allInvoices;

            }
        }

        public List<Invoice> GetInvoiceByCustomer(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select InvoiceCustomer.Total, InvoiceCustomer.CustomerName, InvoiceCustomer.CustomerCountry, Agent.SalesAgent, InvoiceCustomer.InvoiceId InvoiceId
	                                from 
	                                    (select i.Total Total, c.FirstName + ' ' + c.LastName CustomerName, c.Country CustomerCountry, c.SupportRepId AgentId, i.CustomerId CustomerId, i.InvoiceId InvoiceId
		                                from Invoice i
		                                inner join Customer c on i.CustomerId = c.CustomerId) InvoiceCustomer
	                                join 
	                                    (select e.FirstName + ' ' + e.LastName SalesAgent, e.EmployeeId EmployeeId
	                                    from Employee e
	                                    inner join Customer c on e.EmployeeId = c.SupportRepId) Agent 
	                                on Agent.EmployeeId = InvoiceCustomer.AgentId
                                    where InvoiceCustomer.CustomerId = @CustomerId";

                var CustIdParam = new SqlParameter("@CustomerId", System.Data.SqlDbType.Int);
                CustIdParam.Value = customerId;
                cmd.Parameters.Add(CustIdParam);

                connection.Open();
                var reader = cmd.ExecuteReader();

                var invoices = new List<Invoice>();

                while (reader.Read())
                {
                    var invoice = new Invoice();

                        invoice.InvoiceId = int.Parse(reader["InvoiceId"].ToString());


                    if (!reader.IsDBNull(reader.GetOrdinal("Total")))
                    {
                        invoice.Total = double.Parse(reader["Total"].ToString());
                    }
                    else
                    {
                        invoice.Total = 0.00;
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("CustomerName")))
                    {
                        invoice.CustomerName = reader["CustomerName"].ToString();
                    }
                    else
                    {
                        invoice.CustomerName = "Who Dat";
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("CustomerCountry")))
                    {
                        invoice.CustomerCountry = reader["CustomerCountry"].ToString();
                    }
                    else
                    {
                        invoice.CustomerCountry = "Doofusland";
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("SalesAgent")))
                    {
                        invoice.SalesAgent = reader["SalesAgent"].ToString();
                    }
                    else
                    {
                        invoice.SalesAgent = "Smarmy Bastard";
                    }

                    invoices.Add(invoice);
                }

                return invoices;

            }
        }

        }
    }
