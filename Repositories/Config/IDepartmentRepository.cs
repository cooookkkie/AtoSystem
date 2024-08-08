using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public interface IDepartmentRepository
    {
        DataTable GetDepartment(string name, string auth);
        DataTable GetDepartment(string name);
        StringBuilder InsertDepartment(DepartmentModel model);
        StringBuilder DeleteDepartment(DepartmentModel model);
        StringBuilder UpdateDepartment(DepartmentModel model);
    }
}
