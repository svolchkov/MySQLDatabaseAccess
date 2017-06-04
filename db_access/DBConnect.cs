using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace db_access
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        public string database;
        public string table;
        private string uid;
        private string password;
        public List<string> databases;
        public List<string> tables;

        //Constructor
        public DBConnect(string host, string user, string pass)
        {
            this.server = host;
            this.uid = user;
            this.password = pass;
            this.databases = new List<string>();
            this.database = string.Empty;
            Initialize();
            
        }

        //Initialize values
        private void Initialize()
        {
            //server = "localhost";
            //database = "connectcsharptomysql";
            //uid = "username";
            //password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "UID=" + uid + ";"
                + "PASSWORD=" + password + ";" 
               // + "Convert Zero Datetime=True; Allow Zero Datetime=True;";
               + "Allow Zero Datetime=True;";

            this.connection = new MySqlConnection(connectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SHOW DATABASES;";
            MySqlDataReader Reader;
            //connection.Open();
            bool connected = this.OpenConnection();
            if (!(connected))
                return;
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    row += Reader.GetValue(i).ToString();
                this.databases.Add(row);
            }
            // connection.Close();
            this.CloseConnection();
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                this.CloseConnection();
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void GetTables(string db)
        {
            this.database = db;
            this.tables = new List<string>();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = string.Format("SHOW TABLES IN {0};", this.database);
            MySqlDataReader Reader;
            //connection.Open();
            bool connected = this.OpenConnection();
            if (!(connected))
                return;
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    row += Reader.GetValue(i).ToString();
                this.tables.Add(row);
            }
            // connection.Close();
            this.CloseConnection();
        }

        public DataTable GetColumns(string db,string tbl)
        {
            DataTable columns = new DataTable();
            this.database = db;
            this.table = tbl;
            string query = string.Format("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH "
                + "FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=\"{0}\" "
                + "AND TABLE_NAME = \"{1}\";", this.database, this.table);
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                //MySqlDataReader dataReader = cmd.ExecuteReader();
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(columns);
                }
                //Read the data and store them in the list
                //columns.Fill((dataReader);
                

                //close Data Reader
                //dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return columns;
            }
            else
            {
                return columns;
            }
        }


        //Insert statement
        public void Insert(DataTable changes)
        {
            
            using (MySqlDataAdapter sda = new MySqlDataAdapter())
            {
                MySqlCommandBuilder mcb = new MySqlCommandBuilder(sda);
                sda.InsertCommand = mcb.GetInsertCommand();
                sda.Update(changes);
            }
            
        }

        //Update statement
        public bool Update(string db, string tbl, DataTable changes)
        {
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" + db + ";"
                + "UID=" + uid + ";" + "PASSWORD=" + password + ";" 
               // + "Convert Zero Datetime=True;";
               + "Allow Zero Datetime=True;";
            this.connection = new MySqlConnection(connectionString);
            this.table = tbl;
            this.database = db;
            string query = string.Format("SELECT * FROM {0}",this.table);
            this.connection.Open(); //change this
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, this.connection);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    MySqlCommandBuilder mcb = new MySqlCommandBuilder(sda);
                    sda.UpdateCommand = mcb.GetUpdateCommand();
                    sda.DeleteCommand = mcb.GetDeleteCommand();
                    sda.Update(changes);
                    this.connection.Close();
                    return true;
                }
            }
            //Create Command

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                this.connection.Close();
                return false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.connection.Close();
                return false;
            }
        }

        //Delete statement
        public void Delete()
        {
        }

        //Select statement
        public DataTable Select(string db, string tbl)
        {
            DataTable data = new DataTable();
            this.database = db;
            this.table = tbl;
            string query = string.Format("USE {0}; SELECT * FROM {1}", this.database, this.table);
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                //MySqlDataReader dataReader = cmd.ExecuteReader();
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(data);
                }
                
                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return data;
            }
            else
            {
                return data;
            }
        }

        public DataTable Select(string query)
        {
            DataTable data = new DataTable();
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                //MySqlDataReader dataReader = cmd.ExecuteReader();
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(data);
                }

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return data;
            }
            else
            {
                return data;
            }
        }


        ////Count statement
        public int Count(string db, string tbl)
        {
            this.database = db;
            this.table = tbl;
            string query = string.Format("USE {0}; SELECT Count(*) FROM {1}",this.database, this.table);
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        public DataTable GetPrimaryKey(string db, string tbl)
        {
            DataTable data = new DataTable();
            this.database = db;
            this.table = tbl;
            string query = string.Format("SELECT k.COLUMN_NAME " +
                "FROM information_schema.table_constraints t " +
                "LEFT JOIN information_schema.key_column_usage k " +
                "USING(constraint_name,table_schema,table_name) " +
                "WHERE t.constraint_type='PRIMARY KEY' " +
                "AND t.table_schema=\"{0}\" " +
                "AND t.table_name=\"{1}\";", this.database,this.table);
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                //MySqlDataReader dataReader = cmd.ExecuteReader();
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    // to deal with the intermittent error 13
                    bool dataLoaded = false;
                    int numberAttempts = 0;
                    while (!dataLoaded && numberAttempts < 10)
                    {
                        try
                        {
                            sda.Fill(data);
                            dataLoaded = true;
                        }
                        catch (MySqlException Ex)
                        {
                            numberAttempts += 1;
                            if (!(Ex.Number == 13))
                            {
                                MessageBox.Show(Ex.Message);
                                dataLoaded = true;
                            }
                                
                        }
                    }
                    
                    
                }

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return data;
            }
            else
            {
                return data;
            }
            //SHOW INDEX FROM <table_name> WHERE Key_name = 'PRIMARY';
        }


        public DataTable GetConstraints(string db, string tbl)
        {
            DataTable data = new DataTable();
            this.database = db;
            this.table = tbl;
            string query = string.Format("use INFORMATION_SCHEMA; " +
                "select COLUMN_NAME from KEY_COLUMN_USAGE " +
                "where TABLE_SCHEMA = \"{0}\" and TABLE_NAME = \"{1}\" " +
                "and referenced_column_name is not NULL;", this.database, this.table);
            //        
            //select TABLE_NAME,COLUMN_NAME,CONSTRAINT_NAME,
            //REFERENCED_TABLE_NAME,REFERENCED_COLUMN_NAME from KEY_COLUMN_USAGE
            
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                //MySqlDataReader dataReader = cmd.ExecuteReader();
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    sda.Fill(data);
                }

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return data;
            }
            else
            {
                return data;
            }
            
        }




        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }
}
