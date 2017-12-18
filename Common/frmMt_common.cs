using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using System.Text;
using DevExpress.XtraTab;
using System.Reflection;

namespace ERP.Common
{
    /// <summary>
    /// 共用方法 
    /// </summary>
    public static class frmMt_common
    {  
        /// <summary>
        /// 数据表选择符合“指定条件”的记录后，返回为一个数据表
        /// </summary>
        public static DataTable DataTable_Select(DataTable dt, string str_select)
        {
            if (string.IsNullOrEmpty(str_select) || dt == null)
                return dt;

            DataTable _dt = dt.Clone();
            DataRow[] drList = dt.Select(str_select);
            foreach (DataRow dr in drList)
            { 
                _dt.ImportRow(dr);
            }
            return _dt;
        }
         



        /// <summary>
        /// 如果开启筛选，则返回数据表选择符合“指定条件”的记录组成的表
        /// </summary>
        public static DataTable DataTable_Select(DataTable dt, string str_select, bool if_filter)
        {
            if (!if_filter || string.IsNullOrEmpty(str_select) || dt == null)
                return dt;

            DataTable _dt = dt.Clone();
            DataRow[] drList = dt.Select(str_select);
            foreach (DataRow dr in drList)
            {
                _dt.ImportRow(dr);
            }
            return _dt;
        }
 



        /// <summary>
        /// 当lookupedit控件按下回退或删除键时，清空当前值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void lookUpEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == null || !(sender is LookUpEdit))
                return;

            LookUpEdit lkpedit = (LookUpEdit)sender;

