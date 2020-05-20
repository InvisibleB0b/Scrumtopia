using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Scrumtopia_classes;

namespace restService_Scrumtopia.Controllers
{
    public class SprintsController : ApiController
    {

        public static string ConnectionString = "Data Source=scrumtopia.database.windows.net;Initial Catalog=Scrumtopia;User ID=admin10;Password=Nonsecret10;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET api/Stories
        public List<Sprint> GetBacklog(int Project_id)
        {
            List<Sprint> sprints = new List<Sprint>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $@"SELECT * FROM Sprints WHERE Project_Id = {Project_id}";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                   Sprint s = new Sprint()
                   {
                       Sprint_Id = (int)reader["Sprint_Id"],
                       Sprint_Goal = (string)reader["Sprint_Goal"],
                       Sprint_Start = (DateTime)reader["Sprint_Start"],
                       Sprint_End = (DateTime)reader["Sprint_End"]
                   };

                   sprints.Add(s);
                }

                command.Connection.Close();
            }


            return sprints;
        }

        // GET api/Stories/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/Stories
        public Sprint Post([FromBody]Sprint value, int project_Id)
        {
            Sprint s = new Sprint();
            string startDateInsert = value.Sprint_Start.ToString("yyyy - MM - dd HH: mm:ss");
            string endDateInsert = value.Sprint_End.ToString("yyyy - MM - dd HH: mm:ss");
            string idList = "0";

            foreach (int i in value.Story_Ids)
            {
                idList += "," + i.ToString() + " ";
            }
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $@"INSERT INTO Sprints VALUES('{startDateInsert}', {project_Id}, '{endDateInsert}', '{value.Sprint_Goal}') UPDATE Stories SET Sprint_Id = (SELECT Sprint_Id FROM Sprints WHERE Sprint_Id = @@IDENTITY) WHERE Story_Id IN ({idList}) SELECT * FROM Sprints WHERE Sprint_Id = @@IDENTITY";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                        s.Sprint_Id = (int)reader["Sprint_Id"];
                        s.Sprint_Goal = (string) reader["Sprint_Goal"];
                        s.Sprint_Start = (DateTime) reader["Sprint_Start"];
                        s.Sprint_End = (DateTime) reader["Sprint_End"];

                }

                command.Connection.Close();
            }

            return s;
        }

        // PUT api/Stories/5
        public bool Put(int id, [FromBody]Sprint value)
        {
            bool succuess = false;

            string startDateInsert = value.Sprint_Start.ToString("yyyy - MM - dd HH: mm:ss");
            string endDateInsert = value.Sprint_End.ToString("yyyy - MM - dd HH: mm:ss");
            string idList = "0";

            foreach (int i in value.Story_Ids)
            {
                idList += ", " + i.ToString() + " ";
            }
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $@"UPDATE Stories SET Sprint_Id = 0 WHERE Sprint_Id = {id} UPDATE Sprints SET Sprint_Goal = '{value.Sprint_Goal}', Sprint_Start = '{startDateInsert}', Sprint_End = '{endDateInsert}' WHERE Sprint_Id = {id} UPDATE Stories SET Sprint_Id = {id} WHERE Story_Id IN ({idList})";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                succuess = command.ExecuteNonQuery() > 0;
                

                command.Connection.Close();
            }

            return succuess;

        }

        // DELETE api/Stories/5
        public bool Delete(int id)
        {
            bool succuess = false;

        
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $@"DELETE FROM Sprints WHERE Sprint_Id = {id} UPDATE Stories SET Sprint_Id = 0 WHERE Sprint_Id = {id}";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                succuess = command.ExecuteNonQuery() > 0;


                command.Connection.Close();
            }

            return succuess;

        }
    }
}
