using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace db_access
{
    public partial class DGrid : Form
    {
        DBConnect connection;
        string currentTable;
        string currentDatabase;
        DataTable Data;

        public DGrid(DBConnect conn, string db, string tbl)
        {
            InitializeComponent();
            this.currentTable = tbl;
            this.connection = conn;
            this.currentDatabase = db;
            this.Text = string.Format("Data in {0}.{1}", this.currentDatabase, this.currentTable);
            this.Data = this.connection.Select(this.currentDatabase,this.currentTable);
            this.Data.RowChanged += new DataRowChangeEventHandler(Row_Changed);
            this.Data.RowDeleted += new DataRowChangeEventHandler(Row_Deleted);
            dgvData.DataSource = this.Data;
            DataTable primary = this.connection.GetPrimaryKey(this.currentDatabase, this.currentTable);
            //set colour of primary key columns
            foreach (DataGridViewTextBoxColumn d in dgvData.Columns)
            {
                if (primary.AsEnumerable().Any(row => d.HeaderText == row.Field<String>("COLUMN_NAME")))
                {
                    dgvData.Columns[d.HeaderText].DefaultCellStyle.BackColor = Color.LightSkyBlue;
                    dgvData.Columns[d.HeaderText].Frozen = true;
                }
            }
            //set colour of constraint columns
            DataTable constraints = this.connection.GetConstraints(this.currentDatabase, this.currentTable);
            foreach (DataGridViewTextBoxColumn d in dgvData.Columns)
            {
                if (constraints.AsEnumerable().Any(row => d.HeaderText == row.Field<String>("COLUMN_NAME")))
                {
                    if (primary.AsEnumerable().Any(row => d.HeaderText == row.Field<String>("COLUMN_NAME")))
                    {
                        dgvData.Columns[d.HeaderText].DefaultCellStyle.BackColor = Color.Plum;
                    }
                    else
                    {
                        dgvData.Columns[d.HeaderText].DefaultCellStyle.BackColor = Color.LightPink;
                    }
                    
                }
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ReportParams rpt = new ReportParams(this.connection, 
                this.currentDatabase,this.currentTable);
            rpt.Show();
        }

        private void dgvData_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            // MessageBox.Show("Cell validated");
        }

        private void dgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Row_Changed(object sender, DataRowChangeEventArgs e)
        {
            //MessageBox.Show("Cell changed");
            try
            {
                DataTable changes = ((DataTable)dgvData.DataSource).GetChanges();
                if (changes != null)
                {
                    //MessageBox.Show("Updating...");
                    bool success = this.connection.Update(this.currentDatabase, this.currentTable, (DataTable)dgvData.DataSource);
                    if (success)
                    {
                        ((DataTable)dgvData.DataSource).AcceptChanges();
                        MessageBox.Show("Row updated");
                    }
                    else
                    {
                        ((DataTable)dgvData.DataSource).RejectChanges();
                        MessageBox.Show("Row NOT updated");
                    }
                        
                    return;
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show("Bozingwa!");
                //MessageBox.Show(ex.Message);
            }
        }

        private void Row_Deleted(object sender, DataRowChangeEventArgs e)
        {
            //MessageBox.Show("Cell changed");
            try
            {
                DataTable changes = ((DataTable)dgvData.DataSource).GetChanges();
                if (changes != null)
                {
                    //MessageBox.Show("Updating...");
                    bool success = this.connection.Update(this.currentDatabase, this.currentTable, (DataTable)dgvData.DataSource);
                    if (success)
                    {
                        ((DataTable)dgvData.DataSource).AcceptChanges();
                        MessageBox.Show("Row deleted");
                    }
                    else
                    {
                        ((DataTable)dgvData.DataSource).RejectChanges();
                        MessageBox.Show("Unable to delete row");
                    }

                    return;
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show("Bozingwa!");
                //MessageBox.Show(ex.Message);
            }
        }

        private void dgvData_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            //MessageBox.Show(this.dgvData.Columns[e.ColumnIndex].ValueType.ToString());
            if (this.dgvData.Columns[e.ColumnIndex].ValueType == typeof(MySql.Data.Types.MySqlDateTime))
            {
                if (e != null)
                {
                    if (e.Value != null)
                    {
                        try
                        {
                            // Map what the user typed into UTC.
                            DateTime dateValue = DateTime.Parse(e.Value.ToString());
                            // e.Value = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
                            e.Value = new MySql.Data.Types.MySqlDateTime(dateValue);

                            // Set the ParsingApplied property to 
                            // Show the event is handled.
                            e.ParsingApplied = true;

                        }
                        catch (FormatException)
                        {
                            // Set to false in case another CellParsing handler
                            // wants to try to parse this DataGridViewCellParsingEventArgs instance.
                            e.ParsingApplied = false;
                        }
                    }
                }
            }
        }

        private void dgvData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
    // If the column is the DATED column, check the
    // value.
            if (e.ColumnIndex > 0)
            {
                if (this.dgvData.Columns[e.ColumnIndex].ValueType == typeof(MySql.Data.Types.MySqlDateTime))
                {
                    ShortFormDateFormat(e);
                }
            }
        }

    
        private static void ShortFormDateFormat(DataGridViewCellFormattingEventArgs formatting)
        {
            if (formatting.Value != null)
            {
                try
                {
                    DateTime theDate = DateTime.Parse(formatting.Value.ToString());
                    String dateString = theDate.ToString("dd-MM-yyyy");    
                    formatting.Value = dateString;
                    formatting.FormattingApplied = true;
                }
                catch (FormatException)
                {
                    // Set to false in case there are other handlers interested trying to
                    // format this DataGridViewCellFormattingEventArgs instance.
                    formatting.FormattingApplied = false;
                }
            }
        }


    }
}
