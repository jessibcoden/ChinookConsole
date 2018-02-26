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
        static Employee _selectedEmployee;
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

                        break;

                    case '3'://UPDATE an Employee's name with a parameter for Employee Id and new name
                        _selectedEmployeeId = SelectEmployeeId();
                        _updatedEmployeeFirstName = UpdatedFirstName(_selectedEmployeeId);
                        var id = int.Parse(_selectedEmployeeId);
                        _updatedEmployeeLastName = UpdatedLastName(_selectedEmployeeId);
                        var updateEmployeeName = UpdateEmployeeName(id, _updatedEmployeeFirstName, _updatedEmployeeLastName);
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
                //InvoiceList.Add(invoice);
                viewAllInvoices.AddMenuText($"{invoice.Total}  {invoice.CustomerName}  {invoice.CustomerCountry}  {invoice.SalesAgent}");
            };
            viewAllInvoices.AddMenuText("Press 0 to go BACK.");

            Console.Write(viewAllInvoices.GetFullMenu());

            ConsoleKeyInfo userOption = Console.ReadKey();
            return userOption;

        }




        static string SelectEmployeeId()
        {
            var Employees = new List<Employee>();

            var selectEmployeeMenu = new View();

            selectEmployeeMenu.AddMenuText("Enter Employee ID:");

            Console.Write(selectEmployeeMenu.GetFullMenu());
            _selectedEmployeeId = Console.ReadLine();
            return _selectedEmployeeId;
        }

        static string UpdatedFirstName(string employeeId)
        {
            var id = int.Parse(employeeId);
            var employeeNameMenu = new View();
            var employeeQuery = new EmployeeQuery();
            var employee = employeeQuery.GetEmployeeById(id);
            employeeNameMenu.AddMenuText($"Employee Current First Name:  {employee.FirstName}");
            employeeNameMenu.AddMenuText("Enter New First Name:");

            //employeeNameMenu.AddMenuText("Press 0 to go BACK.");

            Console.Write(employeeNameMenu.GetFullMenu());
            _updatedEmployeeFirstName = Console.ReadLine();

            return _updatedEmployeeFirstName;
        }

        static string UpdatedLastName(string employeeId)
        {

            var employeeNameMenu = new View();
            var id = int.Parse(employeeId);
            var employeeQuery = new EmployeeQuery();
            var employee = employeeQuery.GetEmployeeById(id);
            employeeNameMenu.AddMenuText($"Employee's Current Last Name:  {employee.FirstName}");
            employeeNameMenu.AddMenuText("Enter New Last Name:");

            //employeeNameMenu.AddMenuText("Press 0 to go BACK.");

            Console.Write(employeeNameMenu.GetFullMenu());

            _updatedEmployeeLastName = Console.ReadLine();

            return _updatedEmployeeLastName;
        }

        static ConsoleKeyInfo UpdateEmployeeName(int employeeId, string firstName, string lastName)
        {
            var AllSalesAgents = new List<SalesAgent>();
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

