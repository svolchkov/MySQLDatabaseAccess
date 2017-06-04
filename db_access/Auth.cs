using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace db_access
{
    public partial class Auth  : Form
    {
        public DBConnect conn;

        public Auth()
        {
            InitializeComponent();
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            conn = new DBConnect(tbHost.Text, tbUser.Text, tbPass.Text);
            if (conn.databases.Count > 0)
            {
                DBList db = new DBList(conn);
                db.Show();
            }
            else
            {
                MessageBox.Show("No databases found");
            }
        }
    }
}
