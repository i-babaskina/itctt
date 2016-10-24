using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationTest.Models;
using static WebApplicationTest.Helpers.Enums;

namespace WebApplicationTest.Helpers
{
    public class Results
    {
        public const String SMTH_WRONG = "Something wrong. Watch loggs for details.";

        public static JsonResult ErrorResult()
        {
            return new JsonResult
            {
                Data =  new { success = false, responseText = SMTH_WRONG }, 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult JqGridJsonResult(Object data, Int32 pageNumber, Int32 itemsPerPage, SortedColumn sortedColumn, Boolean isSortedDesc, Int32 totalPages, Int32 totalRows)
        {
            return new CustomJsonResult
            {
                Data = new
                {
                    page = pageNumber,
                    records = totalRows,
                    total = totalPages,
                    sortcolumn = sortedColumn.ToString(),
                    sortorder = isSortedDesc? "desc" : "asc",
                    userdata = data
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static List<GoodDetails> GetGoodsWithDetailsResults(List<Good> goods)
        {
            List<GoodDetails> result = new List<GoodDetails>();
            foreach (Good good in goods)
            {
                try
                {
                    GoodDetails goodDetails = new GoodDetails(good.Id, good.Name, good.Price, DataAccess.DAO.GetAllCount(good.Id));
                    result.Add(goodDetails);
                }
                catch
                {
                    throw;
                }
            }
            return result;
        }
    }
}