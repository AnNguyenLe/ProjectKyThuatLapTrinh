using KTLT_QuanLyCuaHang.Entities;

namespace KTLT_QuanLyCuaHang.DataAccess_DAL
{
    public class Shared_DAL
    {
        public static User[]? GetAllUsers()
        {
            StreamReader file = new StreamReader(Constants_DAL.userListFilePath);

            int noOfUsers = int.Parse(file.ReadLine());

            if (noOfUsers <= 0)
            {
                file.Close();
                return null;
            }

            User[] userList = new User[noOfUsers];

            for(int i = 0; i < noOfUsers; i++)
            {
                string lineContent = file.ReadLine();
                string[] lineContentElements = lineContent.Split(',');

                User user;

                user.email = lineContentElements[0];
                user.name = lineContentElements[1];
                user.password = lineContentElements[2];

                userList[i] = user;
            }

            file.Close();

            return userList;
        }

        public static void SaveUserList(User[] userList)
        {
            StreamWriter file = new StreamWriter(Constants_DAL.userListFilePath);

            int noOfUsers = userList.Length;

            file.WriteLine(noOfUsers);

            for(int i = 0; i < noOfUsers; i++)
            {
                User user = userList[i];

                file.Write($"{user.email},");
                file.Write($"{user.name},");
                file.WriteLine(user.password);
            }

            file.Close();
        }
    }
}
