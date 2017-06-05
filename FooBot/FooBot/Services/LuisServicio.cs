using FooBot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FooBot.Services
{
    public class LuisServicio
    {
        public static async Task<LuisResponse> ParseUserConversation(string texto)
        {

            var luisResponse = new LuisResponse();

            using (var client = new HttpClient())
            {
                string escapedString = Uri.EscapeDataString(texto);
                string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/07e95986-30dc-403f-b804-613c04ff93ee?subscription-key=9ac18bd2b94e409bb3235edac058d0b3&verbose=true&timezoneOffset=0&q={escapedString}";

                var msg = await client.GetAsync(uri);
                if(msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    luisResponse = JsonConvert.DeserializeObject<LuisResponse>(jsonResponse);
                }
               
            }
            return luisResponse;
        }
    }
}