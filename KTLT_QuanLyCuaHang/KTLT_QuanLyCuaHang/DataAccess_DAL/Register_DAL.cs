using KTLT_QuanLyCuaHang.Entities;

namespace KTLT_QuanLyCuaHang.DataAccess_DAL
{
    public class Register_DAL
    {

        public static void AddNewUser(User newUser)
        {
            User[]? userList = Shared_DAL.GetAllUsers();

            if(userList == null )
            {
                Shared_DAL.SaveUserList(new User[1] { newUser });
                return;
            }

            int currentNoOfUsers = userList.Length;
            User[] newUserList = new User[currentNoOfUsers + 1];

            for(int i = 0; i < currentNoOfUsers; i++)
            {
                newUserList[i] = userList[i];
            }

            newUserList[currentNoOfUsers] = newUser;
            Shared_DAL.SaveUserList(newUserList);
            return;
        }
    }
}
