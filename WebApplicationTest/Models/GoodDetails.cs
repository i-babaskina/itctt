using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTest.Models
{
    public class GoodDetails
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Double Price { get; set; }
        public Int32 Amount { get; set; }
        public Double TotalPrice { get; set; }

        public GoodDetails(Int32 id, String name, Double price, Int32 amount)
        {
            Id = id;
            Name = name;
            Price = price;
            Amount = amount;
            TotalPrice = price * amount;
        }

    }
}