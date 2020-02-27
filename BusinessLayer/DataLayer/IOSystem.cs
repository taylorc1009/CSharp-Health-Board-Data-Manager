using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
/* For SQL
 * using System.Data.Common;
 * using System.Configuration;
 * using System.Text;
 */

namespace BusinessLayer
{
    public class IOSystem
    {
        /* SQL/Database file handling
         * 
         * This works, but it requires an SQL server (preferably Microsoft SQL Server 2019 Developer Edition) to run. If you wish to try this, create
         * a database in the MSSQLServer, copy its connection string and paste it into the quotes indicated in the App.config file in
         * PresentationLayer. You will need to load the .sql files provided into the database you have created to create the tables required.
         *
        
        //These are the connection details specified in App.config ("provider" is the framework we will be using, "connectionString" is the database's connection details)
        private static string provider = ConfigurationManager.AppSettings["provider"];
        private static string connectionString = ConfigurationManager.AppSettings["connectionString"];
        //This is a factory we will use to create connections only when we need them
        DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
        PersonFactory pFactory = new PersonFactory();
        //Load is compiled into one function, this does seem cluttered but it prevents a lot of duplicated code
        public dynamic load(string table, Dictionary<int, Staff> members, Dictionary<int, Client> clients, List<Visit> visits)
        {
            //Creates a new connection to the server using the factory ("using" keeps this connection open as long as the code to load the data is running)
            using (DbConnection connection = factory.CreateConnection())
            {
                //Detects whether the server connection was successful
                if (connection != null)
                {
                    //Creates a connection to the database using the "connectionString" and attempts to open the database
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    //We will use "command" to run commands (SQL queries) on the database 
                    DbCommand command = factory.CreateCommand();
                    //Detects whether the database connection was successful
                    if (command != null)
                    {
                        //Specifies where the SQL queries shall be carried out
                        command.Connection = connection;
                        //Sets the query to get all the data from the table we specified
                        command.CommandText = "select * from " + table;
                        //Detects if we specified to load the staff table data
                        if (table == "staff")
                        {
                            //This is where the query is processed; "ExecuteReader" processes the "command.CommandText" we specified previously
                            //Of course, the database records are stored in "reader"
                            using (DbDataReader reader = command.ExecuteReader())
                            {
                                //Reads each record from "reader"
                                while (reader.Read())
                                {
                                    if (!members.ContainsKey(Convert.ToInt32(reader["id"])))
                                    {
                                        Staff member = pFactory.createPerson("staff");
                                        //Converts the data format from the SQL data types to C# types
                                        member.Construct(removeBlanks(reader["forename"].ToString()), removeBlanks(reader["surname"].ToString()), removeBlanks(reader["address1"].ToString()), removeBlanks(reader["address2"].ToString()), removeBlanks(reader["category"].ToString()), Convert.ToDouble(reader["longitude"]), Convert.ToDouble(reader["latitude"]));
                                        //Scroll down for the "removeBlanks" method and why we need it
                                        members.Add(Convert.ToInt32(reader["id"]), member);
                                    }
                                }
                                return members;
                            }
                        }
                        //Detects if we same as above but with the clients table data
                        else if (table == "clients")
                        {
                            using (DbDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!clients.ContainsKey(Convert.ToInt32(reader["id"])))
                                    {
                                        Client client = pFactory.createPerson("client");
                                        client.Construct(removeBlanks(reader["forename"].ToString()), removeBlanks(reader["surname"].ToString()), removeBlanks(reader["address1"].ToString()), removeBlanks(reader["address2"].ToString()), Convert.ToDouble(reader["longitude"]), Convert.ToDouble(reader["latitude"]));
                                        clients.Add(Convert.ToInt32(reader["id"]), client);
                                    }
                                }
                                return clients;
                            }
                        }
                        //Again detects if we same as above but with the visits table data
                        else if (table == "visits")
                        {
                            using (DbDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    //When the visits staff ID data is stored in the database, we convert it to a single string, so these few convert it back to an int array
                                    string idString = reader["staffID"].ToString();
                                    String[] ids = idString.Split(',');
                                    int[] staffID = Array.ConvertAll(ids, int.Parse);
                                    try
                                    {
                                        Visit visit = new Visit(staffID, Convert.ToInt32(reader["clientID"]), Convert.ToInt32(reader["vType"]), Convert.ToDateTime(reader["dateAndTime"]));
                                        visit.validateIDs(members, clients, visit.staffID, visit.clientID);
                                        visit.validateType(members, visit.staffID, visit.type);
                                        visit.validateTime(visits, visit.staffID, visit.clientID, visit.dateTime);
                                        visits.Add(visit);
                                    }
                                    catch (Exception e)
                                    {
                                        string eStr = e.ToString();
                                        string[] eCut = eStr.Split('%');
                                        throw new Exception(eCut[0]);
                                    }
                                }
                                return visits;
                            }
                        }
                        else
                            return null;
                    }
                    else
                        throw new Exception("Command NOT FOUND");
                }
                else
                    throw new Exception("Database DOESN'T EXIST");
            }
        }
        public void save(Dictionary<int, Staff> members, Dictionary<int, Client> clients, List<Visit> visits)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                if (connection != null)
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    DbCommand command = factory.CreateCommand();
                    if(command != null)
                    {
                        command.Connection = connection;
                        //We need to clear the table data, we cannot overwrite it as the database will detect that the primary keys already exist
                        command.CommandText = "delete from staff";
                        command.ExecuteNonQuery();
                        //Executes the insert query with the Dictionary data, gets the number of records added and if it's none then throws an exception
                        foreach (KeyValuePair<int, Staff> member in members)
                        {
                            command.CommandText = $"insert into staff ([id], [forename], [surname], [address1], [address2], [category], [longitude], [latitude]) values({member.Key}, '{member.Value.forename}', '{member.Value.surname}', '{member.Value.address1}', '{member.Value.address2}', '{member.Value.category}', {member.Value.longitude}, {member.Value.latitude})";
                            int rowsAdded = command.ExecuteNonQuery();
                            if (rowsAdded == 0)
                                throw new Exception("Record NOT INSERTED");
                        }
                        command.CommandText = "delete from clients";
                        command.ExecuteNonQuery();
                        //Inserts the clients data individually to to the clients table
                        foreach (KeyValuePair<int, Client> client in clients)
                        {
                            command.CommandText = $"insert into clients ([id], [forename], [surname], [address1], [address2], [longitude], [latitude]) values({client.Key}, '{client.Value.forename}', '{client.Value.surname}', '{client.Value.address1}', '{client.Value.address2}', {client.Value.longitude}, {client.Value.latitude})";
                            int rowsAdded = command.ExecuteNonQuery();
                            if (rowsAdded == 0)
                                throw new Exception("Record NOT INSERTED");
                        }
                        command.CommandText = "delete from visits";
                        command.ExecuteNonQuery();
                        //Again inserts the visits data this time individually to to the visits table
                        foreach (Visit visit in visits)
                        {
                            command.CommandText = $"insert into visits ([staffID], [clientID], [vType], [dateAndTime]) values('{string.Join(",", visit.staffID)}', {visit.clientID}, {visit.type}, '{visit.dateTime}')";
                            int rowsAdded = command.ExecuteNonQuery();
                            if (rowsAdded == 0)
                                throw new Exception("Record NOT INSERTED");
                        }
                    }
                    else
                        throw new Exception("Command NOT FOUND");
                }
                else
                    throw new Exception("Database DOESN'T EXIST");
            }
        }
        // When we store the data in the database, it's stored in fields which have a fixed length, so say we store a string in a VARCHAR of length 30
        // but the string is only 20 chars long, the database will complete the now VARCHAR data with 10 empty spaces. These spaces persist when we
        // read the data back in from the database, so we will have to remove them manually.
        private string removeBlanks(string str)
        {
            // We need to rebuild the string char by char, but for all we know the string could fit into the length it came back as, so we need to set
            // its initial length as the length potentially including blanks.
            StringBuilder fixedStr = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                // We don't want to remove spaces we actually need, so we detect if there is a char existing after the space ("i + 1"), if not then we
                // remove it. Though if we try use the index "i + 1" and "i" is currently equal to the string length, we will get an out of bounds
                // error, so we also need to detect if this is the case.
                if (i + 1 != str.Length)
                {
                    if (str[i + 1] != ' ' && str[i] == ' ')
                        //Append simply adds the current char to the StringBuilder
                        fixedStr.Append(str[i]);
                    else if (str[i] != ' ')
                        fixedStr.Append(str[i]);
                }
                else if (str[i] != ' ')
                    fixedStr.Append(str[i]);
            }
            //Converts the StringBuilder to a string and returns it
            return fixedStr.ToString();
        }*/

