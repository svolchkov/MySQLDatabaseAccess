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
    public partial class DBList : Form
    {
        List<string> databases;
        public DBConnect connection;
        string selectedDB;

        public DBList(DBConnect conn)
        {
            this.databases = new List<string>();
            this.connection = conn;
            this.databases = this.connection.databases;
            InitializeComponent();
            lbDatabases.DataSource = this.databases;
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (lbDatabases.SelectedItem != null)
            {
                selectedDB = lbDatabases.GetItemText(lbDatabases.SelectedItem);
                this.connection.GetTables(selectedDB);
                if (this.connection.tables.Count > 0)
                {
                    TableList tables = new TableList(this.connection,selectedDB);
                    tables.Show();
                }
            }
        }
    }
}
