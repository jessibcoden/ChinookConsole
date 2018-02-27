using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookConsole.DataAccess
{
    class InvoiceNew
    {

        readonly string _connectionString = ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString;

        public bool AddNewInvoice(int customerId, string billingAddress)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Invoice (CustomerId	, BillingAddress)
                                    VALUES (@CustomerId, @BillingAddress);";

                var customerIdParam = new SqlParameter("@CustomerId", SqlDbType.Int);
                customerIdParam.Value = customerId;
                cmd.Parameters.Add(customerIdParam);

                var billingAddParam = new SqlParameter("@BillingAddress", SqlDbType.VarChar);
                billingAddParam.Value = billingAddress;
                cmd.Parameters.Add(billingAddParam);

                connection.Open();

                var result = cmd.ExecuteNonQuery();

                return result == 1;

            }
        }
    }
}
