using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationTest.Models;

namespace WebApplicationTest.DataAccess
{
    public static class DAO
    {
        private const String COMING = "Coming";
        private const String CONSUMPTION = "Consumption";
        private const String MOVEMENTS = "Movements";

        public static List<Good> GetAllGoods()
        {
            List<Good> goods = new List<Good>();
            using (GoodsContext context = new GoodsContext())
            {
                goods = context.Goods.Include(MOVEMENTS).ToList<Good>();
            }
            return goods;
        }

        public static bool AddGood(Good good)
        {
            using (GoodsContext context = new GoodsContext())
            {
                context.Set<Good>().Add(good);
                context.SaveChanges();
                return true;
            }
        }

        public static void UpdateGood(Good good)
        {
            using (GoodsContext context = new GoodsContext())
            {
                context.Entry<Good>(good).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static bool DeleteGood(Int32 id)
        {
            using (GoodsContext context = new GoodsContext())
            {
                Good good = GetGoodById(id);
                if (good == null) return false;
                context.Entry<Good>(good).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
        }

        public static bool AddMovement(Movement movement)
        {
            //TODO: remove null cheking after fixing validation
            if (movement != null)
            {
                Int32 commonCount = GetAllCount(movement.GoodId);
                if (String.Equals(movement.Type, CONSUMPTION) && commonCount < movement.Amount)
                    return false;
                using (GoodsContext context = new GoodsContext())
                {
                    context.Set<Movement>().Add(movement);
                    context.SaveChanges();
                    var all = context.Set<Movement>().ToList<Movement>();
                    return true;
                }
            }
            else return false;
        }

        public static List<Movement> GetMovementsByGoodId(Int32 goodId)
        {
            List<Movement> result = new List<Movement>();
            using (GoodsContext context = new GoodsContext())
            {
                var mov = context.Set<Movement>().ToList<Movement>();
                result = context.Set<Movement>().Where(x => x.GoodId == goodId).ToList<Movement>();
            }
            return result;
        }

        public static Good GetGoodById(Int32 goodId)
        {
            Good good = new Good();
            using (GoodsContext context = new GoodsContext())
            {
                good = context.Set<Good>().Find(goodId);
            }
            return good;
        }

        public static Good GetGoodByName(String name)
        {
            Good good = new Good();
            using (GoodsContext context = new GoodsContext())
            {
                good = context.Set<Good>().Where(x => String.Equals(x.Name, name)).FirstOrDefault();
            }
            return good;
        }

        public static Int32 GetComingCount(Int32 goodId)
        {
            Int32 result = 0;
            using (GoodsContext context = new GoodsContext())
            {
                IQueryable<Movement> movements = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, COMING));
                result = movements.Count() == 0 ? 0 : movements.Sum(s => s.Amount);
            }
            return result;
        }

        public static Int32 GetConsumptionCount(Int32 goodId)
        {
            Int32 result = 0;
            using (GoodsContext context = new GoodsContext())
            {
                IQueryable<Movement> movements = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, CONSUMPTION));
                result = movements.Count() == 0 ? 0 : movements.Sum(s => s.Amount);
            }
            return result;
        }

        public static Int32 GetAllCount(Int32 goodId)
        {
            Int32 result = GetComingCount(goodId) - GetConsumptionCount(goodId);
            return result;
        }

        public static Double CalculateAllPrice(Int32 goodId)
        {
            Double result = GetAllCount(goodId) * GetGoodPrice(goodId);
            return result;
        }

        private static Double GetGoodPrice(int goodId)
        {
            Double result = 0;
            using (GoodsContext context = new GoodsContext())
            {
                result = context.Set<Good>().Find(goodId).Price;
            }
            return result;
        }

        public static List<Movement> GetAllComings(Int32 goodId)
        {
            List<Movement> result = new List<Movement>();
            using (GoodsContext context = new GoodsContext())
            {
                result = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, COMING)).ToList<Movement>();
            }
            return result;
        }

        public static List<Movement> GetAllConsumptions(Int32 goodId)
        {
            List<Movement> result = new List<Movement>();
            using (GoodsContext context = new GoodsContext())
            {
                result = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, CONSUMPTION)).ToList<Movement>();
            }
            return result;
        }
    }
}