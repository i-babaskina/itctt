using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace WebApplicationTest.Models
{
    public class Good
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Double Price { get; set; }

        [JsonIgnore]
        public virtual ICollection<Movement> Movements { get; set; }
    }
}