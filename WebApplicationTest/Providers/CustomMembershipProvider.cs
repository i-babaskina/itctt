using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using WebApplicationTest.Models;

namespace WebApplicationTest.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override String ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override Boolean EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Boolean EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Int32 MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Int32 MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Int32 MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Int32 PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override String PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Boolean RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Boolean RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Boolean ChangePassword(String username, String oldPassword, String newPassword)
        {
            throw new NotImplementedException();
        }

        public override Boolean ChangePasswordQuestionAndAnswer(String username, String password, String newPasswordQuestion, String newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(String username, String password, String email, String passwordQuestion, String passwordAnswer, Boolean isApproved, Object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public MembershipUser CreateUser(String username, String password)
        {
            MembershipUser memberUser = GetUser(username, false);
            if (memberUser == null)
            {
                try
                {
                    using (UserContext context = new UserContext())
                    {
                        User user = new User();
                        user.Login = username;
                        user.Password = Crypto.HashPassword(password);
                        user.RoleId = 1;
                        DbSet<Role> roles = context.Roles;
                        user.Role = context.Roles.Where(x => x.Id == 1).FirstOrDefault();
                        context.Users.Add(user);
                        context.SaveChanges();
                        memberUser = GetUser(user.Login, false);
                        return memberUser;
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public override Boolean DeleteUser(String username, Boolean deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(String emailToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(String usernameToMatch, Int32 pageIndex, Int32 pageSize, out Int32 totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(Int32 pageIndex, Int32 pageSize, out Int32 totalRecords)
        {
            throw new NotImplementedException();
        }

        public override Int32 GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override String GetPassword(String username, String answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(String username, Boolean userIsOnline)
        {
            try
            {
                using (UserContext context = new UserContext())
                {
                    IQueryable<User> users = context.Set<User>().Where(x => String.Equals(x.Login, username));
                    if (users.Count() > 0)
                    {
                        User user = users.First();
                        MembershipUser memberUser = new MembershipUser("MyMembershipProvider", user.Login, null, null, null, null,
                            false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                        return memberUser;
                    }
                }
            }
            catch { return null; }
            return null;
        }

        public override MembershipUser GetUser(Object providerUserKey, Boolean userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override String GetUserNameByEmail(String email)
        {
            throw new NotImplementedException();
        }

        public override String ResetPassword(String username, String answer)
        {
            throw new NotImplementedException();
        }

        public override Boolean UnlockUser(String userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override Boolean ValidateUser(String username, String password)
        {
            Boolean isValid = false;

            using (UserContext context = new UserContext())
            {
                try
                {
                    User user = context.Set<User>().Where(x => String.Equals(x.Login, username)).FirstOrDefault<User>();

                    if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
                    {
                        isValid = true;
                    }
                }
                catch { isValid = false; }
            }

            return isValid;
        }
    }
}