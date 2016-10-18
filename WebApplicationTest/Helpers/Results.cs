using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}