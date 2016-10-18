using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using WebApplicationTest.Models;
using WebApplicationTest.DataAccess;

namespace WebApplicationTest.Helpers
{
    public class Converters
    {
        private const Char PAIR_SEPARATOR = '&';
        private const Char KEY_VALUE_SEPARATOR = '=';
        private const Char DATE_SEPARATOR = '-';
        private const Char TIME_SEPARATOR = ':';
        private const Char NULL_CHAR = '0';
        private const String DATETIME_FORMAT = "dd-MM-yyyy HH:mm:ss";
        private const Char DATETIME_SEPARATOR = ' ';

        static CultureInfo ci = CultureInfo.InvariantCulture;

        public static Good ConvertJqGridInputToGood(String input)
        {
            String[] inputPairs = input.Split(PAIR_SEPARATOR);
            Dictionary<String, String> goodAttributes = new Dictionary<String, String>();

            foreach (String pair in inputPairs)
            {
                String[] splitKeyValue = pair.Split(KEY_VALUE_SEPARATOR);
                goodAttributes.Add(splitKeyValue[0], splitKeyValue[1]);
            }

            Good good = new Good();

            if (goodAttributes.Keys.Contains("id"))
            {
                good.Id = Int32.Parse(goodAttributes["id"]);
            }

            good.Name = goodAttributes["Name"];
            String price = goodAttributes["Price"];
            good.Price = Double.Parse(price, ci.NumberFormat);
            return good;
        }

        public static String ConvertInputStreamToString(Stream stream)
        {
            Byte[] buffer = new Byte[stream.Length];
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            String result = System.Text.Encoding.UTF8.GetString(buffer);
            return result;
        }

        public static Movement ConvertJqGridInputToMovement(String input)
        {
            String[] inputPairs = input.Split(PAIR_SEPARATOR);
            Dictionary<String, String> mvmAttributes = new Dictionary<String, String>();

            foreach (String pair in inputPairs)
            {
                String[] splitKeyValue = pair.Split(KEY_VALUE_SEPARATOR);
                mvmAttributes.Add(splitKeyValue[0], splitKeyValue[1]);
            }

            Movement movement = new Movement();
            movement.Amount = Int32.Parse(mvmAttributes["Amount"]);
            String date = mvmAttributes["Date"].Replace("%3A", TIME_SEPARATOR.ToString()).Replace("+", " ");
            if (date.Length != 19) date = ValidateDate(date); 
            movement.Date = DateTime.ParseExact(date, DATETIME_FORMAT, ci);
            movement.Type = mvmAttributes["Type"];
            movement.GoodId = DAO.GetGoodByName(mvmAttributes["Name"]).Id;
            movement.User = HttpContext.Current.User.Identity.Name;
            return movement;
        }

        private static String ValidateDate(String date)
        {
            String[] splitDateTime = date.Split(DATETIME_SEPARATOR);
            String[] splitDate = new String[3];
            String[] splitTime = new String[3];
            splitDate = splitDateTime[0].Split(DATE_SEPARATOR);
            if (splitDateTime[0].Length != 10)
            {
                if (splitDate[0].Length != 2) splitDate[0] = String.Concat(NULL_CHAR, splitDate[0]);
                if (splitDate[1].Length != 2) splitDate[1] = String.Concat(NULL_CHAR, splitDate[1]);
            }

            splitTime = splitDateTime[1].Split(TIME_SEPARATOR);
            if (splitDateTime[1].Length != 8)
            {
                for (Int32 i = 0; i < splitTime.Length; i++)
                {
                    if (splitTime[i].Length != 2) splitTime[i] = String.Concat(NULL_CHAR, splitTime[i]);
                }
            }

            String result = String.Concat(splitDate[0], DATE_SEPARATOR, splitDate[1], DATE_SEPARATOR, splitDate[2], DATETIME_SEPARATOR , splitTime[0], TIME_SEPARATOR, splitTime[1], TIME_SEPARATOR, splitTime[2]);
            return result;
        }

        public static User ConvertLoginInputToUser(String input)
        {
            String[] inputPairs = input.Split(PAIR_SEPARATOR);
            Dictionary<String, String> usrAttributes = new Dictionary<String, String>();

            foreach (String pair in inputPairs)
            {
                String[] splitKeyValue = pair.Split(KEY_VALUE_SEPARATOR);
                usrAttributes.Add(splitKeyValue[0], splitKeyValue[1]);
            }

            User user = new User();
            user.Login = usrAttributes["Login"];
            user.Password = usrAttributes["Password"];
            return user;

        }
    }
}