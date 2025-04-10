using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TicketFunction
{
    internal class Purchase
    {
      public int ConcertId { get; set; }
      public string Email { get; set; } = string.Empty;
      public string FirstName { get; set; } = string.Empty;
      public string LastName { get; set; } = string.Empty;
      public string Phone { get; set; } = string.Empty;
      public int Quantity { get; set; }
      public string CreditCard { get; set; } = string.Empty;
      public string Expiration { get; set; } = string.Empty;
      public string SecurityCode { get; set; } = string.Empty;
      public string Address { get; set; } = string.Empty;
      public string City { get; set; } = string.Empty;
      public string Province { get; set; } = string.Empty;
      public string PostalCode { get; set; } = string.Empty;
      public string Country { get; set; } = string.Empty;
    }
}
