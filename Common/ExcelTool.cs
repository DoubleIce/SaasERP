using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System; 
using System.Reflection; // 引用这个才能使用Missing字段  

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Reflection;
using System.Data.OleDb;

namespace ERP.Common
{
    /// <summary>
    /// C#与Excel交互类
    /// </summary>
    public class ExcelHelper
    {
        static int x = 0;
        public static void test()
        {
            x++;
        }

        #region 导出到Excel
        #region ExportExcelForDataTable
        /// <summary>
        /// 从DataTable导出Excel,指定列别名,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 例:tp.xlsx 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTable(System.Data.DataTable dt, string excelPathName, string pathType, List<string> colName, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0) return false;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    return false;
                }
                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;

                Microsoft.Office.Interop.Excel.Workbook workbook = null;
                if (TemplatePath == "")
                {
                    workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                }
                else
                {
                    workbook = workbooks.Add(TemplatePath); //加载模板
                }
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Range range;

                long totalCount = dt.Rows.Count;
                if (exDataTableList != null && exDataTableList.Count > 0)
                {
                    foreach (System.Data.DataTable item in exDataTableList)
                    {
                        totalCount += item.Rows.Count;
                    }
                }
                long rowRead = 0;
                float percent = 0;
                string exclStr = "";//要排除的列临时项
                object exclType;//DataTable 列的类型,用于做
                int colPosition = 0;//列位置
                if (sheetName != null && sheetName != "")
                {
                    worksheet.Name = sheetName;
                }
                #region 列别名判定
                if (TemplatePath == "")
                {
                    if (colName != null && colName.Count > 0)
                    {
                        #region 指定了列别名
                        for (int i = 0; i < colName.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = colName[i];
                            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                            range.Interior.ColorIndex = 15;
                            range.Font.Bold = true;
                            exclType = dt.Columns[i].DataType.Name;
                            if (exclType.ToString() != "DateTime")
                            {
                                //range.EntireColumn.AutoFit();//全局自动调整列宽,不能再使用单独设置
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.AutoFit();
                            }
                            else
                            {
                                //规定列宽
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                            }
                            //((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 未指定别名
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                            range.Interior.ColorIndex = 15;
                            range.Font.Bold = true;
                            exclType = dt.Columns[i].DataType.Name;
                            if (exclType.ToString() != "DateTime")
                            {
                                //range.EntireColumn.AutoFit();//全局自动调整列宽,不能再使用单独设置
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.AutoFit();
                            }
                            else
                            {
                                //规定列宽
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                            }
                            //((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                        }
                        #endregion
                    }
                }
                else
                {
                    //用了模版，不加载标题
                }
                #endregion
                #region 显示/排除列判定
                if (excludeColumn != null && excludeColumn.Count > 0)
                {
                    switch (excludeType)
                    {
                        case "0":
                            {
                                #region 0为显示所有列
                                #region 常规项
                                int r = 0;
                                for (r = 0; r < dt.Rows.Count; r++)
                                {
                                    colPosition = 0;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        if (TemplatePath == "")
                                        {
                                            worksheet.Cells[r + 2, colPosition + 1] = dt.Rows[r][i].ToString();
                                        }
                                        else
                                        {
                                            worksheet.Cells[r + TemplateRow, colPosition + 1] = dt.Rows[r][i].ToString();
                                        }
                                        colPosition++;
                                    }
                                    rowRead++;
                                    percent = ((float)(100 * rowRead)) / totalCount;
                                }
                                #endregion
                                #region 扩展项
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //生成扩展 DataTable 每行数据
                                            for (int t = 0; t < item.Columns.Count; t++)
                                            {
                                                if (TemplatePath == "")
                                                {
                                                    worksheet.Cells[r + 2, colPosition + 1] = item.Rows[k][t].ToString();
                                                }
                                                else
                                                {
                                                    worksheet.Cells[r + TemplateRow, colPosition + 1] = item.Rows[k][t].ToString();
                                                }
                                                colPosition++;
                                            }
                                            rowRead++;
                                            percent = ((float)(100 * rowRead)) / totalCount;
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }; break;
                        case "1":
                            {
                                #region 1指定的为要显示的列
                                #region 常规项
                                int r = 0;
                                for (r = 0; r < dt.Rows.Count; r++)
                                {
                                    colPosition = 0;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        exclStr = dt.Columns[i].ColumnName;
                                        if (excludeColumn.Contains(exclStr))
                                        {
                                            if (TemplatePath == "")
                                            {
                                                worksheet.Cells[r + 2, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            else
                                            {
                                                worksheet.Cells[r + TemplateRow, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            colPosition++;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    rowRead++;
                                    percent = ((float)(100 * rowRead)) / totalCount;
                                }
                                #endregion
                                #region 扩展项
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //生成扩展 DataTable 每行数据
                                            for (int t = 0; t < item.Columns.Count; t++)
                                            {
                                                exclStr = dt.Columns[t].ColumnName;
                                                if (excludeColumn.Contains(exclStr))
                                                {
                                                    if (TemplatePath == "")
                                                    {
                                                        worksheet.Cells[r + 2, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[r + TemplateRow, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    colPosition++;
                                                }
                                                else
                                                {

                                                }
                                            }
                                            rowRead++;
                                            percent = ((float)(100 * rowRead)) / totalCount;
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }; break;
                        case "2":
                            {
                                #region 2指定的为要排除的列
                                #region 常规项
                                int r = 0;
                                for (r = 0; r < dt.Rows.Count; r++)
                                {
                                    colPosition = 0;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        exclStr = dt.Columns[i].ColumnName;
                                        if (excludeColumn.Contains(exclStr))
                                        {

                                        }
                                        else
                                        {
                                            if (TemplatePath == "")
                                            {
                                                worksheet.Cells[r + 2, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            else
                                            {
                                                worksheet.Cells[r + TemplateRow, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            colPosition++;
                                        }
                                    }
                                    rowRead++;
                                    percent = ((float)(100 * rowRead)) / totalCount;
                                }
                                #endregion
                                #region 扩展项
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //生成扩展 DataTable 每行数据
                                            for (int t = 0; t < item.Columns.Count; t++)
                                            {
                                                exclStr = dt.Columns[t].ColumnName;
                                                if (excludeColumn.Contains(exclStr))
                                                {

                                                }
                                                else
                                                {
                                                    if (TemplatePath == "")
                                                    {
                                                        worksheet.Cells[r + 2, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[r + TemplateRow, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    colPosition++;
                                                }
                                            }
                                            rowRead++;
                                            percent = ((float)(100 * rowRead)) / totalCount;
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }; break;
                        default:
                            break;
                    }

                }
                else
                {
                    //生成每行数据
                    int r = 0;
                    for (r = 0; r < dt.Rows.Count; r++)
                    {
                        //生成每列数据
                        if (TemplatePath == "")
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                worksheet.Cells[r + 1 + TemplateRow, i + 1] = dt.Rows[r][i].ToString();
                            }
                        }
                        rowRead++;
                        percent = ((float)(100 * rowRead)) / totalCount;
                    }
                }
                #endregion
                switch (pathType)
                {
                    case "0": { workbook.Saved = false; }; break;
                    case "1": { workbook.Saved = true; workbook.SaveCopyAs(excelPathName); }; break;
                    default:
                        return false;
                }
                xlApp.Visible = false;//是否在服务器打开
                workbook.Close(true, Type.Missing, Type.Missing);
                workbook = null;
                xlApp.Quit();
                xlApp = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 从DataTable导出Excel,指定列别名
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableC(System.Data.DataTable dt, string excelPathName, string pathType, List<string> colName, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableE(System.Data.DataTable dt, string excelPathName, string pathType, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        /// <summary>
        /// 从DataTable导出Excel，使用默认列名，不排除导出任何列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableZ(System.Data.DataTable dt, string excelPathName, string pathType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        #endregion

        #region ExportExcelForModelList
        /// <summary>
        /// 从DataTable导出Excel,指定列别名,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <<param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelList<T>(List<T> md, string excelPathName, string pathType, List<string> colName, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            System.Data.DataTable dt = ModelListToDataTable(md);
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel,指定列别名
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListC<T>(List<T> md, string excelPathName, string pathType, List<string> colName, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListE<T>(List<T> md, string excelPathName, string pathType, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel，使用默认列名，不排除导出任何列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListZ<T>(List<T> md, string excelPathName, string pathType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        #endregion

        #region 从DataTable导出Excel； ToExcelModel实体传参
        /// <summary>
        /// 从DataTable导出Excel； ToExcelModel实体传参
        /// </summary>
        /// <param name="tem">ExcelHelper.ToExcelModel</param>
        /// <returns></returns>
        public static bool ToExcelForDataTable(ToExcelModel tem)
        {
            if (tem != null)
            {
                return ToExcelForDataTable(tem.DataTable, tem.excelPathName, tem.pathType, tem.colNameList, tem.excludeColumn, tem.excludeType, tem.sheetName, tem.TemplatePath, tem.TemplateRow, tem.exDataTableList);
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Model To DataTable
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static System.Data.DataTable ModelListToDataTable<T>(List<T> modelList)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (modelList == null) return dtReturn;

            foreach (T rec in modelList)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        #endregion

        #region 说明 如何使用
        /*
         * 功能：
         *      1、将System.Data.DataTable数据导出到Excel文件
         *      2、将Model(Entity)数据实体导出到Excel文件
         * 完整调用：
         *      1、ExcelHelper.ToExcelForDataTable(DataTable,excelPathName,pathType,colName,excludeColumn,excludeType,sheetName,TemplatePath,TemplateRow,exDataTableList);
         *      2、ExcelHelper.ToExcelForModelList(Model,excelPathName,pathType,colName,excludeColumn,excludeType,sheetName,TemplatePath,TemplateRow,exDataTableList);
         * 参数说明：
         *      1、DataTable：DataSet.DataTable[0];数据表
         *      2、Model：Model.Users users = new Model.Users(){...};数据实体
         *      3、excelPathName：含Excel名称的保存路径 在pathType＝1时有效。用户自定义保存路径时请赋值空字符串 ""。格式："E://456.xlsx"
         *      4、pathType：路径类型。只能取值：0用户自定义路径，弹出用户选择路径对话框；1服务端定义路径。标识文件保存路径是服务端指定还是客户自定义路径及文件名，与excelPathName参数合用
         *      5、colName：各列的列别名List string，比如：字段名为userName，此处可指定为"用户名"，并以此显示
         *      6、excludeColumn：要显示/排除的列，指定这些列用于显示，或指定这些列用于不显示。倒低这些列是显示还是不显示，由excludeType参数决定
         *      7、excludeType：显示/排除列方式。 0为显示所有列 1指定的是要显示的列 2指定的是要排除的列，与excludeColumn合用
         *      8、sheetName：sheet1的名称，要使期保持默认名称请指定为空字符串 ""
         *      9、TemplatePath：模版在项目服务器中路径 例:tp.xlsx 。当为空字符串 "" 时表示无模版
         *      10、TemplateRow：模版中已存在数据的行数，与TemplatePath合用，无模版时请传入参数 0
         *      11、exDataTableList：扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同
         * 注意：
         *      1、exDataTableList参数为一个List<System.Data.DataTable> 集合，当数据为 Model 时，可先调用 ExcelHelper.ModelListToDataTable(System.Data.DataTable dt)将Model转为System.Data.DataTable
         */
        #endregion
        #endregion
        #region 从Excel导入数据到 Ms Sql
        /// <summary>
        /// 从Excel导入数据到 Ms Sql
        /// </summary>
        /// <param name="excelFile">Excel文件路径(含文件名)</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="DbTableName">存储到数据库中的数据库表名称</param>
        /// <param name="columnType">对应表格的数据类型，如果为null，则为默认类型：double,nvarchar(500),datetime</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static bool FromExcel(string excelFile, string sheetName, string DbTableName, List<string> columnType, string connectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                //获取全部数据   
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelFile + ";" + "Extended Properties=Excel 8.0;";
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + excelFile + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连
                #region 知识扩展
                //HDR=Yes，代表第一行是标题，不做为数据使用。HDR=NO，则表示第一行不是标题，做为数据来使用。系统默认的是YES
                //IMEX=0 只读模式
                //IMEX=1 写入模式
                //IMEX=2 可读写模式
                #endregion
                #region 命名执行
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    string strExcel = "";
                    OleDbDataAdapter myCommand = null;
                    strExcel = string.Format("select * from [{0}$]", sheetName);
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, sheetName);

                    #region 数据库表是否存在的 T-SQL 检测语句准备
                    //如果目标表不存在则创建   
                    string strSql = string.Format("if object_id('{0}') is null create table {0}(", DbTableName != "" ? DbTableName : sheetName);
                    if (columnType != null && columnType.Count > 0)
                    {
                        #region 手动指定定每个字段的数据类型
                        //指定数据格式,要求一一对应
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            System.Data.DataColumn c = ds.Tables[0].Columns[i];
                            strSql += string.Format("[{0}] {1},", c.ColumnName, columnType[i]);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 使用默认数据类型
                        foreach (System.Data.DataColumn c in ds.Tables[0].Columns)
                        {
                            //使用默认格式：只有double,DateTime,String三种类型
                            switch (c.DataType.ToString())
                            {
                                case "DateTime":
                                    {
                                        strSql += string.Format("[{0}] DateTime,", c.ColumnName);
                                    }; break;
                                case "Double":
                                    {
                                        strSql += string.Format("[{0}] double,", c.ColumnName);
                                    }; break;
                                default:
                                    strSql += string.Format("[{0}] nvarchar(500),", c.ColumnName);
                                    break;
                            }
                        }
                        #endregion
                    }
                    strSql = strSql.Trim(',') + ")";
                    #endregion
                    #region 执行 T-SQL 如果数据库表不存在则新建表，如果存在则不新建
                    using (System.Data.SqlClient.SqlConnection sqlconn = new System.Data.SqlClient.SqlConnection(connectionString))
                    {
                        sqlconn.Open();
                        System.Data.SqlClient.SqlCommand command = sqlconn.CreateCommand();
                        command.CommandText = strSql;
                        command.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                    #endregion
                    #region 向数据库表插入数据
                    using (System.Data.SqlClient.SqlBulkCopy sbc = new System.Data.SqlClient.SqlBulkCopy(connectionString))
                    {
                        sbc.SqlRowsCopied += new System.Data.SqlClient.SqlRowsCopiedEventHandler(bcp_SqlRowsCopied);
                        sbc.BatchSize = 100;//每次传输的行数   
                        sbc.NotifyAfter = 100;//进度提示的行数   
                        sbc.DestinationTableName = DbTableName != "" ? DbTableName : sheetName;//数据库表名表名
                        sbc.WriteToServer(ds.Tables[0]);
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #region 进度显示
        /// <summary>
        /// 进度显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void bcp_SqlRowsCopied(object sender, System.Data.SqlClient.SqlRowsCopiedEventArgs e)
        {
            e.RowsCopied.ToString();
        }
        #endregion
        #endregion
    }
    public class ToExcelModel
    {
        #region ToExcelModel自动属性
        /// <summary>
        /// 数据表
        /// </summary>
        public System.Data.DataTable DataTable { get; set; }
        /// <summary>
        /// 含Excel名称的保存路径 在pathType＝1时有效。用户自定义保存路径时请赋值空字符串 ""。格式："E://456.xlsx"
        /// </summary>
        public string excelPathName { get; set; }
        /// <summary>
        /// 路径类型。只能取值：0用户自定义路径，弹出用户选择路径对话框；1服务端定义路径。标识文件保存路径是服务端指定还是客户自定义路径及文件名，与excelPathName参数合用
        /// </summary>
        public string pathType { get; set; }
        /// <summary>
        /// 各列的列别名List string，比如：字段名为userName，此处可指定为"用户名"，并以此显示
        /// </summary>
        public List<string> colNameList { get; set; }
        /// <summary>
        /// 要显示/排除的列，指定这些列用于显示，或指定这些列用于不显示。倒低这些列是显示还是不显示，由excludeType参数决定
        /// </summary>
        public List<string> excludeColumn { get; set; }
        /// <summary>
        /// 显示/排除列方式。 0为显示所有列 1指定的是要显示的列 2指定的是要排除的列，与excludeColumn合用
        /// </summary>
        public string excludeType { get; set; }
        /// <summary>
        /// sheet1的名称，要使期保持默认名称请指定为空字符串 ""
        /// </summary>
        public string sheetName { get; set; }
        /// <summary>
        /// 模版在项目服务器中路径 例:tp.xlsx 。当为空字符串 "" 时表示无模版
        /// </summary>
        public string TemplatePath { get; set; }
        /// <summary>
        /// 模版中已存在数据的行数，与TemplatePath合用，无模版时请传入参数 0
        /// </summary>
        public int TemplateRow { get; set; }
        /// <summary>
        /// 扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同
        /// </summary>
        public List<System.Data.DataTable> exDataTableList { get; set; }
        #endregion
    }
    public class FromExcelModel
    {
        /// <summary>
        /// Excel文件路径(含文件名)
        /// </summary>
        public string excelFile { get; set; }
        /// <summary>
        /// sheet名<
        /// </summary>
        public string sheetName { get; set; }
        /// <summary>
        /// 存储到数据库中的数据库表名称
        /// </summary>
        public string DbTableName { get; set; }
        /// <summary>
        /// 对应表格的数据类型，如果为null，则为默认类型：double,nvarchar(500),datetime
        /// </summary>
        public List<string> columnTypeList { get; set; }
        /// <summary>
        /// 连接字符串 server=serverip;database=databasename;uid=username;pwd=password;
        /// </summary>
        public string connectionString { get; set; }
    }
}

/*
namespace EmeManager.Nurse 
{
    
}
 

namespace CExcel1 
{ 
class Class1 
{ 
[STAThread] 
static void Main(string[] args) 
{ 
//创建Application对象
Excel.ApplicationxApp=new Excel.ApplicationClass();

xApp.Visible=true; 


//得到WorkBook对象,可以用两种方式之一:下面的是打开已有的文件 
Excel.Workbook xBook=xApp.Workbooks._Open(@"D:\Sample.xls", 
Missing.Value,Missing.Value,Missing.Value,Missing.Value,

Missing.Value,Missing.Value,Missing.Value,Missing.Value,

Missing.Value,Missing.Value,Missing.Value,Missing.Value);

//xBook=xApp.Workbooks.Add(Missing.Value);//新建文件的代码 


//指定要操作的Sheet，两种方式：

Excel.WorksheetxSheet=(Excel.Worksheet)xBook.Sheets[1]; 
//Excel.WorksheetxSheet=(Excel.Worksheet)xApp.ActiveSheet;

//读取数据，通过Range对象 
Excel.Rangerng1=xSheet.get_Range("A1",Type.Missing); 
Console.WriteLine(rng1.Value2);

//读取，通过Range对象，但使用不同的接口得到Range 
Excel.Rangerng2=(Excel.Range)xSheet.Cells[3,1]; 
Console.WriteLine(rng2.Value2);

//写入数据 
Excel.Rangerng3=xSheet.get_Range("C6",Missing.Value); 
rng3.Value2="Hello"; 
rng3.Interior.ColorIndex=6;//设置Range的背景色

//保存方式一：保存WorkBook 
xBook.SaveAs(@"D:\CData.xls", 
Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Excel.XlSaveAsAccessMode.xlNoChange,Missing.Value,Missing.Value,Missing.Value, 
Missing.Value,Missing.Value);

//保存方式二：保存WorkSheet 
xSheet.SaveAs(@"D:\CData2.xls", 
Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value,Missing.Value);

//保存方式三 
xBook.Save(); 


xSheet=null; 
xBook=null; 
xApp.Quit(); //这一句是非常重要的，否则Excel对象不能从内存中退出 
xApp=null; 
} 
} 
}

C#如何向EXCEL写入数据

我按着微软技术支持网上的方法写入数据：使用“自动化”功能逐单元格传输数据，代码如下：
// Start a new workbook in Excel.
m_objExcel = new Excel.Application();
m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
m_objBook = (Excel._Workbook)(m_objBooks.Add(m_objOpt));

// Add data to cells in the first worksheet in the newworkbook.
m_objSheets = (Excel.Sheets)m_objBook.Worksheets;
m_objSheet = (Excel._Worksheet)(m_objSheets.get_Item(1));
m_objRange = m_objSheet.get_Range("A1", m_objOpt);
m_objRange.Value = "Last Name";
m_objRange = m_objSheet.get_Range("B1", m_objOpt);
m_objRange.Value = "First Name";
m_objRange = m_objSheet.get_Range("A2", m_objOpt);
m_objRange.Value = "Doe";
m_objRange = m_objSheet.get_Range("B2", m_objOpt);
m_objRange.Value = "John";

// Apply bold to cells A1:B1.
m_objRange = m_objSheet.get_Range("A1", "B1");
m_objFont = m_objRange.Font;
m_objFont.Bold=true;

// Save the Workbook and quit Excel.
m_objBook.SaveAs(m_strSampleFolder + "Book1.xls", m_objOpt, m_objOpt, m_objOpt, m_objOpt, m_objOpt, Excel.XlSaveAsAccessMode.xlNoChange, m_objOpt, m_objOpt, m_objOpt, m_objOpt);
m_objBook.Close(false, m_objOpt, m_objOpt);
m_objExcel.Quit();


EXCEL表用c#来读

using System;

using System.IO;

using System.Web;

using System.Web.SessionState;

using NickLee.Common.ExcelLite;

namespace excel1

{    /// <summary>  

/// excel 的摘要说明。

    /// </summary>  

public class excel   

{           public excel()     

  {                                

   //            // TODO: 在此处添加构造函数逻辑       

fileName = HttpContext.Current.Server.MapPath(".")+"\\temp\\"+g+".xls";     

  }            

    private     Guid g= Guid.NewGuid();     

   private stringfileName;    

    private   ExcelFile ec =new ExcelFile();        

    ///<summary>    

    /// 从model中复制模版到temp中     

   /// </summary>    

    /// <param name="modelName">只需传入模版的名字即可（注意是字符串）</param>    

    public void copyModel(stringmodelName)     

  {           

string MName = HttpContext.    

  Current.Server.MapPath(".")+"\\model\\"+modelName;   

   File.Copy(MName,fileName);   

    }       

/// <summary>      

/// 设置EXCEL表中的字表个数，字表在这个表中统一命名为“续表”+数字序号    

    /// </summary>   

     /// <paramname="sheetnum">设置字表的个数</param>      

public void setSheets(int sheetnum)   

    {          

ec.LoadXls(fileName);      

      ExcelWorksheet wsx=ec.Worksheets[0];      

      for (inty=0;y<sheetnum;y++)     

       {              

int num = y+1;       

         ec.Worksheets.AddCopy("续表"+num,wsx);     

      }      }    

    ///<summary>     

   /// 给EXCEL表的各个字段设置值     

   ///</summary>      

/// <param name="sheetnum">表的序号，注意表是以“0”开始的</param>

/// <param name="x">对应于EXCEL的最左边的数字列</param>   

/// <param name="y">对应于EXCEL的最上面的字母列</param>    

/// <param name="values">需要写入的值</param>      

public void setValues(int sheetnum,int x,int y,stringvalues)   

    {           ec.Worksheets[sheetnum].Cells[x,y].Value=values;;       }    

    ///<summary>     

   /// 保存函数，要是文件中有相同的文件就要删除了，      

   ///然后重新写入相同文件名的新EXCEL表      

/// </summary>   

     public voidsaveFile()     

  {        

    if(File.Exists(fileName))    

       {                                  

            File.Delete(fileName);             

               ec.SaveXls(fileName);   

        }           

       else           

       {             

   ec.SaveXls(fileName);   

        }         

       }      

/// <summary>     

   /// 显示的EXCEL表格       

/// </summary>      

public voidshowFile()      

{         

   HttpContext.Current.Response.Charset =System.Text.Encoding.Default.WebName;            

HttpContext.Current.Response.ContentType ="application/vnd.ms-excel";           

HttpContext.Current.Response.ContentEncoding =System.Text.Encoding.UTF8;             

HttpContext.Current.Response.WriteFile(fileName);         

   HttpContext.Current.Response.End();     

   }      

}

}

    小结一些C#读写Excel文件的相关技巧，毕竟Excel打印更为方便和实用，一个是Excel打印输出编码比Word文件打印数据简单些，另一个是Excel本身对数据超强计算处理功能；赶巧最近项目又涉及Excel报表统计打印的问题，所以在把其中的一些技术记录下来与大家一起分析讨论，次篇主要涉及两个方面内容：

1、读写Excel文件

A、设计Excel模版

B、打开一个目标文件并且读取模版内容

C、目标文件按格式写入需要的数据

D、保存并且输出目标Excel文件

2、 Excel对象资源释放，这个在以前项目没有注意彻底释放使用到Excel对象，对客户计算机资源造成一定浪费，此次得到彻底解决。

   下面是一个Excel打印输出的Demo

1、创建一个叫DemoExcel的项目

2、引用COM，包括：Microsoft.Excel.x.0.Object.Library，Microsoft.Office.x.0.Object.Library建议安装正版OFFICE,而且版本在11.0以上(Office2003以上)，引用以上两个Com后，在项目引用栏发现多了Excel、Microsoft.Office.Core，VBIDE三个 Library.

3、下面建立一些模拟的数据，此处为街镇信息

using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Text;

using System.Windows.Forms;

using Microsoft.Office.Interop.Excel;

using Microsoft.Office.Core;

using System.IO;

using System.Reflection;

 

namespace DemoExcel{   

public partial class Form1 : Form   { 

      

private object missing =Missing.Value;       

private Microsoft.Office.Interop.Excel.Application  ExcelRS;       

private Microsoft.Office.Interop.Excel.Workbook  RSbook;       

private Microsoft.Office.Interop.Excel.Worksheet  RSsheet; 

      

public Form1()       

{

           InitializeComponent();       

}        

private void Form1_Load(object sender, EventArgse)       

{           

// TODO: 这行代码将数据加载到表“dataSet1.STREET”中。您可以根据需要移动或移除它。

           this.sTREETTableAdapter.Fill(this.dataSet1.STREET);       

}       

private void button1_Click(object sender, EventArgse)       

{           

string OutFilePath =System.Windows.Forms.Application.StartupPath + @"emp.xls";                      

string TemplateFilePath =System.Windows.Forms.Application.StartupPath + @"模版.xls";           

PrintInit(TemplateFilePath,OutFilePath);       

}       

//Excle输出前初始化       

///<summary>       

///        

///</summary>       

///<returns></returns>       

public bool PrintInit(string templetFile, stringoutputFile)       

{

           Try

           {

               if (templetFile == null)

               {

                   MessageBox.Show("Excel模板文件路径不能为空！");

                   return false;

               }

               if (outputFile == null)

               {

                   MessageBox.Show("输出Excel文件路径不能为空！");

                   return false;

               }

               //把模版文件templetFile拷贝到目输出文件outputFile中,并且目标文件可以改写

               System.IO.File.Copy(templetFile, outputFile, true);

               if (this.ExcelRS != null)

                   ExcelRS = null;

               //实例化ExcelRS对象

               ExcelRS = new Microsoft.Office.Interop.Excel.ApplicationClass();

               //打开目标文件outputFile

               RSbook = ExcelRS.Workbooks.Open(outputFile, missing, missing, missing, missing,missing,

                   missing, missing, missing, missing, missing, missing, missing, missing,missing);

               //设置第一个工作溥

               RSsheet = (Microsoft.Office.Interop.Excel.Worksheet)RSbook.Sheets.get_Item(1);.

               //激活当前工作溥

               RSsheet.Activate();

                               在当前工作溥写入内容

               for (int i = 0; i < this.dataGridView1.RowCount; i++)

               {

                   RSsheet.Cells[3 + i, 1] = this.dataGridView1[0, i].Value.ToString();

                   RSsheet.Cells[3 + i, 2] = this.dataGridView1[1, i].Value.ToString();

                   RSsheet.Cells[3 + i, 3] = this.dataGridView1[2, i].Value.ToString();

               }

               //保存目标文件

               RSbook.Save();

               //设置DisplayAlerts

               ExcelRS.DisplayAlerts = false;

               ExcelRS.Visible = true;

               //ExcelRS.DisplayAlerts = true;

               //释放对象

               RSsheet = null;

               RSbook = null;

               ExcelRS = null;

               //释放内存

               GcCollect();

           }

           catch (Exception ex)

           {

               MessageBox.Show(ex.ToString());

               return false;

           }

           return true;

        }

        public voidGcCollect()

        {

           GC.Collect();

           GC.WaitForPendingFinalizers();

           GC.Collect();

           GC.WaitForPendingFinalizers();

        }

}

}

特别说明：

a、引用Microsoft.Office.Interop.Excel;

using Microsoft.Office.Core;

b、（关键）在程序中特别释放Excel资源的时候既要设置对象为null,又要强制回收内存，这样才能彻底回收资源。

c、引用的Office组建版本是个敏感问题，不同版本之间有细微差别，需要分别处理。
*/