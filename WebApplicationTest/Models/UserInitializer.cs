using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WebApplicationTest.Models
{
    public class UserInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            List<Role> rolesList = new List<Role>()
            {
                new Role() { Id = 1, Name = "user"}
            };

            rolesList.ForEach(x => context.Roles.Add(x));
            context.SaveChanges();

            List<User> goodList = new List<User>()
            {
                new User() { Id = 1, Login = "testuser", Password = Crypto.HashPassword("123456"), RoleId = 1 },
                new User() { Id = 2, Login = "user22", Password = Crypto.HashPassword("123456"), RoleId = 1 }
            };

            goodList.ForEach(x => context.Users.Add(x));
            context.SaveChanges();
        }
    }
}