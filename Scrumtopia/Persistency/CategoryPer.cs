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
    public static class CategoryPer
    {
        private const string Serverurl = "http://localhost:52512/";

        public static async Task<List<Category>> GetCategories()
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
                    var response = await client.GetAsync($"api/Categories");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<List<Category>>().Result;
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

        public static async Task<Category> Create(Category c)
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
                    var response = await client.PostAsJsonAsync("api/Categories", c);

                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<Category>().Result;
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

        public async static Task<bool> Delete(int category_Id)
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
                    var response = await client.DeleteAsync($"api/Categories/{category_Id}");

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

        public static async Task<bool> EditCategory(Category c, int category_Id)
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
                    var response = await client.PutAsJsonAsync($"api/Categories/{category_Id}", c);

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
