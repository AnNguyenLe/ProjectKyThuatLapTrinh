using System;
using KTLT_QuanLyCuaHang.Entities;
using KTLT_QuanLyCuaHang.Constants;
using KTLT_QuanLyCuaHang.DataAccess_DAL;

namespace KTLT_QuanLyCuaHang.BusinessLogic_BLL
{
	public class Register_BLL
	{
		
		public static string RegisterNewUser(User user)
		{
			bool failCondition = string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.name) || string.IsNullOrEmpty(user.password);

			if (failCondition)
			{
				return ProcessStatusConstants.REJECTED;
			}

			User[]? userList = Shared_DAL.GetAllUsers();

			if(userList == null)
			{
                Register_DAL.AddNewUser(user);
                return ProcessStatusConstants.APPROVED;
            }

			foreach(User existingUser in userList)
			{
				if(existingUser.email == user.email)
				{
					return ProcessStatusConstants.REJECTED;
				}
			}
			
			Register_DAL.AddNewUser(user);
			return ProcessStatusConstants.APPROVED;
        }
	}
}