            //当按下回车键时，如果控件可以编辑，则清空当前选项
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                if (lkpedit.Properties.Editable && !lkpedit.Properties.ReadOnly)
                {
                    (sender as LookUpEdit).EditValue = DBNull.Value;
                    (sender as LookUpEdit).EditValue = null;
                }
            }
        }



        /// <summary>
        /// 给指定Panel里的所有lookupedit，绑定上回退清空功能
        /// </summary>
        /// <param name="Pnl"></param>
        public static void BindEvent_Lookupedit(object Pnl)
        {
            if (Pnl == null)
                return;

            Control.ControlCollection ctrs = null;
            if ( Pnl is Panel)
                ctrs = (Pnl as Panel).Controls;
            if ( Pnl is PanelControl)
                ctrs = (Pnl as PanelControl).Controls;

            if ( ctrs == null)
                return;

            foreach (Control c in ctrs)
            {
                switch (c.GetType().Name)
                {
                    case "Panel":
                    case "TableLayoutPanel":
                    case "SplitContainer":
                    case "FlowLayoutPanel":
                        Panel p = (Panel)c;
                        BindEvent_Lookupedit(p);
                        break;
                    case "PanelControl":
                        PanelControl pc = (PanelControl)c;
                        BindEvent_Lookupedit(pc);
                        break;
                    case "LookUpEdit":
                        LookUpEdit l = (LookUpEdit)c;
                        if (l != null)
                        {
                            l.KeyDown += lookUpEdit_KeyDown;//绑定事件
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// 如果datatable里包含指定条件的行,则提醒并返回true 
        /// </summary>
        /// <param name="Contain_Select">指定条件</param>
        /// <param name="WarnStr">提醒内容</param>
        public static bool DataTable_Contains_Alert(DataTable _dt, string Contain_Select, string WarnStr)
        {
            DataRow[] __dtList = _dt.Select(Contain_Select);
            if (__dtList.Length > 0)
            {
                XtraMessageBox.Show(WarnStr, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 数据源清理掉未正式添加的行,即行状态为Detached的行
        /// </summary>
        public static void Clear_DetachedRows(RjCtrlLib.RjBindingSource _rjBindingSource)
        {
            if  ( _rjBindingSource != null &&
                 (_rjBindingSource.Current as DataRowView) != null &&
                 (_rjBindingSource.Current as DataRowView).Row.RowState == DataRowState.Detached
                )
                    _rjBindingSource.Delete(); 
        }


        public static bool String_IsNullOrEmpty(object _obj)
        {
            if (_obj == null)
                return true;

            return string.IsNullOrEmpty(Convert.ToString(_obj));
        }


        public static object DataTable_FirstAvaiableRowValue(DataTable _dt, string column,string str_Available)
        {
            if (string.IsNullOrEmpty(str_Available))
                str_Available = "STATE=1";

            DataRow[] _drList = _dt.Select(str_Available);
            if (_drList.Length > 0)
            {
                return _drList[0][column];
            }
            return null;
        }


        /// <summary>
        /// 转换为时间,并按格式返回字符串,默认"yyyy-MM-dd",如果字段为空或转换不了时间,返回空
        /// </summary>
        public static string DateTime_ToString(object obj_datetime,string output_format)
        {
            if (output_format == null || output_format.Length == 0)
                output_format = "yyyy-MM-dd";

            DateTime dt;
            //如果字段不为空,且能够转换为时间,则转换后按格式返回
            if (DateTime.TryParse(Convert.ToString(obj_datetime), out dt))
                return dt.ToString(output_format);
            else
                return string.Empty;//否则返回空字符串
        }

        /// <summary>
        /// 去掉字符串中的数字
        /// </summary>
        public static string RemoveNumber(object key)
        {
            return Regex.Replace(Convert.ToString(key), @"\d", "");
        }


        /// <summary>
        /// 去掉字符串中的非数字
        /// </summary>
        public static string RemoveNotNumber(object key)
        {
            return Regex.Replace(Convert.ToString(key), @"[^\d]*", "");
        }

        /// <summary>
        /// 得到字符串的长度，按字节数算
        /// </summary> 
        public static int GetStringLength_ByBit(string str)
        {  
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 得到字符串的长度，按字节数算
        /// </summary> 
        public static int GetStringLength_ByBit(object obj)
        {
            return Encoding.Default.GetBytes(Convert.ToString(obj)).Length;
        }


        /// <summary>
        /// 按字节数做最大长度验证，超出长度限制则提示并返回true
        /// </summary> 
        public static bool MaxLengthCheck(RjCtrlLib.RjBindingSource rjBindingSource)
        {
            DataSet ds = (rjBindingSource.DataSource as DataSet);
            if (ds == null || !ds.Tables.Contains(rjBindingSource.DataMember))
                return false;

            DataTable dt = ds.Tables[rjBindingSource.DataMember]; 
            foreach (DataColumn dtCol in dt.Columns)
            {
                if (dtCol.DataType.Name == "String"
                    && dtCol.MaxLength>0 
                    && !dtCol.ReadOnly)
                {  
                    foreach(DataRow dr in dt.Rows)
                    {
                        //如果字符串的长度，按字节数算，超出了最大长度，则提醒长度超出限制
                        if (Encoding.Default.GetBytes(Convert.ToString(dr[dtCol.ColumnName]))
                            .Length > dtCol.MaxLength)
                        {
                            string strAlert;
                            if (rjBindingSource.GridView != null
                                && rjBindingSource.GridView.Columns[dtCol.ColumnName] != null)
                            {
                                strAlert = string.Format("{0}列内容“{1}”长度超出限制",
                                    rjBindingSource.GridView.Columns[dtCol.ColumnName].Caption , dr[dtCol.ColumnName]);
                            }
                            else
                            {
                                strAlert = string.Format("{0}列内容“{1}”长度超出限制",
                                    dtCol.ColumnName, dr[dtCol.ColumnName]);
                            }
                            XtraMessageBox.Show(strAlert,
                                "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                            return true;
                        }
                    }
                    
                } 
            }

            return false;
        }

        /// <summary>
        /// 启动新菜单页，如果已有则关闭旧页重开
        /// </summary>
        /// <param name="_form">源打开页的窗体对象，this</param>
        /// <param name="full_class_name">目标打开页的类全名</param>
        /// <param name="_newPageCaption">目标打开页的标题</param>
        /// <param name="obj">目标打开页的参数，如PatRow</param>
        public static void ActiveMenuWindow(Control _form ,string full_class_name,string _newPageCaption, object obj)
        {  
            XtraTabControl TabControl1 = (_form.Parent.Parent as XtraTabControl);
            XtraTabPage PerPage = null;//旧页面
            foreach (XtraTabPage Page in TabControl1.TabPages)
            {
                if (Page.Controls[0].GetType().ToString() != full_class_name)
                    continue;

                PerPage = Page;//储存旧页面 
            }
            XtraTabPage TabPage = new XtraTabPage();
            TabPage.Text = _newPageCaption;
            TabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True; 
            string path = "ERP.Forms";
            Form frm = new Form();
            object[] objs = new object[1] { obj };
            frm = (Form)Assembly.Load(path).CreateInstance(full_class_name, true, BindingFlags.Default, null, objs, null, null);
            frm.Text = _newPageCaption;
            frm.TopLevel = false;
            frm.Parent = TabPage;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
            TabControl1.TabPages.Add(TabPage);
            TabControl1.SelectedTabPage = TabPage;
            if (PerPage != null)
                TabControl1.TabPages.Remove(PerPage); //打开新页后关闭旧页
        }

        /// <summary>
        /// 字典表表头增加一行“全选”
        /// </summary> 
        public static DataTable addSelectAllRow(DataTable dt, string col_id, string col_name, string col_spy)
        {
            if (string.IsNullOrWhiteSpace(col_spy))
            {
                DataTable _dt = dt.DefaultView.ToTable(false,
                   new string[] { col_id, col_name  });
                _dt.Columns[col_id].AllowDBNull = true;
                DataRow _dr = _dt.NewRow();
                _dr[col_id] = DBNull.Value;
                _dr[col_name] = "全部"; 
                _dt.Rows.InsertAt(_dr, 0);
                return _dt;
            }
            else
            {
                DataTable _dt = dt.DefaultView.ToTable(false,
                    new string[] { col_id, col_name, col_spy });
                _dt.Columns[col_id].AllowDBNull = true;
                DataRow _dr = _dt.NewRow();
                _dr[col_id] = DBNull.Value;
                _dr[col_name] = "全部";
                _dr[col_spy] = "QB";
                _dt.Rows.InsertAt(_dr, 0);
                return _dt;
            }
        }

        /// <summary>
        /// 另一个表里已新增的，本表不再包含，避免重复选取
        /// </summary> 
        public static DataTable filterDt2Dt(DataTable dt, string dt_key, DataTable dt_f, string dt_fkey )
        {
            if(dt_f==null||dt_f.Rows.Count==0)
                return dt;
            foreach (DataRow dr_f in dt_f.Rows)
            {
                if (dr_f.RowState == DataRowState.Added || dr_f.RowState == DataRowState.Modified)
                {
                    DataRow[] drlist = dt.Select(
                        string.IsNullOrWhiteSpace(Convert.ToString(dr_f[dt_fkey])) ?
                        string.Format("{0} is null", dt_key) :
                        string.Format("{0}='{1}'", dt_key, dr_f[dt_fkey]));
                    for (int i = drlist.Length-1; i >=0; i--)
                    {
                        dt.Rows.Remove(drlist[i]);
                    }
                }
                if (dr_f.RowState == DataRowState.Deleted || dr_f.RowState == DataRowState.Modified)
                {
                    //DataRow[] drlist = dt.Select(string.Format("{0}='{1}'", dt_key, dr_f[dt_fkey]));
                    //for (int i = drlist.Length - 1; i >= 0; i--)
                    //{
                    //    dt.Rows.Remove(drlist[i]);
                    //}
                }
            }
            return dt;
        }
    }
}
