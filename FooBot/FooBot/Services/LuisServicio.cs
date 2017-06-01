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
                string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/ecbc0b1f-e1bd-4a2a-86dc-b67e99165c83?subscription-key=ae40a0cafdba4a62959aae4b6d4d3a3b&verbose=true&timezoneOffset=0&q={escapedString}";

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