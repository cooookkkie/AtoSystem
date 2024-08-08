using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUsersRepository
    {
        int GetDailyTarget(string user_name);
        DataTable GetTragetData(int data_type, string department, string user_name);
        DataTable GetUsersVacation(int year, string user_id);
        DataTable GetUsers(int year, string workplace, string department, string name, string sortType, string user_id = "", bool isRetire = false);
        int UpdateTargetSalesAmount(string user_id, double sales_amount);
        int UpdateCurrentDate(string user_id);
        DataTable GetUsers(string workplace, string department, string team, string name, string status, string grade);
        List<UsersModel> GetUsers(IDbTransaction transaction = null);
        List<UsersModel> GetUsersList(string department, string user_name, string grade, string tel, IDbTransaction transaction = null);
        UsersModel GetByUser(string username, string password, IDbTransaction transaction = null);

        UsersModel GetUserInfo(string user_id, IDbTransaction transaction = null);
        UsersModel GetUserInfo2(string username, IDbTransaction transaction = null);
        int InsertUser(UsersModel model);
        int UpdateRemark(string user_id, string remark);
        int UpdateUser(UsersModel model);
        int UpdateMyInfo(UsersModel model);
        int UpdateMyInfo2(UsersModel model);
        int DeleteUser(UsersModel model);
        DataTable GetTeamMember(string auth_level);
        DataTable GetOneData(string col, string department, string team, string grade, string user_name);
    }
}
