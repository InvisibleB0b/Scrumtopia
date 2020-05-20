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
        private const string Serverurl = "http://localhost:52512/";

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
