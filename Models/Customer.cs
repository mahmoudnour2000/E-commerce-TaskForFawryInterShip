using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Customer
    {
    public string Name { get; set; }
    public decimal Balance { get; set; }

    public Customer(string name, decimal balance)
    {
        Name = name;
        Balance = balance;
    }
    }
}
