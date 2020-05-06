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
        private const string Serverurl = "http://localhost:52512/";

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
    }
}
