using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationTest.Models;
using WebApplicationTest.Helpers;
using static WebApplicationTest.Helpers.Enums;

namespace WebApplicationTest.DataAccess
{
    public static class DAO
    {
        private const String COMING = "Coming";
        private const String CONSUMPTION = "Consumption";
        private const String MOVEMENTS = "Movements";

        public static List<Good> GetAllGoods(ref Int32 pageNumber, Int32 itemsPerPage, SortedColumn sortedColumn, Boolean isDescedingSort, out Int32 goodsCount)
        {
            IEnumerable<Good> goods = null;

            using (GoodsContext context = new GoodsContext())
            {
                goods = context.Goods;//.Include(MOVEMENTS); 
                goodsCount = goods.Count();

                if (goodsCount <= (pageNumber-1) * itemsPerPage)
                {
                    pageNumber = goodsCount / itemsPerPage + 1;
                }
                
                if (sortedColumn == SortedColumn.Name)
                {
                    goods = isDescedingSort ? goods.OrderByDescending(g => g.Name) : goods.OrderBy(g => g.Name);
                }
                else if (sortedColumn == SortedColumn.Price)
                {
                    goods = isDescedingSort ? goods.OrderByDescending(g => g.Price) : goods.OrderBy(g => g.Price);
                }

                goods = goods.Skip((pageNumber - 1) * itemsPerPage);

                if (goodsCount >= itemsPerPage)
                {
                    goods = goods.Take(itemsPerPage);
                }

                return goods.ToList<Good>();
            }

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
            catch
            {
                throw;
            }
        }

        public static Boolean UpdateGood(Good good)
        {
            List<Good> goodList = GetGoodsByName(good.Name);

            if (goodList.Count > 1)
            {
                return false;
            }
            else if (goodList.Count == 1 && goodList[0].Id != good.Id)
            {
                return false;
            }
            
            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    context.Entry<Good>(good).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception exeption)
            {
                throw;
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
            catch (Exception exception)
            {
                throw;
            }
        }

        public static Boolean AddMovement(Movement movement)
        {
            Int32 commonCount = GetAllCount(movement.GoodId);

            if (String.Equals(movement.Type, CONSUMPTION, StringComparison.InvariantCultureIgnoreCase) && commonCount < movement.Amount)
            {
                return false;
            }

            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    context.Set<Movement>().Add(movement);
                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return true;
        }

        public static List<Movement> GetMovementsByGoodId(Int32 goodId, ref Int32 pageNumber, Int32 itemsPerPage, SortedColumn sortedColumn, Boolean isDescedingSort, out Int32 movementsCount)
        {
            List<Movement> movements = new List<Movement>();

            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    IEnumerable<Movement> result = null;
                    result = context.Movements.Include("Good").Where(x => x.GoodId == goodId);
                    movementsCount = result.Count();

                    if (movementsCount <= (pageNumber - 1) * itemsPerPage)
                    {
                        pageNumber = movementsCount / itemsPerPage + 1;
                    }

                    if (sortedColumn == SortedColumn.Date)
                    {
                        result = isDescedingSort ? result.OrderByDescending(r => r.Date) : result.OrderBy(r => r.Date);
                    }
                    else if (sortedColumn == SortedColumn.User)
                    {
                        result = isDescedingSort ? result.OrderByDescending(r => r.User) : result.OrderBy(r => r.User);
                    }
                    else if (sortedColumn == SortedColumn.Type)
                    {
                        result = isDescedingSort ? result.OrderByDescending(r => r.Type) : result.OrderBy(r => r.Type);
                    }
                    else if (sortedColumn == SortedColumn.Amount)
                    {
                        result = isDescedingSort ? result.OrderByDescending(r => r.Amount) : result.OrderBy(r => r.Amount);
                    }

                    result = result.Skip((pageNumber - 1) * itemsPerPage);

                    if (movementsCount >= itemsPerPage)
                    {
                        result = result.Take(itemsPerPage);
                    }

                    movements = result.ToList<Movement>();

                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return movements;
        }

        public static Good GetGoodById(Int32 goodId)
        {
            Good good = new Good();

            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    good = context.Set<Good>().Find(goodId);
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return good;
        }

        public static Good GetGoodByName(String name)
        {
            Good good = new Good();

            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    good = context.Set<Good>().Where(x => String.Equals(x.Name, name)).FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return good;
        }

        public static List<Good> GetGoodsByName(String name)
        {
            List<Good> good = new List<Good>();
            try
            { 
                using (GoodsContext context = new GoodsContext())
                {
                    good = context.Set<Good>().Where(x => String.Equals(x.Name, name)).ToList<Good>();
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return good;
        }

        public static Int32 GetCountByType(Int32 goodId, String type)
        {
            Int32 result = 0;

            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    IQueryable<Movement> movements = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, type));
                    result = movements?.Count() == 0 ? 0 : movements.Sum(s => s.Amount);
                }

            }
            catch (Exception exception)
            {
                throw;
            }
            return result;
        }

        public static Int32 GetAllCount(Int32 goodId)
        {
            try
            {
                Int32 result = GetCountByType(goodId, COMING) - GetCountByType(goodId, CONSUMPTION);
                return result;
            }
            catch (Exception exception)
            {
                throw;
            }
        }
         
        public static List<Movement> GetAllMovementsByType(Int32 goodId, String type)
        {
            List<Movement> result = new List<Movement>();

            try
            {
                using (GoodsContext context = new GoodsContext())
                {
                    result = context.Set<Movement>().Where(x => x.GoodId == goodId && String.Equals(x.Type, type)).ToList<Movement>();
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return result;
        }

    }
}