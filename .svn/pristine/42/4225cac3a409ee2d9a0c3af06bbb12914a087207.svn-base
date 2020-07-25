using System;
using System.Web.Security;

namespace Dal.Interfaces
{
    public interface IMembershipService
    {
        Int32 MinPasswordLength { get; }

        Boolean ValidateUser(String userName, String password);
        MembershipCreateStatus CreateUser(String userName, String password, String email);
        String GetUserNameByEmail(String email);
        Boolean IsUserLockedOut(String userName);
        Boolean ChangeUserPassword(String email, String password);
        Boolean DeleteUser(String email);    
    }
}