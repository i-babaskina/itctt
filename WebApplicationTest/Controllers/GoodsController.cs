using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationTest.DataAccess;
using WebApplicationTest.Models;
using Newtonsoft.Json;
using WebApplicationTest.Helpers;

namespace WebApplicationTest.Controllers
{
    public class GoodsController : Controller
    {
        private GoodsContext db = new GoodsContext();

        // GET: Goods
        public ActionResult Index()
        {
            return View();
        }

        public String GoodsList()
        {
            String result = String.Empty;
            List<Good> goods = DAO.GetAllGoods();
            result = JsonConvert.SerializeObject(goods, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return result;
        }

        //TODO: Remove thos methods!!
        //Random r = new Random();
        //public void AddGoodsToList()
        //{
        //    for (Int32 i = 5; i < 30; i++)
        //    {
        //        Good g = new Good();
        //        g.Id = i;
        //        g.Name = String.Concat("Good", i);
        //        g.Price = r.Next(5, 120) + r.NextDouble();
        //        DAO.AddGood(g);
        //    }
        //}

        //public void AddMovements()
        //{
            //var list1 = DAO.GetAllGoods();
            ////for (Int32 i = 0; i < 20; i++)
            ////{
            ////    Movement movement = new Movement();
            ////    movement.Date = DateTime.Now;
            ////    movement.Amount = r.Next(5, 10);
            ////    movement.User = "testuser";
            ////    movement.Type = "Coming";
            ////    movement.GoodId = r.Next(1, 4);
            ////    movement.Good = DAO.GetGoodById(movement.GoodId);
            ////    DAO.AddMovement(movement);
            //}

            //using (GoodsContext context = new GoodsContext())
            //{
            //    var list = context.Set<Movement>().ToList<Movement>();
            //}

            //for (Int32 i = 0; i < 10; i++)
            //{
            //    Movement movement = new Movement();
            //    movement.Date = DateTime.Now;
            //    movement.Amount = r.Next(5, 10);
            //    movement.User = "testuser";
            //    movement.Type = "Consumption";
            //    movement.GoodId = r.Next(1, 4);
            //    movement.Good = DAO.GetGoodById(movement.GoodId);
            //    DAO.AddMovement(movement);
            //}

            //using (GoodsContext context = new GoodsContext())
            //{
            //    var list = context.Set<Movement>().ToList<Movement>();
            //}
        //}

        public ActionResult AddGood()
        {
            String good = Converters.InputStreamToString(Request.InputStream);
            Good g = new Good();
            g = Converters.JqGridInputToGood(good);
            Boolean success = DAO.AddGood(g);
            if (success)
            {
                return Json(new { success = true, responseText = "Good successfulu added" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { success = false, responseText = "Something wrong" }, JsonRequestBehavior.AllowGet);
        }

        public void DeleteGood()
        {
            String id = Converters.InputStreamToString(Request.InputStream);
            Boolean result = DAO.DeleteGood(Int32.Parse(id));
        }

        public String GetAllMovementsForGood(String goodId)
        {
            Int32 Id = Int32.Parse(goodId);
            String result = String.Empty;
            using (GoodsContext context = new GoodsContext())
            {
                List<Movement> goods = DAO.GetMovementsByGoodId(Id);
                result = JsonConvert.SerializeObject(goods);
            }
            return result;
        }

        public void Update()
        {
            String updateInfo = Converters.InputStreamToString(Request.InputStream);
            Good good = Converters.JqGridInputToGood(updateInfo);
            DAO.UpdateGood(good);
        }

        public ActionResult AddMovement()
        {
            String updateInfo = Converters.InputStreamToString(Request.InputStream);
            Movement movement = Converters.JqGridInputtoMovement(updateInfo);
            Boolean isAdded = DAO.AddMovement(movement);
            return Json(new { success = isAdded, responseText = (isAdded ? "Added successful" : "The are not enought good's amount") }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        public ActionResult GetGoodDetails(String inputId)
        {
            String updateInfo = Converters.InputStreamToString(Request.InputStream);
            Int32 goodId = Int32.Parse(updateInfo);
            Good good = DAO.GetGoodById(goodId);
            if (good != null)
            {
                return Json(new { Id = good.Id, Name = good.Name, Price = good.Price, Amount = DAO.GetAllCount(goodId), TotalPrice = DAO.CalculateAllPrice(goodId) }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { success = false, responseText = "Something wrong" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGoodMovement(String inputId)
        {
            Int32 goodId = Int32.Parse(inputId);
            List<Movement> result = DAO.GetMovementsByGoodId(goodId);
            return Json(result);
        }
        
    }
}
