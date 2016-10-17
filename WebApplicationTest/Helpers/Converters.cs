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
            String date = mvmAttributes["Date"].Replace("%3A", ":").Replace("+", " ");
            if (date.Length != 19) date += "0"; //TODO: Looks like big crutch, yeah: wheh time has 40 seconds there are only 4 in input string (maybe for every time where seconds is multiple 10)
            movement.Date = DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss", ci);
            movement.Type = mvmAttributes["Type"];
            movement.GoodId = DAO.GetGoodByName(mvmAttributes["Name"]).Id;
            movement.User = HttpContext.Current.User.Identity.Name;
            return movement;
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