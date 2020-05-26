using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Scrumtopia_classes;

namespace Scrumtopia.Persistency
{
    static class StoryPer
    {
        /// <summary>
        /// Den Lokation (URL) der bruges til at lave kald til API'en
        /// </summary>
        private const string Serverurl = "http://localhost:52512/";


        /// <summary>
        /// tager den story der er sendt med og sender til API'en for at oprette en ny story
        /// </summary>
        /// <param name="s">Typen skal være en story der indeholder de informationer du ønsker at oprette en story med</param>
        /// <returns>Story, med alle informationer udfyldt</returns>
        public static async Task<Story> Create(Story s)
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
                    var response = await client.PostAsJsonAsync("api/Stories", s);
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<Story>().Result;
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
     /// Henter alle stories tilknyttet et bestemt sprint
     /// </summary>
     /// <param name="sprint_Id">Det valgte sprint fra Singleton</param>
     /// <returns>Liste af alle stories der er knyttet op på det valgte sprints id</returns>
        public static async Task<List<Story>> LoadSprintBacklog(int sprint_Id)
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
                    var response = await client.GetAsync($"api/Stories?Sprint_Id={sprint_Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<List<Story>>().Result;
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
     /// Henter alle stories ud der er tilknyttet et bestemt projekt, i rækkefølge efter story priorite DESC
     /// </summary>
     /// <param name="project_Id">Det valgte projects ID</param>
     /// <returns>List af stories, tilknyttet project ID i rækkefølge efter priority DESC</returns>
        public static async Task<List<Story>> LoadBacklog(int project_Id)
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
                    var response = await client.GetAsync($"api/Stories?Project_id={project_Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<List<Story>>().Result;
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
     /// Ændre story state baseret på story id og den ændring af staten der har været
     /// </summary>
     /// <param name="storyId">Id på den valgte story</param>
     /// <param name="dragStory">Den story der bliver trukket over i en ny OC</param>
     /// <returns>Om det lykkes at ændre staten</returns>
        public static async Task<bool> ChangeState(int storyId, Story dragStory)
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
                    var response = await client.PutAsJsonAsync($"api/Stories?storyId={storyId}", dragStory);
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
     /// Opdaterer en story baseret på informationer der bliver sendt med til den
     /// </summary>
     /// <param name="s">en story der skal indholde strukturen af hvad der skal redigeres</param>
     /// <param name="storyId">ID på den story der skal redigeres</param>
     /// <returns>true/false om det lykkes eller ej</returns>
        public static async Task<bool> Edit(Story s, int storyId)
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
                    var response = await client.PutAsJsonAsync($"api/Stories/{storyId}", s);
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
     /// Slet en story fra databasen baseret på id
     /// </summary>
     /// <param name="Story_Id">Den Story der skal slettes ID</param>
     /// <returns>true/false hvis det lykkes at slette storien</returns>
        public static async Task<bool> Delete(int Story_Id)
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
                    var response = await client.DeleteAsync($"api/Stories/{Story_Id}");

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
