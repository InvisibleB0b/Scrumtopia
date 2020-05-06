using Scrumtopia_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;

namespace Scrumtopia.Persistency
{
    class UsersPer
    {
        private const string Serverurl = "http://localhost:52512/";

        public async static Task<List<ScrumUser>> GetProjectUsers(int project_Id)
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
                    var response = await client.GetAsync($"api/ScrumUsers?ProjectId={project_Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<List<ScrumUser>>().Result;
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
