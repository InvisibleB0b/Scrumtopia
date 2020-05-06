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
    public class ScrumUsersController : ApiController
    {
        public static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scrumtopia;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // GET api/Stories
        public List<ScrumUser> GetAllInProject(int projectId)
        {
            List<ScrumUser> scUsers = new List<ScrumUser>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {


                string queryString = $"SELECT User_Id, User_Name FROM Users WHERE User_Id IN (SELECT User_Id FROM Project_User_Relation WHERE Project_Id =  {projectId})";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                ScrumUser sc = new ScrumUser()
                {
                    User_Id = (int)reader["User_Id"],
                    User_Name = (string)reader["User_Name"]
                };
                scUsers.Add(sc);
                }



                command.Connection.Close();
            }

            return scUsers;
        }
    }
}
