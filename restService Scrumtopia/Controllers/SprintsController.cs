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

        public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scrumtopia;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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
        public Sprint Post([FromBody]Sprint value)
        {
            Sprint s = new Sprint();


            return s;
        }

        // PUT api/Stories/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/Stories/5
        public void Delete(int id)
        {
        }
    }
}
