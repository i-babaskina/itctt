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
            String result = String.Empty;
            try
            {
                List<Good> goods = DAO.GetAllGoods();
                result = JsonConvert.SerializeObject(goods, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.SMTH_WRONG;
            }
            return result;
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
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }
            return Json(new { success = true, responseText = "Good successfulu added." }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteGood()
        {
            try
            {
                String id = Converters.ConvertInputStreamToString(Request.InputStream);
                DAO.DeleteGood(Int32.Parse(id));
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
            Boolean isUpdate = false;
            try
            {
                String updateInfo = Converters.ConvertInputStreamToString(Request.InputStream);
                Good good = Converters.ConvertJqGridInputToGood(updateInfo);
                isUpdate = DAO.UpdateGood(good);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }

            String message = (isUpdate) ? "Good successfuly added." : "Good with this name alredy exist.";
            return Json(new { success = isUpdate, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddMovement()
        {
            String updateInfo = Converters.ConvertInputStreamToString(Request.InputStream);
            Movement movement = Converters.ConvertJqGridInputToMovement(updateInfo);
            Boolean isAdded = false;

            try
            {
                isAdded = DAO.AddMovement(movement);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }

            return Json(new { success = isAdded, responseText = (isAdded ? "Added successful." : "The are not enought good's amount.") }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetGoodDetails()
        {
            String updateInfo = Converters.ConvertInputStreamToString(Request.InputStream);
            Good good = new Good();

            try
            {
                Int32 goodId = Int32.Parse(updateInfo);
                good = DAO.GetGoodById(goodId);

                if (good != null)
                {
                    Int32 amount = DAO.GetAllCount(goodId);
                    Double price = good.Price * amount;
                    return Json(new { success = true, Id = good.Id, Name = good.Name, Price = good.Price, Amount = amount, TotalPrice = price }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Can't find good." }, JsonRequestBehavior.AllowGet);
                }
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
            List<Movement> result = new List<Movement>(); 

            try
            {
                result = DAO.GetMovementsByGoodId(goodId);
            }
            catch(Exception e)
            {
                logger.Error(e.ToString());
                return Results.ErrorResult();
            }

            return Json(result);
        }
        
    }
}
