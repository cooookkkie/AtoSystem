using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Task
{
    public interface ITaskRepository
    {
        DataTable GetTask(string division, string group_name, string form_name);
        DataTable GetTaskDetail(int id);
        StringBuilder InsertGroup(taskModel model);
        StringBuilder DeleteTaskDetail(taskModel model);
        StringBuilder DeleteTask(int id);
    }
}
