using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using restService_Scrumtopia.Controllers;
using Scrumtopia_classes;

namespace UnittestScrumtopia
{
    [TestClass]
    public class TestAfProject
    {
        

        [TestMethod]
        public async Task TestGet()
        {


            //arrange
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            //act

            ProjectsController controller = new ProjectsController();

            List<Project> projects = controller.Get(1);

            //assert

            Assert.IsTrue(projects.Count>0);

        }


        [TestMethod]
        public async Task TestOpret()
        {


            //arrange
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            Project expected = new Project(){Project_Deadline = DateTime.Now.AddDays(14), Project_Description = "unit test", Project_Name = "unit test", UserIds = new List<int>(){1}};

            Project actual;

            //act
          
            ProjectsController controller = new ProjectsController();

            actual = controller.Post(expected);
         


            //assert

            Assert.AreEqual(expected.Project_Description, actual.Project_Description);

        }

        [TestMethod]
        public async Task TestEdit()
        {


            //arrange
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            Project expected = new Project(){Project_Deadline = DateTime.Now.AddDays(14), Project_Description = "unit test ret", Project_Name = "unit test ret", UserIds = new List<int>(){1,2}};
   
            ProjectsController controller = new ProjectsController();

            List<Project> projects = controller.Get(1);

            int insertedID = projects[projects.Count-1].Project_Id;

            //act
          
        

            bool succes = controller.Put(insertedID, expected);
      


            //assert

            Assert.AreEqual(succes, true);

        }

        [TestMethod]
        public async Task TestDelete()
        {


            //arrange
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            ProjectsController controller = new ProjectsController();

            List<Project> projects = controller.Get(1);

            int insertedID = projects[projects.Count - 1].Project_Id;
            //act

          

            bool succes = controller.Delete(insertedID);
      


            //assert

            Assert.AreEqual(succes, true);

        }

    }
}
