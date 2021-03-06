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
        public List<Story> GetSprintBacklog(int Sprint_Id)
        {
            List<Story> stories = new List<Story>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $@"SELECT *, (SELECT User_Name FROM Users WHERE User_Id = Story_Referee) AS Referee_Name, (SELECT User_Name FROM Users WHERE User_Id = Story_Asignee) AS Asignee_Name  FROM Stories LEFT JOIN Categories ON Categories.Category_Id = Stories.Category_Id WHERE Sprint_Id = {Sprint_Id} ORDER BY Story_Priority DESC";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Story s = new Story();
                    s.Story_Id = (int)reader["Story_Id"];
                    s.Project_Id = (int)reader["Project_Id"];
                    s.Sprint_Id = (int)reader["Sprint_Id"];
                    s.Category = new Category() { Category_Id = (int)reader["Category_Id"], Category_Color = (string)reader["Category_Color"], Category_Name = (string)reader["Category_Name"] };
                    s.Story_Name = (string)reader["Story_Name"];
                    s.Story_description = (string)reader["Story_Description"];
                    s.Story_Points = (int)reader["Story_Points"];
                    s.Story_Priority = (int)reader["Story_Priority"];
                    s.Story_State = (string)reader["Story_State"];
                    s.Story_Referee = new ScrumUser() { User_Id = (int)reader["Story_Referee"], User_Name = (string)reader["Referee_Name"] };
                    if (reader["Asignee_Name"] != DBNull.Value)
                    {
                        s.Story_Asignee = new ScrumUser() { User_Id = (int)reader["Story_Asignee"], User_Name = (string)reader["Asignee_Name"] };
                    }
                    else
                    {
                        s.Story_Asignee = new ScrumUser() { User_Id = 0, User_Name = "Not assigned" };
                    }

                    stories.Add(s);
                }

                command.Connection.Close();
            }

            return stories;
        }

        // GET api/Stories/5
        public List<Story> GetBacklog(int Project_id)
        {
            List<Story> stories = new List<Story>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $@"SELECT *, (SELECT User_Name FROM Users WHERE User_Id = Story_Referee) AS Referee_Name, (SELECT User_Name FROM Users WHERE User_Id = Story_Asignee) AS Asignee_Name  FROM Stories LEFT JOIN Categories ON Categories.Category_Id = Stories.Category_Id WHERE Project_Id = {Project_id} ORDER BY Story_Priority DESC";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Story s = new Story();
                    s.Story_Id = (int)reader["Story_Id"];
                    s.Project_Id = (int)reader["Project_Id"];
                    s.Sprint_Id = (int)reader["Sprint_Id"];
                    s.Category = new Category() { Category_Id = (int)reader["Category_Id"], Category_Color = (string)reader["Category_Color"], Category_Name = (string)reader["Category_Name"] };
                    s.Story_Name = (string)reader["Story_Name"];
                    s.Story_description = (string)reader["Story_Description"];
                    s.Story_Points = (int)reader["Story_Points"];
                    s.Story_Priority = (int)reader["Story_Priority"];
                    s.Story_State = (string) reader["Story_State"];
                    s.Story_Referee = new ScrumUser() { User_Id = (int)reader["Story_Referee"], User_Name = (string)reader["Referee_Name"] };
                    if (reader["Asignee_Name"] != DBNull.Value)
                    {
                        s.Story_Asignee = new ScrumUser() { User_Id = (int)reader["Story_Asignee"], User_Name = (string)reader["Asignee_Name"] };
                    }
                    else
                    {
                        s.Story_Asignee = new ScrumUser() { User_Id = 0, User_Name = "Not assigned" };
                    }

                    stories.Add(s);
                }

                command.Connection.Close();
            }




            return stories;
        }

        // POST api/Stories
        public Story Post([FromBody]Story value)
        {

            Story s = new Story();

            

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $@"INSERT INTO Stories VALUES({value.Project_Id}, {value.Sprint_Id}, {value.Category.Category_Id}, '{value.Story_Name}', '{value.Story_description}', {value.Story_Points}, {value.Story_Priority}, {value.Story_Referee.User_Id}, 'ToDo', {value.Story_Asignee.User_Id}) SELECT *, (SELECT User_Name FROM Users WHERE User_Id = Story_Referee) AS Referee_Name, (SELECT User_Name FROM Users WHERE User_Id = Story_Asignee) AS Asignee_Name  FROM Stories LEFT JOIN Categories ON Categories.Category_Id = Stories.Category_Id WHERE Story_Id = @@IDENTITY";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    s.Story_Id = (int) reader["Story_Id"];
                    s.Project_Id = (int) reader["Project_Id"];
                    s.Sprint_Id = (int) reader["Sprint_Id"];
                    s.Category = new Category(){Category_Id = (int) reader["Category_Id"], Category_Color = (string)reader["Category_Color"], Category_Name = (string)reader["Category_Name"]};
                    s.Story_Name = (string) reader["Story_Name"];
                    s.Story_description = (string) reader["Story_Description"];
                    s.Story_Points = (int) reader["Story_Points"];
                    s.Story_Priority = (int) reader["Story_Priority"];
                    s.Story_Referee = new ScrumUser() { User_Id = (int)reader["Story_Referee"], User_Name = (string)reader["Referee_Name"] };
                    if (reader["Asignee_Name"] != DBNull.Value)
                    {
                        s.Story_Asignee = new ScrumUser() { User_Id = (int)reader["Story_Asignee"], User_Name = (string)reader["Asignee_Name"] };
                    }
                    else
                    {
                        s.Story_Asignee = new ScrumUser() { User_Id = 0, User_Name = "Not assigned" };
                    }
                }

                command.Connection.Close();
            }
            return s;
        }

        // PUT api/Stories/5
        public bool Put(int id, [FromBody]Story value)
        {
            int rowAffected = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $" UPDATE Stories SET Category_Id = {value.Category.Category_Id}, Story_Name = '{value.Story_Name}', Story_Description = '{value.Story_description}', Story_Points = {value.Story_Points}, Story_Priority = {value.Story_Priority}, Story_Asignee = {value.Story_Asignee.User_Id} WHERE Story_Id = {id}";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                rowAffected = command.ExecuteNonQuery();
                

                command.Connection.Close();
            }

            return rowAffected == 1;
        }

        public bool Put_ChangeState(int storyId, [FromBody]Story value)
        {
            bool ch = false;

             using (SqlConnection connection = new SqlConnection(ConnectionString))
             {
                 string queryString = $" UPDATE Stories SET Story_State = '{value.Story_State}' WHERE Story_Id = {storyId}";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                ch = command.ExecuteNonQuery() > 0;
               

                command.Connection.Close();
             }

            return ch;
        }
        

        // DELETE api/Stories/5
        public bool Delete(int id)
        {

            bool success = false;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"DELETE FROM STORIES WHERE Story_Id = {id}";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                success = command.ExecuteNonQuery() > 0;

                command.Connection.Close();

            }

            return success;

        }
    }
}
