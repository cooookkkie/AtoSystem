using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.MultiHeaderGrid
{
    class DatagridviewManager
    {
        Stack<DataTable> dtStack = new Stack<DataTable>();
        private int RecordIndex = 0;

        public DatagridviewManager()
        { }

        public DatagridviewManager Push(DataTable dataTable)
        {
            ClearUnnecessaryHistory();
            this.dtStack.Push(dataTable);
            return this;
        }
        public DataTable Redo()
        {
            return dtStack.ToList()[--RecordIndex];
        }

        public DataTable Undo()
        {
            return dtStack.ToList()[++RecordIndex];
        }

        public DataTable GetCurrentState()
        {
            return dtStack.ElementAt(RecordIndex);
        }

        public bool IsUndoable()
        {
            if (RecordIndex == this.dtStack.Count - 1)
                return false;
            else
                return true;
        }

        public bool IsRedoable()
        {
            if (RecordIndex == 0)
                return false;
            else
                return true;
        }

        private void ClearUnnecessaryHistory()
        {
            while (RecordIndex > 0)
            {
                dtStack.Pop();
                RecordIndex--;
            }
            return;
        }
    }
}
