using System;
using System.Web.Security;
using Dal.Interfaces;

namespace Dal.Services
{
    public class MembershipService : IMembershipService
    {
        public Int32 MinPasswordLength
        {
            get { return 6; }
        }
        public Boolean ValidateUser(String userName, String password)
        {
            return Membership.ValidateUser(userName, password);
        }
        public MembershipCreateStatus CreateUser(String userName, String password, String email)
        {
            MembershipCreateStatus createStatus;
            Membership.CreateUser(userName, password, email, null, null, true, out createStatus);
            return createStatus;
        }
        public String GetUserNameByEmail(String email)
        {
            return Membership.GetUserNameByEmail(email);
            //return WebMatrix.WebData.WebSecurity.get
        }
        public Boolean IsUserLockedOut(String email)
        {
            var membershipuser = Membership.GetUser(email);
            if (membershipuser == null) return true;
            return membershipuser.IsLockedOut;
        }
        public Boolean ChangeUserPassword(String email, String password)
        {
            if (!String.IsNullOrEmpty(password) && password.Length >= MinPasswordLength)
            {
                var membershipUser = Membership.GetUser(email);
                return membershipUser.ChangePassword(membershipUser.ResetPassword(), password);
            }
            return false;
        }
        public Boolean DeleteUser(String email)
        {
            return Membership.DeleteUser(email, true);
        }
    }
}