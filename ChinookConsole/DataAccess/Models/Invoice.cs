namespace ChinookConsole.DataAccess.Models
{
    class Invoice
    {
        public int InvoiceId {get; set;}
        public double Total  {get; set;}
        public string CustomerName {get; set;}
        public string CustomerCountry {get; set;}
        public string SalesAgent {get; set;}
    }
}
