using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookConsole.DataAccess.Models
{
    class Invoice
    {
        public double Total  {get; set;}
        public string CustomerName {get; set;}
        public string CustomerCountry {get; set;}
        public string SalesAgent {get; set;}
    }
}
