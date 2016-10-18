using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationTest.Models;

namespace WebApplicationTest.DataAccess
{
    public class GoodsInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<GoodsContext>
    {
        //protected override void Seed(GoodsContext context)
        //{
        //    List<Good> goodList = new List<Good>()
        //    {
        //        new Good() { Id = 1, Name = "Good1", Price = 100 },
        //        new Good() { Id = 2, Name = "Good2", Price = 10.5 },
        //        new Good() { Id = 3, Name = "Good3", Price = 20 },
        //        new Good() { Id = 4, Name = "Good4", Price = 15 }
        //    };

        //    goodList.ForEach(x => context.Goods.Add(x));
        //    context.SaveChanges();

        //    List<Movement> movementsList = new List<Movement>()
        //    {
        //        new Movement() { Id = 1, Type = "Coming", Amount = 3, GoodId = 1, User = "testuser", Date = DateTime.Now },
        //        new Movement() { Id = 1, Type = "Consumption", Amount = 1, GoodId = 1, User = "testuser", Date = DateTime.Now }
        //    };

        //    movementsList.ForEach(x => context.Movements.Add(x));
        //    context.SaveChanges();
        //}
    }
}