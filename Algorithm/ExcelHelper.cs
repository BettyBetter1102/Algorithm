
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

namespace AlgorithmDemo
{
    public class ExcelHelper
    {
        //private static Regex reg = new Regex("[\u4E00-\u9FA5]+");

        private static Regex reg = new Regex("\\s");
        public void ZLNameList()
        {



            Dictionary<string, List<string>> branchNames = new Dictionary<string, List<string>>();
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Sheets sheets;
            object oMissiong = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            if (app == null) return;
            try
            {
                string fileName = "全量品牌";
                string extension = ".xlsx";

                string sumexcelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName + extension);



                workbook = app.Workbooks.Open(sumexcelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
                    oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
                sheets = workbook.Worksheets;
                Microsoft.Office.Interop.Excel.Worksheet worksheetBrand = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);//读取第二张表  
                if (worksheetBrand == null) return;
                //品牌数据集
                branchNames = GetBranchName(worksheetBrand);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }
            finally
            {
                workbook.Close(false, oMissiong, oMissiong);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
            }




            if (branchNames.Count <= 0)
                return;


            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "分表*.xlsx");
            foreach (var item in files)
            {
                ZLNameListByFileName(item, branchNames);
            }

        }
        private void ZLNameListByFileName(string excelFilePath, Dictionary<string, List<string>> branchNames)
        {


            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("文件不存在");
                Console.ReadLine();
                return;
            }
            string fileName = Path.GetFileNameWithoutExtension(excelFilePath);
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Sheets sheets;
            object oMissiong = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;


            try
            {
                if (app == null) return;
                workbook = app.Workbooks.Open(excelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
                    oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
                sheets = workbook.Worksheets;

                //将数据读入
                Microsoft.Office.Interop.Excel.Worksheet worksheetProduct = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);//读取第一张表  
                //Microsoft.Office.Interop.Excel.Worksheet worksheetBrand = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(2);//读取第二张表  
                if (worksheetProduct == null) return;


                //产品数据集
                Dictionary<string, string> productNames = GetProducthName(worksheetProduct);

                Dictionary<string, List<string>> nameListSet = new Dictionary<string, List<string>>();
                List<string> productKeys = productNames.Keys.ToList();
                List<string> conditionKeys = new List<string>();
                foreach (KeyValuePair<string, List<string>> branchItem in branchNames)
                {
                    string brandName = branchItem.Key;
                    List<string> brandSynonymsList = branchItem.Value.ToList();
                    string newBrandName = string.Empty;
                    conditionKeys = FindConditionKeys(productKeys, brandName, brandSynonymsList, out newBrandName);


                    brandSynonymsList.Add(brandName);

                    if (!string.IsNullOrWhiteSpace(newBrandName))
                        brandName = newBrandName;
                    //只在品牌里有
                    if (conditionKeys.Count == 0)
                    {
                        //if (!nameListSet.ContainsKey(brandName))
                        //    nameListSet.Add(brandName, brandSynonymsList);
                    }
                    else
                    {
                        productKeys.RemoveAll(t => conditionKeys.Contains(t));
                        foreach (string productName in conditionKeys)
                        {
                            if (!nameListSet.ContainsKey(productName))
                            {

                                List<string> nameListItem = GetNameListItem(productName, brandName, productNames[productName], brandSynonymsList);
                                if (nameListItem.Count > 0)
                                    nameListSet.Add(productName, nameListItem);
                            }


                        }
                    }

                }
                //只在产品里有
                conditionKeys = productKeys.FindAll(t => !nameListSet.ContainsKey(t));
                foreach (string productName in conditionKeys)
                {
                    if (string.IsNullOrWhiteSpace(productName))
                        continue;
                    List<string> nameListItem = new List<string>() { productName };
                    #region
                    MatchCollection matches = reg.Matches(productName);
                    IEnumerator item = matches.GetEnumerator();
                    int count = matches.Count;
                    string noEmptyProductName = string.Empty;
                    while (item.MoveNext())
                    {
                        if (count == 1)
                        {
                            noEmptyProductName += (item.Current as Match).Value;
                        }
                        --count;

                    }
                    if (!string.IsNullOrWhiteSpace(noEmptyProductName))
                    {
                        string temp = productName.Replace(noEmptyProductName, string.Empty).Trim();
                        if (!string.IsNullOrWhiteSpace(temp))
                            noEmptyProductName = temp + noEmptyProductName;
                    }
                    else
                    {
                        noEmptyProductName = productName.Replace(" ", string.Empty);
                    }
                    #endregion
                    #region
                    //string noEmptyProductName = string.Empty;
                    //int index = reg.Match(productName).Index;
                    //if (index > 0)
                    //{
                    //    string temp = productName.Substring(0, index - 1);
                    //    noEmptyProductName = temp.Trim() + productName.Substring(index + 1).Trim();
                    //}
                    //else
                    //    noEmptyProductName = productName;
                    #endregion

                    if (!string.IsNullOrWhiteSpace(noEmptyProductName) && !nameListItem.Contains(noEmptyProductName))
                        nameListItem.Add(noEmptyProductName);

                    //if (matches.Count > 1)
                    //{
                    //    string noEmptyProductName = string.Empty;
                    //    foreach (Match item in matches)
                    //    {
                    //        if (item.NextMatch() == null)
                    //            noEmptyProductName += item.Value;
                    //    }
                    //    if (!string.IsNullOrWhiteSpace(noEmptyProductName) && !nameListItem.Contains(noEmptyProductName))
                    //        nameListItem.Add(noEmptyProductName);

                    //}
                    //else
                    //{
                    //    string noEmptyProductName = string.Empty;
                    //    foreach (Match item in matches)
                    //    {
                    //        noEmptyProductName += item.Value;
                    //    }
                    //    string temp = productName.Replace(noEmptyProductName, string.Empty).Trim();
                    //    if (!string.IsNullOrWhiteSpace(temp))
                    //        noEmptyProductName = temp + noEmptyProductName;
                    //    if (!string.IsNullOrWhiteSpace(noEmptyProductName) && !nameListItem.Contains(noEmptyProductName))
                    //        nameListItem.Add(noEmptyProductName);
                    //}

                    if (!string.IsNullOrWhiteSpace(productNames[productName]))
                        nameListItem.Add(productNames[productName]);
                    if (nameListItem.Count > 0)
                        nameListSet.Add(productName, nameListItem);
                }


                if (nameListSet.Count > 0)
                    DicToExcel(nameListSet, fileName, ".xls");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                //workbook.Close(false, excelFilePath, oMissiong);
                workbook.Close(false, oMissiong, oMissiong);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
            }
        }

        private List<string> FindConditionKeys(List<string> productKeys, string brandName, List<string> brandSynonymsList, out string newBrandName)
        {
            newBrandName = string.Empty;
            List<string> result = productKeys.FindAll(t => t.Contains(brandName));
            if (result.Count <= 0)
                foreach (var item in brandSynonymsList)
                {
                    result = productKeys.FindAll(t => t.Contains(item));
                    if (result.Count > 0)
                    {
                        newBrandName = item;
                        
                        break;
                    }

                }

            return result;
        }

        private Dictionary<string, List<string>> GetBranchName(Microsoft.Office.Interop.Excel.Worksheet worksheetBrand)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            Microsoft.Office.Interop.Excel.Range rangeBrand;
            int iBrandRowCount = worksheetBrand.UsedRange.Rows.Count;
            int iBrandColCount = worksheetBrand.UsedRange.Columns.Count;
            string brandName = string.Empty;
            string brandSynonyms = string.Empty;
            for (int iRow = 1; iRow <= iBrandRowCount; iRow++)
            {


                for (int jCol = 1; jCol <= iBrandColCount; jCol++)
                {
                    if (jCol == 1)
                    {
                        rangeBrand = (Microsoft.Office.Interop.Excel.Range)worksheetBrand.Cells[iRow, jCol];
                        brandName = (rangeBrand.Value2 == null) ? "" : rangeBrand.Text.ToString();
                        if (string.IsNullOrWhiteSpace(brandName))
                            continue;
                        if (!result.ContainsKey(brandName))
                            result.Add(brandName, new List<string>());
                    }
                    else
                    {
                        rangeBrand = (Microsoft.Office.Interop.Excel.Range)worksheetBrand.Cells[iRow, jCol];
                        brandSynonyms = (rangeBrand.Value2 == null) ? "" : rangeBrand.Text.ToString();
                        if (string.IsNullOrWhiteSpace(brandSynonyms))
                            continue;
                        if (!result[brandName].Contains(brandSynonyms))
                            result[brandName].Add(brandSynonyms);
                    }


                }
            }
            return result;
        }
        private Dictionary<string, string> GetProducthName(Microsoft.Office.Interop.Excel.Worksheet worksheetProduct)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Microsoft.Office.Interop.Excel.Range rangeBrand;
            int iProductRowCount = worksheetProduct.UsedRange.Rows.Count;
            int iProductColCount = worksheetProduct.UsedRange.Columns.Count;
            string productName = string.Empty;
            string productSynonyms = string.Empty;
            for (int iRow = 1; iRow <= iProductRowCount; iRow++)
            {


                for (int jCol = 1; jCol <= iProductColCount; jCol++)
                {
                    if (jCol == 1)
                    {
                        rangeBrand = (Microsoft.Office.Interop.Excel.Range)worksheetProduct.Cells[iRow, jCol];
                        productName = (rangeBrand.Value2 == null) ? "" : rangeBrand.Text.ToString();
                        if (string.IsNullOrWhiteSpace(productName))
                            continue;
                        if (!result.ContainsKey(productName))
                            result.Add(productName, null);
                    }
                    else
                    {
                        rangeBrand = (Microsoft.Office.Interop.Excel.Range)worksheetProduct.Cells[iRow, jCol];
                        productSynonyms = (rangeBrand.Value2 == null) ? "" : rangeBrand.Text.ToString();
                        if (string.IsNullOrWhiteSpace(productSynonyms))
                            continue;

                        result[productName] = productSynonyms;
                    }


                }
            }
            return result;
        }
        private void WhenPSynContainBSyn(List<string> brandSynonymsList, List<string> containeBSynoList, string productSynonyms, ref List<string> nameListItem)
        {
            foreach (var item in brandSynonymsList)
            {
                foreach (string containeBSyno in containeBSynoList)
                {
                    string temp3 = productSynonyms.Replace(containeBSyno, item);
                    if (!string.IsNullOrWhiteSpace(temp3))
                    {
                        if (!nameListItem.Contains(temp3))
                            nameListItem.Add(temp3);
                        string temp4 = temp3.Replace(item, string.Empty).Trim();
                        string noEmptytemp3 = item + temp4;
                        if (!nameListItem.Contains(noEmptytemp3))
                            nameListItem.Add(noEmptytemp3);
                    }
                }
            }

        }

        private List<string> GetNameListItem(string productName, string brandName, string productSynonyms, List<string> brandSynonymsList)
        {
            List<string> nameListItem = new List<string>();
            string noEmptyProductName = string.Empty;
            string temp1 = productName.Replace(brandName, string.Empty).Trim();
            if (!string.IsNullOrEmpty(temp1))
                noEmptyProductName = brandName + temp1;
            nameListItem = new List<string>() { productName, noEmptyProductName };
            if (!string.IsNullOrWhiteSpace(productSynonyms))
                nameListItem.Add(productSynonyms);

            List<string> containeBSynoList = new List<string>();

            bool isPSynContainBSyn = false;
            foreach (string brandSynonyms in brandSynonymsList)
            {
                int itemCount = nameListItem.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    if (nameListItem[i].Contains(brandName))
                    {
                        string temp2 = nameListItem[i].Replace(brandName, brandSynonyms);
                        if (!string.IsNullOrWhiteSpace(temp2) && !nameListItem.Contains(temp2))
                            nameListItem.Add(temp2);
                    }

                }
                if (productSynonyms == null)
                    continue;
                if (productSynonyms.Contains(brandSynonyms))
                {
                    isPSynContainBSyn = true;
                    if (!containeBSynoList.Contains(brandSynonyms))
                        containeBSynoList.Add(brandSynonyms);
                }
            }

            if (isPSynContainBSyn && containeBSynoList.Count > 0)
            {
                brandSynonymsList.RemoveAll(t => containeBSynoList.Contains(t));
                WhenPSynContainBSyn(brandSynonymsList, containeBSynoList, productSynonyms, ref nameListItem);
            }
            return nameListItem;
        }

        private void DicToExcel(Dictionary<string, List<string>> nameListSet, string fileName, string extension)
        {
            string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "整理" + "_" + fileName + "_" + "代码" + extension);
            if (File.Exists(savePath))
                File.Delete(savePath);
            //没有数据的话就不往下执行  
            if (nameListSet.Count == 0)
                return;
            //实例化一个Excel.Application对象  
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {


                //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写  
                excel.Visible = true;

                //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错  
                Microsoft.Office.Interop.Excel.Workbook wookBook = excel.Application.Workbooks.Add(true);

                //把当前页的数据保存在Excel中  
                List<string> keys = nameListSet.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    string brandName = keys[i];
                    System.Windows.Forms.Application.DoEvents();
                    for (int j = 0; j < nameListSet[brandName].Count; j++)
                    {
                        excel.Cells[i + 1, j + 1] = nameListSet[brandName][j].ToString();
                    }
                }

                //设置禁止弹出保存和覆盖的询问提示框  
                excel.DisplayAlerts = false;
                excel.AlertBeforeOverwriting = false;

                //保存工作簿
                wookBook.SaveAs(savePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, null, null, null, null, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);






            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误提示");
            }
            finally
            {
                //确保Excel进程关闭  
                excel.Quit();
                excel = null;
                GC.Collect();//如果不使用这条语句会导致excel进程无法正常退出，使用后正常退出
                             // MessageBox.Show(this, "文件已经成功导出！", "信息提示");
            }
        }

        public void ToExcel(DataTable dataTable)
        {
            try
            {
                //没有数据的话就不往下执行  
                if (dataTable.Rows.Count == 0)
                    return;
                //实例化一个Excel.Application对象  
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写  
                excel.Visible = true;

                //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错  
                excel.Application.Workbooks.Add(true);
                //生成Excel中列头名称  
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {

                    excel.Cells[1, i + 1] = dataTable.Columns[i].Caption;


                }
                //把DataGridView当前页的数据保存在Excel中  
                for (int i = 0; i < dataTable.Rows.Count - 1; i++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {

                        if (dataTable.Columns[i].DataType == typeof(string))
                        {
                            excel.Cells[i + 2, j + 1] = "'" + dataTable.Rows[j][i].ToString();
                        }
                        else
                        {
                            excel.Cells[i + 2, j + 1] = dataTable.Rows[j][i].ToString();
                        }


                    }
                }

                //设置禁止弹出保存和覆盖的询问提示框  
                excel.DisplayAlerts = false;
                excel.AlertBeforeOverwriting = false;

                //保存工作簿  
                excel.Application.Workbooks.Add(true).Save();
                //保存excel文件  
                excel.Save("D:" + "\\KKHMD.xls");

                //确保Excel进程关闭  
                excel.Quit();
                excel = null;
                GC.Collect();//如果不使用这条语句会导致excel进程无法正常退出，使用后正常退出
                             // MessageBox.Show(this, "文件已经成功导出！", "信息提示");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误提示");
            }

        }

        /// <summary>  
        /// 读取Excel文件到DataSet中  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns></returns>  
        private DataSet ToDataTable(string filePath, string fileName, string tblName)
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(fileType)) return null;

            if (fileType == ".xls")
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            string sql_F = "Select * FROM [{0}]";

            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataTable dtSheetName = null;

            DataSet ds = new DataSet();
            try
            {
                // 初始化连接，并打开  
                conn = new OleDbConnection(connStr);
                conn.Open();

                // 获取数据源的表定义元数据                         
                string SheetName = "";
                dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                // 初始化适配器  
                da = new OleDbDataAdapter();
                for (int i = 0; i < dtSheetName.Rows.Count; i++)
                {
                    SheetName = (string)dtSheetName.Rows[i]["TABLE_NAME"];

                    if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
                    {
                        continue;
                    }

                    da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
                    DataSet dsItem = new DataSet();
                    da.Fill(dsItem, tblName);

                    ds.Tables.Add(dsItem.Tables[0].Copy());
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // 关闭连接  
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }
            return ds;
        }


        private System.Data.DataTable ToDataTable()
        {
            System.Data.DataTable result = new System.Data.DataTable();

            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "快消top名单.xlsx");
            try
            {

                string strCon = "provider=microsoft.jet.oledb.4.0;data source=" + strPath + ";extended properties=excel 8.0";//关键是红色区域
                OleDbConnection Con = new OleDbConnection(strCon);//建立连接
                string strSql = "select * from [产品清单$]";//表名的写法也应注意不同，对应的excel表为sheet1，在这里要在其后加美元符号$，并用中括号
                OleDbCommand Cmd = new OleDbCommand(strSql, Con);//建立要执行的命令
                OleDbDataAdapter da = new OleDbDataAdapter(Cmd);//建立数据适配器
                DataSet ds = new DataSet();//新建数据集
                da.Fill(ds, "shyman");//把数据适配器中的数据读到数据集中的一个表中（此处表名为shyman，可以任取表名）
                                      //指定datagridview1的数据源为数据集ds的第一张表（也就是shyman表），也可以写ds.Table["shyman"]

                result = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//捕捉异常
            }

            return result;
        }

        private DataTable GetDataFromExcelByCom(bool hasTitle = false)
        {
            //OpenFileDialog openFile = new OpenFileDialog();
            //openFile.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
            //openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //openFile.Multiselect = false;
            //if (openFile.ShowDialog() == DialogResult.Cancel) return null;
            //var excelFilePath = openFile.FileName;
            string excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "快消top名单.xlsx");
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Sheets sheets;
            object oMissiong = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            DataTable dt = new DataTable();

            try
            {
                if (app == null) return null;
                workbook = app.Workbooks.Open(excelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
                    oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
                sheets = workbook.Worksheets;

                //将数据读入到DataTable中
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);//读取第一张表  
                if (worksheet == null) return null;

                int iRowCount = worksheet.UsedRange.Rows.Count;
                int iColCount = worksheet.UsedRange.Columns.Count;
                //生成列头
                for (int i = 0; i < iColCount; i++)
                {
                    var name = "column" + i;
                    if (hasTitle)
                    {
                        var txt = ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Text.ToString();
                        if (!string.IsNullOrWhiteSpace(txt)) name = txt;
                    }
                    while (dt.Columns.Contains(name)) name = name + "_1";//重复行名称会报错。
                    dt.Columns.Add(new DataColumn(name, typeof(string)));
                }
                //生成行数据
                Microsoft.Office.Interop.Excel.Range range;
                int rowIdx = hasTitle ? 2 : 1;
                for (int iRow = rowIdx; iRow <= iRowCount; iRow++)
                {
                    DataRow dr = dt.NewRow();
                    for (int iCol = 1; iCol <= iColCount; iCol++)
                    {
                        range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[iRow, iCol];
                        dr[iCol - 1] = (range.Value2 == null) ? "" : range.Text.ToString();
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch { return null; }
            finally
            {
                workbook.Close(false, oMissiong, oMissiong);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
            }
        }

    }
}
