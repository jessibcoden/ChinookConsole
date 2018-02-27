using ChinookConsole.DataAccess;
using ChinookConsole.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookConsole
{
    class Program
    {
        static SalesAgent _selectedAgent;
        static string _customerId;
        static string _billingAddress;
        static string _selectedEmployeeId;
        static string _updatedEmployeeFirstName;
        static string _updatedEmployeeLastName;

        static void Main(string[] args)
        {

            var run = true;
            while (run)
            {
                ConsoleKeyInfo userInput = MainMenu();

                switch (userInput.KeyChar)
                {
                    case '0':
                        run = false;
                        break;

                    case '1': 
                        ConsoleKeyInfo userOption = SelectInvoiceQueryType();
                            switch (userOption.KeyChar)
                            {
                                case '1': //Provide a query that shows the invoices associated with each sales agent. The resultant table should include the Sales Agent's full name.
                                _selectedAgent = SelectSalesAgent();
                                    var invoicesBySelectedAgent = ShowInvoicesByAgent(_selectedAgent);
                                    break;

                                case '2': //Provide a query that shows the Invoice Total, Customer name, Country and Sale Agent name for all invoices.
                                var allInvoices = ShowAllInvoices();
                                    break;

                                case '3': //Looking at the InvoiceLine table, provide a query that COUNTs the number of line items for an Invoice with a parameterized Id from user input (use ExecuteScalar)

                                break;

                                default:
                                    break;
                            }
                        break;

                    case '2': //INSERT a new invoice with parameters for customerid and billing address
                        _customerId = SelectCustomerId();
                        var custId = int.Parse(_customerId);
                        _billingAddress = GrabBillingAddress();
                        var createInvoice = CreateNewInvoice(custId, _billingAddress);
                        break;

                    case '3'://UPDATE an Employee's name with a parameter for Employee Id and new name
                        _selectedEmployeeId = SelectEmployeeId();
                        var empId = int.Parse(_selectedEmployeeId);
                        _updatedEmployeeFirstName = UpdatedFirstName(empId);
                        _updatedEmployeeLastName = UpdatedLastName(empId);
                        var updateEmployeeName = UpdateEmployeeName(empId, _updatedEmployeeFirstName, _updatedEmployeeLastName);
                        break;

                    default:
                        break;
                }
            }
        }

        static ConsoleKeyInfo MainMenu()
        {
            View mainMenu = new View();

            mainMenu.AddMenuOption("Search Invoice(s)")
            .AddMenuOption("Create Invoice")
            .AddMenuOption("Update Employee Record")
            .AddMenuText("Press 0 to exit.");

            Console.Write(mainMenu.GetFullMenu());
            ConsoleKeyInfo userOption = Console.ReadKey();
            return userOption;
        }

        static ConsoleKeyInfo SelectInvoiceQueryType()
        {
            var invoiceQueryMenu = new View();

            invoiceQueryMenu.AddMenuText("Select how to view invoices:")
                .AddMenuOption("Search by Sales Agent")
                .AddMenuOption("Show all invoices")
                .AddMenuOption("Search # of Line Items by Invoice")
                .AddMenuText("Press 0 to go BACK.");

            Console.Write(invoiceQueryMenu.GetFullMenu());
            ConsoleKeyInfo userOption = Console.ReadKey();
            return userOption;
        }

        static SalesAgent SelectSalesAgent()
        {
            var AllSalesAgents = new List<SalesAgent>();
            var agentQuery = new SalesAgentQuery();
            var agents = agentQuery.GetAllSalesReps();

            var salesAgentMenu = new View();

            salesAgentMenu.AddMenuText("Select Sales Agent");
                foreach (var agent in agents)
                {
                    AllSalesAgents.Add(agent);
                    salesAgentMenu.AddMenuOption($"{agent.AgentName}");
                };
            salesAgentMenu.AddMenuText("Press 0 to go BACK.");

            Console.Write(salesAgentMenu.GetFullMenu());

            int agentSelected = int.Parse(Console.ReadKey().KeyChar.ToString());
            var selectedSalesAgent = AllSalesAgents[agentSelected - 1];
            return AllSalesAgents.First<SalesAgent>(x => x == selectedSalesAgent);
        }

        static ConsoleKeyInfo ShowInvoicesByAgent(SalesAgent salesAgent)
        {
            var InvoiceList = new List<InvoiceBySalesRep>();
            var invoiceQuery = new InvoiceQuery();
            var agent = salesAgent.ToString();
            var invoices = invoiceQuery.GetInvoiceBySalesAgent(salesAgent.AgentId);

            var invoicesByAgentView = new View();

            invoicesByAgentView.AddMenuText("Agent Results:");
            foreach (var invoice in invoices)
            {
                InvoiceList.Add(invoice);
                invoicesByAgentView.AddMenuText($"{invoice.AgentName}  {invoice.InvoiceId} ");
            };
            invoicesByAgentView.AddMenuText("Press 0 to go BACK.");

            Console.Write(invoicesByAgentView.GetFullMenu());

            ConsoleKeyInfo userOption = Console.ReadKey();
            return userOption;

        }

        static ConsoleKeyInfo ShowAllInvoices ()
        {
            var invoiceQuery = new InvoiceQuery();
            var invoices = invoiceQuery.GetAllInvoices();

            var viewAllInvoices = new View();

            viewAllInvoices.AddMenuText("Agent Results:");
            foreach (var invoice in invoices)
            {
                viewAllInvoices.AddMenuText($"{invoice.Total}  {invoice.CustomerName}  {invoice.CustomerCountry}  {invoice.SalesAgent}");
            };
            viewAllInvoices.AddMenuText("Press 0 to go BACK.");

            Console.Write(viewAllInvoices.GetFullMenu());

            ConsoleKeyInfo userOption = Console.ReadKey();
            return userOption;

        }

//ADD NEW INVOICE
        static string SelectCustomerId()
        {
            var selectCustomerMenu = new View();

            selectCustomerMenu.AddMenuText("Enter Customer ID:");

            Console.Write(selectCustomerMenu.GetFullMenu());
            _customerId = Console.ReadLine();

            return _customerId;
        }

        static string GrabBillingAddress()
        {
            var getBillingAddMenu = new View();

            getBillingAddMenu.AddMenuText("Enter Billing Address:");

            Console.Write(getBillingAddMenu.GetFullMenu());
            _billingAddress = Console.ReadLine();

            return _customerId;
        }

        static ConsoleKeyInfo CreateNewInvoice(int customerId, string billingAddress)
        {
            var InvoiceList = new List<Invoice>();

            var newInvoiceQuery = new InvoiceNew();
            var addNewInvoice = newInvoiceQuery.AddNewInvoice(customerId, billingAddress);

            var invoiceQuery = new InvoiceQuery();
            var invoices = invoiceQuery.GetInvoiceByCustomer(customerId);

            var invoicesByCustView = new View();
            invoicesByCustView.AddMenuText($"All invoices for this customer:");
            foreach (var invoice in invoices)
            {
                invoicesByCustView.AddMenuText($"{invoice.InvoiceId} {invoice.CustomerName} {invoice.SalesAgent} {invoice.Total} ");
            };

            invoicesByCustView.AddMenuText("Press 0 to go BACK.");

            Console.Write(invoicesByCustView.GetFullMenu());

            ConsoleKeyInfo userOption = Console.ReadKey();

            return userOption;

        }

        //UPDATE EMPLOYEE NAME
        static string SelectEmployeeId()
        {
            var selectEmployeeMenu = new View();

            selectEmployeeMenu.AddMenuText("Enter Employee ID:");

            Console.Write(selectEmployeeMenu.GetFullMenu());
            _selectedEmployeeId = Console.ReadLine();

            return _selectedEmployeeId;
        }

        static string UpdatedFirstName(int employeeId)
        {
            var employeeNameMenu = new View();

            var employeeQuery = new EmployeeQuery();
            var employee = employeeQuery.GetEmployeeById(employeeId);

            employeeNameMenu.AddMenuText($"Employee Current First Name:  {employee.FirstName}");
            employeeNameMenu.AddMenuText("Enter New First Name:");

            Console.Write(employeeNameMenu.GetFullMenu());

            _updatedEmployeeFirstName = Console.ReadLine();

            return _updatedEmployeeFirstName;
        }

        static string UpdatedLastName(int employeeId)
        {
            var employeeNameMenu = new View();

            var employeeQuery = new EmployeeQuery();
            var employee = employeeQuery.GetEmployeeById(employeeId);

            employeeNameMenu.AddMenuText($"Employee's Current Last Name:  {employee.FirstName}");
            employeeNameMenu.AddMenuText("Enter New Last Name:");

            Console.Write(employeeNameMenu.GetFullMenu());

            _updatedEmployeeLastName = Console.ReadLine();

            return _updatedEmployeeLastName;
        }

        static ConsoleKeyInfo UpdateEmployeeName(int employeeId, string firstName, string lastName)
        {
            var modifyEmployeeQuery = new EmployeeModifier();
            var updateEmployee = modifyEmployeeQuery.UpdateName(employeeId, firstName, lastName);

            var employeeQuery = new EmployeeQuery();
            var employee = employeeQuery.GetEmployeeById(employeeId);

            var updatedEmployeeName = new View();

            updatedEmployeeName.AddMenuText($"Employee name has been updated to: {employee.FirstName} {employee.LastName}");

            updatedEmployeeName.AddMenuText("Press 0 to go BACK.");

            Console.Write(updatedEmployeeName.GetFullMenu());

            ConsoleKeyInfo userOption = Console.ReadKey();

            return userOption;

        }

    }
}

