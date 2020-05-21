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
    public class CategoriesController : ApiController
    {
        public static string ConnectionString = "Data Source=scrumtopia.database.windows.net;Initial Catalog=Scrumtopia;User ID=admin10;Password=Nonsecret10;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET api/Projects
        public List<Category> Get()
        {
            List<Category> categories = new List<Category>();


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {


                string queryString = $"SELECT * FROM Categories";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                   Category c = new Category()
                   {
                       Category_Id = (int)reader["Category_Id"],
                       Category_Color = (string)reader["Category_Color"],
                       Category_Name = (string)reader["Category_Name"]
                   };

                   categories.Add(c);
                }

                command.Connection.Close();
            }
            return categories;
        }

        // GET api/Categorys/5
        public Category Get(int id)
        {
            Category c = new Category();
            return c;
        }

        // POST api/categorys
        public Category Post([FromBody]Category value)
        {
            Category c = new Category();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"INSERT INTO Categories VALUES('{value.Category_Name}', '{value.Category_Color}') SELECT * FROM Categories WHERE Category_Id = @@IDENTITY";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    c.Category_Name = (string) reader["Category_Name"];
                    c.Category_Color = (string) reader["Category_Color"];
                    c.Category_Id = (int) reader["Category_Id"];
                }

                command.Connection.Close();
                
            }

            return c;
        }

        // PUT api/Categories/5
        public bool Put(int id, [FromBody]Category value)
        {
            bool succes = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"UPDATE Categories SET Category_Name = '{value.Category_Name}', Category_Color = '{value.Category_Color}' WHERE Category_Id = {id}";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                 succes = command.ExecuteNonQuery() > 0;
                
                command.Connection.Close();
           
            }

            return succes;
        }

        // DELETE api/Categories/5
        public bool Delete(int id)
        {
            bool success = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"DELETE FROM Categories WHERE Category_Id = {id}";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                success = command.ExecuteNonQuery() > 0;

                command.Connection.Close();

            }

            return success;

        }
    }
}
