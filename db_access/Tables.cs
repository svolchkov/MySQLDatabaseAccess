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
    public partial class TableList : Form
    {
        DBConnect connection;
        string database;

        public TableList(DBConnect conn, string db)
        {
            InitializeComponent();
            this.connection = conn;
            this.database = db;
            this.Text = "Tables in " + this.database;
            lbTables.DataSource = this.connection.tables;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string selectedTable = lbTables.GetItemText(lbTables.SelectedItem);
            if (selectedTable != null)
            {
                int tableCount = this.connection.Count(this.database,selectedTable);
                //DataTable dt = this.connection.Select(selectedTable);
                //if (tableCount > 0){
                    DGrid dg = new DGrid(this.connection, this.database,selectedTable);
                    dg.Show();
                //}
                //else
                //{
                //   MessageBox.Show("The selected table is empty");
                //}
            }
        }
    }
}
