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
using System.Web.Security;

namespace WebApplicationTest.Controllers
{
    [Authorize]
    public class GoodsController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }

        public String GoodsList()
        {
            try
            {
                String result = String.Empty;
                List<Good> goods = DAO.GetAllGoods();
                result = JsonConvert.SerializeObject(goods, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return result;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.SMTH_WRONG;
            }
        }

        public ActionResult AddGood()
        {
            String goodAttributes = Converters.ConvertInputStreamToString(Request.InputStream);
            Good good = new Good();
            good = Converters.ConvertJqGridInputToGood(goodAttributes);

            if (DAO.GetGoodByName(good.Name) != null)
            {
                return Json(new { success = false, responseText = "Good with this name already exist." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                DAO.AddGood(good);
                return Json(new { success = true, responseText = "Good successfulu added." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }

        }

        public ActionResult DeleteGood()
        {
            try
            {
                String id = Converters.ConvertInputStreamToString(Request.InputStream);
                DAO.DeleteGood(Int32.Parse(id));
                return Json(new { success = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }
        }

        public String GetAllMovementsForGood(String goodId)
        {
            Int32 Id = Int32.Parse(goodId);
            String result = String.Empty;

            try
            {
                List<Movement> goods = DAO.GetMovementsByGoodId(Id);
                result = JsonConvert.SerializeObject(goods);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                result = Results.SMTH_WRONG;
            } 

            return result;
        }

        public ActionResult Update()
        {
            try
            {
                String updateInfo = Converters.ConvertInputStreamToString(Request.InputStream);
                Good good = Converters.ConvertJqGridInputToGood(updateInfo);
                Boolean isUpdate = DAO.UpdateGood(good);
                String message = (isUpdate) ? "Good successfuly added." : "Good with this name alredy exist.";
                return Json( new { success = isUpdate, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }
        }

        public ActionResult AddMovement()
        {
            String updateInfo = Converters.ConvertInputStreamToString(Request.InputStream);
            Movement movement = Converters.ConvertJqGridInputToMovement(updateInfo);

            try
            {
                Boolean isAdded = DAO.AddMovement(movement);
                return Json(new { success = isAdded, responseText = (isAdded ? "Added successful." : "The are not enought good's amount.") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }
        }
        
        public ActionResult GetGoodDetails()
        {
            String updateInfo = Converters.ConvertInputStreamToString(Request.InputStream);

            try
            {
                Int32 goodId = Int32.Parse(updateInfo);
                Good good = new Good();
                good = DAO.GetGoodById(goodId);

                if (good != null)
                {
                    Int32 amount = DAO.GetAllCount(goodId);
                    Double price = good.Price * amount;
                    return Json(new { Id = good.Id, Name = good.Name, Price = good.Price, Amount = amount, TotalPrice = price }, JsonRequestBehavior.AllowGet);
                }

                else return Json(new { success = false, message = "Can't find good." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }
        }

        public ActionResult GetGoodMovement(String inputId)
        {
            Int32 goodId = Int32.Parse(inputId);

            try
            {
                List<Movement> result = DAO.GetMovementsByGoodId(goodId);
                return Json(result);
            }
            catch(Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }
        }
        
    }
}
