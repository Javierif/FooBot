using FooBot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FooBot.Services
{
    public class JokeServicio
    {

        public static string firstName = "Ilitri";
        public static string lastName = "";
        public static string url = $"https://api.icndb.com/jokes/random?firstName={firstName}&lastName={lastName}";

        public static async Task<string> getRandomJoke()
        {
            string ret = "Ahora no me apetece...";

            using (HttpClient client = new HttpClient())
            {

                var msg = await client.GetAsync(url);
                if (msg.IsSuccessStatusCode)
                {
                    string response = await msg.Content.ReadAsStringAsync();
                    JokeResponse joke = JsonConvert.DeserializeObject<JokeResponse>(response);
                    if (joke.type.Equals("success"))
                    {
                        ret = joke.value.joke;
                    }
                }
            }

            return ret;
        }
    }
}