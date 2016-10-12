using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplicationTest.Models;

namespace WebApplicationTest.DataAccess
{
    public class GoodsContext : DbContext
    {
        public DbSet<Good> Goods { get; set; }
        public DbSet<Movement> Movements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Good>()
                .HasMany(s => s.Movements)
                .WithRequired(e => e.Good)
                .HasForeignKey(e => e.GoodId);            
        }
    }
}