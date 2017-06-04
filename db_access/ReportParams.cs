using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Diagnostics;

namespace db_access
{
    public partial class ReportParams : Form
    {
        DBConnect connection;
        string database;
        string table;
        List<FieldSelector> filters;
        DataTable columns;

        public ReportParams(DBConnect conn,string db, string tbl)
        {
            InitializeComponent();
            this.connection = conn;
            this.database = db;
            this.table = tbl;
            this.columns = this.connection.GetColumns(this.database,this.table);
            filters = new List<FieldSelector>();
            foreach (DataRow d in columns.Rows)
            {
                FieldSelector f = new FieldSelector();
                f.lblFieldName.Text = d["COLUMN_NAME"].ToString();
                flpFields.Controls.Add(f);
                filters.Add(f);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            string query = string.Format("USE {0}; SELECT ", this.database);
            string whereClause = string.Empty;
            bool anyFieldsIncluded = false;
            List<string> fieldsSelected = new List<string>();
            var dateColumns = from myRow in this.columns.AsEnumerable()
                              where myRow.Field<string>("DATA_TYPE") == "date"
                              select myRow["COLUMN_NAME"];
            foreach (FieldSelector f in filters)
            {
                string currentField = f.lblFieldName.Text;
                if (f.cbInclude.Checked)
                {
                    
                    //date columns need to be converted into strings 
                    //or they won't show on report 
                    // DATE_FORMAT(NOW(), '%d %m %Y') AS your_date;
                    if (dateColumns.Contains(currentField)){
                        currentField = string.Format("DATE_FORMAT({0}, '%d-%m-%Y') AS {1}", currentField, currentField);
                    }

                    fieldsSelected.Add(currentField);
                    anyFieldsIncluded = true;
                }
                if (f.cbFrom.Text != String.Empty && f.cbTo.Text != String.Empty)
                {
                    currentField = f.lblFieldName.Text;
                    string fromString = f.cbFrom.Text;
                    string toString = f.cbTo.Text;
                    //date columns need to be converted into strings 
                    //or they won't show on report 
                    // DATE_FORMAT(NOW(), '%d %m %Y') AS your_date;
                    if (dateColumns.Contains(currentField))
                    {
                        
                        try
                        {
                            DateTime fromDate = DateTime.Parse(f.cbFrom.Text);
                            fromString = fromDate.ToString("yyyy-MM-dd"); 
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("FROM date for field {0} incorrect", currentField));
                            return;
                        }
                        try
                        {
                            DateTime toDate = DateTime.Parse(f.cbTo.Text);
                            toString = toDate.ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("TO date for field {0} incorrect", currentField));
                            return;
                        } 
                    }

                    if (whereClause != string.Empty)
                    {

                        whereClause += string.Format("AND {0} BETWEEN \"{1}\" AND \"{2}\" ",
                            f.lblFieldName.Text, fromString, toString);
                    }
                    else
                    {
                        whereClause += "WHERE";
                        whereClause += string.Format(" {0} BETWEEN \"{1}\" AND \"{2}\" ",
                            f.lblFieldName.Text, fromString, toString);
                    }
                }
            }
            if (!(anyFieldsIncluded))
            {
                MessageBox.Show("No fields selected");
                return;
            }

            query += string.Join(",",fieldsSelected);
            query += string.Format(" FROM {0} ", this.table);
            query += whereClause;
            DataTable reportData = this.connection.Select(query);
            if (reportData != null && reportData.Rows.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(reportData, "Report");
                string xlsFilename = string.Format("{0}-{1}.xlsx",
                    this.table, DateTime.Now.ToString("yyyyMMddHHmmss"));
                wb.SaveAs(xlsFilename);
                Process.Start(xlsFilename);
            }
            else
            {
                MessageBox.Show("The query did not return any rows");
            }
        }
    }
}
