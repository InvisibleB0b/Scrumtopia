using Scrumtopia_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Scrumtopia.Persistency
{
    static class SprintsPer
    {
        /// <summary>
        /// Den Lokation (URL) der bruges til at lave kald til API'en
        /// </summary>
        private const string Serverurl = "http://localhost:52512/";

        /// <summary>
        /// Henter alle sprints der er tilknyttet et bestemt projekt
        /// </summary>
        /// <param name="project_Id"> Det valgte projekt fra Singleton</param>
        /// <returns>Liste af alle sprints der er tilknyttet til det valgte projekt id</returns>
        public static async Task<List<Sprint>> LoadBacklog(int project_Id)
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {

                client.BaseAddress = new Uri(Serverurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));

                try
                {
                    var response = await client.GetAsync($"api/Sprints?Project_id={project_Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<List<Sprint>>().Result;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

        /// <summary>
        /// tager det sprint der er sendt med og sender til API'en for at oprette et nyt sprint
        /// </summary>
        /// <param name="sprint">Typen skal være et sprint der indeholder de ønskende informationer for det ny oprettet sprint</param>
        /// <param name="project_Id">Det nye sprint skal tilknyttes det ønskende projekt</param>
        /// <returns>med alle informationer udfyldt</returns>
        public static async Task<Sprint> Create(Sprint sprint, int project_Id)
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {

                client.BaseAddress = new Uri(Serverurl);

                client.DefaultRequestHeaders.Clear();


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));

                try
                {
                    var response = await client.PostAsJsonAsync($"/api/Sprints?project_Id={project_Id}", sprint);
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<Sprint>().Result;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

        }

        /// <summary>
        /// Opdaterer en sprintet baseret på informationer der bliver sendt med til de
        /// </summary>
        /// <param name="sprint">Et sprint skal indholde strukturen af hvad der skal redigeres</param>
        /// <param name="SprintId">Id'et på det sprint der skal redigeres</param>
        /// <returns></returns>
        public static async Task<bool> EditSprint(Sprint sprint, int SprintId)
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {

                client.BaseAddress = new Uri(Serverurl);

                client.DefaultRequestHeaders.Clear();


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));

                try
                {
                    var response = await client.PutAsJsonAsync($"/api/Sprints/{SprintId}", sprint);
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<bool>().Result;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

        /// <summary>
        /// Slet en sprintet ud fra databasen baseret på sprint id'et
        /// </summary>
        /// <param name="sprintId">Det sprint der skal slettes ID</param>
        /// <returns>true/false hvis det lykkes at slette storien</returns>
        public static async Task<bool> DeleteSprint(int sprintId)
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {

                client.BaseAddress = new Uri(Serverurl);

                client.DefaultRequestHeaders.Clear();


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));

                try
                {
                    var response = await client.DeleteAsync($"/api/Sprints/{sprintId}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<bool>().Result;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }
    }
}
