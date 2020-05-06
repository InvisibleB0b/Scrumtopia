﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Scrumtopia_classes;

namespace restService_Scrumtopia.Controllers
{
    public class StoriesController : ApiController
    {
        public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scrumtopia;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET api/Stories
        public List<Story> Get()
        {
            List<Story> stories = new List<Story>();


            return stories;
        }

        // GET api/Stories/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/Stories
        public Story Post([FromBody]Story value)
        {

            Story s = new Story();

            

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"INSERT INTO Stories VALUES({value.Project_Id}, {value.Sprint_Id}, {value.Category.Category_Id}, '{value.Story_Name}', '{value.Story_description}', {value.Story_Points}, {value.Story_Priority}, {value.Story_Referee.User_Id}, {value.Story_Asignee.User_Id}) SELECT * FROM Stories WHERE Story_Id = @@IDENTITY";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    s.Story_Id = (int) reader["Story_Id"];
                    s.Project_Id = (int) reader["Project_Id"];
                    s.Sprint_Id = (int) reader["Sprint_Id"];
                    s.Category = new Category(){Category_Id = (int) reader["Category_Id"]};
                    s.Story_Name = (string) reader["Story_Name"];
                    s.Story_description = (string) reader["Story_Description"];
                    s.Story_Points = (int) reader["Story_Points"];
                    s.Story_Priority = (int) reader["Story_Priority"];
                    s.Story_Referee = new User(){User_Id = (int) reader["Story_Referee"]};
                    s.Story_Asignee = new User(){User_Id = (int) reader["Story_Asignee"]};
                }

                command.Connection.Close();
            }
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