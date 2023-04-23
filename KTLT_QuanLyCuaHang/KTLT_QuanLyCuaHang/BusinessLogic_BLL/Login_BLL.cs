using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.DataAccess_DAL;
using KTLT_QuanLyCuaHang.Constants;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
    public class Login_BLL
    {
        public static User? LoginUser(User loginUser)
        {
            User[]? userList = Shared_DAL.GetAllUsers();

            if(userList == null)
            {
                return null;
            }

            int userListLength = userList.Length;

            for(int i = 0; i < userListLength; i++)
            {
                User user = userList[i];
                if(loginUser.email == user.email)
                {
                    if(loginUser.password == user.password)
                    {
                        return user;
                    }
                    return null;
                }
            }

            return null;
        }
    }
}
