using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationTest.Models;
using WebApplicationTest.Helpers;

namespace WebApplicationTest.DataAccess
{
    public static class DAO
    {
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

        public static void AddGood(Good good)
        {
            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    context.Set<Good>().Add(good);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void UpdateGood(Good good)
        {
            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    context.Entry<Good>(good).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void DeleteGood(Int32 id)
        {
            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    Good good = GetGoodById(id);
                    context.Entry<Good>(good).State = System.Data.Entity.EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Boolean AddMovement(Movement movement)
        {
            Int32 commonCount = GetAllCount(movement.GoodId);
            if (String.Equals(movement.Type, Types.Consumption) && commonCount < movement.Amount)
               return false;
            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    context.Set<Movement>().Add(movement);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static List<Movement> GetMovementsByGoodId(Int32 goodId)
        {
            try
            {
                List<Movement> result = new List<Movement>();
                using (GoodsContext context = new GoodsContext())
                {
                    var mov = context.Set<Movement>().ToList<Movement>();
                    result = context.Set<Movement>().Where(x => x.GoodId == goodId).ToList<Movement>();
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Good GetGoodById(Int32 goodId)
        {
            try
            {
                Good good = new Good();

                using (GoodsContext context = new GoodsContext())
                {
                    good = context.Set<Good>().Find(goodId);
                }

                return good;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Good GetGoodByName(String name)
        {
            try
            {
                Good good = new Good();
                using (GoodsContext context = new GoodsContext())
                {
                    good = context.Set<Good>().Where(x => String.Equals(x.Name, name)).FirstOrDefault();
                }
                return good;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Int32 GetCountByType(Int32 goodId, Types type)
        {
            try
            {
                Int32 result = 0;

                using (GoodsContext context = new GoodsContext())
                {
                    IQueryable<Movement> movements = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, type));
                    result = movements?.Count() == 0 ? 0 : movements.Sum(s => s.Amount);
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Int32 GetAllCount(Int32 goodId)
        {
            try
            {
                Int32 result = GetCountByType(goodId, Types.Coming) - GetCountByType(goodId, Types.Consumption);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Movement> GetAllMovementsByType(Int32 goodId, Types type)
        {
            try
            {
                List<Movement> result = new List<Movement>();
                using (GoodsContext context = new GoodsContext())
                {
                    result = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, type)).ToList<Movement>();
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}