using MySql.Data.MySqlClient;
using System.Data;
using static MudBlazor.CategoryTypes;

namespace BlazorApp1.Data
{
    public class Database
    {
        private readonly string MySQLConnectionString;
        public Database()
        {
            //MySQLConnectionString = "Server=127.0.0.1; Database=employees; Uid=usrEmployees; Pwd=password;";
            MySQLConnectionString = "Server=localhost; Database=BlazorApp1; Uid=root; Pwd=root;";
        }

        public DataTable MySQLConnection_Datatable()
        {
            DataTable dtDaten = new DataTable();
            // Best practice is to scope the MySqlConnection to a "using" block
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                // The MySqlCommand class represents a SQL statement to execute against a MySQL database
                MySqlCommand selectCommand = new MySqlCommand("SELECT * FROM Users", conn);
                // execute the reader To query the database. Results are usually returned in a MySqlDataReader object, created by ExecuteReader.
                using (MySqlDataReader rdr = selectCommand.ExecuteReader())
                {
                    dtDaten.Load(rdr);
                }
                conn.Close();
            }
            return dtDaten;
        }

        public int AddUser(string name, string email, string password)
        {
            int records = 0;
            string TSQLString = "INSERT INTO Users (nombre, email, password) VALUES ('" + name
                    + "', '" + email + "', '" + password + "')";
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                //The MySqlCommand class represents a SQL statement to execute against a MySQL database
                MySqlCommand selectCommand = new MySqlCommand(TSQLString, conn);
                // Executes a Transact-SQL statement against the connection and returns the number of rows affected.
                // to insert, update, and delete data.
                records = selectCommand.ExecuteNonQuery();

                conn.Close();
            }
            return records;
        }

        public Boolean FindUser(string name, string email, string password)
        {
            DataTable dtDaten = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                MySqlCommand selectCommand = new MySqlCommand("SELECT * FROM Users WHERE nombre = '" + name + "' and email = '" + email + "' and password = '" + password + "'", conn);
                // execute the reader To query the database. Results are usually returned in a MySqlDataReader object, created by ExecuteReader.
                using (MySqlDataReader rdr = selectCommand.ExecuteReader())
                {
                    dtDaten.Load(rdr);
                }
                conn.Close();
            }
            if (dtDaten.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                User.id = Int32.Parse(dtDaten.Rows[0]["id"].ToString());
                User.name = (string)dtDaten.Rows[0]["nombre"];
                User.email = (string)dtDaten.Rows[0]["email"];
                User.password = (string)dtDaten.Rows[0]["password"];
                return true;
            };
        }

        public int RemoveUser(string name, string email, string password)
        {
            int records = 0;
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                //The MySqlCommand class represents a SQL statement to execute against a MySQL database
                MySqlCommand selectCommand = new MySqlCommand("DELETE FROM Users WHERE nombre = '" + name + "' and email = '" + email + "' and password = '" + password + "'", conn);
                // Executes a Transact-SQL statement against the connection and returns the number of rows affected.
                // to insert, update, and delete data.
                records = selectCommand.ExecuteNonQuery();
                conn.Close();
            }
            return records;
        }

        public int UpdateUser(string name, string email, string password, string newName, string newEmail, string newPassword)
        {
            int records = 0;
            DataTable dtDaten = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                conn.Open();
                MySqlCommand selectCommand = new MySqlCommand("SELECT * FROM Users WHERE nombre = '" + name + "' and email = '" + email + "' and password = '" + password + "'", conn);
                // execute the reader To query the database. Results are usually returned in a MySqlDataReader object, created by ExecuteReader.
                using (MySqlDataReader rdr = selectCommand.ExecuteReader())
                {
                    dtDaten.Load(rdr);
                }
                //The MySqlCommand class represents a SQL statement to execute against a MySQL database
                MySqlCommand selectCommand2 = new MySqlCommand("Update Users SET id = " + Int32.Parse(dtDaten.Rows[0]["id"].ToString())+", nombre = '" + newName +"', email = '"+newEmail +"', password = '"+newPassword+"' WHERE nombre = '" + name + "' and email = '" + email + "' and password = '" + password + "'", conn);
                records = selectCommand2.ExecuteNonQuery();
                conn.Close();
            }

            return records;
        }

        public int AddTask(string name, int duracion, string fechaInicio,  string fechaFin)
        {
            int records = 0;
            string TSQLString = "INSERT INTO Tasks (id_user, name, duracion, fechaInicio, fechaFin) VALUES ('" + User.id
                    + "', '" + name + "', " + duracion + ", '" + fechaInicio + "', '" + fechaFin + "')";
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                //The MySqlCommand class represents a SQL statement to execute against a MySQL database
                MySqlCommand selectCommand = new MySqlCommand(TSQLString, conn);
                // Executes a Transact-SQL statement against the connection and returns the number of rows affected.
                // to insert, update, and delete data.
                records = selectCommand.ExecuteNonQuery();

                conn.Close();
            }
            return records;
        }

        public IEnumerable<DataTask> FindTasks()
        {
            DataTable dtDaten = new DataTable();
            List<DataTask> tasks = new List<DataTask>();

            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                MySqlCommand selectCommand = new MySqlCommand("SELECT * FROM Tasks WHERE id_user = " + User.id, conn);
                // execute the reader To query the database. Results are usually returned in a MySqlDataReader object, created by ExecuteReader.
                using (MySqlDataReader rdr = selectCommand.ExecuteReader())
                {
                    dtDaten.Load(rdr);
                }
                conn.Close();
            }

            foreach(DataRow row in dtDaten.Rows)
            {
                DataTask dataTask = new DataTask();
                dataTask.name = row["name"].ToString();
                dataTask.duracion = int.Parse(row["duracion"].ToString());
                dataTask.fechaInicio = row["fechaInicio"].ToString();
                dataTask.fechaFinal = row["fechaFin"].ToString();
                tasks.Add(dataTask);
            }
            return tasks;
        }

        public int FindTask(string name, int duracion, string fechaInicio, string fechaFin)
        {
            DataTable dtDaten = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                MySqlCommand selectCommand = new MySqlCommand("SELECT * FROM Tasks WHERE name = '" + name + "' and duracion = " + duracion + " and fechaInicio = '" + fechaInicio + "' and fechaFin = '" + fechaFin + "'", conn);
                // execute the reader To query the database. Results are usually returned in a MySqlDataReader object, created by ExecuteReader.
                using (MySqlDataReader rdr = selectCommand.ExecuteReader())
                {
                    dtDaten.Load(rdr);
                }
                conn.Close();
            }
            if (dtDaten.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(dtDaten.Rows[0]["id_task"].ToString());
            };
        }

        public int UpdateTask(int id, string name, int duracion, string fechaInicio, string fechaFin)
        {
            int records = 0;
            DataTable dtDaten = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                conn.Open();
                MySqlCommand selectCommand2 = new MySqlCommand("Update Tasks SET name = '" + name + "', duracion = " + duracion + ", fechaInicio = '" + fechaInicio + "', fechaFin = '" + fechaFin + "' WHERE id_task = "+id, conn);
                records = selectCommand2.ExecuteNonQuery();
                conn.Close();
            }

            return records;
        }
        public int RemoveTask(int id)
        {
            int records = 0;
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                // Connect to the database
                conn.Open();
                //The MySqlCommand class represents a SQL statement to execute against a MySQL database
                MySqlCommand selectCommand = new MySqlCommand("DELETE FROM Tasks WHERE id_task = " + id, conn);
                // Executes a Transact-SQL statement against the connection and returns the number of rows affected.
                // to insert, update, and delete data.
                records = selectCommand.ExecuteNonQuery();
                conn.Close();
            }
            return records;
        }
    }
}

