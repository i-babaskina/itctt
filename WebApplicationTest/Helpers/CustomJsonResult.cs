﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationTest.Helpers
{
    public class CustomJsonResult : JsonResult
    {
        private const String DATE_FORMAT = "dd-MM-yyyy HH:mm:ss";

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                IsoDateTimeConverter isoConvert = new IsoDateTimeConverter();
                isoConvert.DateTimeFormat = DATE_FORMAT;
                response.Write(JsonConvert.SerializeObject(Data, isoConvert));
            }
        }
    }
}