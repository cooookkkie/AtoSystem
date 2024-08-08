using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Libs.MultiHeaderGrid
{
    public class MultiHeaderGrid : DataGridView
    {
        Pen blackPen = new Pen(Color.Black);
        SolidBrush back = new SolidBrush(Color.White);
        SolidBrush fore = new SolidBrush(Color.Black);

        List<HighHeader> highHeaders = new List<HighHeader>();
        /// <summary>
        /// 마지막에 사용자가 클릭한 헤더 인덱스
        /// </summary>
        int lastHeaderClickedIndex = 0;
        bool isControlPressed = false;

        bool isShiftPressed = false;
        bool isPasteRow = false;

        bool isPasteEnable = true;
        bool isCopyEnabled = true;
        SelectModifyView selectModifyView;

        // Drag & Drop 이벤트 함수에 필요한 변수
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private int columnIndexOfItemUnderMouseToDrop;

        //DatagridviewManager DataGridViewManager1 = new DatagridviewManager();

        public MultiHeaderGrid()
            : base()
        {
            /*CreateContextMenuStrip();*/
            CreateSelectModifyView();
            DoubleBuffered = true;
            //2023-05-12 CellPainting 깜빡이 문제로 추가(결과를 모름)
            Type dgvType = this.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this, true, null);

            //기본 1 Stack
            Stack<DataTable> dtStack = new Stack<DataTable>();
            dtStackDic.Add(1, dtStack);

            //문자 -> 숫자정렬
            this.SortCompare += MultiHeaderGrid_SortCompare;
        }

        public MultiHeaderGrid CopyDgv()
        {
            this.EndEdit();
            MultiHeaderGrid mainDataGridView = this;
            MultiHeaderGrid cloneDataGridView = new MultiHeaderGrid();

            if (cloneDataGridView.Columns.Count == 0)
            {
                foreach (DataGridViewColumn datagrid in mainDataGridView.Columns)
                {
                    cloneDataGridView.Columns.Add(datagrid.Clone() as DataGridViewColumn);
                }
            }

            DataGridViewRow dataRow = new DataGridViewRow();

            for (int i = 0; i < mainDataGridView.Rows.Count; i++)
            {
                dataRow = (DataGridViewRow)mainDataGridView.Rows[i].Clone();
                int Index = 0;
                foreach (DataGridViewCell cell in mainDataGridView.Rows[i].Cells)
                {
                    dataRow.Cells[Index].Value = cell.Value;
                    Index++;
                }
                cloneDataGridView.Rows.Add(dataRow);
            }
            cloneDataGridView.AllowUserToAddRows = false;
            cloneDataGridView.Refresh();


            return cloneDataGridView;
        }


        #region Redo, Undo
        Dictionary<int, Stack<DataTable>> dtStackDic = new Dictionary<int, Stack<DataTable>>();
        int RecordIndex = 0;
        bool UndoRedo = false;
        int currentStackIdx = 1;
        public void StackDictionaryInitialize(int[] keys)
        {
            dtStackDic = new Dictionary<int, Stack<DataTable>>();
            for (int i = 1; i <= keys.Length; i++)
            {
                Stack<DataTable> dtStack = new Stack<DataTable>();
                dtStackDic.Add(keys[i], dtStack);
            }
        }
        public void StackDictionaryInitialize(int keys)
        {
            if (dtStackDic.ContainsKey(keys))
                dtStackDic[keys] = new Stack<DataTable>();
            else
                dtStackDic.Add(keys, new Stack<DataTable>());
        }

        public void SetStackIdx(int idx)
        {
            currentStackIdx = idx;
            this.Push();

            if (dtStackDic.ContainsKey(currentStackIdx))
            {
                Stack<DataTable> dtStack = dtStackDic[currentStackIdx];
                RecordIndex = 0;
            }
            else
            { 
                dtStackDic.Add(idx, new Stack<DataTable>());
            }
        }

        public void Push()
        {
            if (dtStackDic.ContainsKey(currentStackIdx))
            {
                Stack<DataTable> dtStack = dtStackDic[currentStackIdx];
                dtStack.Push(GetDataTableFromDGV(this));
            }
        }
        public void DoInit(bool isDrag = true)
        {
            if (dtStackDic.ContainsKey(currentStackIdx))
            {
                Stack<DataTable> dtStack = dtStackDic[currentStackIdx];
                dtStack.Clear();
                dtStack.Push(GetDataTableFromDGV(this));
                dtStackDic[currentStackIdx] = dtStack;
                //Redo, Undo event
                //this.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.MultiHeaderGrid_CellValidated);
                this.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.MultiHeaderGrid_CellValueChanged);
            }
        }
        public void CopyEnabled(bool enabled)
        {
            isCopyEnabled = enabled;
        }
        public DataTable GetDataTableFromDGV(DataGridView dgv)
        {
            var dt = new DataTable();

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                dt.Columns.Add(column.Name);
            }

            object[] cellValues = new object[dgv.Columns.Count];

            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(cellValues);
            }
            return dt;
        }

        public void datatablaToDataGrid(DataGridView dgv, DataTable datatable)
        {
            try
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    for (int j = 0; j < datatable.Columns.Count; j++)
                    {
                        string origin_txt = string.Empty;
                        if (dgv.Rows[i].Cells[j].Value != null)
                            origin_txt = dgv.Rows[i].Cells[j].Value.ToString();

                        if (dgv.Columns[j].CellType == typeof(DataGridViewTextBoxCell) && origin_txt != datatable.Rows[i][j].ToString())
                            dgv.Rows[i].Cells[j].Value = datatable.Rows[i][j].ToString();
                    }
                }
            }
            catch
            { }
        }

        public bool isPush = true;
        public void SetUndoRedo(bool isFlag)
        {
            UndoRedo = isFlag;
        }
        public void MultiHeaderGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!isPush)
                return;

            if (UndoRedo)
                return;

            try
            {
                DataGridView dgv = (DataGridView)sender;
                int r = e.RowIndex;
                int c = e.ColumnIndex;
                if (dgv.Rows[r].Cells[c].Value != null)
                {
                    if (dtStackDic.ContainsKey(currentStackIdx))
                    {
                        Stack<DataTable> dtStack = dtStackDic[currentStackIdx];
                        string dgvResult = dgv.Rows[r].Cells[c].Value.ToString();
                        string dtResult = dtStack.ElementAt(RecordIndex).Rows[r][c].ToString();
                        if (dgvResult != dtResult)
                        {
                            while (RecordIndex > 0)
                            {
                                dtStack.Pop();
                                RecordIndex--;
                            }

                            dtStack.Push(GetDataTableFromDGV(this));
                            dtStackDic[currentStackIdx] = dtStack;
                        }
                    }
                }
            }
            catch
            { }
        }
        public void Undo()
        {
            if (dtStackDic.ContainsKey(currentStackIdx))
            {
                Stack<DataTable> dtStack = dtStackDic[currentStackIdx];
                if (dtStack.Count > RecordIndex + 1)
                {
                    UndoRedo = true;
                    datatablaToDataGrid(this, dtStack.ToList()[++RecordIndex]);
                    dtStackDic[currentStackIdx] = dtStack;
                    UndoRedo = false;
                }
            }
        }
        public void Redo()
        {
            if (0 < RecordIndex - 1)
            {
                if (dtStackDic.ContainsKey(currentStackIdx))
                {
                    Stack<DataTable> dtStack = dtStackDic[currentStackIdx];
                    UndoRedo = true;
                    datatablaToDataGrid(this, dtStack.ToList()[--RecordIndex]);
                    dtStackDic[currentStackIdx] = dtStack;
                    UndoRedo = false;
                }
            }

        }

        #endregion

        /// <summary>
        /// 문자열정렬 -> 숫자정렬
        /// </summary>
        void MultiHeaderGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            double a, b;
            if (e.CellValue1 != null && e.CellValue2 != null && double.TryParse(e.CellValue1.ToString(), out a) && double.TryParse(e.CellValue2.ToString(), out b))
            {
                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 일괄 수정 창을 생성합니다.
        /// </summary>
        private void CreateSelectModifyView()
        {
            this.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            this.selectModifyView = new SelectModifyView();
            this.selectModifyView.SelectModifyAccepted += selectModifyView_SelectModifyAccepted;
        }

        /// <summary>
        /// 일괄 수정을 적용합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void selectModifyView_SelectModifyAccepted(object sender, AcceptSelectModifyEventArgs e)
        {
            foreach (DataGridViewCell cell in this.SelectedCells)
            {
                cell.Value = e.Value;
            }
        }

        /// <summary>
        /// ContextMenuStrip을 생성합니다.
        /// </summary>
        private void CreateContextMenuStrip()
        {
            ContextMenuStrip contextMs = new ContextMenuStrip();

            this.ContextMenuStrip = contextMs;

            ToolStripMenuItem btnCopyAndDelete = new ToolStripMenuItem();
            ToolStripMenuItem btnCopy = new ToolStripMenuItem();
            ToolStripMenuItem btnPaste = new ToolStripMenuItem();
            ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
            ToolStripMenuItem btnRowInsert = new ToolStripMenuItem();
            ToolStripMenuItem btnDelete = new ToolStripMenuItem();
            ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
            ToolStripMenuItem btnSelectMod = new ToolStripMenuItem();
            ToolStripMenuItem btnRowDelete = new ToolStripMenuItem();

            contextMs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            btnCopyAndDelete,
            btnCopy,
            btnPaste,
            btnDelete,
            /*toolStripSeparator1,
            btnRowInsert,
            btnRowDelete,
            toolStripSeparator2,*/
            btnSelectMod});
            contextMs.Name = "contextMenuStrip1";
            contextMs.Size = new System.Drawing.Size(123, 120);

            btnCopyAndDelete.Name = "btnCopyAndDelete";
            btnCopyAndDelete.Size = new System.Drawing.Size(122, 22);
            btnCopyAndDelete.Text = "잘라내기";
            btnCopyAndDelete.Click += btnCopyAndDelete_Click;

            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(122, 22);
            btnCopy.Text = "복사";
            btnCopy.Click += btnCopy_Click;

            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(122, 22);
            btnPaste.Text = "붙여넣기";
            btnPaste.Click += btnPaste_Click;

            btnRowInsert.Name = "btnInsert";
            btnRowInsert.Size = new System.Drawing.Size(122, 22);
            btnRowInsert.Text = "행 삽입";
            btnRowInsert.Click += btnRowInsert_Click;


            btnRowDelete.Name = "btnRowDelete";
            btnRowDelete.Size = new System.Drawing.Size(122, 22);
            btnRowDelete.Text = "행 삭제";
            btnRowDelete.Click += btnRowDelete_Click;

            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(122, 22);
            btnDelete.Text = "삭제";
            btnDelete.Click += btnDelete_Click;

            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(119, 6);

            btnSelectMod.Name = "btnChange";
            btnSelectMod.Size = new System.Drawing.Size(122, 22);
            btnSelectMod.Text = "일괄수정";
            btnSelectMod.Click += btnSelectMod_Click;
        }

        private bool isPaste = true;
        public void setPaste(bool paste = true)
        {
            isPaste = paste;
        }


        /// <summary>
        /// 초기화합니다.
        /// </summary>
        public void Init(bool isDrag = true)
        {
            this.ColumnWidthChanged += MultiHeaderGrid_ColumnWidthChanged;
            this.RowHeightChanged += MultiHeaderGrid_RowHeightChanged;
            this.HorizontalScrollBar.Scroll += HorizontalScrollBar_Scroll;
            this.CellPainting += MultiHeaderGrid_CellPainting;
            this.ColumnHeaderMouseClick += MultiHeaderGrid_ColumnHeaderMouseClick;
            this.CellMouseClick += MultiHeaderGrid_CellMouseClick;
            this.RowHeaderMouseClick += MultiHeaderGrid_RowHeaderMouseClick;


            this.PreviewKeyDown += dataGridView_PreviewKeyDown;
            this.SelectionChanged += dataGridView_SelectionChanged;

            foreach (DataGridViewColumn col in this.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //Drag & Drop
            // 이벤트 함수 설정
            if (isDrag)
            { 
                this.MouseMove += dataGridView_MouseMove;
                this.MouseDown += dataGridView_MouseDown;
                this.DragOver += dataGridView_DragOver;
                this.DragDrop += dataGridView_DragDrop;
                this.AllowDrop = true;
            }

            base.DoubleBuffered = true;
            this.fore = new SolidBrush(this.ColumnHeadersDefaultCellStyle.ForeColor);
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewCell cell = this.CurrentCell;
            if(cell != null && cell.EditType == typeof(DataGridViewComboBoxEditingControl))
            {
                SendKeys.Send("{F4}");
            }
        }

        private void dataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRowHeader)
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    if (dragBoxFromMouseDown != Rectangle.Empty &&
                        !dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        DragDropEffects dropEffect = this.DoDragDrop(this.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                    }
                }
            }
        }

        bool isRowHeader = false;
        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            int col_idx = this.HitTest(e.X, e.Y).ColumnIndex;

            if (col_idx == -1)
            {
                isRowHeader = true;
            }
            else
            {
                isRowHeader = false;
            }

            rowIndexFromMouseDown = this.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                Size dragSize = SystemInformation.DragSize;

                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridView_DragOver(object sender, DragEventArgs e)
        {
            if (isRowHeader)
            {
                e.Effect = DragDropEffects.Move;
            }
        }
        private void dataGridView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.C)
            {
                Libs.Tools.Common common = new Tools.Common();
                common.GetDgvSelectCellsCapture(this);
            }
        }

        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (isRowHeader)
            { 
                Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
                rowIndexOfItemUnderMouseToDrop = this.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (e.Effect == DragDropEffects.Move)
                {
                    // get 한 행을 삭제하고 원하는 위치에 넣어줍니다.
                    DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                    this.Rows.RemoveAt(rowIndexFromMouseDown);
                    if (rowIndexOfItemUnderMouseToDrop < 0)
                        rowIndexOfItemUnderMouseToDrop = this.Rows.Count;
                    this.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);

                    this.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
                }
            }
        }


        /// <summary>
        /// 가로 스크롤을 하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Rectangle rtHeader = this.DisplayRectangle;
            rtHeader.Height = this.ColumnHeadersHeight;
            this.Invalidate(rtHeader);
        }
        public void AutoPaste(bool isEnable = true)
        {
            isPasteEnable = isEnable;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            this.isControlPressed = e.Control;
            this.isShiftPressed = e.Shift;

            switch (e.KeyCode)
            {
                case Keys.X: CopyAndDelete(); break;
                case Keys.Z:
                    if (isControlPressed)
                    {
                        this.EndEdit();
                        Undo();
                    }
                        
                    break;
                case Keys.V:
                    if (isPasteEnable)
                        Paste();
                    break;
                case Keys.Y:
                    if (isControlPressed)
                    {
                        this.EndEdit();
                        Redo();
                    }
                        
                    break;
                case Keys.Delete: Delete(); break;
            }

            /*if (isControlPressed && isShiftPressed)
            {
                Libs.Tools.Common common = new Tools.Common();
                common.GetDgvSelectCellsCapture(this);
                *//*switch (e.KeyCode)
                {
                    case Keys.C: common.GetDgvSelectCellsCapture(this); break;
                }*//*
            }*/
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            this.isControlPressed = e.Control;
            this.isShiftPressed = e.Shift;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            foreach (var highHeader in this.highHeaders)
            {
                int startCol = highHeader.StartCol;
                int endCol = highHeader.EndCol;

                Rectangle rect = this.GetHeaderRectangle(startCol);

                int width = 0;

                for (int i = startCol; i <= endCol; i++)
                {
                    //width += this.Columns[i].Width;
                    width += 100;
                }
                ColorConverter conv = new ColorConverter();
                Color bCol = (Color)conv.ConvertFromString(highHeader.background_color);
                back.Color = bCol;

                conv = new ColorConverter();
                Color fCol = (Color)conv.ConvertFromString(highHeader.fore_color);
                fore.Color = fCol;

                rect.Height = (rect.Height / 2);
                rect.Width = width;
                rect.Y -= 1;

                e.Graphics.FillRectangle(back, rect);
                e.Graphics.DrawRectangle(blackPen, rect);
                e.Graphics.DrawString(highHeader.Text,
                    this.ColumnHeadersDefaultCellStyle.Font,
                    fore,
                    rect,
                    format);
            }
        }

        /// <summary>
        /// Cell을 그릴때 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MultiHeaderGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                bool isContains = false;
                int width = 0;
                string bColor = "White";
                string fColor = "Black";
                foreach (var highHeader in this.highHeaders)
                {
                    width = highHeader.width;
                    bColor = highHeader.background_color;
                    fColor = highHeader.fore_color;
                    isContains = highHeader.Contains(e.ColumnIndex);
                    if (isContains == true)
                        break;
                }
                //보더 그리기
                Rectangle r = e.CellBounds;

                if (isContains)
                {
                    r.Height /= 2;
                    r.Y += r.Height;
                }
                r.X -= 1;
                r.Y -= 1;
                r.Height -= 1;
                r.Width = width;
                e.PaintBackground(r, true);
                e.PaintContent(r);

                ColorConverter conv = new ColorConverter();
                Color bCol = (Color)conv.ConvertFromString(bColor);
                back.Color = bCol;

                conv = new ColorConverter();
                Color fCol = (Color)conv.ConvertFromString(fColor);
                fore.Color = fCol;

                e.Graphics.DrawRectangle(blackPen, r);

                e.Handled = true;
            }
        }



        /// <summary>
        /// 잘라내기를 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCopyAndDelete_Click(object sender, EventArgs e)
        {
            CopyAndDelete();
        }

        /// <summary>
        /// 복사를 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }


        /// <summary>
        /// 삽입 버튼을 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRowInsert_Click(object sender, EventArgs e)
        {
            int row = 0;
            if (this.CurrentCell != null)
            {
                row = this.CurrentCell.RowIndex + 1;
            }
            this.Rows.Insert(row, 1);
        }

        /// <summary>
        /// 행 삭제 버튼을 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRowDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.SelectedRows)
            {
                this.Rows.Remove(row);
            }
        }

        /// <summary>
        /// 붙여넣기를 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        /// <summary>
        /// 삭제를 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        /// <summary>
        /// 사용자가 선택한 셀을 잘라냅니다.
        /// </summary>
        public void CopyAndDelete()
        {
            Copy();
            if (this.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in this.SelectedRows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                            cell.Value = cell.Value.ToString().Replace("\n", @"\n");
                    }
                    this.Rows.Remove(row);
                }
            }
            else
            {
                Delete();
            }
            isPasteRow = true;
        }

        /// <summary>
        /// 선택한 셀을 복사합니다.
        /// </summary>
        /// 
        //Dictionary<string, 

        

        public void Copy()
        {
            /*DataObject clipboardContent = base.GetClipboardContent();

            if (clipboardContent != null)
            {
                Clipboard.SetDataObject((object)clipboardContent);
            }*/

            //2023-11-15 복사제한
            if (!isCopyEnabled)
                return;

            //Visible이 False 인 cell은 넘어가길래 직접구현
            string clipboardTxt = "";
            if (this.SelectedRows.Count > 0)
            {
                int rowCnt = 0;
                for (int i = 0; i < this.Rows.Count; i++)
                {
                    if (this.Rows[i].Selected)
                    {
                        string rowTxt = "";
                        int dataCnt = 0;
                        for (int j = 0; j < this.Columns.Count; j++)
                        {
                            if (this.Rows[i].Cells[j].Value == null)
                                this.Rows[i].Cells[j].Value = string.Empty;

                            string cellTxt = this.Rows[i].Cells[j].Value.ToString().Replace("\n", " ").Trim();
                            if (dataCnt == 0)
                            {
                                rowTxt = cellTxt;
                                dataCnt++;
                            }
                            else
                            {
                                rowTxt += "\t" + cellTxt;
                                dataCnt++;
                            }
                        }

                        if (rowCnt == 0)
                        {
                            clipboardTxt = rowTxt;
                            rowCnt++;
                        }
                        else
                        {
                            clipboardTxt += "\r\n" + rowTxt;
                            rowCnt++;
                        }
                    }
                }
            }
            else if (this.SelectedCells.Count > 0)
            {
                int rowCnt = 0;
                int min_col = 99999, max_col = 0;

                foreach (DataGridViewCell cell in this.SelectedCells)
                {
                    if (cell.Selected)
                    {
                        if (min_col >= cell.ColumnIndex)
                            min_col = cell.ColumnIndex;
                        if (max_col <= cell.ColumnIndex)
                            max_col = cell.ColumnIndex;
                    }
                }


                for (int i = 0; i < this.Rows.Count; i++)
                {
                    bool isSelectRow = false;
                    foreach (DataGridViewCell cell in this.SelectedCells)
                    {
                        if (cell.RowIndex == i)
                        {
                            isSelectRow = true;
                            break;
                        }
                    }

                    if (isSelectRow)
                    {
                        int dataCnt = 0;
                        string rowTxt = "";

                        for (int j = min_col; j <= max_col; j++)
                        {
                            if (this.Rows[i].Cells[j].Value == null)
                                this.Rows[i].Cells[j].Value = string.Empty;

                            string cellTxt = this.Rows[i].Cells[j].Value.ToString().Replace("\n", " ").Trim();
                            if (dataCnt == 0)
                            {
                                rowTxt = cellTxt;
                                dataCnt++;
                            }
                            else
                            {
                                rowTxt += "\t" + cellTxt;
                                dataCnt++;
                            }
                        }

                        if (rowCnt == 0)
                        {
                            clipboardTxt = rowTxt;
                            rowCnt++;
                        }
                        else
                        {
                            clipboardTxt += "\r\n" + rowTxt;
                            rowCnt++;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(clipboardTxt))
                Clipboard.SetDataObject((object)clipboardTxt);
            isPasteRow = false;
        }

        /// <summary>
        /// 선택한 셀을 삭제합니다.
        /// </summary>
        public void Delete()
        {
            try
            {
                if (this.SelectedRows.Count > 0)
                {
                    int delete_cnt = this.SelectedRows.Count;
                    for (int i = this.Rows.Count - 1; i >= 0; i--)
                    {
                        if (this.Rows[i].Selected)
                        {
                            this.Rows.Remove(this.Rows[i]);
                            delete_cnt--;
                            if (delete_cnt == 0)
                                break;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewCell oneCell in this.SelectedCells)
                        oneCell.Value = string.Empty;
                }
            }
            catch { }
        }


        /// <summary>
        /// 붙여넣기 합니다.
        /// </summary>
        /// 

        public void SetIsPasteRow(bool is_add)
        {
            isPasteRow = is_add;
        }


        public void Paste()
        {
            isPush = false;
            if (this.CurrentCell != null && isPaste)
            {
                string clipText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipText) == false)
                {
                    string[] lines = clipText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    string[] texts = lines[0].Split('\t');
                    bool isLastRow = false;

                    int startRow = this.CurrentCell.RowIndex;
                    int startCol = this.CurrentCell.ColumnIndex;

                    int row = startRow;

                    	

                    //행단위 붙혀넣기
                    if (this.SelectedRows.Count > 0)
                    {
                        //행 추가하지 않고 붙혀넣기 상태
                        if (!isPasteRow)
                        {
                            for (int i = 0; i < lines.Length; i++)
                            {
                                int selectLines = 0;
                                foreach (DataGridViewRow rows in this.SelectedRows)
                                { 
                                    selectLines++;
                                }

                                //복사한 줄이랑 붙혀넣을때 잡은 줄수랑 같을때
                                if (lines.Length == selectLines)
                                {

                                    DataGridViewRow selectRow = this.Rows[startRow];
                                    row = startRow;
                                    texts = lines[i].Split('\t');

                                    int col = startCol;
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (this.RowCount <= row || this.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    this[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach(DataGridViewCell cell in selectRow.Cells)
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                    }
                                    startRow++;
                                }
                                //복사한 줄은 많은데 붙잡은 줄은 하나일 때
                                else if (lines.Length > selectLines && selectLines == 1)
                                {
                                    row = startRow;
                                    int col = startCol;

                                    texts = lines[i].Split('\t');
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (this.RowCount <= row || this.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    this[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    startRow++;
                                }
                                //나머지
                                else
                                {
                                    foreach (DataGridViewCell cell in this.SelectedCells)
                                    {
                                        row = cell.RowIndex;
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        if (texts.Length > 1)
                                        {
                                            for (int j = 0; j < texts.Length; j++)
                                            {
                                                if (this.RowCount <= row || this.ColumnCount <= col)
                                                { break; }
                                                else
                                                {
                                                    if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string txt = texts[j];
                                                        this[col, row].Value = txt.Replace(@"\n", "\n");
                                                        col++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                        }
                                    }
                                }
                            }
                        }
                        //행 추가하고 붙혀넣기 상태
                        else
                        {
                            //행추가
                            int rowIndex = 0;
                            if (this.CurrentCell != null)
                            {
                                rowIndex = this.CurrentCell.RowIndex;
                            }
                            this.Rows.Insert(rowIndex, lines.Length);
                            //추가한 행에 붙혀넣기
                            for (int i = 0; i < lines.Length; i++)
                            {
                                int selectLines = 0;
                                foreach (DataGridViewRow rows in this.SelectedRows)
                                {
                                    selectLines++;
                                }

                                //복사한 줄이랑 붙혀넣을때 잡은 줄수랑 같을때
                                if (lines.Length == selectLines)
                                {

                                    DataGridViewRow selectRow = this.Rows[startRow];
                                    row = startRow;
                                    texts = lines[i].Split('\t');

                                    int col = startCol;
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (this.RowCount <= row || this.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    this[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataGridViewCell cell in selectRow.Cells)
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                    }
                                    startRow++;
                                }
                                //복사한 줄은 많은데 붙잡은 줄은 하나일 때
                                else if (lines.Length > selectLines && selectLines == 1)
                                {
                                    row = startRow;
                                    int col = startCol;

                                    texts = lines[i].Split('\t');
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (this.RowCount <= row || this.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[j]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    this[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    startRow++;
                                }
                                //나머지
                                else
                                {
                                    foreach (DataGridViewCell cell in this.SelectedCells)
                                    {
                                        row = cell.RowIndex;
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        if (texts.Length > 1)
                                        {
                                            for (int j = 0; j < texts.Length; j++)
                                            {
                                                if (this.RowCount <= row || this.ColumnCount <= col)
                                                { break; }
                                                else
                                                {
                                                    if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string txt = texts[j];
                                                        this[col, row].Value = txt.Replace(@"\n", "\n");
                                                        col++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                        }
                                    }
                                }
                            }
                        }
                        //행 추가 후 붙혀넣기 상태 취소
                        isPasteRow = false;
                    }
                    //셀단위 붙혀넣기
                    else
                    {
                        // Multi rows, Multi columns
                        if (lines.Length > 1 && texts.Length > 1)
                        {
                            for (int i = 0; i < lines.Length; i++)
                            {
                                texts = lines[i].Split('\t');

                                int col = startCol;
                                for (int j = 0; j < texts.Length; j++)
                                {
                                    if (this.RowCount <= row || this.ColumnCount <= col)
                                        break;

                                    string txt = texts[j];
                                    this[col, row].Value = txt;
                                    col++;
                                }
                                row++;
                            }
                        }
                        // one row, Multi columns
                        else if (lines.Length == 1 && texts.Length > 1)
                        {
                            //한점잡고 붙혀넣기
                            if (this.SelectedCells.Count == 1)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    foreach (DataGridViewCell cell in this.SelectedCells)
                                    {

                                        if (cell.RowIndex == this.Rows.Count - 1 && this.AllowUserToAddRows)
                                        {
                                            this.Rows.Add();

                                            DataGridViewCell tmpCell = this.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex];
                                            row = tmpCell.RowIndex;
                                        }
                                        else
                                        {
                                            row = cell.RowIndex;
                                        }
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (this.RowCount <= row || this.ColumnCount <= col)
                                                break;

                                            string txt = texts[j];
                                            this[col, row].Value = txt;
                                            col++;
                                        }
                                    }
                                }
                            }
                            //복사, 붙혀넣기 규격이 같을때
                            else if (this.SelectedCells.Count == texts.Length)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    foreach (DataGridViewCell cell in this.SelectedCells)
                                    {

                                        if (cell.RowIndex == this.Rows.Count - 1 && this.AllowUserToAddRows)
                                        {
                                            this.Rows.Add();

                                            DataGridViewCell tmpCell = this.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex];
                                            row = tmpCell.RowIndex;
                                        }
                                        else
                                        {
                                            row = cell.RowIndex;
                                        }
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        foreach (DataGridViewCell c in this.SelectedCells)
                                        {
                                            if (col > c.ColumnIndex)
                                                col = c.ColumnIndex;
                                        }
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (this.RowCount <= row || this.ColumnCount <= col)
                                                break;

                                            string txt = texts[j];
                                            this[col, row].Value = txt;
                                            col++;
                                        }
                                    }
                                }
                            }
                        }
                        // one row, Multi columns
                        else if (lines.Length > 1 && texts.Length == 1)
                        {
                            row = this.SelectedCells[0].RowIndex;
                            foreach (DataGridViewCell cell in this.SelectedCells)
                            {
                                if (row > cell.RowIndex)
                                    row = cell.RowIndex;
                            }
                            //한점잡고 붙혀넣기
                            if (this.SelectedCells.Count == 1)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    foreach (DataGridViewCell cell in this.SelectedCells)
                                    {

                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (this.RowCount <= row || this.ColumnCount <= col)
                                                break;

                                            string txt = texts[j];
                                            this[col, row].Value = txt;
                                            row++;
                                        }
                                    }
                                }
                            }
                            //같은 규격으로 붙혀넣기
                            else if (this.SelectedCells.Count == lines.Length)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    texts = lines[i].Split('\t');

                                    int col = startCol;
                                    for (int j = 0; j < texts.Length; j++)
                                    {
                                        if (this.RowCount <= row || this.ColumnCount <= col)
                                            break;

                                        string txt = texts[j];
                                        this[col, row].Value = txt;
                                        row++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string txt = lines[0];
                            foreach (DataGridViewCell cell in this.SelectedCells)
                            {
                                if (cell.RowIndex == this.Rows.Count - 1 && this.AllowUserToAddRows)
                                {
                                    this.Rows.Add();
                                    this.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex].Value = txt;
                                }
                                else
                                {
                                    cell.Value = txt;
                                }
                                
                            }
                        }
                    }
                }
            }
            isPush = true;
            if (dtStackDic.ContainsKey(currentStackIdx))
            {
                Stack<DataTable> dtStack = dtStackDic[currentStackIdx];
                dtStack.Push(GetDataTableFromDGV(this));
                dtStackDic[currentStackIdx] = dtStack;
            }
        }


        /// <summary>
        /// 일괄수정 버튼을 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectMod_Click(object sender, EventArgs e)
        {
            this.selectModifyView.Location = MousePosition;
            this.selectModifyView.Show(this);
        }

        /// <summary>
        /// 로우헤더를 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiHeaderGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.Rows[e.RowIndex].Selected = true;
            }
        }

        /// <summary>
        /// 셀을 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MultiHeaderGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if(e.Button == MouseButtons.Left)
                    this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }
        }

        /// <summary>
        /// 컬럼헤더를 클릭하면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MultiHeaderGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*if (e.Button == MouseButtons.Left)
            {*/
            
            int col = e.ColumnIndex;
            this.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;

            if (this.isShiftPressed)
            {
                int startCol = Math.Min(col, this.lastHeaderClickedIndex);
                int endCol = Math.Max(col, this.lastHeaderClickedIndex);

                for (int i = startCol; i <= endCol; i++)
                {
                    this.Columns[i].Selected = true;

                }
            }
            else if (this.isControlPressed)
            {
                this.Columns[e.ColumnIndex].Selected = true;
            }
            else
            {
                isShiftPressed = false;
                isControlPressed = false;

                this.ClearSelection();
                this.Columns[e.ColumnIndex].Selected = true;            
            }
            /*}*/

            if (this.isShiftPressed == false)
                this.lastHeaderClickedIndex = e.ColumnIndex;
        }


        /// <summary>
        /// 컬럼의 가로가 변경되면 발생합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MultiHeaderGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Rectangle rtHeader = this.DisplayRectangle;
            rtHeader.Height = this.ColumnHeadersHeight / 2;
            this.Invalidate(rtHeader);
            try
            {
                if (this.SelectedCells.Count > 1 && this.Columns.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.SelectedCells)
                    {
                        this.Columns[cell.ColumnIndex].Width = e.Column.Width;
                    }
                }
            }
            catch
            { }
        }


        /// <summary>
        /// Header의 영역을 가져옵니다.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private Rectangle GetHeaderRectangle(int col)
        {
            int x = this.Left + this.RowHeadersWidth - this.HorizontalScrollBar.Value;

            for (int i = 0; i < col; i++)
            {
                //x += this.Columns[i].Width;
                x += 100;
            }

            System.Drawing.Size size = new Size();
            try
            {
                size = this.Columns[col].HeaderCell.Size;
            }
            catch(Exception ee)
            {
                size = new Size(100, 46);
            }

            return new Rectangle(new Point(x, 1), size);
        }

        public void DeleteHighHeader()
        {
            this.highHeaders.Clear();
        }

        public void AddHighHeader(int startCol, int endCol, string text, string bColor = "White", string fColor = "Black")
        {
            int width = 0;
            for (int i = startCol; i <= endCol; i++)
            {
                width += this.Columns[i].Width;
            }
            HighHeader hh = new HighHeader() { StartCol = startCol, EndCol = endCol, Text = text, width = width, background_color = bColor, fore_color = fColor};
            highHeaders.Add(hh);

            for (int i = startCol; i <= endCol; i++)
            {
                this.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            }
        }

        private void MultiHeaderGrid_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (this.SelectedRows.Count > 1)
            {
                foreach (DataGridViewRow row in this.SelectedRows)
                {
                    row.Height = e.Row.Height;
                }
            }
        }
    }
    /// <summary>
    /// 상위 헤더 클래스
    /// </summary>
    internal class HighHeader
    {
        int startCol;
        /// <summary>
        /// 시작 컬럼 위치
        /// </summary>
        internal int StartCol
        {
            get { return startCol; }
            set { this.startCol = value; }
        }
        int endCol;
        /// <summary>
        /// 종료 컬럼 위치
        /// </summary>
        internal int EndCol
        {
            get { return this.endCol; }
            set { this.endCol = value; }
        }
        internal int width { get; set; }
        internal string Text { get; set; }
        internal string background_color { get; set; }
        internal string fore_color { get; set; }

        internal bool Contains(int col)
        {
            return this.startCol <= col && this.endCol >= col;
        }
    }

    
}