        public Dictionary<int, Staff> loadStaff()
        {
            //Simply detects whether the binary file "Staff" exists
            if (File.Exists("Staff.dat"))
            {
                //Creates a Dictionary for the files data to be loaded into
                Dictionary<int, Staff> members = new Dictionary<int, Staff>();
                //Keeps the file "Staff.dat" open as long as the code in this "using" statement is active
                using (Stream file = File.Open("Staff.dat", FileMode.Open))
                {
                    //We use BinaryFormatter type to convert the Dictionary stored as binary back to a regular Dictionary 
                    BinaryFormatter formatter = new BinaryFormatter();
                    //Converts (deserializes) the binary data back to a dictionary and stores it in members
                    members = (Dictionary<int, Staff>)formatter.Deserialize(file);
                }
                return members;
            }
            else
                throw new Exception("File DOESN'T EXIST");
        }
        //Does the same as "loadStaff" except with the clients class, dictionary and file
        public Dictionary<int, Client> loadClients()
        {
            if (File.Exists("Clients.dat"))
            {
                Dictionary<int, Client> clients = new Dictionary<int, Client>();
                using (Stream file = File.Open("Clients.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    clients = (Dictionary<int, Client>)formatter.Deserialize(file);
                }
                return clients;
            }
            else
                throw new Exception("File DOESN'T EXIST");
        }
        //Again does the same as "loadStaff" except with the visits class, list and file
        public List<Visit> loadVisits()
        {
            if (File.Exists("Clients.dat"))
            {
                List<Visit> visits = new List<Visit>();
                using (Stream file = File.Open("Visits.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    visits = (List<Visit>)formatter.Deserialize(file);
                }
                return visits;
            }
            else
                throw new Exception("File DOESN'T EXIST");
        }
        public void save(Dictionary<int, Staff> members, Dictionary<int, Client> clients, List<Visit> visits)
        {
            //This time the BinaryFormatter will be used to convert data TO binary format
            BinaryFormatter formatter = new BinaryFormatter();
            //Each converts and stores the data in their corresponding binary files (FileMode.Create will cause the file to be created first if it doesn't exist)
            using (Stream staffFile = File.Open("Staff.dat", FileMode.Create))
                formatter.Serialize(staffFile, members);
            using (Stream clientFile = File.Open("Clients.dat", FileMode.Create))
                formatter.Serialize(clientFile, clients);
            using (Stream visitFile = File.Open("Visits.dat", FileMode.Create))
                formatter.Serialize(visitFile, visits);
        }
    }
}