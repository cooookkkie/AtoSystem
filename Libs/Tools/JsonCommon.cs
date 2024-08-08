using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;

namespace Libs.Tools
{
    public class JsonCommon
    {
        private string rootPath;

        public JsonCommon(string rootPath)
        {
            this.rootPath = rootPath;
            // 폴더가 존재하지 않으면 생성
            CreateFolder(rootPath);
        }


        public bool CreateJson(string savePath)
        {
            bool isCreate = false;

            if (!File.Exists(rootPath + @"\" + savePath))
            {
                try
                {
                    using (File.Create(rootPath + @"\" + savePath))
                    {
                        isCreate = true;
                        Console.WriteLine($"Json 파일 생성완료 [{rootPath + @"\" + savePath}]");
                    }
                }
                catch (Exception e)
                {
                    isCreate = false;
                    Console.WriteLine($"Json 파일 생성실패 [{rootPath + @"\" + savePath}]\n * Error : {e.Message}");
                }
            }
            else
            {
                isCreate = true;
                Console.WriteLine($"Json 파일 존재 [{rootPath + @"\" + savePath}]");
            }
            return isCreate;
        }



        public bool InputJson(string savePath, Dictionary<string, DataTable> shtData, List<string> sheetOrderList)
        {
            bool isSuccess = false;

            if (CreateFolder(savePath))
            {
                DeleteJsonFile(savePath);
                try
                {
                    string jsonData;
                    foreach (string key in shtData.Keys) 
                    {
                        DataTable shtDataDt = shtData[key];
                        if (shtDataDt != null && shtDataDt.Rows.Count > 0)
                        {
                            var data = ConvertToJson(key, shtDataDt);
                            // JSON으로 직렬화하여 파일에 쓰기
                            jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                            File.WriteAllText(rootPath + @"\" + savePath + @"\" + key + ".json", jsonData);

                            isSuccess = true;
                            Console.WriteLine($"Json 저장 완료 [{rootPath + @"\" + savePath}]");
                        }
                    }


                    var shtOrderDt = ConvertToJsonForSheetOrder(sheetOrderList);
                    // JSON으로 직렬화하여 파일에 쓰기
                    jsonData = JsonConvert.SerializeObject(shtOrderDt, Formatting.Indented);
                    File.WriteAllText(rootPath + @"\" + savePath + @"\" + "Order.json", jsonData);


                }
                catch (Exception e)
                {
                    isSuccess = true;
                    Console.WriteLine($"Json 저장 실패 [{savePath}]\n * Error : {e.Message}");
                }
            }

            return isSuccess;
        }



        private bool CreateFolder(string folderPath)
        {
            bool isCreate = false;
            // 폴더가 존재하지 않으면 생성
            if (!Directory.Exists(rootPath + @"\" + folderPath))
            {
                try 
                {
                    Directory.CreateDirectory(rootPath + @"\" + folderPath);
                    Console.WriteLine("폴더가 생성되었습니다.");
                    isCreate = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("폴더가 생성중 에러가 발생하였습니다.");
                    isCreate = false;
                }
                
            }
            else
            {
                Console.WriteLine("폴더가 이미 존재합니다.");
                isCreate= true;
            }
            return isCreate;
        }




        public void InputJson(string folderPath, Dictionary<string, dynamic> shtDataDic)
        {
            if (CreateFolder(folderPath))
            {
                foreach (string key in shtDataDic.Keys)
                {
                    dynamic shtData = shtDataDic[key];
                    if (shtData != null)
                    {
                        DataTable shtDataDt = JsonConvertToDatatable(shtData);
                        if (shtDataDt != null && shtDataDt.Rows.Count > 0)
                        {
                            string shtName = shtDataDt.Rows[0]["shtName"].ToString();
                            //Json 파일 저장
                            string fileName = key + ".json";
                            string savePath = rootPath + @"\" + folderPath + @"\" + fileName;

                            try
                            {
                                // JSON으로 직렬화하여 파일에 쓰기
                                string jsonData = JsonConvert.SerializeObject(shtData, Formatting.Indented);
                                File.WriteAllText(savePath, jsonData);

                                Console.WriteLine($"Json 저장 완료 [{savePath}]");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Json 저장 실패 [{savePath}]\n * Error : {e.Message}");
                            }
                        }
                    }
                }
            }
        }


        public void DeleteJsonFile(string folderPath)
        {
            if (Directory.Exists(rootPath + @"\" + folderPath))
            {
                // 폴더 내의 모든 파일을 가져옴
                string[] files = Directory.GetFiles(rootPath + @"\" + folderPath);

                // 각 파일을 순회하며 삭제
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"파일 {file}이(가) 삭제되었습니다.");
                }

                Console.WriteLine("모든 파일이 삭제되었습니다.");
            }
            else
                Console.WriteLine("폴더가 존재하지 않습니다.");
        }


        public Dictionary<string, DataTable> GetSheetData(string folderPath, out Dictionary<string, int> sheetOrderDic, out DateTime lastModified)
        {
            Dictionary<string, DataTable> sheetDataDic = new Dictionary<string, DataTable>();
            sheetOrderDic = new Dictionary<string, int>();
            lastModified = new DateTime(1900, 1, 1);
            if (Directory.Exists(rootPath + @"\" + folderPath))
            {
                string[] jsonFiles = Directory.GetFiles(rootPath + @"\" + folderPath, "*.json");

                foreach (string jsonFile in jsonFiles)
                {
                    //파일 최종수정시간
                    FileInfo fileInfo = new FileInfo(jsonFile);
                    if(lastModified < fileInfo.LastWriteTime)
                        lastModified = fileInfo.LastWriteTime;

                    Console.WriteLine($"파일: {jsonFile}");
                    //시트정보
                    string sheetName = Path.GetFileNameWithoutExtension(jsonFile);

                    // 파일 내용 읽기
                    string jsonContent = File.ReadAllText(jsonFile);

                    // JSON 파싱하여 객체로 변환
                    dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
                    DataTable shtDataDt = JsonConvertToDatatable(jsonData);
                    //시트내용
                    if (!Path.GetFileName(jsonFile).Equals("Order.json"))
                    {
                        sheetDataDic.Add(sheetName, shtDataDt);
                    }
                    //시트순서
                    else
                    {
                        Console.WriteLine($"파일: {jsonFile}");

                        try
                        {
                            for (int i = 0; i < shtDataDt.Rows.Count; i++)
                                sheetOrderDic.Add(shtDataDt.Rows[i]["shtName"].ToString(), Convert.ToInt32(shtDataDt.Rows[i]["orderCount"].ToString()));
                        }
                        catch
                        {
                            sheetOrderDic = null;
                        }
                        
                    }
                }
            }
            else
                sheetDataDic = null;
            return sheetDataDic;
        }

        public DataTable JsonConvertToDatatable(dynamic jsonData)
        {
            DataTable dataTable = new DataTable();
            // 첫 번째 행의 키들을 열로 추가
            foreach (var key in jsonData[0].ToObject<IDictionary<string, object>>().Keys)
            {
                dataTable.Columns.Add(key);
            }

            // JSON 데이터를 DataTable에 추가
            foreach (var item in jsonData)
            {
                DataRow row = dataTable.NewRow();
                foreach (DataColumn column in dataTable.Columns)
                {
                    row[column.ColumnName] = item[column.ColumnName]?.ToString() ?? ""; // null 값 처리
                }
                dataTable.Rows.Add(row);
            }

            // DataTable 출력
            foreach (DataColumn column in dataTable.Columns)
            {
                Console.Write(column.ColumnName + "\t");
            }
            Console.WriteLine();

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write(item + "\t");
                }
                Console.WriteLine();
            }

            return dataTable;
        }

        private List<Dictionary<string, object>> ConvertToJson(string shtName, DataTable dgv)
        {
            // DataGridView의 데이터를 리스트로 추출
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            //foreach (DataGridViewRow row in dgv.Rows)
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataRow row = dgv.Rows[i];

                var rowData = new Dictionary<string, object>();
                rowData["shtName"] = dgv.Rows[i]["shtName"].ToString();
                rowData["row"] = i;

                foreach (DataColumn column in dgv.Columns)
                {
                    if (!column.ColumnName.Equals("shtName") && !column.ColumnName.Equals("row"))
                    {
                        //입력값
                        rowData[column.ColumnName] = row[column.ColumnName].ToString();
                    }
                }
                data.Add(rowData);
            }
            return data;
        }


        private List<Dictionary<string, object>> ConvertToJsonForSheetOrder(List<string> shtData)
        {
            // DataGridView의 데이터를 리스트로 추출
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            //foreach (DataGridViewRow row in dgv.Rows)
            int shtCnt = 1;
            foreach(string key in shtData) 
            {
                var rowData = new Dictionary<string, object>();
                rowData["shtName"] = key;
                rowData["orderCount"] = shtCnt++;

                data.Add(rowData);
            }
            return data;
        }


        public DataTable ConvertToDatatable(string shtText, DataGridView dgv)
        {
            // DataTable 생성
            DataTable dataTable = new DataTable();
            if (dgv != null)
            {
                
                dataTable.Columns.Add("shtName", typeof(string));
                dataTable.Columns.Add("row", typeof(Int32));

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    dataTable.Columns.Add(column.Name);
                    dataTable.Columns.Add(column.Name + "_style");
                }


                foreach (DataGridViewRow row in dgv.Rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["shtName"] = shtText;
                    dataRow["row"] = row.Index;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dataRow[cell.OwningColumn.Name] = cell.Value;

                        //폰트
                        float font_size = 9;
                        string font_name = "나눔고딕";
                        bool font_bold = false;
                        bool font_italic = false;
                        Font fff = cell.Style.Font;
                        if (fff == null)
                            fff = dgv.Font;
                        if (fff != null)
                        {
                            font_size = fff.Size;
                            font_name = fff.Name;
                            font_bold = fff.Bold;
                            font_italic = fff.Italic;
                        }
                        //폰트색
                        Color fore_col = cell.Style.ForeColor;
                        if (fore_col.Name == "0")
                            fore_col = Color.Black;
                        int fore_col_rgb = fore_col.ToArgb();
                        //배경색
                        Color back_col = cell.Style.BackColor;
                        if (back_col.Name == "0")
                            back_col = Color.White;
                        int back_col_rgb = back_col.ToArgb();
                        //셀 스타일
                        string cell_style = font_size.ToString() + "_" + font_name + "_" + font_bold.ToString() + "_" + font_italic.ToString() + "_#" + fore_col_rgb + "_#" + back_col_rgb;
                        dataRow[cell.OwningColumn.Name + "_style"] = cell_style;

                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }


    }
}
