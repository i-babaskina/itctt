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
        private const char PAIR_SEPARATOR = '&';
        private const char KEY_VALUE_SEPARATOR = '=';

        static CultureInfo ci = CultureInfo.InvariantCulture;

        public static Good JqGridInputToGood(String input)
        {
            String[] inputPairs = input.Split(PAIR_SEPARATOR);
            Dictionary<string, string> goodAttributes = new Dictionary<string, string>();
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

        public static String InputStreamToString(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            String result = System.Text.Encoding.UTF8.GetString(buffer);
            return result;
        }

        public static Movement JqGridInputtoMovement(String input)
        {
            String[] inputPairs = input.Split(PAIR_SEPARATOR);
            Dictionary<string, string> mvmAttributes = new Dictionary<string, string>();
            foreach (String pair in inputPairs)
            {
                String[] splitKeyValue = pair.Split(KEY_VALUE_SEPARATOR);
                mvmAttributes.Add(splitKeyValue[0], splitKeyValue[1]);
            }
            //if (mvmAttributes.Count < 3) return null;
            Movement movement = new Movement();
            //if (mvmAttributes["Amount"] == "") return null;
            movement.Amount = Int32.Parse(mvmAttributes["Amount"]);
            movement.Date = DateTime.Now;//Convert.ToDateTime(mvmAttributes["Date"]);
            movement.Type = mvmAttributes["Type"];
            movement.GoodId = DAO.GetGoodByName(mvmAttributes["Name"]).Id;
            //movement.Good = DAO.GetGoodByName(mvmAttributes["Name"]);
            return movement;
        }

        public static User LoginInputToUser(String input)
        {
            String[] inputPairs = input.Split(PAIR_SEPARATOR);
            Dictionary<string, string> usrAttributes = new Dictionary<string, string>();
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