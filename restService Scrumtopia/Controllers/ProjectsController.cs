using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Scrumtopia_classes;

namespace restService_Scrumtopia.Controllers
{
    public class ProjectsController : ApiController
    {

        public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scrumtopia;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET api/Projects
        public List<Project> Get()
        {
            List<Project> projects = new List<Project>(); 
            return projects;
        }

        // GET api/Projects/5
        public List<Project> Get(int id)
        {
            List<Project> projects = new List<Project>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
               

                string queryString = $"SELECT * FROM Projects WHERE Project_Id IN (SELECT Project_Id FROM Project_User_Relation WHERE User_Id =  {id})";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Project p = new Project()
                    {
                        Project_Id = (int)reader["Project_Id"],
                        Project_Name = (string)reader["Project_Name"],
                        Project_Description = (string)reader["Project_Description"],
                        Project_Deadline = (DateTime)reader["Project_Deadline"]
                    };
                    projects.Add(p);
                }



                command.Connection.Close();
            }
            return projects;
        }

        // POST api/Projects
        public Project Post([FromBody]Project value)
        {

            Project p = new Project();


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string dateInsert = value.Project_Deadline.ToString("yyyy-MM-dd HH:mm:ss");

                string queryString = $"INSERT INTO Projects VALUES('{value.Project_Name}', '{value.Project_Description}', '{dateInsert}') SELECT * FROM Projects WHERE Project_Id = @@IDENTITY";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    p.Project_Id = (int)reader["Project_Id"];
                    p.Project_Name = (string)reader["Project_Name"];
                    p.Project_Description = (string)reader["Project_Description"];
                    p.Project_Deadline = (DateTime)reader["Project_Deadline"];
                }

                command.Connection.Close();

                string valueString = "";

                foreach (int valueUserId in value.UserIds)
                {
                    if (valueString == "")
                    {
                        valueString += $"({p.Project_Id}, {valueUserId})";
                    }
                    else
                    {
                        valueString += $", ({p.Project_Id}, {valueUserId})";
                    }
                    
                }

                string queryString2 = $"INSERT INTO Project_User_Relation VALUES{valueString}";
                SqlCommand command2 = new SqlCommand(queryString2, connection);
                command2.Connection.Open();

                command2.ExecuteNonQuery();

                command2.Connection.Close();
            }
            return p;
        }

        // PUT api/Projects/5
        public void Put(int id, [FromBody]string value)
        {
            
        }

        // DELETE api/Projects/5
        public bool Delete(int id)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"DELETE FROM Sprints WHERE Project_Id = {id}; DELETE FROM Project_User_Relation WHERE Project_Id = {id}; DELETE FROM Stories WHERE Project_Id = {id}; DELETE FROM Projects WHERE Project_Id = {id}";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                success = command.ExecuteNonQuery() > 0;

                command.Connection.Close();

            }

            return success;

        }
    }
}