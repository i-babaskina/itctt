using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationTest.Models
{
    public class Movement
    {
        public Int32 Id { get; set; }
        public Int32 GoodId { get; set; }
        public String Type { get; set; }
        public Int32 Amount { get; set; }
        public DateTime Date { get; set; }
        public String User { get; set; }
        
        public virtual Good Good { get; set; }
    }
}