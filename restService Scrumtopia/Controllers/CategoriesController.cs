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
    public class CategoriesController : ApiController
    {
        public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scrumtopia;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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

            Category p = new Category();

            
            return p;
        }

        // PUT api/Categories/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/Categories/5
        public void Delete(int id)
        {
        }
    }
}