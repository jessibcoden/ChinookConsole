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
        static string _selectedAgent;

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

                    case '1': //Provide a query that shows the invoices associated with each sales agent. The resultant table should include the Sales Agent's full name.
                        var searchInvoicesMenu = SelectInvoiceQueryType();
                        _selectedAgent = SelectSalesAgent();
                        var invoicesBySelectedAgent = ShowInvoicesByAgent();
                        break;

                    case '2': //Provide a query that shows the Invoice Total, Customer name, Country and Sale Agent name for all invoices.
                        break;

                    case '3'://Looking at the InvoiceLine table, provide a query that COUNTs the number of line items for an Invoice with a parameterized Id from user input (use ExecuteScalar)
                        break;

                    case '4': //INSERT a new invoice with parameters for customerid and billing address
                        break;

                    case '5': //UPDATE an Employee's name with a parameter for Employee Id and new name
                        break;

                    default:
                        break;
                }
            }
        }

        static ConsoleKeyInfo MainMenu()
        {
            View mainMenu = new View();

            mainMenu.AddMenuOption("Search Invoices")
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
            return agentQuery.GetAllSalesReps().Where<SalesAgent>(selectedSalesAgent.AgentId);
            //return selectedSalesAgent.AgentId;
        }

        // static string ShowInvoicesByAgent()
        //{
        //    var AllSalesAgents = new List<SalesAgent>();
        //    var invoiceQuery = new InvoiceQuery();
        //    var invoices = invoiceQuery.GetInvoiceBySalesAgent(_selectedAgent.Agent);

        //    var invoicesByAgentView = new View();

        //    invoicesByAgentView.AddMenuText("Select Sales Agent");
        //        foreach (var agent in agents)
        //        {
        //            AllSalesAgents.Add(agent);
        //            salesAgentMenu.AddMenuOption($"{agent.AgentName}");
        //        };
        //    salesAgentMenu.AddMenuText("Press 0 to go BACK.");

        //    Console.Write(salesAgentMenu.GetFullMenu());

        //    int agentSelected = int.Parse(Console.ReadKey().KeyChar.ToString());
        //    var selectedSalesAgent = AllSalesAgents[agentSelected - 1];
        //    return selectedSalesAgent.AgentId;
        //}


    }
}

