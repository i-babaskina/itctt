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
using static WebApplicationTest.Helpers.Enums;

namespace WebApplicationTest.Controllers
{
    [Authorize]
    public class GoodsController : Controller
    {
        private const String DESCENDING = "desc";

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GoodsList(Boolean _search, String nd, String rows, String page, String sidx, String sord)
        {
            Boolean isDescedingSort = sord == DESCENDING;
            Int32 pageNumber, itemsPerPage, goodsCount;
            SortedColumn sortedColumn;

            if (!Int32.TryParse(page, out pageNumber) || !Int32.TryParse(rows, out itemsPerPage) || !Enum.TryParse<SortedColumn>(sidx, out sortedColumn))
            {
                return Json(new { success = false, message = "inalid input data." }); //TODO: Change to some Responce error code
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            JsonResult result = new JsonResult();

            try
            {
                List<Good> goods = DAO.GetAllGoods(ref pageNumber, itemsPerPage, sortedColumn, isDescedingSort, out goodsCount);

                Int32 numberOfPages = goodsCount / itemsPerPage + 1;
                result = Results.JqGridJsonResult(Results.GetGoodsWithDetailsResults(goods), pageNumber, itemsPerPage, sortedColumn, isDescedingSort, numberOfPages, goodsCount);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                result = Results.ErrorResult();
            }
            return result;
        }

        
        public ActionResult GoodsList(Boolean _search, String nd, String rows, String page, String sord)
        {
            return GoodsList(_search, nd, rows, page, null, sord);   
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

        [HttpGet]
        public JsonResult GetAllMovementsForGood(String goodId, Boolean _search, String nd, String rows, String page, String sidx, String sord)
        {
            Int32 id;
            Boolean isDescedingSort = sord == DESCENDING;
            Int32 pageNumber, itemsPerPage, movementsCount;
            SortedColumn sortedColumn;

            if (!Int32.TryParse(goodId, out id) || !Int32.TryParse(page, out pageNumber) || !Int32.TryParse(rows, out itemsPerPage) || !Enum.TryParse<SortedColumn>(sidx, out sortedColumn))
            {
                return Json(new { success = false, message = "inalid input data." }); //TODO: Change to some Responce error code
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            JsonResult result = new JsonResult() ;

            try
            {
                List<Movement> movements = DAO.GetMovementsByGoodId(id, ref pageNumber, itemsPerPage, sortedColumn, isDescedingSort, out movementsCount).Select(c => { c.Good = null; return c; }).ToList<Movement>();
                Int32 numberOfPages = movementsCount / itemsPerPage + 1;
                result = Results.JqGridJsonResult(movements, pageNumber, itemsPerPage, sortedColumn, isDescedingSort, numberOfPages, movementsCount);
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                result = Results.ErrorResult();
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
                
    }
}
